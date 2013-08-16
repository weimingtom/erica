using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestVector3 {
        [TestMethod]
        public void Test_New_1 () {
            var p = new Vector3 (1, 2, 3);

            Assert.AreEqual (3, p.ComponentCount);
            Assert.AreEqual (1, p.X);
            Assert.AreEqual (2, p.Y);
            Assert.AreEqual (3, p.Z);
        }

        [TestMethod]
        public void Test_New_2 () {
            var p = new Vector3 (new Vector2(1, 2), 3);

            Assert.AreEqual (3, p.ComponentCount);
            Assert.AreEqual (1, p.X);
            Assert.AreEqual (2, p.Y);
            Assert.AreEqual (3, p.Z);
        }

        [TestMethod]
        public void Test_Vector3 () {

            Assert.AreEqual (0f, Vector3.Zero.X);
            Assert.AreEqual (0f, Vector3.Zero.Y);
            Assert.AreEqual (0f, Vector3.Zero.Z);
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
        public void Test_Angle () {
            var r = 2.0f;
            var a = 45 / 180.0f * Math.PI;

            var v0 = new Vector3 (0, 0, 0);
            var v1 = new Vector3 ((float)(r * Math.Cos (a * 0)), (float)(r * Math.Sin (a * 0)), 0);
            var v2 = new Vector3 ((float)(r * Math.Cos (a * 1)), (float)(r * Math.Sin (a * 1)), 0);
            var v3 = new Vector3 ((float)(r * Math.Cos (a * 2)), (float)(r * Math.Sin (a * 2)), 0);
            var v4 = new Vector3 ((float)(r * Math.Cos (a * 3)), (float)(r * Math.Sin (a * 3)), 0);
            var v5 = new Vector3 ((float)(r * Math.Cos (a * 4)), (float)(r * Math.Sin (a * 4)), 0);
            var v6 = new Vector3 ((float)(r * Math.Cos (a * 5)), (float)(r * Math.Sin (a * 5)), 0);
            var v7 = new Vector3 ((float)(r * Math.Cos (a * 6)), (float)(r * Math.Sin (a * 6)), 0);
            var v8 = new Vector3 ((float)(r * Math.Cos (a * 7)), (float)(r * Math.Sin (a * 7)), 0);

            Assert.AreEqual (0, Vector3.Angle (v1, v1), 0.0001f);
            Assert.AreEqual (45, Vector3.Angle (v1, v2), 0.0001f);
            Assert.AreEqual (90, Vector3.Angle (v1, v3), 0.0001f);
            Assert.AreEqual (135, Vector3.Angle (v1, v4), 0.0001f);
            Assert.AreEqual (180, Vector3.Angle (v1, v5), 0.0001f);
            Assert.AreEqual (135, Vector3.Angle (v1, v6), 0.0001f);
            Assert.AreEqual (90, Vector3.Angle (v1, v7), 0.0001f);
            Assert.AreEqual (45, Vector3.Angle (v1, v8), 0.0001f);
            Assert.AreEqual (0, Vector3.Angle (v1, v1), 0.0001f);

            Assert.AreEqual (0, Vector3.Angle (v1, v1), 0.0001f);
            Assert.AreEqual (45, Vector3.Angle (v2, v1), 0.0001f);
            Assert.AreEqual (90, Vector3.Angle (v3, v1), 0.0001f);
            Assert.AreEqual (135, Vector3.Angle (v4, v1), 0.0001f);
            Assert.AreEqual (180, Vector3.Angle (v5, v1), 0.0001f);
            Assert.AreEqual (135, Vector3.Angle (v6, v1), 0.0001f);
            Assert.AreEqual (90, Vector3.Angle (v7, v1), 0.0001f);
            Assert.AreEqual (45, Vector3.Angle (v8, v1), 0.0001f);
            Assert.AreEqual (0, Vector3.Angle (v1, v1), 0.0001f);

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

            Assert.IsTrue (a.Equals (b));   // 誤差を許容する比較
            Assert.IsFalse (a == b);        // 厳密な比較
            Assert.AreNotEqual (a.GetHashCode (), b.GetHashCode ()); // ハッシュは厳密な比較を基準
        }
    }
}
