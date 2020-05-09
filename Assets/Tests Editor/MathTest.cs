using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests {
    public class MathTest {
        // A Test behaves as an ordinary method
        [Test]
        public void VectorOperations() {
            var a = new float[] {1, 2, 3};
            var b = new float[] {1, 3, 3};
            var res = a.add(b);

            Assert.AreEqual(res, new float[] {2, 5, 6});
            res = a.neg();

            Assert.AreEqual(res, new float[] {-1, -2, -3});

            var resFloat = a.dot(b);
            Assert.AreEqual(resFloat, 1 * 1 + 2 * 3 + 3 * 3);
        }

        [Test]
        public void MatrixDot() {
            var a = new float[,] {
                {1, 2, 3},
                {3, 2, 1}
            };

            var b = new float[,] {
                {1, 1, 1},
                {0, 1, 2}
            };

            Assert.Catch<Exception>(() => {
                var res = a.dot(b);
                Debug.Log(res.Str());
            });

            Assert.AreEqual(a.Transpose(), new float[,] {{1, 3}, {2, 2}, {3, 1}});
            Assert.AreEqual(a.Transpose().dot(b), new float[,] {{1, 4, 7}, {2, 4, 6}, {3, 4, 5}});
            Assert.AreEqual(a.dot(b.Transpose()), new float[,] {{6, 8}, {6, 4}});
        }
    }
}