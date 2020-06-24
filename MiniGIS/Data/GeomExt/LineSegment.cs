using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Data
{

    public class LineSegment:Tuple<GeomPoint, GeomPoint>
    {
        // 长度、角度
        Lazy<double> _length;
        Lazy<double> _angle = new Lazy<double>();
        public double Length { get { return _length.Value; } }
        public double Angle { get { return _angle.Value; } }
        double CalcLength() => ((Vector2)Item1).Distance(Item2);
        double CalcAngle() => ((Vector2)Item2 - Item1).Rotation();

        public LineSegment(GeomPoint p1, GeomPoint p2) : base(p1,p2)
        {
            _length = new Lazy<double>(CalcLength);
            _angle = new Lazy<double>(CalcAngle);
        }

        public static Tuple<double, double> CheckCross(LineSegment s1, LineSegment s2)
        {
            double
                ax = s1.Item1.X, ay = s1.Item1.Y,
                bx = s1.Item2.X, by = s1.Item2.Y,
                cx = s2.Item1.X, cy = s2.Item1.Y,
                dx = s2.Item2.X, dy = s2.Item2.Y;

            // 求解
            double dom = (ax - bx) * (cy - dy) - (ay - by) * (cx - dx);
            if (dom == 0) return null;
            double
                r1 = ((ax - cx) * (cy - dy) - (ay - cy) * (cx - dx)) / dom,
                r2 = (-(ax - bx) * (ay - cy) + (ax - cx) * (ay - by)) / dom;
            return new Tuple<double, double>(r1, r2);
        }
    }
}
