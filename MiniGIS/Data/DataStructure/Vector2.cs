using MiniGIS.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MiniGIS.Data
{
    // 二维向量
    public class Vector2 : IComparable<Vector2>
    {
        // XY坐标
        public double X, Y;

        // 操作符
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);
        public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);
        public static Vector2 operator *(Vector2 a, double b) => new Vector2(a.X * b, a.Y * b);
        public static Vector2 operator /(Vector2 a, double b) => new Vector2(a.X / b, a.Y / b);

        // 比较相等
        public override int GetHashCode() => X.GetHashCode() ^ Y.GetHashCode();
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                default: return false;
                case Vector2 v:
                    return X == v.X && Y == v.Y;
            }
        }
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            if ((object)a == null) return (object)b == null;
            return a.Equals(b);
        }
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);

        // 大小关系
        public int CompareTo(Vector2 other)
        {
            if (X != other.X) return Math.Sign(X - other.X);
            return Math.Sign(Y - other.Y);
        }
        public static bool operator <(Vector2 a, Vector2 b) => a.CompareTo(b) < 0;
        public static bool operator >(Vector2 a, Vector2 b) => a.CompareTo(b) > 0;
        public static bool operator <=(Vector2 a, Vector2 b) => a.CompareTo(b) <= 0;
        public static bool operator >=(Vector2 a, Vector2 b) => a.CompareTo(b) >= 0;

        // 初始化方法
        public Vector2(double x = 0, double y = 0)
        {
            X = x;
            Y = y;
        }

        public static implicit operator PointF(Vector2 v) => new PointF((float)v.X, (float)v.Y);
        public static explicit operator GeomPoint(Vector2 v) => new GeomPoint(v.X, v.Y);
        public static implicit operator Vector2(GeomPoint p) => new Vector2(p.X, p.Y);

        public Vector2(GeomPoint p1, GeomPoint p2)
        {
            X = p2.X - p1.X;
            Y = p2.Y - p1.Y;
        }

        // 字符化
        public override string ToString() => String.Format("({0}, {1})", X, Y);
    }

    public static class Vector2Ext
    {
        // 角度
        public static double CosAngle(this Vector2 a, Vector2 b)
        {
            double lab = a.Length() * b.Length();
            if (lab == 0) return 1;
            return a.Dot(b) / lab;
        }
        public static double Rotation(this Vector2 a) => Math.Atan2(a.Y, a.X);

        // 长度
        public static double Length(this Vector2 a) => Math.Sqrt(a.X * a.X + a.Y * a.Y);
        public static double LengthSq(this Vector2 a) => a.X * a.X + a.Y * a.Y;
        public static double Distance(this Vector2 a, Vector2 b) => (a - b).Length();
        public static double DistanceSq(this Vector2 a, Vector2 b) => (a - b).LengthSq();

        // 叉乘
        public static double Cross(this Vector2 a, Vector2 b) => a.X * b.Y - a.Y * b.X;

        // 点乘
        public static double Dot(this Vector2 a, Vector2 b) => a.X * b.X + a.Y * b.Y;
    }
}
