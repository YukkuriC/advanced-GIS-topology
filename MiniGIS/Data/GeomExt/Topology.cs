using MiniGIS.Layer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiniGIS.Data
{
    public class Topology
    {
        GeomLayer parent;

        Dictionary<GeomPoint, List<GeomArc>> originArcs;
        Dictionary<GeomArc, GeomPoly> arcLeft, arcRight;

        #region helpers

        // 向列表集合中加入新元素
        void AddList<TKey, TVal>(Dictionary<TKey, List<TVal>> pool, TKey key, TVal newVal)
        {
            if (!pool.ContainsKey(key)) pool[key] = new List<TVal>();
            pool[key].Add(newVal);
        }

        // 输出多边形内所有弧段
        void DumpPoly(GeomPoly poly, bool leftInner)
        {
            foreach (var pair in poly.arcs)
            {
                bool isRight = leftInner ^ pair.Item2;
                var container = isRight ? arcRight : arcLeft;
                if (container.ContainsKey(pair.Item1)) throw new Exception("重边");
                container[pair.Item1] = poly;
            }
        }
        IEnumerable<string> ExportPoly(GeomPoly poly)
        {
            foreach (var pair in poly.arcs)
                yield return String.Concat(
                    pair.Item2 ? '-' : '+',
                    pair.Item1.id
                );
        }

        #endregion

        public Topology(GeomLayer _parent)
        {
            parent = _parent;
            parent.topology = this;

            originArcs = new Dictionary<GeomPoint, List<GeomArc>>();
            arcLeft = new Dictionary<GeomArc, GeomPoly>();
            arcRight = new Dictionary<GeomArc, GeomPoly>();

            // 点：起始弧段
            foreach (var arc in parent.arcs)
            {
                AddList(originArcs, arc.First, arc);
                AddList(originArcs, arc.Last, arc);
            }

            // 弧段：左右多边形
            foreach (var poly in parent.polygons)
            {
                DumpPoly(poly, poly.Clockwise);
                if (poly.holes != null)
                    foreach (var hole in poly.holes)
                        DumpPoly(hole, !hole.Clockwise);
            }
        }

        // 输出内容至文件流
        public void ExportContent(StreamWriter fs)
        {
            fs.WriteLine("#POLYGON\tarcs\tholes");
            foreach (var poly in parent.polygons)
            {
                fs.WriteLine("{0}\t{1}\t{2}",
                    poly.id,
                    string.Join(",", ExportPoly(poly)),
                    poly.holes != null && poly.holes.Count > 0 ?
                        string.Join("|",
                            from hole in poly.holes
                            select string.Join(",", ExportPoly(hole))
                        ) : "NULL"
                    );
            }

            fs.WriteLine("#ARC\tleft_poly\tright_poly\tpoints");
            foreach (var arc in parent.arcs)
            {
                fs.WriteLine("{0}\t{1}\t{2}\t{3}",
                    arc.id,
                    arcLeft.ContainsKey(arc) ? arcLeft[arc].id.ToString() : "NULL",
                    arcRight.ContainsKey(arc) ? arcRight[arc].id.ToString() : "NULL",
                    string.Join(",", from p in arc.points select p.id)
                    );
            }

            fs.WriteLine("#POINT\tx\ty\tarcs");
            var allPoints = new HashSet<GeomPoint>();
            foreach (var arc in parent.arcs) foreach (var pt in arc.points) allPoints.Add(pt);
            foreach (var point in allPoints)
            {
                fs.WriteLine("{0}\t{1}\t{2}\t{3}",
                    point.id,
                    point.X,
                    point.Y,
                    originArcs.ContainsKey(point) ?
                    string.Join(",", from a in originArcs[point] select a.id) :
                    "NULL"
                    );
            }
        }
    }
}