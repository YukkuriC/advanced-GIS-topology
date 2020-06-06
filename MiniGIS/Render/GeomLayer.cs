using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MiniGIS.Data;

namespace MiniGIS.Render
{
    /// <summary>
    /// 矢量渲染层
    /// 颜色参数:
    ///     point: 点上色
    ///     arc: 弧段上色
    ///     polygon: 多边形填充色
    /// 尺寸参数:
    ///     point: 点大小
    ///     arc: 弧段粗细
    /// 可见性参数:
    ///     point; arc; polygon
    /// </summary>
    public class GeomLayer : Layer
    {
        // 显示图层类型
        public override string layerType
        {
            get
            {
                if (polygons != null) return "多边形";
                if (arcs != null) return "弧段";
                if (points != null) return "多点";
                return "矢量图层";
            }
        }

        // 外包矩形
        Rect _mbr;
        public Rect MBR
        {
            get
            {
                if (_mbr == null) _mbr = new Rect(points);
                return _mbr;
            }
        }

        // 数据容器
        public List<GeomPoint> points;
        public List<GeomArc> arcs;
        public List<GeomPoly> polygons;

        public override void Render(ViewPort port, Graphics canvas)
        {
            // 初始化随机颜色
            ColorOps.InitRandom(seed);

            // 创建画笔
            Color clr = GetColor("polygon");
            bool randColor = clr == Color.Empty;
            Pen pen = new Pen(clr);
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

            // 填色多边形
            if (polygons != null && GetPartVisible("polygon")) foreach (var x in polygons)
                {
                    if (randColor) pen.Color = ColorOps.Random();
                    x.Render(port, canvas, pen);
                }

            // 绘制弧段
            if (arcs != null && GetPartVisible("arc"))
            {
                clr = GetColor("arc");
                randColor = clr == Color.Empty;
                pen.Color = clr;
                pen.Width = GetSize("arc");
                foreach (var x in arcs)
                {
                    if (randColor) pen.Color = ColorOps.Random();
                    x.Render(port, canvas, pen);
                }
            }

            // 绘制点
            if (points != null && GetPartVisible("point"))
            {
                clr = GetColor("point");
                randColor = clr == Color.Empty;
                pen.Color = clr;
                pen.Width = GetSize("point");
                foreach (var x in points)
                {
                    if (randColor) pen.Color = ColorOps.Random();
                    x.Render(port, canvas, pen);
                }
            }
        }

        // 计算外包矩形并设置镜头
        public override void Focus(ViewPort port)
        {
            if (points == null || points.Count == 0) return;
            port.Focus(MBR);
        }

        public GeomLayer(GeomType type = GeomType.Point, string name = "矢量图层") : base(name)
        {
            if (type >= GeomType.Point) points = new List<GeomPoint>();
            if (type >= GeomType.Arc) arcs = new List<GeomArc>();
            if (type == GeomType.Polygon) polygons = new List<GeomPoly>();
        }
    }
}
