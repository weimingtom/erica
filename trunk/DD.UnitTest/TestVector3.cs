using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestVector3 {
        [TestMethod]
        public void Test_New () {
            var p = new Vector3 (1, 2, 3);

            Assert.AreEqual (3, p.ComponentCount);
            Assert.AreEqual (1, p.X);
            Assert.AreEqual (2, p.Y);
            Assert.AreEqual (3, p.Z);
        }

        [TestMethod]
        public void Test_Indexer () {
            var p = new Vector3 (1, 2, 3);

            Assert.AreEqual (1, p[0]);
            Assert.AreEqual (2, p[1]);
            Assert.AreEqual (3, p[2]);
        }

        [TestMethod]
        public void Test_Length () {
            var p = new Vector3 (1, 2, 3);
            var expected1 = (float)Math.Sqrt (1 + 4 + 9);
            var expected2 = 1 + 4 + 9;

            Assert.AreEqual (expected1, p.Length, 0.0001f);
            Assert.AreEqual (expected2, p.Length2, 0.0001f);
        }

        [TestMethod]
        public void Test_Normalize () {
            var p = new Vector3 (1,  2 ,  3).Normalize();

            Assert.AreEqual (1, p.Length, 0.0001f);
        }

        [TestMethod]
        public void Test_Vector_x_Float () {
            var v1 = new Vector3 (1, 2, 3) * 2;
            var v2 = 2 * new Vector3 (1, 2, 3);
            var v3 = new Vector3 (1, 2, 3) / 0.5f;
            var v4 = new Vector3 (1, 2, 3) + new Vector3(1,2,3);
            var v5 = new Vector3 (1, 2, 3) - new Vector3 (-1, -2, -3);
            var v6 = - new Vector3 (-2, -4, -6);

            var expected = new Vector3 (2, 4, 6);

            Assert.AreEqual (expected, v1);
            Assert.AreEqual (expected, v2);
            Assert.AreEqual (expected, v3);
            Assert.AreEqual (expected, v4);
            Assert.AreEqual (expected, v5);
            Assert.AreEqual (expected, v6);
        }

        [TestMethod]
        public void Test_Dot () {
            var a = new Vector3 (1, 2, 3);
            var b = new Vector3 (1, 2, 3);
            var expected = 14;

            Assert.AreEqual (expected, Vector3.Dot(a, b));
        }

        [TestMethod]
        public void Test_Cross () {
            var a = new Vector3 (1, 2, 3);
            var b = new Vector3 (2, 2, -1);
            var expected = new Vector3(-8,7,-2);

            Assert.AreEqual (expected, Vector3.Cross (a, b));
        }

        [TestMethod]
        public void Test_Equals () {
            var a = new Vector3 (1, 2, 3.00001f);
            var b = new Vector3 (1, 2, 3.00002f);

            Assert.IsTrue (a.Equals (b));   // 誤差を許容する
            Assert.IsFalse (a == b);        // 厳密な比較
            Assert.AreNotEqual (a.GetHashCode (), b.GetHashCode ()); // ハッシュは厳密な比較を基準
        }
    }
}
