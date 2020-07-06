using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class GenTopology
    {
        // 结果数据存储
        static Dictionary<Vector2, GeomPoint> cPoint;
        static List<GeomArc> cArc;
        static List<GeomPoly> cPoly;

        // 中间变量
        static Dictionary<LineSegment, SortedSet<double>> segSplit;
        static Dictionary<GeomPoint, SortedDictionary<double, GeomArc>> vertPool;
        static Dictionary<GeomPoint, Dictionary<GeomArc, GeomArc>> vertNext;
        static HashSet<GeomPoly> inners;
        static LinkedList<Tuple<GeomPoint, GeomArc>> stack;
        static HashSet<Tuple<GeomPoint, GeomArc>> arcUsed;
        static List<Tuple<GeomPoly, HashSet<GeomPoly>>> holesGroup;

        // 1. 合法化并分割输入弧段
        static void SplitArcs(List<GeomArc> origin, Rect MBR)
        {
            // 生成待分割弧段
            var rawArcs = new List<GeomArc>();
            foreach (var a in origin)
            {
                var tmp = Arc(a.points);
                if (tmp != null) rawArcs.Add(tmp);
            }
            rawArcs.AddRange(SplitMBR(MBR));

            // 构造数据结构：弧段->线段数组
            var arcSegs = new Dictionary<GeomArc, LineSegment[]>();
            foreach (var a in rawArcs) arcSegs[a] = a.IterSegments().ToArray();
            segSplit = new Dictionary<LineSegment, SortedSet<double>>();
            var arcMarker = new HashSet<GeomArc>();

            // 线段互相求交 O(N^2*L^2)
            for (int i = 0; i < rawArcs.Count - 1; i++)
            {
                var pool1 = arcSegs[rawArcs[i]];
                for (int j = i + 1; j < rawArcs.Count; j++)
                {
                    var pool2 = arcSegs[rawArcs[j]];

                    // 线段互查
                    bool toSplit = false;
                    foreach (var s1 in pool1) foreach (var s2 in pool2) toSplit |= CheckCross(s1, s2);

                    // 标记需分割
                    if (toSplit)
                    {
                        arcMarker.Add(rawArcs[i]);
                        arcMarker.Add(rawArcs[j]);
                    }
                }
            }

            // 根据交点分割线段
            foreach (var a in rawArcs)
            {
                // 按顺序分割线段
                if (arcMarker.Contains(a))
                {
                    var tmp = new List<GeomPoint>(); // 待创建弧段包含点
                    LineSegment last = null; // 最后访问的线段
                    foreach (var seg in arcSegs[a])
                    {
                        tmp.Add(seg.Item1); // 直接添加起点
                        if (segSplit.ContainsKey(seg)) // 该线段有交点
                        {
                            Vector2
                                pos1 = seg.Item1,
                                dpos = seg.Item2 - pos1;
                            foreach (double r in segSplit[seg])
                            {
                                var splitter = Point(pos1 + dpos * r);
                                DumpArc(ref tmp, splitter); // 输出已储存点
                            }
                        }
                        last = seg;
                    }
                    DumpArc(ref tmp, last.Item2); // 添加末点
                }

                // 无分割直接添加
                else cArc.Add(a);
            }
        }

        // 2. 使用已分割弧段搜索多边形
        static void CreatePoly()
        {
            // 创建顶点-弧段拓扑
            vertPool = new Dictionary<GeomPoint, SortedDictionary<double, GeomArc>>();
            vertNext = new Dictionary<GeomPoint, Dictionary<GeomArc, GeomArc>>();
            foreach (var a in cArc)
            {
                VertexTopo(a, a.IterSegments().First());
                VertexTopo(a, a.IterSegments(true).First());
            }
            // TODO: 移除线头

            // 创建转向映射表
            foreach (var pair in vertPool)
            {
                var arcs = pair.Value.Values.ToArray();
                int l = arcs.Length;
                var mapper = new Dictionary<GeomArc, GeomArc>();
                for (int i = 0; i < l; i++) mapper[arcs[i]] = arcs[(i + 1) % l];
                vertNext[pair.Key] = mapper;
            }

            // 递归搜索多边形
            holesGroup = new List<Tuple<GeomPoly, HashSet<GeomPoly>>>();
            arcUsed = new HashSet<Tuple<GeomPoint, GeomArc>>();
            foreach (var pair in vertPool) PolyTopoGroup(pair.Key, pair.Value.Values.First());
        }

        // 2.1 从指定弧段与端点开始搜索相连多边形
        static void PolyTopoGroup(GeomPoint pt, GeomArc arc)
        {
            inners = new HashSet<GeomPoly>();
            stack = new LinkedList<Tuple<GeomPoint, GeomArc>>();
            stack.AddFirst(new Tuple<GeomPoint, GeomArc>(pt, arc));

            while (stack.Count > 0)
            {
                var data = stack.First;
                stack.RemoveFirst();
                PolyTopoUnit(data.Value);
            }

            if (inners.Count == 0) return;

            // 获取并删除外包多边形（面积最大）
            GeomPoly outer = null;
            if (inners.Count > 1)
            {
                outer = null;
                foreach (var poly in inners) if (outer == null || outer.OuterArea < poly.OuterArea) outer = poly;
                inners.Remove(outer);
            }
            else outer = inners.First();

            // 加入待处理孔洞列表
            holesGroup.Add(new Tuple<GeomPoly, HashSet<GeomPoly>>(outer, inners));

            cPoly.AddRange(inners);
        }

        // 2.1.1 搜索相连多边形：单元操作，返回当前搜索
        static void PolyTopoUnit(Tuple<GeomPoint, GeomArc> data)
        {
            if (arcUsed.Contains(data)) return;
            var res = new List<Tuple<GeomPoint, GeomArc>>();

            // 搜索直到首尾相接
            while (true)
            {
                // 加入当前弧段
                res.Add(data);
                arcUsed.Add(data);
                // 获取下一弧段
                GeomPoint prevPt = data.Item1;
                GeomArc prevArc = data.Item2;
                GeomPoint nextPt = prevArc.First;
                if (nextPt == prevPt) nextPt = prevArc.Last;
                GeomArc nextArc = vertNext[nextPt][data.Item2];
                data = new Tuple<GeomPoint, GeomArc>(nextPt, prevArc); // 同弧段不同顶点
                if (!arcUsed.Contains(data)) stack.AddFirst(data);
                data = new Tuple<GeomPoint, GeomArc>(nextPt, nextArc);
                // 首尾相接时跳出
                if (data.Equals(res[0])) break;
            }

            // 创造多边形
            var newPoly = new GeomPoly(from pair in res select pair.Item2);
            inners.Add(newPoly);
        }

        // 3. 处理孔洞
        static void HandleHoles()
        {
            holesGroup.Sort((Tuple<GeomPoly, HashSet<GeomPoly>> p1, Tuple<GeomPoly, HashSet<GeomPoly>> p2) => Math.Sign(p1.Item1.OuterArea - p2.Item1.OuterArea));
            for (int i = 0; i < holesGroup.Count - 1; i++)
            {
                var hole = holesGroup[i].Item1;
                bool binded = false;
                for (int j = i + 1; j < holesGroup.Count; j++)
                {
                    var toCheck = holesGroup[j];
                    if (!toCheck.Item1.MBR.Include(hole.MBR)) continue;
                    foreach (var poly in toCheck.Item2) if (poly.IncludeSimple(hole)) // 找到包含该孔洞的多边形
                        {
                            if (poly.holes == null) poly.holes = new List<GeomPoly>();
                            poly.holes.Add(hole);
                            binded = true;
                            break;
                        }
                    if (binded) break;
                }
                if (!binded) throw new Exception();
            }
        }

        #region helpers

        // 获取或创建指定坐标点
        static GeomPoint Point(Vector2 pos)
        {
            GeomPoint ret;
            if (cPoint.ContainsKey(pos)) ret = cPoint[pos];
            else cPoint[pos] = ret = (GeomPoint)pos;
            return ret;
        }

        // 复制指定弧段为新弧段，并对点去重
        static GeomArc Arc(IEnumerable<GeomPoint> points)
        {
            var pool = new List<GeomPoint>();
            GeomPoint lastPt = null;
            foreach (var pt in points)
            {
                var newPt = Point(pt);
                if (newPt == lastPt) continue; // 去重
                pool.Add(lastPt = newPt);
            }
            if (pool.Count < 2) return null;
            return new GeomArc(pool);
        }

        // 由MBR创建分割弧段
        static IEnumerable<GeomArc> SplitMBR(Rect MBR)
        {
            var xs = new double[] {
                MBR.XMin,
                (MBR.XMin+MBR.XMax)/2,
                MBR.XMax,
            };
            var ys = new double[] {
                MBR.YMin,
                (MBR.YMin+MBR.YMax)/2,
                MBR.YMax,
            };
            var pts = new GeomPoint[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    pts[i, j] = Point(new Vector2(xs[i], ys[j]));

            GeomPoint[] tmp;
            for (int i = 0; i < 2; i++)
            {
                // 横线+竖线
                for (int j = 0; j < 3; j++)
                {
                    tmp = new GeomPoint[] { pts[i, j], pts[i + 1, j] };
                    yield return new GeomArc(tmp);
                    tmp = new GeomPoint[] { pts[j, i], pts[j, i + 1] };
                    yield return new GeomArc(tmp);
                }

                // 斜线
                tmp = new GeomPoint[] { pts[i, i], pts[i + 1, i + 1] };
                yield return new GeomArc(tmp);
                tmp = new GeomPoint[] { pts[i, 2 - i], pts[i + 1, 1 - i] };
                yield return new GeomArc(tmp);
            }
        }

        // 检查两线段相交情况，将交点加入相应线段交点数组
        static bool CheckCross(LineSegment s1, LineSegment s2)
        {
            var crosser = LineSegment.CheckCross(s1, s2);
            if (crosser == null) return false;
            double r1 = crosser.Item1, r2 = crosser.Item2;
            if (r1 < 0 || r2 < 0 || r1 > 1 || r2 > 1) return false;

            // 写入对应位置
            if (r1 < 1)
            {
                if (!segSplit.ContainsKey(s1)) segSplit[s1] = new SortedSet<double>();
                segSplit[s1].Add(r1);
            }
            if (r2 < 1)
            {
                if (!segSplit.ContainsKey(s2)) segSplit[s2] = new SortedSet<double>();
                segSplit[s2].Add(r2);
            }

            return true;
        }

        // 使用指定点序列与末点创建弧段，并加入全局序列
        static void DumpArc(ref List<GeomPoint> points, GeomPoint ender)
        {
            points.Add(ender);
            var newArc = Arc(points);
            points = new List<GeomPoint>(new GeomPoint[] { ender });
            if (newArc != null) cArc.Add(newArc);
        }

        // 给定弧段计算夹角并加入拓扑结构
        static void VertexTopo(GeomArc arc, LineSegment seg)
        {
            if (!vertPool.ContainsKey(seg.Item1))
                vertPool[seg.Item1] = new SortedDictionary<double, GeomArc>();
            vertPool[seg.Item1][seg.Angle] = arc;
        }

        // 比较多边形外包面积大小
        static int CompArea(GeomPoly p1, GeomPoly p2) => Math.Sign(p1.OuterArea - p2.OuterArea);

        // 列表设置递增ID
        static void ApplyID(IEnumerable<BaseGeom> geoms, ref int i)
        {
            foreach (var g in geoms) g.id = ++i;
        }

        #endregion

        public static void Entry(List<GeomArc> origin, Rect MBR, out List<GeomPoint> points, out List<GeomArc> arcs, out List<GeomPoly> polygons, bool crossingOnly = true)
        {
            // 初始化全局变量
            cPoint = new Dictionary<Vector2, GeomPoint>();
            arcs = cArc = new List<GeomArc>();
            polygons = cPoly = new List<GeomPoly>();

            // 运行算法步骤
            SplitArcs(origin, MBR);
            CreatePoly();
            HandleHoles();

            // 输出点（仅交点或全部顶点）
            if (crossingOnly)
            {
                points = new List<GeomPoint>();
                foreach (var a in cArc)
                {
                    points.Add(a.points.First());
                    points.Add(a.points.Last());
                }
            }
            else points = new List<GeomPoint>(cPoint.Values);

            // 指定ID顺序
            int i = 0;
            ApplyID(polygons, ref i);
            ApplyID(arcs, ref i);
            ApplyID(cPoint.Values, ref i);
        }
    }
}
