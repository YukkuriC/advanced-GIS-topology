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

        // 三对角矩阵求解
        #region matrix
        public static double[] SolveMatrix(double[][] A)
        {
            int m = A.Length;

            // 首行
            A[0][1] /= A[0][0];
            A[0][m] /= A[0][0];

            // 后续
            for (int i = 1; i < m; i++)
            {
                var dom = A[i][i] - A[i - 1][i] * A[i][i - 1];
                if (i < m - 1) A[i][i + 1] /= dom;
                A[i][m] = (A[i][m] - A[i - 1][m]) / dom;
            }

            // 返回结果
            var res = new double[m];
            res[m - 1] = (double)A[m - 1][m];
            for (int i = m - 2; i >= 0; i--) res[i] = (double)A[i][m] - (double)A[i][i + 1] * res[i + 1];
            return res;
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
