using MiniGIS.Data;
using MiniGIS.Render;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class GenDelaunay
    {
        // 计算生成点集凸包
        public static List<GeomPoint> GetConvex(List<GeomPoint> points)
        {
            // 获取基准点并排序
            GeomPoint bp = null;
            foreach (GeomPoint p in points) if (bp == null || p.Y < bp.Y || (p.Y == bp.Y && p.X < bp.X)) bp = p;
            List<GeomPoint> ordered = new List<GeomPoint>(points);
            ordered.Sort((GeomPoint a, GeomPoint b) =>
            {
                if (a == bp) return -1;
                if (b == bp) return 1;
                return Math.Sign(new Vector2(bp, a).Rotation() - new Vector2(bp, b).Rotation());
            });

            List<GeomPoint> stack = new List<GeomPoint>(); // 凸包栈
            foreach (GeomPoint p in ordered)
            {
                stack.Add(p); // 按顺序入栈
                while (stack.Count >= 3) // 判断角度出栈
                {
                    GeomPoint pbase = stack[stack.Count - 3];
                    GeomPoint pnear = stack[stack.Count - 2];
                    GeomPoint pfar = stack[stack.Count - 1];
                    if (Triangle.CheckCross(pbase, pfar, pnear) >= 0)
                        stack.RemoveAt(stack.Count - 2); // 移除凹点
                    else break; // 完成查找
                }
            }

            return stack;
        }

        // 在大三角形内按X坐标增加顺序逐点扫描并创建TIN
        // 返回TIN图层
        public static GeomLayer DelaunayScan(List<GeomPoint> points)
        {
            // 创建字典用于反查
            var pt_mapper = new Dictionary<Vector2, GeomPoint>();
            foreach (GeomPoint tmp in points) pt_mapper[tmp] = tmp;

            // 确定扫描顺序
            List<Vector2> scanner = new List<Vector2>(from p in points select (Vector2)p); // 复制为新点
            scanner.Sort();

            // 创建初始三角
            List<Triangle> unknown = new List<Triangle>(); // 待定三角
            List<Triangle> targets = new List<Triangle>(); // 扫描完成的Delaunay三角
            List<Vector2> scanned_points = new List<Vector2>(); // 已扫描的点
            Rect MBR = new Rect(points);
            double ymin = (MBR.yMin * 10 - MBR.yMax) / 9;
            Triangle init = new Triangle(
                new Vector2((MBR.xMin + MBR.xMax) / 2, MBR.yMax * 2 - MBR.yMin),
                new Vector2((MBR.xMin * 3.2 - MBR.xMax) / 2.2, ymin),
                new Vector2((MBR.xMax * 3.2 - MBR.xMin) / 2.2, ymin)
            );
            unknown.Add(init);

            // 扫描所有点
            for (int idx = 0; idx < scanner.Count; idx++)
            {
                var edges = new HashSet<Tuple<Vector2, Vector2>>(); // 仅出现1次的散边
                var edges_multi = new HashSet<Tuple<Vector2, Vector2>>(); // 多次出现的散边
                Vector2 cur_point = scanner[idx]; // 当前点
                List<Triangle> newUnknown = new List<Triangle>(); // 下轮迭代三角

                // 扫描当前待定三角
                foreach (Triangle tri in unknown)
                {
                    // 已扫描，确定为Delaunay
                    if (tri.center.X + tri.radius < cur_point.X)
                    {
                        if (tri.Inside(MBR))
                            targets.Add(tri);
                    }

                    // 在圆内，分解为组合边
                    else if (tri.center.Distance(cur_point) <= tri.radius)
                    {
                        foreach (var tmp in tri.Edges())
                        {
                            if (edges.Contains(tmp)) edges.Remove(tmp); // 记录为重复
                            else edges.Add(tmp); // 添加新边
                        }
                    }

                    // 否则保留
                    else newUnknown.Add(tri);
                }

                // 生成新待定三角
                foreach (var tmp in edges)
                {
                    Triangle tri = new Triangle(tmp.Item1, tmp.Item2, cur_point);
                    newUnknown.Add(tri);
                }
                unknown = newUnknown;
            }

            // 将待定三角合并至已确定集合
            foreach (Triangle tri in unknown)
                if (tri.Inside(MBR)) targets.Add(tri);

            // 构造矢量图层
            GeomLayer result = new GeomLayer(GeomType.Polygon);
            var g_points = new Dictionary<Vector2, GeomPoint>();
            var g_arcs = new Dictionary<Tuple<Vector2, Vector2>, GeomArc>();
            int point_cnt = 0, arc_cnt = 0, poly_cnt = 0;
            foreach (Triangle tri in targets)
            {
                // 按需创建点
                foreach (Vector2 tmp in tri.Points())
                {
                    if (!g_points.ContainsKey(tmp))
                    {
                        var new_point = pt_mapper[tmp].Copy(); // 反查原始数据
                        new_point.id = ++point_cnt;
                        g_points[tmp] = new_point;
                    }
                }

                // 去重获取三边
                var arcs = new List<GeomArc>();
                foreach (var tmp in tri.Edges())
                {
                    GeomArc cur_arc;
                    if (g_arcs.ContainsKey(tmp)) cur_arc = g_arcs[tmp];
                    else
                    {
                        cur_arc = new GeomArc(new GeomPoint[] { g_points[tmp.Item1], g_points[tmp.Item2] }, ++arc_cnt);
                        var tmp_rev = new Tuple<Vector2, Vector2>(tmp.Item2, tmp.Item1);
                        g_arcs[tmp] = g_arcs[tmp_rev] = cur_arc;
                    }
                    arcs.Add(cur_arc);
                }

                // 创建三角形
                GeomPoly tri_poly = new GeomPoly(arcs, ++poly_cnt);
                result.polygons.Add(tri_poly);
            }

            // 加入点集、边集
            result.points.AddRange(g_points.Values);
            result.arcs.AddRange(g_arcs.Values);

            return result;
        }

        #region helpers
        public static bool HasEdge(HashSet<Tuple<Vector2, Vector2>> pool, Vector2 a, Vector2 b)
        {
            if (pool.Contains(new Tuple<Vector2, Vector2>(a, b))) return true;
            if (pool.Contains(new Tuple<Vector2, Vector2>(b, a))) return true;
            return false;
        }
        #endregion
    }

    // 包含三点的三角组合，默认按逆时针序
    class Triangle
    {
        public Vector2 p1, p2, p3;
        public Vector2 center;
        public double radius;

        public Triangle(Vector2 pt1, Vector2 pt2, Vector2 pt3)
        {
            p1 = pt1; p2 = pt2; p3 = pt3;

            // 计算外接圆圆心、半径
            {
                Vector2 dp2 = p2 - p1, dp3 = p3 - p1;// 减小计算误差
                double dom = Utils.CalcDeterminant(0, 0, 1,
                                                   dp2.X, dp2.Y, 1,
                                                   dp3.X, dp3.Y, 1);
                if (Math.Abs(dom) < Utils.EPSILON) // 分母为0
                {
                    center = p1;
                    radius = double.MaxValue;
                    return;
                }
                double l2 = dp2.LengthSq(), l3 = dp3.LengthSq();
                double dx = Utils.CalcDeterminant(0, 0, 1,
                                                  l2, dp2.Y, 1,
                                                  l3, dp3.Y, 1) / 2;
                double dy = Utils.CalcDeterminant(0, 0, 1,
                                                  dp2.X, l2, 1,
                                                  dp3.X, l3, 1) / 2;
                Vector2 dcenter = new Vector2(dx / dom, dy / dom);
                center = p1 + dcenter;
                radius = dcenter.Length();
            }
        }

        // 迭代器
        public IEnumerable<Vector2> Points()
        {
            yield return p1;
            yield return p2;
            yield return p3;
        }
        public IEnumerable<Tuple<Vector2, Vector2>> Edges() // 固定返回(小点, 大点)
        {
            yield return edge_helper(p1, p2);
            yield return edge_helper(p1, p3);
            yield return edge_helper(p2, p3);
        }
        Tuple<Vector2, Vector2> edge_helper(Vector2 p1, Vector2 p2) => (p1 < p2) ? new Tuple<Vector2, Vector2>(p1, p2) : new Tuple<Vector2, Vector2>(p2, p1);

        // 辅助检查点朝向
        public static double CheckCross(Vector2 p1, Vector2 p2, Vector2 p3) => (p1 - p2).Cross(p1 - p3);

        // 检查三角是否位于矩形内
        public bool Inside(Rect MBR) => (MBR.Inside(p1) && MBR.Inside(p2) && MBR.Inside(p3));
    }

    // key为两点的字典，用于按边去重
    class EdgeSet<T> : IEnumerable<T>
    {
        Dictionary<Tuple<Vector2, Vector2>, T> inner;

        // 迭代器
        IEnumerable<Tuple<Vector2, Vector2>> Keys() => inner.Keys;
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => inner.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => inner.Values.GetEnumerator();

        public T this[Vector2 p1, Vector2 p2]
        {
            get
            {
                var key = new Tuple<Vector2, Vector2>(p1, p2);
                if (inner.ContainsKey(key)) return inner[key];
                key = new Tuple<Vector2, Vector2>(p2, p1);
                if (inner.ContainsKey(key)) return inner[key];
                return default(T);
            }
            set
            {
                var key = new Tuple<Vector2, Vector2>(p1, p2);
                if (inner.ContainsKey(key)) inner[key] = value;
                else inner[new Tuple<Vector2, Vector2>(p2, p1)] = value;
            }
        }

        public EdgeSet()
        {
            inner = new Dictionary<Tuple<Vector2, Vector2>, T>();
        }
    }
}
