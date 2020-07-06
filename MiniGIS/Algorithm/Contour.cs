using MiniGIS.Data;
using MiniGIS.Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class Contour
    {
        #region globals

        static double currValue;

        #region grid
        // 当前转化对象
        static Grid currGrid;
        // 最大横纵下标
        static int nx, ny;
        // 记录插值位置数组
        static double[,] splitV, splitH;
        // 已追踪等值线
        static List<Vector2> trackedPoints;
        #endregion

        #region TIN
        // 当前转化对象
        static Dictionary<Edge, HashSet<Triangle>> currEdges;
        // 边内插位置
        static Dictionary<Edge, double> splitEdges;
        // 起始搜索边
        static HashSet<Edge> edgeOuter, edgeInner;
        // 已追踪等值线
        static List<Tuple<Edge, double>> trackedEdges;
        #endregion

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

        public static void GenContourTIN(
            Dictionary<Edge, HashSet<Triangle>> edges, Dictionary<Vector2, double> triValues,
            IEnumerable<double> values, out List<GeomPoint> cPoints, out List<GeomArc> cArcs)
        {
            // 初始化全局变量
            currEdges = edges;
            splitEdges = new Dictionary<Edge, double>();
            edgeOuter = new HashSet<Edge>();
            edgeInner = new HashSet<Edge>();
            geomPoints = new List<GeomPoint>();
            geomArcs = new List<GeomArc>();

            // 查找边内外关系
            foreach (var edge_tri in edges)
            {
                var edge = edge_tri.Key; var tri = edge_tri.Value;
                if (edge.Ordered()) continue; // 每边只计算一次
                (tri.Count == 1 ? edgeOuter : edgeInner).Add(edge); // 外层每边连接1个三角；内层2个
            }

            // 逐数值查找
            foreach (double val in values)
            {
                currValue = val;

                // 迭代计算所有边插值
                foreach (var edge in edges.Keys)
                {
                    if (edge.Ordered()) continue; // 每边只计算一次
                    splitEdges[edge] = ContourLerp(val, triValues[edge.Item1], triValues[edge.Item2]);
                }

                // 初始化追踪数组
                trackedEdges = new List<Tuple<Edge, double>>();

                // 递归追踪外部等值线
                foreach (var edge in edgeOuter) TrackContour(edge);

                // 递归追踪内部等值线
                foreach (var edge in edgeInner) TrackContour(edge);
            }

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
            else if (res == 0 || res == 1) res += 1e-6;

            return res;
        }

        #region grid
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

        #region TIN
        // TIN递归搜索等值线
        static void TrackContour(Edge edge, Triangle from = null)
        {
            // 获取当前位置插值
            if (edge.Ordered()) edge = edge.Reverse();
            double val = splitEdges[edge];
            if (double.IsNaN(val)) return;

            // 已追踪序列加入当前点
            trackedEdges.Add(new Tuple<Edge, double>(edge, val));
            splitEdges[edge] = double.NaN;

            // 查找下一三角形
            var tmp = currEdges[edge].ToArray();
            Triangle tri2 = tmp[0];
            if (tri2 == from)
            {
                if (tmp.Length == 1) // 追踪至边界
                {
                    DumpContourTIN();
                    return;
                }
                else tri2 = tmp[1];
            }

            // 递归搜索
            foreach (Edge e in tri2.Edges())
            {
                Edge nextEdge = e.Ordered() ? e.Reverse() : e;
                if (!double.IsNaN(splitEdges[nextEdge])) // 递归进入下一边
                {
                    TrackContour(nextEdge, tri2);
                    return;
                }
            }

            // 递归出口
            DumpContourTIN();
        }
        // TIN输出等值线
        static void DumpContourTIN()
        {
            List<GeomPoint> newPoints = new List<GeomPoint>();
            foreach (var edge_val in trackedEdges)
            {
                Edge edge = edge_val.Item1; double val = edge_val.Item2;
                Vector2 pos = edge.Item1 * (1 - val) + edge.Item2 * val;
                GeomPoint newPt = new GeomPoint(pos.X, pos.Y, idxPoint++, currValue);
                geomPoints.Add(newPt);
                newPoints.Add(newPt);
            }

            // 成环判断
            var tmp = currEdges[trackedEdges[0].Item1].Intersect(currEdges[trackedEdges.Last().Item1]);
            if (tmp.Count() > 0) newPoints.Add(newPoints[0]);

            // 创建弧段并重置
            geomArcs.Add(new GeomArc(newPoints, idxArc++, currValue));
            trackedEdges = new List<Tuple<Edge, double>>();
        }
        #endregion

        #endregion
    }
}
