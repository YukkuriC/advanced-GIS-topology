using MiniGIS.Data;
using MiniGIS.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class API
    {
        // 点图层转栅格
        public static GridLayer Point2Grid(GeomLayer layer, string method,
            double xmin, double xmax, double ymin, double ymax,
            uint xsplit, uint ysplit)
        {
            Grid result = new Grid(xmin, xmax, ymin, ymax, xsplit, ysplit);
            switch (method)
            {
                case "方位取点加权法": result.GenGrid_DirGrouped(layer.points); break;
                case "距离倒数法": result.GenGrid_DistRev(layer.points); break;
                case "距离平方倒数法": result.GenGrid_DistPow2Rev(layer.points); break;
            }

            // 创建图层
            return new GridLayer(result, layer.Name + "_" + method);
        }

        // 栅格图层插值
        public static GridLayer GridInterpolation(GridLayer layer, uint xstep, uint ystep)
        {
            Grid newGrid = GenGrid.LinearInterpolation(layer.data, xstep, ystep);
            return new GridLayer(newGrid, layer.Name + String.Format("_{0}x{1}加密", xstep, ystep));
        }

        // 点图层转TIN
        public static TINLayer Point2TIN(GeomLayer layer) => new TINLayer(layer.points, layer.Name + "_TIN");

        // 栅格/TIN图层等值线
        public static GeomLayer Value2Contour(ValueLayer layer, IEnumerable<double> targetSplits, string postFix = "")
        {
            List<GeomPoint> points = null;
            List<GeomArc> arcs = null;

            // 执行搜索算法
            switch (layer)
            {
                case GridLayer layer1:
                    Contour.GenContourGrid(layer1.data, targetSplits, out points, out arcs);
                    break;
                case TINLayer layer1:
                    Contour.GenContourTIN(layer1.edgeSides, layer1.values, targetSplits, out points, out arcs);
                    break;
                default:
                    return null;
            }

            // 创建图层
            var result = new GeomLayer(GeomType.Arc, layer.Name + "_等值线" + postFix)
            {
                points = points,
                arcs = arcs
            };

            // 默认样式
            result.colors["arc"] = ColorOps.Random();
            result.SetPartVisible("point", false);

            return result;
        }

        // 等值线光滑
        public static GeomLayer ContourSmooth(GeomLayer layer)
        {
            // 逐个平滑转换
            List<double> values = new List<double>();
            List<List<Vector2>> raw_arcs = new List<List<Vector2>>();
            foreach (var arc in layer.arcs)
            {
                var smoother = new Spline2D(from v in arc.points select (Vector2)v);
                int nsplit = (arc.points.Count() - 1) * 40;
                raw_arcs.Add(smoother.Smooth(nsplit));
                values.Add(arc.value);
            }

            // TODO: 处理相交

            // 输出至图层
            GeomLayer result = new GeomLayer(GeomType.Arc, layer.Name + "_平滑");
            int idxPoint = 0, idxArc = 0;
            for (int i = 0; i < values.Count; i++)
            {
                var currValue = values[i];
                var newPoints = new List<GeomPoint>();
                foreach (var v in raw_arcs[i]) newPoints.Add(new GeomPoint(v.X, v.Y, ++idxPoint, currValue));
                result.points.AddRange(newPoints);
                result.arcs.Add(new GeomArc(newPoints, ++idxArc, currValue));
            }

            // 默认样式
            result.colors["arc"] = Color.Red;
            result.sizes["arc"] = 1;
            result.SetPartVisible("point", false);

            return result;
        }

        // 等值线图层生成拓扑多边形
        public static GeomLayer Arc2Topology(GeomLayer layer)
        {
            GeomLayer newLayer = new GeomLayer(GeomType.Polygon, layer.Name + "_拓扑");
            GenTopology.Entry(layer.arcs, layer.MBR, out newLayer.points, out newLayer.arcs, out newLayer.polygons);

            return newLayer;
        }
    }
}
