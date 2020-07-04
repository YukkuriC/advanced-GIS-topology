using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MiniGIS.Algorithm
{
    public static class Utils
    {
        public const double EPSILON = 1e-8; // 浮点计算误差

        public static double Lerp(this double v, double min, double max) => (max == min) ? 0 : (v - min) / (max - min);
        public static double Lerp(this double v, double min, double max, double tmin, double tmax) => (tmax == tmin) ? tmin : tmin + (tmax - tmin) * v.Lerp(min, max);
        public static string SciString(this double val, uint digit = 6)
        {
            if (val < 0) return "-" + (-val).SciString(digit);
            string res;
            if (val == 0)
            {
                if (digit == 0) return "0";
                res = "0.";
                for (int i = 0; i < digit; i++) res += "0";
                return res;
            }
            int exp = (int)Math.Floor(Math.Log10(val));
            res = Math.Round(val / Math.Pow(10, exp), (int)digit).ToString();
            if (exp != 0) res += "e" + exp.ToString();
            return res;
        }

        // 计算线段相交
        public static Tuple<double,double> CheckCross(Vector2 s1A, Vector2 s1B, Vector2 s2A, Vector2 s2B)
        {
            double
                ax = s1A.X, ay = s1A.Y,
                bx = s1B.X, by = s1B.Y,
                cx = s2A.X, cy = s2A.Y,
                dx = s2B.X, dy = s2B.Y;

            // 求解
            double dom = (ax - bx) * (cy - dy) - (ay - by) * (cx - dx);
            if (dom == 0) return null;
            double
                r1 = ((ax - cx) * (cy - dy) - (ay - cy) * (cx - dx)) / dom,
                r2 = (-(ax - bx) * (ay - cy) + (ax - cx) * (ay - by)) / dom;
            return new Tuple<double, double>(r1, r2);
        }

        // 获取指定数在有序表内区间
        public static int GetInterval(this double v, double[] values)
        {
            if (v <= values[0]) return -1;
            else if (v >= values.Last()) return values.Length;

            // 线性查找
            if (values.Length < 5)
            {
                int i = 1;
                while (values[i] < v) i++;
                i--;
                return i;
            }

            // 二分查找
            else
            {
                int l = 0, r = values.Length - 1;
                while (l < r)
                {
                    int m = (l + r) / 2;
                    if (v < values[m]) r = m - 1;
                    else if (values[m + 1] < v) l = m + 1;
                    else return m;
                }
                return l;
            }
        }

        // 输出等分点
        public static IEnumerable<double> Linear(double start, double step, int s1, int s2)
        {
            for (int i = s1; i <= s2; i++)
                yield return start + step * i;
        }

        // 行列式求值
        public static double CalcDeterminant(double a1, double b1, double c1,
                                             double a2, double b2, double c2,
                                             double a3, double b3, double c3)
        {
            return (a1 * b2 * c3) + (b1 * c2 * a3) + (c1 * a2 * b3) -
                   (a3 * b2 * c1) - (a2 * b1 * c3) - (a1 * b3 * c2);
        }

        // 矩阵求解
        #region matrix
        public static double[] SolveMatrix(double[][] A)
        {
            int m = A.Length;
            for (int k = 0; k < m; k++) // column
            {
                // pivot for column
                int i_max = 0; double vali = double.MinValue;
                for (int i = k; i < m; i++) if (Math.Abs(A[i][k]) > vali) { i_max = i; vali = Math.Abs(A[i][k]); }
                var tmp = A[k]; A[k] = A[i_max]; A[i_max] = tmp;

                // for all rows below pivot
                for (int i = k + 1; i < m; i++)
                {
                    double cf = (A[i][k] / A[k][k]);
                    for (int j = k; j < m + 1; j++) A[i][j] -= A[k][j] * cf;
                }
            }

            var x = new double[m];

            for (int i = m - 1; i >= 0; i--)    // rows = columns
            {
                double v = A[i][m] / A[i][i];
                x[i] = v;
                for (int j = i - 1; j >= 0; j--)    // rows
                {
                    A[j][m] -= A[j][i] * v;
                    A[j][i] = 0;
                }
            }

            return x;
        }

        public static double[][] ZerosMat(int r, int c)
        {
            var res = new double[r][];
            for (int i = 0; i < r; i++)
            {
                res[i] = new double[c];
            }
            return res;
        }
        #endregion

        public static void Debug(string fmt = "", params object[] val)
        {
            var res = MessageBox.Show(String.Format(fmt, val), "", MessageBoxButtons.OKCancel);
            if (res == DialogResult.Cancel) throw new Exception();
        }
    }
}
