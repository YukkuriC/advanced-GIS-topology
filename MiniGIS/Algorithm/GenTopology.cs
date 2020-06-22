using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public static class GenTopology
    {
        static Dictionary<Vector2, GeomPoint> cPoint;
        static List<GeomArc> cArc;
        static List<GeomPoly> cPoly;

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
            var segSplit = new Dictionary<LineSegment, SortedSet<double>>();
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
                    foreach (var s1 in pool1) foreach (var s2 in pool2) toSplit |= CheckCross(s1, s2, segSplit);

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
            // 递归搜索多边形
            // 处理孔洞
            // TODO
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
        static bool CheckCross(LineSegment s1, LineSegment s2, Dictionary<LineSegment, SortedSet<double>> segSplit)
        {
            double
                ax = s1.Item1.X, ay = s1.Item1.Y,
                bx = s1.Item2.X, by = s1.Item2.Y,
                cx = s2.Item1.X, cy = s2.Item1.Y,
                dx = s2.Item2.X, dy = s2.Item2.Y;

            // 求解
            double dom = (ax - bx) * (cy - dy) - (ay - by) * (cx - dx);
            if (dom == 0) return false;
            double
                r1 = ((ax - cx) * (cy - dy) - (ay - cy) * (cx - dx)) / dom,
                r2 = (-(ax - bx) * (ay - cy) + (ax - cx) * (ay - by)) / dom;
            if (r1 < 0 || r2 < 0 || r1 >= 1 || r2 >= 1) return false;

            // 写入对应位置
            if (!segSplit.ContainsKey(s1)) segSplit[s1] = new SortedSet<double>();
            if (!segSplit.ContainsKey(s2)) segSplit[s2] = new SortedSet<double>();
            segSplit[s1].Add(r1);
            segSplit[s2].Add(r2);

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

        // 列表设置递增ID
        static void ApplyID(IEnumerable<BaseGeom> geoms)
        {
            int i = 0;
            foreach (var g in geoms) g.id = ++i;
        }

        #endregion

        public static void Entry(List<GeomArc> origin, Rect MBR, out List<GeomPoint> points, out List<GeomArc> arcs, out List<GeomPoly> polygons, bool crossingOnly = true)
        {
            // 初始化全局变量
            cPoint = new Dictionary<Vector2, GeomPoint>();
            cArc = new List<GeomArc>();
            cPoly = new List<GeomPoly>();

            // 运行算法步骤
            SplitArcs(origin, MBR);
            CreatePoly();

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

            // 输出弧段与多边形
            arcs = cArc;
            polygons = new List<GeomPoly>();

            // 指定ID顺序
            ApplyID(points);
            ApplyID(arcs);
            ApplyID(polygons);
        }
    }
}
