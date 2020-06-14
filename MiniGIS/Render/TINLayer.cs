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
    class TINLayer : GridLayerBase
    {
        public override string layerType
        {
            get { return "TIN"; }
        }

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
        public Dictionary<Tuple<Vector2, Vector2>, HashSet<Triangle>> edgeSides; // 三角邻接关系
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
                    var colors = (from p in tri.Points() select ColorOps.Linear(cmin, cmax, values[p].Lerp(valmin, valmax))).ToArray();

                    // 计算中心颜色
                    double tmp = 0;
                    foreach (var p in tri.Points()) tmp += values[p];
                    var cmid = ColorOps.Linear(cmin, cmax, (tmp / 3).Lerp(valmin, valmax));

                    // 创建渐变笔刷
                    GraphicsPath path = new GraphicsPath(
                        (from p in tri.Points() select port.ScreenCoord(p.X, p.Y)).ToArray(),
                        new byte[] { (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line }
                    );
                    var brush = new PathGradientBrush(path);
                    brush.CenterColor = cmid;
                    brush.SurroundColors = (from p in tri.Points() select ColorOps.Linear(cmin, cmax, values[p].Lerp(valmin, valmax))).ToArray();
                    canvas.FillPath(brush, path);
                }
            }

            // 绘制边
            if (GetPartVisible("border") && GetSize("grid") > 0)
            {
                Pen pen = new Pen(GetColor("grid"), GetSize("grid"));
                pen.StartCap = pen.EndCap = LineCap.Round;
                foreach (Triangle tri in triangles)
                {
                    var pts = tri.Points().ToArray();
                    for (int i = 0; i < 3; i++)
                    {
                        p1 = port.ScreenCoord(pts[i].X, pts[i].Y);
                        p2 = port.ScreenCoord(pts[(i + 1) % 3].X, pts[(i + 1) % 3].Y);
                        canvas.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
                    }
                }
            }
        }

        public override void Focus(ViewPort port)
        {
            port.Focus(MBR);
        }

        public TINLayer(List<GeomPoint> points)
        {
            // 创建三角网
            GenDelaunay.DelaunayConvex(points, out triangles, out edgeSides);

            // 映射坐标取值+求最值
            values = new Dictionary<Vector2, double>();
            double _max = double.MinValue, _min = double.MaxValue;
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
