using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class Utils
    {
        public static double Lerp(this double v, double min, double max) => (v - min) / (max - min);
    }
}
