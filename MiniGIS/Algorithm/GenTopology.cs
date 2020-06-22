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
            foreach (var a in origin)
            {
                var tmp = Arc(a.points, a.value);
                if (tmp != null) cArc.Add(tmp);
            }
            foreach (var a in SplitMBR(MBR)) cArc.Add(a);

            // 构造数据结构：弧段->线段数组、线段->交点位置列表
            // 线段互相求交 O(N^2*L^2)
            // 根据交点分割线段
            // TODO
        }

        #region helpers

        // 获取或创建指定坐标点
        static GeomPoint Point(Vector2 pos, double val = 0)
        {
            GeomPoint ret;
            if (cPoint.ContainsKey(pos)) ret = cPoint[pos];
            else
            {
                cPoint[pos] = ret = (GeomPoint)pos;
                ret.value = val;
            }
            return ret;
        }

        // 复制指定弧段为新弧段，并对点去重
        static GeomArc Arc(IEnumerable<GeomPoint> points, double val = 0)
        {
            var pool = new List<GeomPoint>();
            GeomPoint lastPt = null;
            foreach (var pt in points)
            {
                var newPt = Point(pt, pt.value);
                if (newPt == lastPt) continue; // 去重
                pool.Add(lastPt = newPt);
            }
            if (pool.Count < 2) return null;
            return new GeomArc(pool, 0, val);
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

        // 列表设置递增ID
        static void ApplyID(IEnumerable<BaseGeom> geoms)
        {
            int i = 0;
            foreach (var g in geoms) g.id = ++i;
        }

        #endregion

        public static void Entry(List<GeomArc> origin, Rect MBR, out List<GeomPoint> points, out List<GeomArc> arcs, out List<GeomPoly> polygons)
        {
            // 初始化全局变量
            cPoint = new Dictionary<Vector2, GeomPoint>();
            cArc = new List<GeomArc>();
            cPoly = new List<GeomPoly>();

            // 运行算法步骤
            SplitArcs(origin, MBR);
            // TODO

            // 输出结果
            points = new List<GeomPoint>(cPoint.Values);
            arcs = cArc;
            polygons = new List<GeomPoly>();

            // 指定ID顺序
            ApplyID(points);
            ApplyID(arcs);
            ApplyID(polygons);
        }
    }
}
