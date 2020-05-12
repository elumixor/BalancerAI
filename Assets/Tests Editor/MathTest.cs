using System;
using NN;
using Num;
using NUnit.Framework;
using UnityEngine;

namespace Tests_Editor {
    public class MathTest {
        // A Test behaves as an ordinary method
        [Test]
        public void VectorOperations() {
            Vector a = new float[] {1, 2, 3};
            Vector b = new float[] {1, 3, 3};
            var res = a + b;

            Assert.AreEqual(res, new Vector(2, 5, 6));
            res = -a;

            Assert.AreEqual(res, new Vector(-1, -2, -3));

            var resFloat = a.Dot(b);
            Assert.AreEqual(resFloat, 1 * 1 + 2 * 3 + 3 * 3);
        }

        [Test]
        public void MatrixDot() {
            Matrix a = new float[,] {
                {1, 2, 3},
                {3, 2, 1}
            };

            Matrix b = new float[,] {
                {1, 1, 1},
                {0, 1, 2}
            };

            Assert.Catch<Exception>(() => { a.Dot(b); });

            Assert.AreEqual(a.T, new float[,] {{1, 3}, {2, 2}, {3, 1}});
            Assert.AreEqual(a.T.Dot(b), new float[,] {{1, 4, 7}, {2, 4, 6}, {3, 4, 5}});
            Assert.AreEqual(a.Dot(b.T), new float[,] {{6, 8}, {6, 4}});
        }

        [Test]
        public void LinearTest() {
            // var l = new Linear(1, 1) {w = new[,] {{-0.98f}}, b = new[] {-0.2f}};
            //
            // Debug.Log(l.Forward((Vector) 11.09449f));
            // Debug.Log(-0.98f * 11.09449f - 0.2f);

            Matrix w = new[,] {{0f}};
            Vector b = new[] {1f};
            Vector x = new[] {11f};
            // var l = new Linear(1, 1) {w = new[,] {{0f}}, b = new[] {-1f}};

            // Debug.Log(l.Forward((Vector) 11.09449f));
            Debug.Log(x + b);
            Debug.Log(x - b);

            Debug.Log(w.Dot(x));
        }
    }
}