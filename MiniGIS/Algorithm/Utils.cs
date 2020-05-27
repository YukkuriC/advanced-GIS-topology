using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class Utils
    {
        public static double Lerp(this double v, double min, double max) => (v - min) / (max - min);
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
    }
}
