using MiniGIS.Algorithm;
using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace MiniGIS.Render
{
    /// <summary>
    /// TIN渲染层
    /// 渲染参数同GridLayer
    /// </summary>
    class TINLayer : ValueLayer
    {
        public override string layerType
        {
            get { return "TIN"; }
        }

        // 最值
        double _max, _min;
        public override double Max { get => _max; }
        public override double Min { get => _min; }

        // 外包矩形
        Rect _mbr;
        public Rect MBR
        {
            get
            {
                if (_mbr == null) _mbr = new Rect(from p in values.Keys select (GeomPoint)p);
                return _mbr;
            }
        }

        // 数据容器
        public HashSet<Triangle> triangles; // 各三角
        public Dictionary<Edge, HashSet<Triangle>> edgeSides; // 三角邻接关系
        public Dictionary<Vector2, double> values; // 坐标取值

        // 图层接口
        public override void Render(ViewPort port, Graphics canvas)
        {
            PointF p1, p2;

            // 三角上色
            if (GetPartVisible("fill"))
            {
                float valmin = GetSize("low"), valmax = GetSize("high");
                Color cmin = GetColor("low"), cmax = GetColor("high");
                foreach (Triangle tri in triangles)
                {
                    Vector2[] raw = tri.Points().ToArray();

                    // 计算颜色+坐标
                    Color[] colors = new Color[3];
                    PointF[] verts = new PointF[3];
                    PointF[] mids = new PointF[3];
                    double tmp = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        Vector2 vec = raw[i];
                        double val = values[vec];
                        tmp += val;
                        verts[i] = port.ScreenCoord(vec);
                        colors[i] = ColorOps.Linear(cmin, cmax, val.Lerp(valmin, valmax));
                        mids[i] = port.ScreenCoord((raw[(i + 1) % 3] + raw[(i + 2) % 3]) / 2);
                    }
                    Color cmid = ColorOps.Linear(cmin, cmax, (tmp / 3).Lerp(valmin, valmax));

                    // 实心填充
                    canvas.FillPolygon(new SolidBrush(cmid), mids);
                    for (int i = 0; i < 3; i++)
                        canvas.FillPolygon(new SolidBrush(colors[i]), (from j in Enumerable.Range(0, 3) select (i == j ? verts : mids)[j]).ToArray());
                }
            }

            // 绘制边
            if (GetPartVisible("border") && GetSize("grid") > 0)
            {
                Pen pen = new Pen(GetColor("grid"), GetSize("grid"));
                pen.StartCap = pen.EndCap = LineCap.Round;
                foreach (var edge in edgeSides.Keys)
                {
                    if (edge.Ordered()) continue; // 每边只绘制一次
                    p1 = port.ScreenCoord(edge.Item1);
                    p2 = port.ScreenCoord(edge.Item2);
                    canvas.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
                }
            }
        }

        public override void Focus(ViewPort port)
        {
            port.Focus(MBR);
        }

        public TINLayer(List<GeomPoint> points, string name = "TIN图层") : base(name)
        {
            // 创建三角网
            GenDelaunay.DelaunayConvex(points, out triangles, out edgeSides);

            // 映射坐标取值+求最值
            values = new Dictionary<Vector2, double>();
            _max = double.MinValue; _min = double.MaxValue;
            foreach (GeomPoint tmp in points)
            {
                values[tmp] = tmp.value;
                _max = Math.Max(_max, tmp.value);
                _min = Math.Min(_min, tmp.value);
            }

            // 配置默认参数
            sizes["low"] = (float)_min;
            sizes["high"] = (float)_max;
        }
    }
}
