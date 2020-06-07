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

        // 继承访问接口
        public double X;
        public double Y;

        #endregion

        #region method

        // 计算距离
        public double DistanceSq(double x, double y) => Math.Pow(x - X, 2) + Math.Pow(y - Y, 2);
        public double Distance(double x, double y) => Math.Sqrt(DistanceSq(x, y));

        // 绘制接口
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
        }
        public GeomPoint Copy() => new GeomPoint(X, Y, id, value);

        // 字符串化
        public override string ToString()
        {
            return String.Format("#{0}({1},{2})={3}", id, X, Y, value);
        }
    }
}
