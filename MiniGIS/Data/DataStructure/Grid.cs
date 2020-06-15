using MiniGIS.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Data
{
    // 二维网格数据结构
    public class Grid : Rect
    {
        public uint XSplit, YSplit;
        public double[,] values;

        // 最值缓存系统
        double _max = double.NaN, _min = double.NaN;
        public double Max
        {
            get
            {
                if (double.IsNaN(_max)) CalcMaxMin();
                return _max;
            }
        }
        public double Min
        {
            get
            {
                if (double.IsNaN(_min)) CalcMaxMin();
                return _min;
            }
        }
        public void CalcMaxMin()
        {
            _max = double.MinValue; _min = double.MaxValue;
            foreach (double x in values)
            {
                _max = Math.Max(_max, x);
                _min = Math.Min(_min, x);
            }
        }
        public void ResetMinMax()
        {
            _max = _min = double.NaN;
        }

        // 重载下标运算符
        public double this[int i, int j]
        {
            get { return values[i, j]; }
            set { values[i, j] = value; ResetMinMax(); }
        }

        // 获取格点坐标
        public double XCoord(int i) => Utils.Lerp(i, 0, XSplit, XMin, XMax);
        public double YCoord(int j) => Utils.Lerp(j, 0, YSplit, YMin, YMax);

        public Grid(double xmin, double xmax, double ymin, double ymax, uint xsplit, uint ysplit) : base(xmin, xmax, ymin, ymax)
        {
            XMin = xmin; XMax = xmax; YMin = ymin; YMax = ymax;
            XSplit = xsplit; YSplit = ysplit;
            values = new double[xsplit + 1, ysplit + 1];
        }
    }
}
