using MiniGIS.Algorithm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Data
{
    // 用于TIN结构的三角单元
    public class Triangle
    {
        public Vector2 p1, p2, p3;
        public Vector2 center;
        public double radius;
        public Rect MBR;

        public Triangle(Vector2 pt1, Vector2 pt2, Vector2 pt3)
        {
            p1 = pt1; p2 = pt2; p3 = pt3;
        }

        // 计算外接圆圆心、半径、MBR
        public void CalcCircum()
        {
            if (center != null) return; // 已完成计算

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
        public void CalcMBR()
        {
            if (MBR != null) return;
            MBR = new Rect(from p in Points() select (GeomPoint)p);
        }

        // 迭代器
        public IEnumerable<Vector2> Points()
        {
            yield return p1;
            yield return p2;
            yield return p3;
        }
        public IEnumerable<Edge> Edges() // 固定返回(小点, 大点)
        {
            yield return edge_helper(p1, p2);
            yield return edge_helper(p1, p3);
            yield return edge_helper(p2, p3);
        }
        Edge edge_helper(Vector2 p1, Vector2 p2) => (p1 < p2) ? new Edge(p1, p2) : new Edge(p2, p1);

        // 检查点是否在三角形内
        // -1: 内部; 0: 外部; 1-3: p{i}-p{i+1}边上
        public int Contains(Vector2 pos)
        {
            CalcMBR();
            if (!MBR.Include(pos)) return 0;
            Vector2[] tmp = Points().ToArray();
            int res = 0;
            for (int i = 0; i < 3; i++)
            {
                int crosser = Math.Sign(CheckCross(pos, tmp[i], tmp[(i + 1) % 3]));
                if (crosser == 0) return i + 1;
                res += crosser;
            }
            return Math.Abs(res) == 3 ? -1 : 0;
        }

        // 辅助检查点朝向
        public static double CheckCross(Vector2 p1, Vector2 p2, Vector2 p3) => (p1 - p2).Cross(p1 - p3);

        // 检查三角是否位于矩形内
        public bool Inside(Rect MBR) => (MBR.Include(p1) && MBR.Include(p2) && MBR.Include(p3));
    }
}
