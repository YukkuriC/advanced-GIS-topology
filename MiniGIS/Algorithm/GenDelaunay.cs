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
        // 分割凸包生成初始三角
        // 通过递归调整生成TIN
        public static void DelaunayConvex(List<GeomPoint> points, out HashSet<Triangle> triangles, out Dictionary<Edge, HashSet<Triangle>> edgeSides)
        {
            // 获取基准点并排序
            Vector2 bp = null;
            foreach (GeomPoint p in points) if (bp == null || p.Y < bp.Y || (p.Y == bp.Y && p.X < bp.X)) bp = p;
            List<Vector2> ordered = new List<Vector2>(from p in points select (Vector2)p);
            ordered.Sort((Vector2 a, Vector2 b) =>
            {
                if (a == bp) return -1;
                if (b == bp) return 1;
                return Math.Sign((a - bp).Rotation() - (b - bp).Rotation());
            });

            // 旋转扫描获得凸包
            List<Vector2> stack = new List<Vector2>(); // 凸包栈
            List<Vector2> pointsLeft = new List<Vector2>(); // 凸包未包含的坐标
            foreach (Vector2 p in ordered)
            {
                stack.Add(p); // 按顺序入栈
                while (stack.Count >= 3) // 判断角度出栈
                {
                    Vector2 pbase = stack[stack.Count - 3],
                            pnear = stack[stack.Count - 2],
                            pfar = stack[stack.Count - 1];
                    if (Triangle.CheckCross(pbase, pfar, pnear) >= 0)// 移除凹点
                    {
                        pointsLeft.Add(pnear);
                        stack.RemoveAt(stack.Count - 2);
                    }
                    else break; // 完成查找
                }
            }

            // 凸包划分为三角
            triangles = new HashSet<Triangle>();
            //for (int i = 2; i < stack.Count; i++) firstpass.Add(new Triangle(stack[0], stack[i - 1], stack[i]));
            var next = new Dictionary<Vector2, Vector2>();
            for (int i = 0; i < stack.Count; i++) next[stack[i]] = stack[(i + 1) % stack.Count]; // 创建循环链表
            Vector2 p1 = stack[0], p2 = stack[1];
            for (int curLen = stack.Count; curLen > 3; curLen--)
            {
                // 搜索最小外角
                Vector2 mp1 = p1; double mcos = (p2 - p1).CosAngle(next[p2] - p2);
                for (int i = 1; i < curLen; i++)
                {
                    p1 = p2; p2 = next[p2];
                    double ncos = (p2 - p1).CosAngle(next[p2] - p2);
                    if (ncos < mcos) // 转向角尽可能大
                    {
                        mcos = ncos;
                        mp1 = p1;
                    }
                }

                // 分离对应三角
                p1 = mp1; p2 = next[next[mp1]];
                triangles.Add(new Triangle(p1, next[p1], p2));
                next[p1] = p2;
            }
            triangles.Add(new Triangle(p1, p2, next[p2])); // 最后3点

            // 剩余点插入三角形
            foreach (var pos in pointsLeft)
            {
                foreach (var tri in triangles)
                {
                    int containStat;
                    switch (containStat = tri.Contains(pos))
                    {
                        case 0: continue; // 外部点，三角保留
                        case -1: // 内部
                            foreach (var border in tri.Edges())
                                triangles.Add(new Triangle(border.Item1, border.Item2, pos));
                            break;
                        default: // 边上
                            var pts = tri.Points().ToArray();
                            triangles.Add(new Triangle(pts[containStat], pts[(containStat + 2) % 3], pos));
                            triangles.Add(new Triangle(pts[(containStat + 1) % 3], pts[(containStat + 2) % 3], pos));
                            break;
                    }

                    // 其余情况完成搜索
                    triangles.Remove(tri);
                    break;
                }
            }

            // 创建边邻接字典
            edgeSides = new Dictionary<Edge, HashSet<Triangle>>();
            foreach (var tri in triangles) tri.AddToPool(edgeSides, null);

            // 逐边检查Delaunay合法性
            var initEdges = edgeSides.Keys.ToArray();
            foreach (var edge in initEdges) DelaunayCheck(edge, edgeSides, triangles);
        }

        // 在大三角形内按X坐标增加顺序逐点扫描并创建TIN
        // **生成结果非凸包，未使用**
        public static GeomLayer DelaunayScan(List<GeomPoint> points)
        {
            // 确定扫描顺序
            List<Vector2> scanner = new List<Vector2>(from p in points select (Vector2)p); // 复制为新点
            scanner.Sort();

            // 创建初始三角
            List<Triangle> unknown = new List<Triangle>(); // 待定三角
            List<Triangle> targets = new List<Triangle>(); // 扫描完成的Delaunay三角
            List<Vector2> scanned_points = new List<Vector2>(); // 已扫描的点
            Rect MBR = new Rect(points);
            double ymin = (MBR.YMin * 10 - MBR.YMax) / 9;
            Triangle init = new Triangle(
                new Vector2((MBR.XMin + MBR.XMax) / 2, MBR.YMax * 2 - MBR.YMin),
                new Vector2((MBR.XMin * 3.2 - MBR.XMax) / 2.2, ymin),
                new Vector2((MBR.XMax * 3.2 - MBR.XMin) / 2.2, ymin)
            );
            unknown.Add(init);

            // 扫描所有点
            for (int idx = 0; idx < scanner.Count; idx++)
            {
                var edges = new HashSet<Edge>(); // 仅出现1次的散边
                var edges_multi = new HashSet<Edge>(); // 多次出现的散边
                Vector2 cur_point = scanner[idx]; // 当前点
                List<Triangle> newUnknown = new List<Triangle>(); // 下轮迭代三角

                // 扫描当前待定三角
                foreach (Triangle tri in unknown)
                {
                    // 计算外接圆
                    tri.CalcCircum();

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

            // 创建图层
            return FromTriangles(targets, points);
        }

        #region helpers

        // 添加三角至当前集合
        static void AddToPool(this Triangle tri, Dictionary<Edge, HashSet<Triangle>> edgeSides, HashSet<Triangle> allTriangles)
        {
            // 各边添加
            foreach (var edge in tri.Edges())
            {
                // 获取或创建边集合
                HashSet<Triangle> edgeGroup;
                if (edgeSides.ContainsKey(edge)) edgeGroup = edgeSides[edge];
                else
                {
                    edgeGroup = new HashSet<Triangle>();
                    edgeSides[edge] = edgeSides[edge.Reverse()] = edgeGroup;
                }

                // 添加三角
                edgeGroup.Add(tri);
            }

            // 添加至总集合
            if (allTriangles != null) allTriangles.Add(tri);
        }

        // 递归检查Delaunay合法性
        static void DelaunayCheck(Edge edge, Dictionary<Edge, HashSet<Triangle>> edgeSides, HashSet<Triangle> allTriangles)
        {
            // 判断边是否合法
            if (!edgeSides.ContainsKey(edge)) return;
            var triangles = edgeSides[edge].ToArray();
            if (triangles.Length != 2) return;

            // 获取待判断各点
            Vector2 pa = edge.Item1, pb = edge.Item2;
            Vector2 pc = null, pd = null;
            foreach (var v in triangles[0].Points()) if (v != pa && v != pb) { pc = v; break; }
            foreach (var v in triangles[1].Points()) if (v != pa && v != pb) { pd = v; break; }

            // 判断pc、pd对角和是否大于180度
            // cos(C)+cos(D)<0
            if ((pc - pa).CosAngle(pc - pb) + (pd - pa).CosAngle(pd - pb) < 0)
            {
                // 移除现有三角
                foreach (var tri in triangles)
                {
                    allTriangles.Remove(tri);
                    foreach (var triEdge in tri.Edges()) edgeSides[triEdge].Remove(tri);
                }
                edgeSides.Remove(edge);
                edgeSides.Remove(edge.Reverse()); // 移除双向引用

                // 创建新三角
                new Triangle(pc, pd, pa).AddToPool(edgeSides, allTriangles);
                new Triangle(pc, pd, pb).AddToPool(edgeSides, allTriangles);

                // 递归检查四边形
                DelaunayCheck(new Edge(pa, pc), edgeSides, allTriangles);
                DelaunayCheck(new Edge(pa, pd), edgeSides, allTriangles);
                DelaunayCheck(new Edge(pb, pc), edgeSides, allTriangles);
                DelaunayCheck(new Edge(pb, pd), edgeSides, allTriangles);
            }
        }

        // 从三角序列创建矢量图层
        static GeomLayer FromTriangles(IEnumerable<Triangle> triangles, List<GeomPoint> originalPoints)
        {
            // 创建字典用于反查
            var pt_mapper = new Dictionary<Vector2, GeomPoint>();
            foreach (GeomPoint tmp in originalPoints) pt_mapper[tmp] = tmp;

            // 构造矢量图层
            GeomLayer result = new GeomLayer(GeomType.Polygon);
            var g_points = new Dictionary<Vector2, GeomPoint>();
            var g_arcs = new Dictionary<Edge, GeomArc>();
            int point_cnt = 0, arc_cnt = 0, poly_cnt = 0;
            foreach (Triangle tri in triangles)
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
                        g_arcs[tmp] = g_arcs[tmp.Reverse()] = cur_arc;
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

        #endregion
    }
}
