using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Layer
{
    public enum LayerTag
    {
        None = 0,
        Points = 1,  // 原始点数据
        TIN = 2,     // TIN三角网
        Grid = 4,    // 格网
        Contour = 8, // 等值线
        Smooth = 16, // 等值线平滑
        Topo = 32,   // 拓扑多边形图层
    }
}
