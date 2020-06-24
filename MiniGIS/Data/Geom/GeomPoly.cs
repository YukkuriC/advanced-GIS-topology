using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using MiniGIS.Render;
using System.Linq;

namespace MiniGIS.Data
{
    // 多边形几何体
    public class GeomPoly : BaseGeom
    {
        #region prop

        // 数据

        /// <summary>
        /// Item1: 弧段
        /// Item2: 弧段是否反向连接
        /// </summary>
        public List<Tuple<GeomArc, bool>> arcs;

        public List<GeomPoly> holes;

        #endregion

        #region method

        // 迭代返回所有点
        public IEnumerable<GeomPoint> IterPoints(bool inner = false)
        {
            GeomArc arc;
            foreach (var data in arcs)
            {
                arc = data.Item1;
                if (data.Item2)// 反向迭代
                {
                    for (int i = arc.points.Count - 1; i > 0; i--) yield return arc.points[i];
                }
                else // 正向
                {
                    for (int i = 0; i < arc.points.Count - 1; i++) yield return arc.points[i];
                }
            }
            if (inner && holes != null)
                foreach (var poly in holes)
                    foreach (var p in poly.IterPoints(false))
                        yield return p;
        }

        // 迭代返回所有线段
        public IEnumerable<LineSegment> IterSegments(bool inner = false)
        {
            foreach (var pair in arcs)
                foreach (var s in pair.Item1.IterSegments(pair.Item2))
                    yield return s;
            if (inner && holes != null)
                foreach (var poly in holes)
                    foreach (var s in poly.IterSegments(false))
                        yield return s;
        }

        // 检查某坐标是否在多边形内部
        public bool Include(Vector2 pos)
        {
            if (!MBR.Include(pos)) return false;
            var checker = new LineSegment((GeomPoint)pos, new GeomPoint(MBR.XMax + 10, pos.Y));
            int crosses = 0;
            foreach (var seg in IterSegments(true))
            {
                var tmp = LineSegment.CheckCross(checker, seg);
                if (tmp == null) // 平行或共线
                {
                    if (seg.Item1.Y != pos.Y) continue;
                    if (seg.Item1.X < pos.X != seg.Item2.X < pos.X) crosses++; // 重合线横跨待检查坐标两侧
                }
                if (0 < tmp.Item1 && tmp.Item1 < 1 && 0 <= tmp.Item2 && tmp.Item2 < 1) crosses++; // Item2检查一端
            }

            return crosses % 2 == 1; // 射线交点为奇数
        }

        // 简单检查多边形包含关系
        // 不考虑孔洞、凹多边形等
        public bool IncludeSimple(GeomPoly other)
        {
            foreach (var p in other.IterPoints())
                if (!Include(p)) return false;
            return true;
        }

        public override void Render(ViewPort port, Graphics canvas, Pen pen)
        {
            var polyPath = new GraphicsPath();
            polyPath.AddPolygon((from p in IterPoints() select port.ScreenCoord(p.X, p.Y)).ToArray());
            if (holes != null) foreach (var poly in holes) polyPath.AddPolygon((from p in poly.IterPoints() select port.ScreenCoord(p.X, p.Y)).ToArray());
            canvas.FillPath(pen.Brush, polyPath);
        }

        #endregion

        public override Rect CalcMBR() => new Rect(IterPoints());

        // 计算周长、面积
        Lazy<double> _circum, _area, _circum0, _area0;
        public double Circum { get => _circum.Value; }
        public double Area { get => _area.Value; }
        public double OuterCircum { get => _circum0.Value; }
        public double OuterArea { get => _area0.Value; }
        double CalcOuterCircum() // 外围周长
        {
            double res = 0;
            foreach (var pair in arcs) res += pair.Item1.Length;
            return res;
        }
        double CalcCircum() // 含孔洞总周长
        {
            double res = OuterCircum;
            if (holes != null) foreach (var poly in holes) res += poly.OuterCircum;
            return res;
        }
        double CalcOuterArea() // 外围面积
        {
            double res = 0;
            var pts = IterPoints().ToArray();
            var pt0 = (Vector2)pts[0];
            Vector2 lastOffset = null;
            for (int i = 1; i < pts.Length; i++)
            {
                var currOffset = pts[i] - pt0;
                if (lastOffset != null) res += lastOffset.Cross(currOffset);
                lastOffset = currOffset;
            }
            return Math.Abs(res);
        }
        double CalcArea() // 去除孔洞后的面积
        {
            double res = OuterArea;
            if (holes != null) foreach (var poly in holes) res -= poly.OuterArea;
            return res;
        }
        public void UpdateHoles() // 用于更新孔洞后重置缓存
        {
            _circum = new Lazy<double>(CalcCircum);
            _area = new Lazy<double>(CalcArea);
        }

        public GeomPoly(IEnumerable<GeomArc> _data, int _id = 0, double _value = 0) : base(_id, _value)
        {
            _circum0 = new Lazy<double>(CalcOuterCircum);
            _area0 = new Lazy<double>(CalcOuterArea);
            UpdateHoles();

            List<GeomArc> arc_raw = new List<GeomArc>(_data);

            // 判断弧段成环时反向状态
            arcs = new List<Tuple<GeomArc, bool>>();
            {
                // 首个弧段
                GeomArc arc1 = arc_raw[0], arc2 = arc_raw.Last();
                bool reversed; GeomPoint pt_end;
                if ((pt_end = arc1.points[0]) == arc2.points[0] || pt_end == arc2.points.Last())
                {
                    pt_end = arc1.points.Last();
                    reversed = false;
                }
                else if ((pt_end = arc1.points.Last()) == arc2.points[0] || pt_end == arc2.points.Last())
                {
                    pt_end = arc1.points[0];
                    reversed = true;
                }
                else throw new TypeLoadException("弧段不闭合");
                arcs.Add(new Tuple<GeomArc, bool>(arc1, reversed));

                // 后续弧段
                for (int i = 1; i < arc_raw.Count; i++)
                {
                    arc2 = arc_raw[i];
                    if (arc2.points[0] == pt_end) reversed = false;
                    else if (arc2.points.Last() == pt_end) reversed = true;
                    else throw new TypeLoadException("弧段不闭合");
                    arcs.Add(new Tuple<GeomArc, bool>(arc2, reversed));

                    arc1 = arc2;
                    pt_end = reversed ? arc2.points[0] : arc2.points.Last();
                }
            }
        }
    }
}
