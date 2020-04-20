using System;
using System.Collections.Generic;
using System.Drawing;
using MiniGIS.Render;

namespace MiniGIS.Data
{
    // 点几何体
    public class GeomPoint : BaseGeom
    {
        #region prop

        // 拓扑关系
        public HashSet<GeomArc> arcs;

        // 继承访问接口
        public double X;
        public double Y;

        #endregion

        #region method

        public override void Render(ViewPort port, Graphics canvas, Pen pen)
        {
            PointF pt = port.ScreenCoord(X, Y);
            canvas.DrawLine(pen, pt, new PointF(pt.X, pt.Y + 0.01f));
        }

        #endregion

        // 构造函数
        public GeomPoint(double _x, double _y, int _id = 0, double _value = 0) : base(_id, _value)
        {
            X = _x; Y = _y;
            arcs = new HashSet<GeomArc>();
        }

        // 字符串化
        public override string ToString()
        {
            return String.Format("#{0}({1},{2})={3}", id, X, Y, value);
        }
    }
}
