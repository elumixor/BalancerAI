using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class F {
    public static IEnumerable<T> SliceRow<T>(this T[,] array, int row) {
        for (var i = array.GetLowerBound(1); i <= array.GetUpperBound(1); i++) {
            yield return array[row, i];
        }
    }

    public static IEnumerable<T> SliceColumn<T>(this T[,] array, int column) {
        for (var i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++) {
            yield return array[i, column];
        }
    }

    public static T[,] ToRectangular<T>(this T[][] arr) {
        var height = arr.Length;
        if (height == 0) return new T[0, 0];

        var width = arr[0].Length;
        var res = new T[height, width];

        for (var i = 0; i < height; i++)
        for (var j = 0; j < width; j++)
            res[i, j] = arr[i][j];

        return res;
    }

    private static void CheckSize(float[] a, float[] b) {
        if (a.Length != b.Length) throw new Exception($"Vectors had different size {a.Length} != {b.Length}");
    }

    public static float[] add(this float[] a, float[] b) {
        CheckSize(a, b);

        var res = new float[a.Length];
        for (var i = 0; i < a.Length; i++) {
            res[i] = a[i] + b[i];
        }

        return res;
    }

    public static float[] neg(this float[] x) {
        var res = new float[x.Length];
        for (var i = 0; i < x.Length; i++) {
            res[i] = -x[i];
        }

        return res;
    }

    public static float[] sub(this float[] a, float[] b) => a.add(b.neg());

    public static float[] mul(this float[] x, float scal) {
        var res = new float[x.Length];
        for (var i = 0; i < x.Length; i++) {
            res[i] = x[i] * scal;
        }

        return res;
    }

    public static float dot(this float[] a, float[] b) {
        CheckSize(a, b);
        return a.Select((t, i) => t * b[i]).Sum();
    }

    public static float[] dot(this float[,] mat, float[] vec) {
        var height = mat.GetLength(0);
        var res = new float[height];

        for (var i = 0; i < height; i++)
            res[i] = mat.SliceRow(i).ToArray().dot(vec);

        return res;
    }

    public static float[,] Transpose(this float[,] mat) {
        var height = mat.GetLength(0);
        var width = mat.GetLength(1);

        var res = new float[width, height];
        for (var i = 0; i < height; i++)
        for (var j = 0; j < width; j++)
            res[j, i] = mat[i, j];

        return res;
    }

    public static float Sigmoid(this float x) {
        return 1 / (1 + Mathf.Exp(-x));
    }

    public static float[,] dot(this float[,] a, float[,] b) {
        var m = a.GetLength(0);
        var n = a.GetLength(1);

        var k = b.GetLength(0);
        var l = b.GetLength(1);

        // n should equal k
        var result = new float[m, l];
        for (var i = 0; i < m; i++)
        for (var j = 0; j < l; j++)
            result[i, j] = a.SliceRow(i).ToArray().dot(b.SliceColumn(j).ToArray());

        return result;
    }

    public static int Height(this float[,] mat) => mat.GetLength(0);
    public static int Width(this float[,] mat) => mat.GetLength(1);
    public static (int, int) Shape(this float[,] mat) => (mat.Height(), mat.Width());

    public static IEnumerable<T> Rows<T>(this float[,] mat, Func<float[], T> func) {
        var height = mat.Height();
        for (var i = 0; i < height; i++)
            yield return func(mat.SliceRow(i).ToArray());
    }

    public static string Str(this float[,] mat) {
        return string.Join("\n", mat.Rows(r => string.Join(", ", r.ToArray())));
    }

    public static string Str(this IReadOnlyCollection<float> vec) {
        return string.Join(", ", vec);
    }
}