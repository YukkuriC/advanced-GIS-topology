using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public abstract class SplineBase<T, SELF> where SELF : SplineBase<T, SELF>
    {
        public abstract T Eval(double x);

        public static List<T> Smooth(IEnumerable<T> xs, int nsplit = 10)
        {
            var smoother = (SELF)Activator.CreateInstance(typeof(SELF), xs);
            List<T> result = new List<T>();
            int maxi = (xs.Count() - 1) * nsplit;
            for (int i = 0; i <= maxi; i++) result.Add(smoother.Eval((double)i / nsplit));
            return result;
        }
    }

    // 三次样条插值
    public class Spline : SplineBase<double, Spline>
    {
        int n;
        double[] xs, ys, ms, dx, dy;

        // 求解参数矩阵
        // loop：是否首尾循环（默认为否）
        void Init(double[] _xs, double[] _ys, bool loop = false)
        {
            xs = _xs; ys = _ys; n = xs.Length - 1;
            var A = Utils.ZerosMat(n + 1, n + 2);

            // 计算xy变量步长
            dx = new double[n];
            dy = new double[n];
            for (int i = 0; i < n; i++)
            {
                dx[i] = xs[i + 1] - xs[i];
                dy[i] = ys[i + 1] - ys[i];
            }

            // 写入共用矩阵
            for (int i = 1; i < n; i++)
            {
                A[i][i - 1] = dx[i - 1];
                A[i][i] = 2 * (dx[i - 1] + dx[i]);
                A[i][i + 1] = dx[i];
                A[i][n + 1] = 6 * (dy[i] / dx[i] - dy[i - 1] / dx[i - 1]);
            }

            // 首末行
            if (loop) // Clamped
            {
                // 计算微分值
                // y[n-1] y[n]=y[0] y[1]
                double D = (ys[1] - ys[n - 1]) / (xs[1] - xs[0] + xs[n] - xs[n - 1]);
                A[0][0] = 2 * (A[0][1] = dx[0]);
                A[n][n] = 2 * (A[n][n - 1] = dx[n - 1]);
                A[0][n + 1] = 6 * (dy[0] / dx[0] - D);
                A[n][n + 1] = 6 * (D - dy[n - 1] / dx[n - 1]);
            }
            else // Natural
            {
                A[0][0] = A[n][n] = 1;
            }

            // 求解矩阵
            ms = Utils.SolveMatrix(A);
        }

        public override double Eval(double x)
        {

            // 越界处理
            if (x <= xs[0]) return ys[0];
            if (x >= xs[n]) return ys[n];

            // 查找对应区间
            // TODO: 二分查找
            var i = 1;
            while (xs[i] < x) i++;
            i--;

            var tx = x - xs[i];

            double dm = (ms[i + 1] - ms[i]) / 6;
            double a1 = dy[i] / dx[i] - dx[i] * ms[i] / 2 - dx[i] * dm;
            double a2 = ms[i] / 2;
            double a3 = dm / dx[i];

            return ys[i] + (a1 + (a2 + a3 * tx) * tx) * tx;
        }

        public Spline(double[] values) => Init(
            (from i in Enumerable.Range(0, values.Length) select (double)i).ToArray(),
            values, false);

        public Spline(double[] _xs, double[] _ys, bool loop = false) => Init(_xs, _ys, loop);
    }

    // 三次样条插值-二维点
    public class Spline2D : SplineBase<Vector2, Spline2D>
    {
        Spline spx, spy;

        public override Vector2 Eval(double x) => new Vector2(spx.Eval(x), spy.Eval(x));

        public Spline2D(IEnumerable<Vector2> points)
        {
            double[] idx = new double[points.Count()];
            double[] xs = new double[points.Count()];
            double[] ys = new double[points.Count()];
            int i = 0;
            foreach (Vector2 p in points)
            {
                idx[i] = i;
                xs[i] = p.X;
                ys[i] = p.Y;
                i++;
            }
            bool looped = xs[0] == xs[i - 1] && ys[0] == ys[i - 1];
            spx = new Spline(idx, xs, looped);
            spy = new Spline(idx, ys, looped);
        }
    }
}
