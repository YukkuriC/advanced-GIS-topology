using System;
using System.Drawing;

namespace MiniGIS.Render
{
    public static class ColorOps
    {
        static Random rnd;

        #region method

        // 随机初始化
        public static void InitRandom(int seed = 42)
        {
            rnd = new Random(seed);
        }

        // 获取随机颜色
        public static Color Random()
        {
            return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }

        // 线性颜色插值
        public static Color Linear(Color c1, Color c2, double v)
        {
            v = Math.Min(1, Math.Max(0, v));
            return Color.FromArgb(
                c1.R + (int)((c2.R - c1.R) * v),
                c1.G + (int)((c2.G - c1.G) * v),
                c1.B + (int)((c2.B - c1.B) * v)
            );
        }

        #endregion
    }
}
