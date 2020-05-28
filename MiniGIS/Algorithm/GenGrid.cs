using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MiniGIS.Algorithm;
using MiniGIS.Data;

namespace MiniGIS.Algorithm
{
    public static class GenGrid
    {
        static int gsplit = 3;// 方位加权各象限划分数

        #region methods

        // 距离倒数
        static double Z_DistRev(IEnumerable<GeomPoint> points, double x, double y)
        {
            double sum = 0, weight = 0;
            foreach (GeomPoint p in points)
            {
                // 点重合
                if (x == p.X && y == p.Y) return p.value;

                double w = 1 / p.Distance(x, y);
                sum += p.value * w;
                weight += w;
            }

            // 求均值
            return sum / weight;
        }

        // 距离平方倒数
        static double Z_DistPow2Rev(IEnumerable<GeomPoint> points, double x, double y)
        {
            double sum = 0, weight = 0;
            foreach (GeomPoint p in points)
            {
                // 点重合
                if (x == p.X && y == p.Y) return p.value;

                double w = 1 / p.DistanceSq(x, y);
                sum += p.value * w;
                weight += w;
            }

            // 求均值
            return sum / weight;
        }

        // 方位加权均值
        static double Z_DirGrouped(IEnumerable<GeomPoint> points, double x, double y)
        {
            double sum = 0, weight = 0, tmp;

            // 寻找最近点
            Dictionary<int, Tuple<GeomPoint, double>> nearestPoints = new Dictionary<int, Tuple<GeomPoint, double>>();
            foreach (GeomPoint p in points)
            {
                // 点重合
                if (x == p.X && y == p.Y) return p.value;

                // 分组取最近点
                int group = (int)(Math.Atan2(p.Y - y, p.X - x) * 2 * gsplit / Math.PI);// 取值范围: [0, 4*gsplit)
                tmp = p.DistanceSq(x, y);
                if (!nearestPoints.ContainsKey(group) || nearestPoints[group].Item2 > tmp) nearestPoints[group] = new Tuple<GeomPoint, double>(p, tmp);
            }
            List<Tuple<GeomPoint, double>> selectedPoints = new List<Tuple<GeomPoint, double>>(nearestPoints.Values);

            // 加权求均值
            double allMult = 1;
            foreach (var pair in selectedPoints) allMult *= pair.Item2;
            foreach (var pair in selectedPoints)
            {
                tmp = allMult / pair.Item2;
                sum += pair.Item1.value * tmp;
                weight += tmp;
            }
            return sum / weight;
        }

        #endregion

        // 格网逐点迭代基础方法
        static void GenGrid_Base(Grid grid, IEnumerable<GeomPoint> points, Func<IEnumerable<GeomPoint>, double, double, double> method)
        {
            for (int i = 0; i <= grid.XSplit; i++)
            {
                double x = grid.XCoord(i);
                for (int j = 0; j <= grid.YSplit; j++)
                {
                    double y = grid.YCoord(j);

                    // 逐点求值
                    grid[i, j] = method(points, x, y);
                }
            }
        }

        // 各方法接口
        public static void GenGrid_DistRev(this Grid grid, IEnumerable<GeomPoint> points) => GenGrid_Base(grid, points, Z_DistRev);// 距离倒数
        public static void GenGrid_DistPow2Rev(this Grid grid, IEnumerable<GeomPoint> points) => GenGrid_Base(grid, points, Z_DistPow2Rev);// 距离平方倒数
        public static void GenGrid_DirGrouped(this Grid grid, IEnumerable<GeomPoint> points) => GenGrid_Base(grid, points, Z_DirGrouped);// 方位加权
    }
}
