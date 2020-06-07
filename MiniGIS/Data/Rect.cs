using MiniGIS.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Data
{
    public class Rect
    {
        public double xMin, xMax, yMin, yMax;
        public Rect(IEnumerable<GeomPoint> points)
        {
            xMin = double.MaxValue; xMax = double.MinValue;
            yMin = double.MaxValue; yMax = double.MinValue;
            foreach (GeomPoint pt in points)
            {
                xMin = Math.Min(xMin, pt.X);
                xMax = Math.Max(xMax, pt.X);
                yMin = Math.Min(yMin, pt.Y);
                yMax = Math.Max(yMax, pt.Y);
            }
        }
        public Rect(double x1, double x2, double y1, double y2)
        {
            xMin = x1; xMax = x2;
            yMin = y1; yMax = y2;
        }

        public bool Inside(double x, double y) => xMin <= x && x <= xMax && yMin <= y && y <= yMax;
        public bool Inside(Vector2 v) => Inside(v.X,v.Y);
    }
}
