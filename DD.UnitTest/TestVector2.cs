using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestVector2 {
        [TestMethod]
        public void Test_New_1 () {
            var v = new Vector2 ();

            Assert.AreEqual (2, v.ComponentCount);
            Assert.AreEqual (0, v.X);
            Assert.AreEqual (0, v.Y);
            Assert.AreEqual (0, v[0]);
            Assert.AreEqual (0, v[1]);
        }

        [TestMethod]
        public void Test_New_2 () {
            var v = new Vector2 (1, 2);

            Assert.AreEqual (2, v.ComponentCount);
            Assert.AreEqual (1, v.X);
            Assert.AreEqual (2, v.Y);
            Assert.AreEqual (1, v[0]);
            Assert.AreEqual (2, v[1]);
        }

        [TestMethod]
        public void Test_New_Length () {
            var v = new Vector2 (3, 4);

            Assert.AreEqual (5.0f, v.Length, 0.0001f);
            Assert.AreEqual (25.0f, v.Length2, 0.0001f);
        }

        [TestMethod]
        public void Test_New_Normalize () {
            var v = new Vector2 (3, 4).Normalize ();

            Assert.AreEqual (0.6f, v.X, 0.0001f);
            Assert.AreEqual (0.8f, v.Y, 0.0001f);
        }

        [TestMethod]
        public void Test_Promote_to_Vector3 () {
            var v = (Vector3)new Vector2 (1, 2);

            Assert.AreEqual (1, v.X);
            Assert.AreEqual (2, v.Y);
            Assert.AreEqual (0, v.Z);
        }

        [TestMethod]
        public void Test_Operator () {
            var a = new Vector2 (1, 2);
            var b = new Vector2 (3, 4);

            Assert.AreEqual (new Vector2 (4, 6), a + b);
            Assert.AreEqual (new Vector2 (-2, -2), a - b);
            Assert.AreEqual (new Vector2 (2, 4), a * 2);
            Assert.AreEqual (new Vector2 (2, 4), 2 * a);
            Assert.AreEqual (new Vector2 (0.5f, 1.0f), a / 2);
        }

        [TestMethod]
        public void Test_Dot () {
            var a = new Vector2 (1, 2);
            var b = new Vector2 (3, 4);

            Assert.AreEqual (11.0f, Vector2.Dot (a, b), 0.0001f);
        }

        [TestMethod]
        public void Test_Equals () {
            var a = new Vector2 (1, 2.00001f);
            var b = new Vector2 (1, 2.00002f);

            Assert.IsTrue (a.Equals (b));   // 誤差を許容する比較
            Assert.IsFalse (a == b);        // 厳密な比較
            Assert.AreNotEqual (a.GetHashCode (), b.GetHashCode ()); // ハッシュは厳密な比較を基準
   
        }


    }
}
