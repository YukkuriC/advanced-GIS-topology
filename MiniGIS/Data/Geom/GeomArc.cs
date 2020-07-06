using System;
using System.Collections.Generic;
using System.Drawing;
using MiniGIS.Layer;
using System.Linq;

namespace MiniGIS.Data
{
    // 弧几何体
    public class GeomArc : BaseGeom
    {
        #region prop

        // 数据
        public List<GeomPoint> points;

        public GeomPoint First { get => points.First(); }
        public GeomPoint Last { get => points.Last(); }

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
                (from p in points select (PointF)port.ScreenCoord(p.X, p.Y))
                    .ToArray()
            );
        }

        #endregion

        public override Rect CalcMBR() => new Rect(points);

        // 计算长度
        Lazy<double> _length;
        public double Length { get => _length.Value; }
        double CalcLength()
        {
            double res = 0;
            foreach (var seg in IterSegments()) res += seg.Length;
            return res;
        }

        // 构造函数
        public GeomArc(IEnumerable<GeomPoint> _data, int _id = 0, double _value = 0) : base(_id, _value)
        {
            points = new List<GeomPoint>(_data);
            _length = new Lazy<double>(CalcLength);
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
