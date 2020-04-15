using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Data
{

    public class LineSegment
    {
        // 点数据
        GeomPoint pt1, pt2;
        public GeomPoint From { get { return pt1; } }
        public GeomPoint To { get { return pt2; } }

        // 坐标差
        double dx, dy;
        public double deltaX { get { return dx; } }
        public double deltaY { get { return dy; } }

        // 长度、角度
        Lazy<double> _length;
        Lazy<double> _angle = new Lazy<double>();
        public double Length { get { return _length.Value; } }
        public double Angle { get { return _length.Value; } }
        double CalcLength()
        {
            return Math.Sqrt(dx * dx + dy * dy);
        }
        double CalcAngle()
        {
            return Math.Atan2(dy, dx);
        }

        public LineSegment(GeomPoint p1, GeomPoint p2)
        {
            pt1 = p1; pt2 = p2;
            dx = pt2.Y - pt1.X; dy = pt2.Y - pt1.Y;
            _length = new Lazy<double>(CalcLength);
            _angle = new Lazy<double>(CalcAngle);
        }
    }
}
