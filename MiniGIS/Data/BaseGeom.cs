using System;
using System.Linq;
using System.Text;
using System.Drawing;
using MiniGIS.Render;

namespace MiniGIS.Data
{
    // 几何体基类
    public class BaseGeom
    {
        public int id;
        public string name;
        public double value;

        public virtual void Render(ViewPort port, Graphics canvas, Pen pen) { }

        public BaseGeom(int _id, double _value)
        {
            id = _id;
            value = _value;
        }
        public BaseGeom() { }
    }

    public enum GeomType
    {
        Null = 0,
        Point = 1,
        Arc = 2,
        Polygon = 3,
        Grid = -1,
    }
}
