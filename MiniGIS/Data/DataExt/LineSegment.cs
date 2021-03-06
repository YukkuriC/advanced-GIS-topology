﻿using MiniGIS.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Data
{
    public class LineSegment : Tuple<GeomPoint, GeomPoint>
    {
        // 长度、角度
        Lazy<double> _length;
        Lazy<double> _angle = new Lazy<double>();
        public double Length { get { return _length.Value; } }
        public double Angle { get { return _angle.Value; } }
        double CalcLength() => ((Vector2)Item1).Distance(Item2);
        double CalcAngle() => ((Vector2)Item2 - Item1).Rotation();

        public LineSegment(GeomPoint p1, GeomPoint p2) : base(p1, p2)
        {
            _length = new Lazy<double>(CalcLength);
            _angle = new Lazy<double>(CalcAngle);
        }

        public static Tuple<double, double> CheckCross(LineSegment s1, LineSegment s2) => Utils.CheckCross(s1.Item1, s1.Item2, s2.Item1, s2.Item2);
    }
}
