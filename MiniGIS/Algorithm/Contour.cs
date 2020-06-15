using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class Contour
    {
        #region globals

        // 当前转化对象
        static double currValue;
        static Grid currGrid;

        // 最大横纵下标
        static int nx, ny;

        // 记录插值位置数组
        static double[,] splitV, splitH;

        // 已追踪等值线
        static List<Vector2> trackedPoints;

        // 转化等值线为矢量图形
        static int idxPoint, idxArc;
        static List<GeomPoint> geomPoints;
        static List<GeomArc> geomArcs;

        #endregion

        public static void GenContourGrid(Grid grid, IEnumerable<double> values, out List<GeomPoint> cPoints, out List<GeomArc> cArcs)
        {
            // 初始化全局变量
            currGrid = grid;
            nx = (int)grid.XSplit; ny = (int)grid.YSplit;
            splitV = new double[nx + 1, ny];
            splitH = new double[nx, ny + 1];

            idxPoint = idxArc = 1;
            geomPoints = new List<GeomPoint>();
            geomArcs = new List<GeomArc>();

            // 逐数值查找
            foreach (double val in values)
            {
                currValue = val;

                // 计算位置数组
                for (int i = 0; i < nx; i++) // 横向 --
                    for (int j = 0; j <= ny; j++)
                        splitH[i, j] = ContourLerp(val, grid[i, j], grid[i + 1, j]);
                for (int i = 0; i <= nx; i++) // 纵向 |
                    for (int j = 0; j < ny; j++)
                        splitV[i, j] = ContourLerp(val, grid[i, j], grid[i, j + 1]);

                // 初始化等值线数组
                trackedPoints = new List<Vector2>();

                // 搜索开放等值线
                for (int i = 0; i < nx; i++)
                {
                    TrackContour(i, 0, 1); // j++
                    TrackContour(i, ny - 1, 3); // j--
                }
                for (int j = 0; j < ny; j++)
                {
                    TrackContour(0, j, 0); // i++
                    TrackContour(nx - 1, j, 2); // i--
                }

                // 搜索闭合等值线
                for (int i = 1; i < nx; i++)
                    for (int j = 0; j < ny; j++)
                        TrackContour(i, j, 0);
            }

            // 输出结果
            cPoints = geomPoints;
            cArcs = geomArcs;
        }

        #region helper

        // 计算内插位置+排除奇点
        static double ContourLerp(double value, double start, double end)
        {
            // 插值点相等情况
            if (start == end) return value == start ? 0.5 : double.NaN;

            // 计算内插位置
            double res = (value - start) / (end - start);
            if (res < 0 || res > 1) return double.NaN;
            else if (res == 0) return 1e-6;
            else if (res == 1) return 1 - 1e-6;

            return res;
        }

        // 各方向对应偏移量
        // 搜索方向: 0-i++; 1-j++; 2-i--; 3-j--
        static readonly int[] offsetI = new int[] { 0, 0, 1, 0 }; // 数组下标
        static readonly int[] offsetJ = new int[] { 0, 0, 0, 1 };
        static readonly int[] nextI = new int[] { 1, 0, -1, 0 }; // 递归前进方向
        static readonly int[] nextJ = new int[] { 0, 1, 0, -1 };

        // 网格递归搜索等值线
        static void TrackContour(int i, int j, int direction, Vector2 ptA = null)
        {
            // 获取当前位置插值
            if (ptA == null) ptA = TrackContourStart(i, j, direction);
            if (ptA == null) return; // 该位置不可起始
            else // 已追踪序列加入当前点
            {
                trackedPoints.Add(ptA);
                TrackContourClear(i, j, direction);
            }

            // 获取最近坐标作为下一延伸方向
            int newDir = -1;
            Vector2 ptB = null;
            for (int di = 1; di < 4; di++)
            {
                int currDir = (direction + di) % 4;
                Vector2 currB = TrackContourStart(i, j, currDir);
                if (currB == null) continue;
                if (ptB == null || ptA.DistanceSq(ptB) > ptA.DistanceSq(currB))
                {
                    ptB = currB;
                    newDir = (currDir + 2) % 4; // 翻转方向
                }
            }

            // 递归搜索
            if (ptB == null) DumpContourGrid();
            else TrackContour(i + nextI[newDir], j + nextJ[newDir], newDir, ptB);
        }

        // 网格等值线起点
        static Vector2 TrackContourStart(int i, int j, int direction)
        {
            int newI = i + offsetI[direction],
                newJ = j + offsetJ[direction];

            // 越界判断
            int maxI = nx, maxJ = ny;
            if (direction % 2 == 0) maxI++;
            else maxJ++;
            if (!(0 <= newI && newI < maxI &&
                0 <= newJ && newJ < maxJ)) return null;

            // 获取当前插值
            double offset = (direction % 2 == 0 ? splitV : splitH)[newI, newJ];
            if (double.IsNaN(offset)) return null; // 当前内插不存在

            // 返回点坐标
            if (direction % 2 == 0) return new Vector2(newI, newJ + offset);
            return new Vector2(newI + offset, newJ);
        }

        // 清除网格指定位置偏移
        static void TrackContourClear(int i, int j, int direction)
        {
            int newI = i + offsetI[direction],
                newJ = j + offsetJ[direction];
            (direction % 2 == 0 ? splitV : splitH)[newI, newJ] = double.NaN;
        }

        // 导出并重置当前已搜索等值线
        static void DumpContourGrid()
        {
            List<GeomPoint> newPoints = new List<GeomPoint>();
            foreach (Vector2 offset in trackedPoints)
            {
                GeomPoint newPt = new GeomPoint(
                    offset.X.Lerp(0, nx, currGrid.XMin, currGrid.XMax),
                    offset.Y.Lerp(0, ny, currGrid.YMin, currGrid.YMax),
                    idxPoint++,
                    currValue
                );
                geomPoints.Add(newPt);
                newPoints.Add(newPt);
            }
            // 内部点判断
            if (newPoints[0].X != currGrid.XMin && newPoints[0].X != currGrid.XMax &&
                newPoints[0].Y != currGrid.YMin && newPoints[0].Y != currGrid.YMax) newPoints.Add(newPoints[0]);
            geomArcs.Add(new GeomArc(newPoints, idxArc++, currValue));

            trackedPoints = new List<Vector2>();
        }

        #endregion
    }
}
