using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MiniGIS.Algorithm;
using MiniGIS.Data;

namespace MiniGIS.Layer
{
    /// <summary>
    /// 栅格渲染层
    /// 颜色参数:
    ///     low: 低值颜色
    ///     high: 高值颜色
    ///     grid: 网格线颜色
    /// 尺寸参数:
    ///     low: 低值
    ///     high: 高值
    ///     grid: 网格边缘粗细
    /// 可见性参数:
    ///     border: 网格边缘
    ///     fill: 网格上色
    /// </summary>
    public class GridLayer : ValueLayer
    {
        public override string layerType
        {
            get
            {
                if (data != null) return String.Format("{0}x{1}栅格", data.XSplit, data.YSplit);
                return "栅格";
            }
        }
        public Grid data;

        public override double Max { get => data.Max; }
        public override double Min { get => data.Min; }

        public override void Render(ViewPort port, Graphics canvas)
        {
            double xstep = (data.XMax - data.XMin) / data.XSplit,
                  ystep = (data.YMax - data.YMin) / data.YSplit;
            double cx, cy, x1, x2, y1, y2;
            PointF p1, p2;
            // 栅格填色
            if (GetPartVisible("fill"))
            {
                float valmin = GetSize("low"), valmax = GetSize("high");
                Color cmin = GetColor("low"), cmax = GetColor("high");
                for (int i = 0; i <= data.XSplit; i++)
                {
                    x1 = x2 = cx = data.XMin + i * xstep;
                    if (i > 0) x1 -= xstep / 2;
                    if (i < data.XSplit) x2 += xstep / 2;
                    for (int j = 0; j <= data.YSplit; j++)
                    {
                        y1 = y2 = cy = data.YMin + j * ystep;
                        if (j > 0) y1 -= ystep / 2;
                        if (j < data.YSplit) y2 += ystep / 2;
                        p1 = MainForm.port.ScreenCoord(x1, y2); p2 = MainForm.port.ScreenCoord(x2, y1);
                        Color clr = ColorOps.Linear(cmin, cmax, data[i, j].Lerp(valmin, valmax));
                        canvas.FillRectangle(new SolidBrush(clr), new RectangleF(p1, new SizeF(p2.X - p1.X, p2.Y - p1.Y)));
                    }
                }
            }
            // 绘制边界
            if (GetPartVisible("border") && GetSize("grid") > 0)
            {
                Pen pen = new Pen(GetColor("grid"), GetSize("grid"));
                pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                p1 = MainForm.port.ScreenCoord(data.XMin, data.YMax); p2 = MainForm.port.ScreenCoord(data.XMax, data.YMin);
                for (int i = 0; i <= data.XSplit || i <= data.YSplit; i++)
                {
                    PointF pt = MainForm.port.ScreenCoord(
                        data.XMin + i * xstep,
                        data.YMin + i * ystep
                    );
                    if (i <= data.XSplit) canvas.DrawLine(pen, pt.X, p1.Y, pt.X, p2.Y);
                    if (i <= data.YSplit) canvas.DrawLine(pen, p1.X, pt.Y, p2.X, pt.Y);
                }
            }
        }

        public override void Focus(ViewPort port) => port.Focus(data);

        public GridLayer(Grid _data, string name = "栅格图层") : base(name)
        {
            data = _data;
            sizes["low"] = (float)data.Min;
            sizes["high"] = (float)data.Max;
        }
    }
}
