using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    public abstract class SplineBase<T>
    {
        public abstract T Eval(double x);

        public abstract double Min();
        public abstract double Max();

        public List<T> Smooth(int nsplit) => Smooth(nsplit, Min(), Max());
        public List<T> Smooth(int nsplit, double start, double stop)
        {
            List<T> result = new List<T>();
            List<Double> t1 = new List<double>();
            double step = (stop - start) / nsplit;
            for (int i = 0; i <= nsplit; i++)
            {
                double p = start + step * i;
                result.Add(Eval(p));
                t1.Add(p);
            }
            return result;
        }
    }

    // 三次样条插值
    public class Spline : SplineBase<double>
    {
        protected int n;
        protected double[] xs, ys, ms, dx, dy;
        protected double _min, _max;

        public override double Min() => _min;
        public override double Max() => _max;

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

            // 执行表达式
            return Eval(x - xs[i], i);
        }
        
        // 写入公共参数
        protected virtual void InitParams(double[] _xs, double[] _ys)
        {
            xs = _xs; ys = _ys; n = xs.Length - 1;

            // 确定最值
            _min = xs[0];
            _max = _xs[n];

            // 计算xy变量步长
            dx = new double[n];
            dy = new double[n];
            for (int i = 0; i < n; i++)
            {
                dx[i] = xs[i + 1] - xs[i];
                dy[i] = ys[i + 1] - ys[i];
            }
        }

        // 公共初始化
        protected void Init(double[] _xs, double[] _ys, bool loop = false)
        {
            InitParams(_xs, _ys);
            var A = InitMatrix(loop);
            ms = Utils.SolveMatrix(A);
        }

        public Spline() { }
        public Spline(double[] _xs, double[] _ys, bool loop = false) => Init(_xs, _ys, loop);

        #region 可继承部分

        // 执行指定分段样条函数
        protected virtual double Eval(double x, int i)
        {
            double dm = (ms[i + 1] - ms[i]) / 6;
            double a1 = dy[i] / dx[i] - dx[i] * ms[i] / 2 - dx[i] * dm;
            double a2 = ms[i] / 2;
            double a3 = dm / dx[i];

            return ys[i] + (a1 + (a2 + a3 * x) * x) * x;
        }

        // 求解参数矩阵
        // loop：是否首尾循环（默认为否）
        protected virtual double[][] InitMatrix(bool loop)
        {
            var A = Utils.ZerosMat(n + 1, n + 2);

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

            return A;
        }

        #endregion
    }

    // 三次样条插值-二维点
    public class Spline2D : SplineBase<Vector2>
    {
        protected Spline spx, spy;

        protected double _max;
        public override double Max() => _max;
        public override double Min() => 0;

        protected double[] xs, ys, ls;
        protected bool looped;

        public override Vector2 Eval(double x)
        {
            if (x < 0) x = 0;
            else if (x > _max) x = _max;
            return new Vector2(spx.Eval(x), spy.Eval(x));
        }

        // 静态方法，用于提取线段长度
        public static double[] CumLength(IEnumerable<Vector2> points)
        {
            Vector2 vz = points.First();
            double[] res = new double[points.Count()];
            double cum = 0;
            int i = 0;
            foreach (var p in points)
            {
                cum += vz.Distance(p);
                res[i++] = cum;
                vz = p;
            }
            return res;
        }
        
        // 写入公共参数
        protected void InitParams(IEnumerable<Vector2> points)
        {
            // 计算累积线段长度
            ls = CumLength(points);
            _max = ls[ls.Length - 1];

            // 组装xy数组
            xs = new double[points.Count()];
            ys = new double[points.Count()];
            int i = 0;
            foreach (Vector2 p in points)
            {
                xs[i] = p.X;
                ys[i] = p.Y;
                i++;
            }
            looped = xs[0] == xs[i - 1] && ys[0] == ys[i - 1];
        }

        // 公共初始化
        protected void Init(IEnumerable<Vector2> points)
        {
            InitParams(points);
            LoadSplines();
        }

        public Spline2D() { }
        public Spline2D(IEnumerable<Vector2> points) => Init(points);

        #region 可继承部分

        protected virtual void LoadSplines()
        {
            spx = new Spline(ls, xs, looped);
            spy = new Spline(ls, ys, looped);
        }

        #endregion
    }

    // 张力样条
    public class SplineTension : Spline
    {
        protected double[] sigma;
        protected double[] Ss, Cs, Ts, sig2;

        public SplineTension(double _sigma, double[] _xs, double[] _ys, bool loop = false)
        {
            // 写入参数
            sigma = new double[_xs.Length];
            for (int i = 0; i < sigma.Length; i++) sigma[i] = _sigma;

            // 执行初始化
            Init(_xs, _ys, loop);
        }

        protected override void InitParams(double[] _xs, double[] _ys)
        {
            base.InitParams(_xs, _ys);

            // 预计算双曲函数值
            Ss = new double[n];
            Cs = new double[n];
            Ts = new double[n];
            for (int i = 0; i < n; i++)
            {
                Ss[i] = Math.Sinh(dx[i] * sigma[i]);
                Cs[i] = Math.Cosh(dx[i] * sigma[i]);
                Ts[i] = Ss[i] / Cs[i];
            }
        }

        protected override double Eval(double x, int i)
        {
            double c = (sig2[i] * ms[i + 1] - Cs[i] * ms[i]) / Ss[i];
            double b = (dy[i] + ms[i] - sig2[i] * ms[i + 1]) / dx[i];
            double res = ys[i] - ms[i] + b * x + c * Math.Sinh(sigma[i] * x) + ms[i] * Math.Cosh(sigma[i] * x);

            // TODO: 解决浮点误差
            if (Double.IsNaN(res)) throw new Exception();
            if (Math.Abs(res * 2 - ys[i] - ys[i + 1]) > Math.Abs(ys[i] - ys[i + 1]) * 3)
            {
                return x.Lerp(0, dx[i], ys[i], ys[i + 1]);
            }

            return res;
        }

        protected override double[][] InitMatrix(bool loop)
        {
            var A = Utils.ZerosMat(n + 1, n + 2);

            // 创建缓存数组
            sig2 = new double[n]; // (sigma[i+1] / sigma[i])^2
            for (int i = 0; i < n; i++) sig2[i] = Math.Pow(sigma[i + 1] / sigma[i], 2);

            // 写入共用矩阵
            for (int i = 1; i < n; i++)
            {
                A[i][i - 1] = sigma[i - 1] * (-Cs[i - 1] * Ts[i - 1] + Ss[i - 1]) + 1 / dx[i - 1];
                A[i][i] = sig2[i - 1] * (sigma[i - 1] * Ts[i - 1] - 1 / dx[i - 1]) + sigma[i] * Ts[i] - 1 / dx[i];
                A[i][i + 1] = 1 / dx[i] - sigma[i] / Ss[i];
                A[i][n + 1] = dy[i] / dx[i] - dy[i - 1] / dx[i - 1];
            }

            // TODO: 首末行
            {
                A[0][0] = A[n][n] = 1;
            }

            return A;
        }
    }

    // 二维张力样条
    public class SplineTension2D : Spline2D
    {
        protected double sigma;

        public SplineTension2D(IEnumerable<Vector2> points) => Init(points);

        protected override void LoadSplines()
        {
            // 计算初始sigma
            sigma = 1.5 * (xs.Length - 1) / ls.Last();

            // 设置xy样条
            spx = new SplineTension(sigma, ls, xs, looped);
            spy = new SplineTension(sigma, ls, ys, looped);
        }
    }
}
