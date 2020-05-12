using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Communication;
using UnityEngine;
using R = Num.Random;

namespace Num {
    public readonly struct Vector : IEnumerable<float>, IToBytes {
        public readonly float[] elements;
        public readonly int size;

        public Vector(params float[] elements) {
            this.elements = elements;
            size = elements.Length;
        }

        public Vector(IEnumerable<float> elements) {
            this.elements = elements.ToArray();
            size = this.elements.Length;
        }

        public static implicit operator Vector(float[] elements) => new Vector(elements);
        public static explicit operator Vector(float element) => new[] {element};

        public float this[int i] => elements[i];
        public IEnumerator<float> GetEnumerator() => ((IEnumerable<float>) elements).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Vector Zeros(int size) => new float[size];

        public static Vector Ones(int size) {
            var e = new float[size];
            for (var i = 0; i < size; i++) e[i] = 1f;
            return e;
        }

        public static Vector Random(int size, float min = 0f, float max = 1f) {
            var e = new float[size];
            for (var i = 0; i < size; i++) e[i] = R.Range(min, max);
            return e;
        }

        private static void CheckSize(Vector a, Vector b) {
            if (a.size != b.size) throw new DimensionException($"Vectors had different size {a.size} != {b.size}");
        }

        public static Vector operator +(Vector a, Vector b) {
            CheckSize(a, b);

            var res = new float[a.size];
            for (var i = 0; i < a.size; i++) res[i] = a[i] + b[i];

            return res;
        }

        public static Vector operator +(float a, Vector x) {
            var res = new float[x.size];
            for (var i = 0; i < x.size; i++) res[i] = x[i] + a;

            return res;
        }

        public static Vector operator *(Vector a, Vector b) {
            CheckSize(a, b);

            var res = new float[a.size];
            for (var i = 0; i < a.size; i++) res[i] = a[i] * b[i];

            return res;
        }

        public static Vector operator -(Vector x) {
            var res = new float[x.size];
            for (var i = 0; i < x.size; i++) res[i] = -x[i];

            return res;
        }

        public static Vector operator *(Vector x, float a) {
            var res = new float[x.size];
            for (var i = 0; i < x.size; i++) res[i] = x[i] * a;

            return res;
        }

        public static Vector operator *(float a, Vector x) => x * a;
        public static Vector operator /(Vector x, float a) => x * (1f / a);
        public static Vector operator +(Vector x, float a) => a + x;
        public static Vector operator -(Vector a, Vector b) => a + -b;
        public static Vector operator -(Vector x, float a) => x + -a;
        public static Vector operator -(float a, Vector x) => -x + a;

        public float Dot(Vector b) {
            CheckSize(this, b);
            var s = 0f;
            for (var i = 0; i < size; i++) s += elements[i] * b[i];

            return s;
        }

        public Matrix Outer(Vector b) {
            var res = new float[size, b.size];
            for (var i = 0; i < size; i++)
            for (var j = 0; j < b.size; j++)
                res[i, j] = elements[i] * b[j];
            return res;
        }

        public float Mean => elements.Average();

        public float STD {
            get {
                var m = Mean;
                var r = 0f;
                foreach (var e in this) {
                    var d = e - m;
                    r += d * d;
                }

                return Mathf.Sqrt(r / (size - 1));
            }
        }
        public byte[] ToBytes() {
            var byteArray = new byte[sizeof(int) + size * sizeof(float)];
            Buffer.BlockCopy(new[] {size}, 0, byteArray, 0, sizeof(int));
            Buffer.BlockCopy(elements, 0, byteArray, sizeof(int), byteArray.Length - sizeof(int));
            return byteArray;
        }

        public override string ToString() {
            return $"[{string.Join(", ", elements.Select(e => e.ToString("##.##")))}]";
        }
    }

    public static class VectorExtensions {
        public static Vector Normalize(this Vector x) => (x - x.Mean) / x.STD;

        public static Vector Map(this Vector x, Func<float, float> mapper) {
            var r = new float[x.size];
            for (var i = 0; i < x.size; i++) r[i] = mapper(x[i]);
            return r;
        }

        public static Vector Max(this Vector x, float v) => x.Map(el => el < v ? el : v);
        public static Vector Min(this Vector x, float v) => x.Map(el => el > v ? el : v);
        public static Vector Clamp(this Vector x, float min = 0f, float max = 1f) => x.Max(min).Min(max);
        public static Vector Sigmoid(this Vector x) => x.Map(F.Sigmoid);
    }
}