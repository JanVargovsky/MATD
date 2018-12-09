using System;

namespace MATD.Lesson8
{
    public static class VectorModelMath
    {
        public static double DotProduct(double[] a, double[] b)
        {
            double result = 0d;
            for (int i = 0; i < a.Length; i++)
                result += a[i] * b[i];
            return result;
        }

        public static double Length(double[] a)
        {
            double length = 0d;
            for (int i = 0; i < a.Length; i++)
                length += a[i] * a[i];
            length = Math.Sqrt(length);
            return length;
        }

        public static double[] Normalize(double[] a, bool inplace)
        {
            double[] normalized = inplace ? a : new double[a.Length];
            double length = Length(a);
            for (int i = 0; i < a.Length; i++)
                normalized[i] = a[i] / length;
            return normalized;
        }
    }
}
