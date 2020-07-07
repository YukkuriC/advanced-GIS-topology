using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Data
{
    // 包含两点元组的边
    public class Edge : Tuple<Vector2, Vector2>
    {
        public Edge(Vector2 v1, Vector2 v2) : base(v1, v2) { }

        #region method
        // 翻转边元组
        public Edge Reverse() => new Edge(Item2, Item1);
        // 边是否正向
        public bool Ordered() => Item1 <= Item2;
        #endregion
    }
}
