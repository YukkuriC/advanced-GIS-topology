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

        public static Rect operator |(Rect a, Rect b)
        {
            if (a == null) return b;
            if (b == null) return a;
            return new Rect(Math.Min(a.XMin, b.XMin), Math.Max(a.XMax, b.XMax), Math.Min(a.YMin, b.YMin), Math.Max(a.YMax, b.YMax));
        }

        public static Rect operator &(Rect a, Rect b)
        {
            if (a == null || b == null) return null;
            double
                x1 = Math.Max(a.XMin, b.XMin),
                x2 = Math.Min(a.XMax, b.XMax),
                y1 = Math.Max(a.YMin, b.YMin),
                y2 = Math.Min(a.YMax, b.YMax);
            if (x1 > x2 || y1 > y2) return null;
            return new Rect(x1, x2, y1, y2);
        }

        public bool Include(double x, double y) => XMin <= x && x <= XMax && YMin <= y && y <= YMax;
        public bool Include(Vector2 v) => Include(v.X, v.Y);
        public bool Include(Rect r) => XMin <= r.XMin && r.XMax <= XMax && YMin <= r.YMin && r.YMax <= YMax;
    }
}
