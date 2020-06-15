using System;
using System.Collections.Generic;
using System.Drawing;
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

        #endregion

        #region method

        // 迭代返回所有点
        public IEnumerable<GeomPoint> IterPoints()
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
        }

        public override void Render(ViewPort port, Graphics canvas, Pen pen)
        {
            canvas.FillPolygon(pen.Brush,
                (from p in IterPoints() select port.ScreenCoord(p.X, p.Y)).ToArray());
        }

        #endregion

        public GeomPoly(IEnumerable<GeomArc> _data, int _id = 0, double _value = 0) : base(_id, _value)
        {
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
