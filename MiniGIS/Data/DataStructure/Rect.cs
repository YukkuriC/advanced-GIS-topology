using MiniGIS.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Data
{
    // 外包矩形
    public class Rect
    {
        public double XMin, XMax, YMin, YMax;
        public Rect(IEnumerable<GeomPoint> points)
        {
            XMin = double.MaxValue; XMax = double.MinValue;
            YMin = double.MaxValue; YMax = double.MinValue;
            foreach (GeomPoint pt in points)
            {
                XMin = Math.Min(XMin, pt.X);
                XMax = Math.Max(XMax, pt.X);
                YMin = Math.Min(YMin, pt.Y);
                YMax = Math.Max(YMax, pt.Y);
            }
        }
        public Rect(double x1, double x2, double y1, double y2)
        {
            XMin = x1; XMax = x2;
            YMin = y1; YMax = y2;
        }

        public bool Inside(double x, double y) => XMin <= x && x <= XMax && YMin <= y && y <= YMax;
        public bool Inside(Vector2 v) => Inside(v.X,v.Y);
    }
}
