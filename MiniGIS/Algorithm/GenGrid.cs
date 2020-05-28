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
        static int knear = 10;// 计入均值的最近点的个数
        static int gsplit = 3;// 方位加权各象限划分数

        #region point to grid

        #region methods

        // 通用k邻近加权函数
        static double Z_KNearBase(IEnumerable<GeomPoint> points, double x, double y, string distFunc)
        {
            double tmp;

            // 创建有序表，取前k位
            object[] distInput = new object[] { x, y };
            SortedList<double, double> nearestPoints = new SortedList<double, double>();// 距离: 取值
            foreach (GeomPoint p in points)
            {
                // 点重合
                if (x == p.X && y == p.Y) return p.value;
                tmp = (double)typeof(GeomPoint).GetMethod(distFunc).Invoke(p, distInput);
                nearestPoints.Add(tmp, p.value);
            }

            // 计算加权平均
            double sum = 0, weight = 0;
            var pts = nearestPoints.ToList();
            int size = Math.Min(nearestPoints.Count, knear);
            for (int i = 0; i < size; i++)
            {
                var pair = pts[i];
                tmp = 1 / pair.Key;
                weight += tmp;
                sum += tmp * pair.Value;
            }
            return sum / weight;
        }
        static double Z_DistRev(IEnumerable<GeomPoint> points, double x, double y) => Z_KNearBase(points, x, y, "Distance");// 距离倒数
        static double Z_DistPow2Rev(IEnumerable<GeomPoint> points, double x, double y) => Z_KNearBase(points, x, y, "DistanceSq");// 距离平方倒数

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

        #endregion

        #region grid interpolation

        public static Grid LinearInterpolation(Grid origin, uint xstep, uint ystep)
        {
            Grid newGrid = new Grid(origin.XMin, origin.XMax, origin.YMin, origin.YMax, origin.XSplit * xstep, origin.YSplit * ystep);

            // 逐单元插值
            for(int oldI = 0; oldI < origin.XSplit; oldI++)
            {
                int newI = (int)(oldI * xstep);
                for (int oldJ = 0; oldJ < origin.YSplit; oldJ++)
                {
                    int newJ = (int)(oldJ * ystep);
                    for(int i = (oldI > 0) ? 1 : 0; i <= xstep; i++)
                    {
                        double vI = Utils.Lerp(i, 0, xstep, origin[oldI, oldJ], origin[oldI + 1, oldJ]);// 下方
                        double vII = Utils.Lerp(i, 0, xstep, origin[oldI, oldJ+1], origin[oldI + 1, oldJ+1]);// 上方
                        for(int j = (oldJ > 0) ? 1 : 0; j <= ystep; j++)
                        {
                            newGrid[newI + i, newJ + j] = Utils.Lerp(j, 0, ystep, vI, vII);
                        }
                    }
                }
            }

            return newGrid;
        }

        #endregion
    }
}
