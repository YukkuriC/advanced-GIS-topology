using System;
using System.Collections.Generic;
using System.Drawing;
using MiniGIS.Render;
using System.Linq;

namespace MiniGIS.Data
{
    // 弧几何体
    public class GeomArc : BaseGeom
    {
        #region prop

        // 数据
        public List<GeomPoint> points;

        // 拓扑关系
        public GeomPoly left;
        public GeomPoly right;

        #endregion

        #region method

        public IEnumerable<LineSegment> IterSegments(bool reversed = false)
        {
            if (reversed) for (int i = points.Count - 1; i > 0; i--)
                {
                    yield return new LineSegment(points[i], points[i - 1]);
                }
            else for (int i = 0; i < points.Count - 1; i++)
                {
                    yield return new LineSegment(points[i], points[i + 1]);
                }
        }

        public override void Render(ViewPort port, Graphics canvas, Pen pen)
        {
            canvas.DrawLines(pen,
                (from p in points select port.ScreenCoord(p.X, p.Y))
                    .ToArray()
            );
        }

        #endregion

        // 构造函数
        public GeomArc(IEnumerable<GeomPoint> _data, int _id = 0, double _value = 0) : base(_id, _value)
        {
            points = new List<GeomPoint>(_data);
            foreach (GeomPoint p in points) p.arcs.Add(this);
        }

        // 字符串化
        public override string ToString()
        {
            return String.Format("#{0}({1})={2}",
                id,
                String.Join(",", (from p in points select p.id)),
                value);
        }
    }
}
