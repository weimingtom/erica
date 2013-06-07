using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestMatrix3x3 {

        [TestMethod]
        public void Test_New_0 () {
            var m = new Matrix3x3 ();

            Assert.AreEqual (9, m.ComponentCount);
            Assert.AreEqual (0, m.M00);
            Assert.AreEqual (0, m.M01);
            Assert.AreEqual (0, m.M02);
            Assert.AreEqual (0, m.M10);
            Assert.AreEqual (0, m.M11);
            Assert.AreEqual (0, m.M12);
            Assert.AreEqual (0, m.M20);
            Assert.AreEqual (0, m.M21);
            Assert.AreEqual (0, m.M22);
        }

        [TestMethod]
        public void Test_New_1 () {
            var m = new Matrix3x3 (1, 2, 3,
                                  4, 5, 6,
                                  7, 8, 9);
            Assert.AreEqual (1, m.M00);
            Assert.AreEqual (2, m.M01);
            Assert.AreEqual (3, m.M02);
            Assert.AreEqual (4, m.M10);
            Assert.AreEqual (5, m.M11);
            Assert.AreEqual (6, m.M12);
            Assert.AreEqual (7, m.M20);
            Assert.AreEqual (8, m.M21);
            Assert.AreEqual (9, m.M22);
        }

        [TestMethod]
        public void Test_New_2 () {
            var m = new Matrix3x3 (new float[]{1, 2, 3,
                                               4, 5, 6,
                                               7, 8, 9});
            Assert.AreEqual (1, m.M00);
            Assert.AreEqual (2, m.M01);
            Assert.AreEqual (3, m.M02);
            Assert.AreEqual (4, m.M10);
            Assert.AreEqual (5, m.M11);
            Assert.AreEqual (6, m.M12);
            Assert.AreEqual (7, m.M20);
            Assert.AreEqual (8, m.M21);
            Assert.AreEqual (9, m.M22);
        }

      
        [TestMethod]
        public void Test_Indexer () {
            var m = new Matrix3x3 (1, 2, 3,
                                   4, 5, 6,
                                   7, 8, 9);
            Assert.AreEqual (1, m[0]);
            Assert.AreEqual (2, m[1]);
            Assert.AreEqual (3, m[2]);
            Assert.AreEqual (4, m[3]);
            Assert.AreEqual (5, m[4]);
            Assert.AreEqual (6, m[5]);
            Assert.AreEqual (7, m[6]);
            Assert.AreEqual (8, m[7]);
            Assert.AreEqual (9, m[8]);
        }


        [TestMethod]
        public void Test_Convert_1 () {
            var mat = new Matrix3x3 (1, 2, 3,
                                     4, 5, 6,
                                     7, 8, 9);
            var m = (float[])mat;
            Assert.AreEqual (1, m[0]);
            Assert.AreEqual (2, m[1]);
            Assert.AreEqual (3, m[2]);
            Assert.AreEqual (4, m[3]);
            Assert.AreEqual (5, m[4]);
            Assert.AreEqual (6, m[5]);
            Assert.AreEqual (7, m[6]);
            Assert.AreEqual (8, m[7]);
            Assert.AreEqual (9, m[8]);
        }

        [TestMethod]
        public void Test_Convert_2 () {
            var mat = new Matrix3x3 (1, 2, 3,
                                     4, 5, 6,
                                     7, 8, 9);
            var m = (Matrix4x4)mat;
            Assert.AreEqual (1, m[0]);
            Assert.AreEqual (2, m[1]);
            Assert.AreEqual (3, m[2]);
            Assert.AreEqual (0, m[3]);
            Assert.AreEqual (4, m[4]);
            Assert.AreEqual (5, m[5]);
            Assert.AreEqual (6, m[6]);
            Assert.AreEqual (0, m[7]);
            Assert.AreEqual (7, m[8]);
            Assert.AreEqual (8, m[9]);
            Assert.AreEqual (9, m[10]);
            Assert.AreEqual (0, m[11]);
            Assert.AreEqual (0, m[12]);
            Assert.AreEqual (0, m[13]);
            Assert.AreEqual (0, m[14]);
            Assert.AreEqual (1, m[15]);
        }

        [TestMethod]
        public void Test_Determinant () {
            var m = new Matrix3x3 (1, 2, 1,
                                   1, 1, -1,
                                   2, 1, 1);
            Assert.AreEqual (-5, m.Determinant);
        }

        [TestMethod]
        public void Test_Inverse () {
            var m = new Matrix3x3 (1, 2, 1,
                                   1, 1, -1,
                                   2, 1, 1);
            var inversed = m.Inverse ();
            var expected = new Matrix3x3 (-0.4f, 0.2f, 0.6f,
                                          0.6f, 0.2f, -0.4f,
                                          0.2f, -0.6f, 0.2f);

            Assert.AreEqual (expected, inversed);
        }

        [TestMethod]
        public void Test_Transpose () {
            var m = new Matrix3x3 (1, 2, 1,
                                   1, 1, -1,
                                   2, 1, 1);
            var transposed = m.Transpose ();
            var expected = new Matrix3x3 (1, 1, 2,
                                          2, 1, 1,
                                          1, -1, 1);

            Assert.AreEqual (expected, transposed);
        }

        [TestMethod]
        public void Test_Identity () {
            var m = Matrix3x3.Identity;
            var expected = new Matrix3x3 (1,0,0,
                                          0,1,0,
                                          0,0,1);
            Assert.AreEqual (expected, m);
        }

        [TestMethod]
        public void Test_Matrix_Matrix_Multiply () {
            var a = new Matrix3x3 (1, 2, 1,
                                   1, 1, -1,
                                   2, 1, 1);
            var b = new Matrix3x3 (1, 1, -1,
                                  1, 2, 1,
                                  1, -1, 1);
            var ab = a * b;
            var ba = b * a;

            var expected1 = new Matrix3x3 (4, 4, 2,
                                           1, 4, -1,
                                           4, 3, 0);
            var expected2 = new Matrix3x3 (0, 2, -1,
                                           5, 5, 0,
                                           2, 2, 3);

            Assert.AreEqual (expected1, ab);
            Assert.AreEqual (expected2, ba);
        }

        [TestMethod]
        public void Test_Matrix_Vector_Multiply () {
            var m = new Matrix3x3 (1, 2, 1,
                                  1, 1, -1,
                                  2, 1, 1);
            var v = new Vector3 (1, 2, 3);
            var mv = m*v;

            var expected = new Vector3 (8,0,7);

            Assert.AreEqual (expected, mv);

        }

        [TestMethod]
        public void Test_Equals () {
            var delta = 0.00001f;
            var a = new Matrix3x3 (1, 1, 1,
                                   1, 1, 1,
                                   1, 1, 1);
            var b = new Matrix3x3 (1 + delta, 1 + delta, 1 + delta,
                                   1 + delta, 1 + delta, 1 + delta,
                                   1 + delta, 1 + delta, 1 + delta);

            Assert.IsTrue (a.Equals (b));                            // 誤差許容
            Assert.IsFalse (a == b);                                 // 厳密な比較
            Assert.IsFalse (a.GetHashCode () == b.GetHashCode ());   // 厳密な比較に基づくハッシュ値
        }


    }
}
