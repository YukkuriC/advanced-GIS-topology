﻿using System;
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
    /// </summary>
    class GeomLayer : Layer
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

        // 数据容器
        public List<GeomPoint> points;
        public List<GeomArc> arcs;
        public List<GeomPoly> polygons;

        public override void Render(ViewPort port, Graphics canvas)
        {
            base.Render(port, canvas);

            // 创建画笔
            Color clr = Color.Empty;
            bool randColor = !colors.TryGetValue("polygon", out clr);
            Pen pen = new Pen(clr);
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

            // 填色多边形
            if (polygons != null) foreach (var x in polygons)
                {
                    if (randColor) pen.Color = ColorOps.Random();
                    x.Render(port, canvas, pen);
                }

            // 绘制弧段
            if (arcs != null)
            {
                clr = Color.Empty;
                randColor = !colors.TryGetValue("arc", out clr);
                pen.Color = clr;
                pen.Width = size_arc;
                foreach (var x in arcs)
                {
                    if (randColor) pen.Color = ColorOps.Random();
                    x.Render(port, canvas, pen);
                }
            }

            // 绘制点
            if (points != null)
            {
                clr = Color.Empty;
                randColor = !colors.TryGetValue("point", out clr);
                pen.Color = clr;
                pen.Width = size_pt;
                foreach (var x in points)
                {
                    if (randColor) pen.Color = ColorOps.Random();
                    x.Render(port, canvas, pen);
                }
            }
        }

        public GeomLayer(GeomType type = GeomType.Point, string name = "矢量图层") : base(name)
        {
            if (type >= GeomType.Point) points = new List<GeomPoint>();
            if (type >= GeomType.Arc) arcs = new List<GeomArc>();
            if (type == GeomType.Polygon) polygons = new List<GeomPoly>();
        }
    }
}