using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using R = Num.Random;

namespace Num {
    public readonly struct Matrix : IEnumerable<float> {
        public readonly float[,] elements;
        public readonly int height;
        public readonly int width;
        public int Size { get; }
        public (int height, int width) Shape { get; }

        public Matrix(float[,] elements) {
            this.elements = elements;
            height = elements.GetLength(0);
            width = elements.GetLength(1);
            Size = width * height;
            Shape = (height, width);
        }

        public static implicit operator Matrix(float[,] elements) => new Matrix(elements);

        public static explicit operator Matrix(Vector v) {
            var elements = new float[1, v.size];
            for (var i = 0; i < v.size; i++) elements[0, i] = v[i];
            return elements;
        }

        public float this[int i, int j] => elements[i, j];

        public IEnumerator<float> GetEnumerator() {
            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
                yield return elements[i, j];
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public Vector Row(int i) {
            var res = new float[width];
            for (var j = 0; j < width; j++)
                res[j] = elements[i, j];
            return res;
        }

        public Vector Column(int j) {
            var res = new float[height];
            for (var i = 0; i < height; i++)
                res[i] = elements[i, j];
            return res;
        }

        public IEnumerable<Vector> Rows {
            get {
                for (var i = 0; i < height; i++)
                    yield return Row(i);
            }
        }

        public IEnumerable<Vector> Columns {
            get {
                for (var j = 0; j < width; j++)
                    yield return Column(j);
            }
        }

        public static Matrix Zeros((int height, int width) shape) => new float[shape.height, shape.width];

        public static Matrix Ones((int height, int width) shape) {
            var (height, width) = shape;

            var e = new float[height, width];
            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
                e[i, j] = 1f;

            return e;
        }

        public static Matrix Random((int height, int width) shape, float min = 0f, float max = 1f) {
            var (height, width) = shape;

            var e = new float[height, width];
            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
                e[i, j] = R.Range(min, max);

            return e;
        }

        private static void CheckSize(Matrix a, Matrix b) {
            if (a.height != b.height || a.width != b.width)
                throw new DimensionException($"Matrices had different dimensions {a.Shape.ToString()} != {b.Shape.ToString()}");
        }

        public static Matrix operator +(Matrix a, Matrix b) {
            CheckSize(a, b);
            var res = new float[a.height, a.width];

            for (var i = 0; i < a.height; i++)
            for (var j = 0; j < a.width; j++)
                res[i, j] = a[i, j] + b[i, j];

            return res;
        }

        public static Matrix operator -(Matrix m) {
            var res = new float[m.height, m.width];

            for (var i = 0; i < m.height; i++)
            for (var j = 0; j < m.width; j++)
                res[i, j] = -m[i, j];

            return res;
        }

        public static Matrix operator *(Matrix m, float a) {
            var res = new float[m.height, m.width];

            for (var i = 0; i < m.height; i++)
            for (var j = 0; j < m.width; j++)
                res[i, j] = m[i, j] * a;

            return res;
        }

        public static Matrix operator +(Matrix m, float a) {
            var res = new float[m.height, m.width];

            for (var i = 0; i < m.height; i++)
            for (var j = 0; j < m.width; j++)
                res[i, j] = m[i, j] + a;

            return res;
        }

        public static Matrix operator *(float a, Matrix m) => m * a;
        public static Matrix operator +(float a, Matrix m) => m + a;
        public static Matrix operator -(Matrix m, float a) => m + -a;
        public static Matrix operator -(float a, Matrix m) => -m + a;
        public static Matrix operator /(Matrix m, float a) => m * (1f / a);

        public Vector Flat => new Vector(this);

        public Matrix T {
            get {
                var res = new float[width, height];
                for (var i = 0; i < height; i++)
                for (var j = 0; j < width; j++)
                    res[j, i] = this[i, j];

                return res;
            }
        }

        public Matrix Dot(Matrix b) {
            if (width != b.height)
                throw new DimensionException($"Matrices had incompatible shapes for dot: {Shape.ToString()} and {b.Shape.ToString()}");

            var res = new float[height, b.width];
            for (var i = 0; i < height; i++)
            for (var j = 0; j < b.width; j++)
                res[i, j] = Row(i).Dot(b.Column(j));

            return res;
        }

        public Vector Dot(Vector v) {
            if (width != v.size)
                throw new DimensionException($"Matrix and vector had incompatible shapes for dot: {Shape.ToString()} and {v.size}");

            var res = new float[height];
            for (var i = 0; i < height; i++)
                res[i] = Row(i).Dot(v);

            return res;
        }

        public override string ToString() {
            return $"[{string.Join("\n ", Rows.Select(r => r.ToString()))}]";
        }
    }
}