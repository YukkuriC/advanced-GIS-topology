using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniGIS.Algorithm
{
    // 张力样条曲线
    public class TensionSpline
    {
        public const double SIGMA_DEFAULT = 1.5;

        List<Vector2> points;
        Rect MBR;
        readonly int N;
        double minSplit;
        readonly bool longEnough, looping;

        double[] dataX, dataY;
        double[] dx, dy, cumLen, lenStep;
        double[] xp, yp, a, b, c;
        double sigma, sigma_param;
        public TensionNode[] segments;

        // 构造函数，传入数据点+细节参数（默认为1200）
        public TensionSpline(IEnumerable<GeomPoint> origin, int detail = 1200)
        {
            // 写入基础参数
            points = new List<Vector2>(from v in origin select (Vector2)v);
            MBR = new Rect(origin);
            N = points.Count;
            looping = points[0] == points[N - 1];
            longEnough = points.Count > 2;
            sigma_param = SIGMA_DEFAULT;
            segments = (from i in Enumerable.Range(0, N - 1) select new TensionNode(this, i)).ToArray(); // 创建分段列表
            SetDetail(detail); // 初始化细节数

            // 计算参数
            if (longEnough)
            {
                InitMemory();
                InitParams();
                CalcParams();
            }
        }

        // 重置所有分段
        void ResetSegments()
        {
            foreach (var seg in segments) seg.Reset();
        }

        // 开辟内存空间
        void InitMemory()
        {
            dataX = new double[N]; // 数据点X
            dataY = new double[N]; // 数据点Y
            dx = new double[N]; // X方向差分
            dy = new double[N]; // Y方向差分
            cumLen = new double[N]; // 累计长度
            lenStep = new double[N - 1]; // 分段长度
            a = new double[N];
            b = new double[N];
            c = new double[N];
            xp = new double[N]; // 结果：X二阶导
            yp = new double[N]; // 结果：Y二阶导
        }

        // 生成后固定不更改的参数
        void InitParams()
        {
            double ds1, six1, siy1, sixn, siyn;
            double dx1, dy1, ds2, dx2, dy2;
            double alf;
            Vector2 vStart, vEnd, dv, dv2;

            // 点数据至数组
            for (int i = 0; i < N; i++)
            {
                dataX[i] = points[i].X;
                dataY[i] = points[i].Y;
            }

            // 计算累计长度、方向差分
            // 起始段
            dv = points[1] - points[0];
            ds1 = dv.Length();
            vStart = dv / ds1;
            dv = vStart;
            dx[0] = dy[0] = 0;
            lenStep[0] = ds1;
            cumLen[0] = 0;
            cumLen[1] = ds1;
            // 中段
            for (int i = 1; i < N - 1; i++)
            {
                // 累计长度
                lenStep[i] = ds2 = points[i + 1].Distance(points[i]);
                cumLen[i + 1] = cumLen[i] + ds2;
                // 方向向量差分
                dv2 = points[i + 1] - points[i];
                dv2 /= dv2.Length();
                dx[i] = dv2.X - dv.X;
                dy[i] = dv2.Y - dv.Y;
                dv = dv2;
            }
            // 尾段（若输入点首尾循环）
            if (looping)
            {
                dx[N - 1] = vStart.X - dv.X;
                dy[N - 1] = vStart.Y - dv.Y;
            }
        }

        // 需动态计算的参数
        void CalcParams()
        {
            ResetSegments();
            double ds;
            double d1, d2, q;

            // 拷贝差分数据
            dx.CopyTo(xp, 0);
            dy.CopyTo(yp, 0);

            // 求解
            sigma = sigma_param * (N - 1) / cumLen[N - 1];
            ds = sigma * lenStep[0];
            d1 = sigma / Math.Tanh(ds) - 1 / lenStep[0];
            b[0] = d1;
            c[0] = 1 / lenStep[0] - sigma / Math.Sinh(ds);
            a[0] = c[0];
            for (int i = 1; i < N - 1; i++)
            {
                ds = sigma * lenStep[i];
                c[i] = 1 / lenStep[i] - sigma / Math.Sinh(ds);
                a[i] = c[i - 1];
                d2 = sigma / Math.Tanh(ds) - 1 / lenStep[i];
                b[i] = d1 + d2;
                d1 = d2;
            }
            b[N - 1] = d1;
            xp[0] /= b[0];
            yp[0] /= b[0];
            q = b[0];
            for (int i = 0; i < N - 1; i++)
            {
                b[i] = c[i] / q;
                q = b[i + 1] - a[i] * b[i];
                xp[i + 1] = (xp[i + 1] - a[i] * xp[i]) / q;
                yp[i + 1] = (yp[i + 1] - a[i] * yp[i]) / q;
            }
            for (int i = 0; i < N - 1; i++)
            {
                int k = N - i - 2;
                xp[k] -= b[k] * xp[k + 1];
                yp[k] -= b[k] * yp[k + 1];
            }
        }

        public void SetDetail(int detail)
        {
            minSplit = (MBR.XMax + MBR.YMax - MBR.XMin - MBR.YMin) / detail;
            if (minSplit < 0.001) minSplit = 1;
            ResetSegments();
        }

        // 检查相交情况
        public bool Crossing(TensionSpline other)
        {
            if (this == other) // 自交检查
            {
                for (int i = 0; i < segments.Length - 2; i++)
                    for (int j = i + 2; j < segments.Length; j++)
                        if ((looping && (i != 0 || j != segments.Length - 1)) // 循环时不检查首尾
                            && segments[i].Crossing(segments[j])) return true;
                return false;
            }
            foreach (var seg1 in segments)
                foreach (var seg2 in other.segments)
                    if (seg1.Crossing(seg2)) return true;
            return false;
        }

        // 整体增加曲线sigma值
        public void IncreaseTension(double val)
        {
            sigma_param += val;
            CalcParams();
        }

        // 由已计算参数输出平滑后曲线
        public List<Vector2> Smooth()
        {
            // 不够长时直接输出原始点
            if (!longEnough) return new List<Vector2>(points);
            // 输出曲线
            var res = new List<Vector2>();
            res.Add(points[0]);
            foreach (var seg in segments) res.AddRange(seg.Points(false));
            return res;
        }

        // 计算指定分段曲线
        public IEnumerable<Vector2> Smooth(int i, bool outStart, bool outEnd)
        {
            // 端点1
            if (outStart) yield return points[i];
            if (longEnough) // 插值段
            {
                double ss, e1, e2, e3, x1, y1;
                int k = (int)(lenStep[i] / minSplit);
                for (int j = 0; j < k; j++)
                {
                    ss = cumLen[i] + j * minSplit;
                    e1 = sigma * (cumLen[i + 1] - ss);
                    e2 = sigma * (ss - cumLen[i]);
                    e3 = sigma * lenStep[i];
                    x1 = (xp[i] * Math.Sinh(e1) + xp[i + 1] * Math.Sinh(e2)) / Math.Sinh(e3) +
                        (dataX[i] - xp[i]) * (cumLen[i + 1] - ss) / lenStep[i] +
                        (dataX[i + 1] - xp[i + 1]) * (ss - cumLen[i]) / lenStep[i];
                    y1 = (yp[i] * Math.Sinh(e1) + yp[i + 1] * Math.Sinh(e2)) / Math.Sinh(e3) +
                        (dataY[i] - yp[i]) * (cumLen[i + 1] - ss) / lenStep[i] +
                        (dataY[i + 1] - yp[i + 1]) * (ss - cumLen[i]) / lenStep[i];
                    yield return new Vector2(x1, y1);
                }
            }
            // 端点2
            if (outEnd) yield return points[i + 1];
        }
    }

    // 张力样条单位
    public class TensionNode
    {
        TensionSpline parent;
        int index;
        bool updated;

        public List<Vector2> data;
        public Rect MBR;

        public TensionNode(TensionSpline origin, int idx)
        {
            parent = origin;
            index = idx;
            Reset();
        }

        // 判断相交
        public bool Crossing(TensionNode other)
        {
            Update(); other.Update();
            if ((MBR & other.MBR) == null) return false;
            for (int i = 0; i < data.Count - 1; i++)
                for (int j = 0; j < other.data.Count - 1; j++)
                {
                    var crossing = Utils.CheckCross(data[i], data[i + 1], other.data[j], other.data[j + 1]);
                    if (crossing != null &&
                        crossing.Item1 >= 0 && crossing.Item1 <= 1 &&
                        crossing.Item2 >= 0 && crossing.Item2 <= 1) return true;
                }
            return false;
        }

        // 输出点
        public IEnumerable<Vector2> Points(bool outStart = true)
        {
            Update();
            for (int i = 1 - Convert.ToInt32(outStart); i < data.Count; i++) yield return data[i];
        }

        // 重置分段
        public void Reset()
        {
            data = null;
            MBR = null;
            updated = false;
        }

        // 计算分段内容
        public void Update()
        {
            if (updated) return;
            data = new List<Vector2>(parent.Smooth(index, true, true));
            MBR = new Rect(data.Select<Vector2, GeomPoint>(v => (GeomPoint)v));
            updated = true;
        }
    }
}
