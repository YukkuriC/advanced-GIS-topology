using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class Utils
    {
        public const double EPSILON = 1e-8; // 浮点计算误差

        public static double Lerp(this double v, double min, double max) => (max == min) ? 0 : (v - min) / (max - min);
        public static double Lerp(this double v, double min, double max, double tmin, double tmax) => (tmax == tmin) ? tmin : tmin + (tmax - tmin) * v.Lerp(min, max);
        public static string SciString(this double val, uint digit = 6)
        {
            if (val < 0) return "-" + (-val).SciString(digit);
            string res;
            if (val == 0)
            {
                if (digit == 0) return "0";
                res = "0.";
                for (int i = 0; i < digit; i++) res += "0";
                return res;
            }
            int exp = (int)Math.Floor(Math.Log10(val));
            res = Math.Round(val / Math.Pow(10, exp), (int)digit).ToString();
            if (exp != 0) res += "e" + exp.ToString();
            return res;
        }

        // 行列式求值
        public static double CalcDeterminant(double a1, double b1, double c1,
                                             double a2, double b2, double c2,
                                             double a3, double b3, double c3)
        {
            return (a1 * b2 * c3) + (b1 * c2 * a3) + (c1 * a2 * b3) -
                   (a3 * b2 * c1) - (a2 * b1 * c3) - (a1 * b3 * c2);
        }
    }
}
