using System;
using System.Drawing;

namespace MiniGIS.Render
{
    public static class ColorOps
    {
        static Random rnd;

        #region method

        // 获取随机颜色
        public static Color Random()
        {
            return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }

        #endregion

        // 初始化
        public static void Init(int seed = 42)
        {
            rnd = new Random(seed);
        }
    }
}
