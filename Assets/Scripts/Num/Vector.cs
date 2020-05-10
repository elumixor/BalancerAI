using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NN;
using UnityEngine;

namespace Num {
    public readonly struct Vector : IEnumerable<float> {
        private readonly float[] elements;
        public readonly int size;

        public Vector(float[] elements) {
            this.elements = elements;
            size = elements.Length;
        }

        public Vector(IEnumerable<float> elements) {
            this.elements = elements.ToArray();
            size = this.elements.Length;
        }

        public static implicit operator Vector(float[] elements) => new Vector(elements);

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
            var r = new System.Random();
            var e = new float[size];
            for (var i = 0; i < size; i++) e[i] = (float) (r.NextDouble() * (max - min) + min);
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
            for (var i = 0; i < size; i++) s += elements[i] + b[i];

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

        public Vector Normalize() => (this - Mean) / STD;
    }
}