using System;
using System.Collections.Generic;
using System.Linq;
using Basic;
using Communication;
using Num;
using UnityEngine;

namespace NN {
    public static class ByteTransformer {
        public static float[] ReadArray(byte[] bytes, int offset, out int newOffset) {
            var length = BitConverter.ToInt32(bytes, offset);
            var res = new float[length];

            unsafe {
                fixed (byte* pBuffer = bytes) {
                    var pSample = (float*) (pBuffer + 4 + offset);
                    for (var i = 0; i < length; i++) res[i] = pSample[i];
                }
            }

            newOffset = offset + 4 * (length + 1);
            return res;
        }

        public static (float[] shape, float[] data) ReadTensor(byte[] bytes, int offset, out int newOffset) {
            var shape = ReadArray(bytes, offset, out newOffset);
            var data = ReadArray(bytes, newOffset, out newOffset);

            return (shape, data);
        }

        public static Matrix ToMatrix(this byte[] bytes, int offset, out int newOffset) {
            var tensor = ReadTensor(bytes, offset, out newOffset);
            var (shape, data) = tensor;

            var height = (int) shape[0];
            var width = (int) shape[1];

            var matrix = new float[height, width];

            for (var i = 0; i < height; i++)
            for (var j = 0; j < width; j++)
                matrix[i, j] = data[i * width + j];

            return matrix;
        }

        public static Vector ToVector(this byte[] bytes, int offset, out int newOffset) {
            var tensor = ReadTensor(bytes, offset, out newOffset);
            var (shape, data) = tensor;

            var height = (int) shape[0];
            var vector = new float[height];

            for (var i = 0; i < height; i++)
                vector[i] = data[i];

            return vector;
        }

        public static byte[] ToBytes<T>(this IEnumerable<T> vector, int size) {
            var v = vector.ToArray();
            var byteArray = new byte[sizeof(int) + v.Length * size];
            Buffer.BlockCopy(new[] {v.Length}, 0, byteArray, 0, sizeof(int));
            Buffer.BlockCopy(v, 0, byteArray, sizeof(int), byteArray.Length - sizeof(int));
            return byteArray;
        }

        public static byte[] ToBytes(this IEnumerable<int> vector) => ToBytes(vector, sizeof(int));
        public static byte[] ToBytes(this IEnumerable<float> vector) => ToBytes(vector, sizeof(float));

        public static byte[] ToBytes<T>(this T value, int size) {
            var byteArray = new byte[size];
            Buffer.BlockCopy(new[] {value}, 0, byteArray, 0, size);
            return byteArray;
        }

        public static byte[] ToBytes(this int value) => ToBytes(value, sizeof(int));
        public static byte[] ToBytes(this float value) => ToBytes(value, sizeof(float));

        public static byte[] ToBytes<T>(this IEnumerable<T> enumerable) where T : IToBytes {
            var arr = enumerable as T[] ?? enumerable.ToArray();
            return Concat(arr.Length.ToBytes(), Concat(arr.Select(e => e.ToBytes())));
        }

        public static byte[] Concat(this IEnumerable<byte[]> bytes) {
            var bts = bytes.ToArray();
            var s = bts.Sum(bArr => bArr.Length);
            var z = new byte[s];

            var off = 0;
            foreach (var b in bts) {
                b.CopyTo(z, off);
                off += b.Length;
            }

            return z;
        }

        public static byte[] Concat(params byte[][] bytes) => bytes.Concat();
    }
}