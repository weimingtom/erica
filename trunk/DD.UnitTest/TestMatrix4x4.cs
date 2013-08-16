using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestMatrix4x4 {
        [TestMethod]
        public void Test_New_1 () {
            var mat = new Matrix4x4 ();

            Assert.AreEqual (0, mat.M00);
            Assert.AreEqual (0, mat.M01);
            Assert.AreEqual (0, mat.M02);
            Assert.AreEqual (0, mat.M03);
            Assert.AreEqual (0, mat.M10);
            Assert.AreEqual (0, mat.M11);
            Assert.AreEqual (0, mat.M12);
            Assert.AreEqual (0, mat.M13);
            Assert.AreEqual (0, mat.M20);
            Assert.AreEqual (0, mat.M21);
            Assert.AreEqual (0, mat.M22);
            Assert.AreEqual (0, mat.M23);
            Assert.AreEqual (0, mat.M30);
            Assert.AreEqual (0, mat.M31);
            Assert.AreEqual (0, mat.M32);
            Assert.AreEqual (0, mat.M33);
        }


        [TestMethod]
        public void Test_New_2 () {
            var mat = new Matrix4x4 (1, 2, 3, 4,
                                     5, 6, 7, 8,
                                     9, 10, 11, 12,
                                     13, 14, 15, 16);

            Assert.AreEqual (1, mat.M00);
            Assert.AreEqual (2, mat.M01);
            Assert.AreEqual (3, mat.M02);
            Assert.AreEqual (4, mat.M03);
            Assert.AreEqual (5, mat.M10);
            Assert.AreEqual (6, mat.M11);
            Assert.AreEqual (7, mat.M12);
            Assert.AreEqual (8, mat.M13);
            Assert.AreEqual (9, mat.M20);
            Assert.AreEqual (10, mat.M21);
            Assert.AreEqual (11, mat.M22);
            Assert.AreEqual (12, mat.M23);
            Assert.AreEqual (13, mat.M30);
            Assert.AreEqual (14, mat.M31);
            Assert.AreEqual (15, mat.M32);
            Assert.AreEqual (16, mat.M33);

        }

        [TestMethod]
        public void Test_New_3 () {
            var mat = new Matrix4x4 (new float[]{1,2,3,4,
                                                 5,6,7,8,
                                                 9,10,11,12,
                                                 13,14,15,16});
            Assert.AreEqual (1, mat.M00);
            Assert.AreEqual (2, mat.M01);
            Assert.AreEqual (3, mat.M02);
            Assert.AreEqual (4, mat.M03);
            Assert.AreEqual (5, mat.M10);
            Assert.AreEqual (6, mat.M11);
            Assert.AreEqual (7, mat.M12);
            Assert.AreEqual (8, mat.M13);
            Assert.AreEqual (9, mat.M20);
            Assert.AreEqual (10, mat.M21);
            Assert.AreEqual (11, mat.M22);
            Assert.AreEqual (12, mat.M23);
            Assert.AreEqual (13, mat.M30);
            Assert.AreEqual (14, mat.M31);
            Assert.AreEqual (15, mat.M32);
            Assert.AreEqual (16, mat.M33);
        }
        
        [TestMethod]
        public void Test_Identity () {
            var mat = Matrix4x4.Identity;

            Assert.AreEqual (1, mat[0]);
            Assert.AreEqual (0, mat[1]);
            Assert.AreEqual (0, mat[2]);
            Assert.AreEqual (0, mat[3]);
            Assert.AreEqual (0, mat[4]);
            Assert.AreEqual (1, mat[5]);
            Assert.AreEqual (0, mat[6]);
            Assert.AreEqual (0, mat[7]);
            Assert.AreEqual (0, mat[8]);
            Assert.AreEqual (0, mat[9]);
            Assert.AreEqual (1, mat[10]);
            Assert.AreEqual (0, mat[11]);
            Assert.AreEqual (0, mat[12]);
            Assert.AreEqual (0, mat[13]);
            Assert.AreEqual (0, mat[14]);
            Assert.AreEqual (1, mat[15]);
        }


        [TestMethod]
        public void Test_Indexer () {
            var mat = new Matrix4x4 (1, 2, 3, 4,
                                     5, 6, 7, 8,
                                     9, 10, 11, 12,
                                     13, 14, 15, 16);

            Assert.AreEqual (16, mat.ComponentCount);
            Assert.AreEqual (1, mat[0]);
            Assert.AreEqual (2, mat[1]);
            Assert.AreEqual (3, mat[2]);
            Assert.AreEqual (4, mat[3]);
            Assert.AreEqual (5, mat[4]);
            Assert.AreEqual (6, mat[5]);
            Assert.AreEqual (7, mat[6]);
            Assert.AreEqual (8, mat[7]);
            Assert.AreEqual (9, mat[8]);
            Assert.AreEqual (10, mat[9]);
            Assert.AreEqual (11, mat[10]);
            Assert.AreEqual (12, mat[11]);
            Assert.AreEqual (13, mat[12]);
            Assert.AreEqual (14, mat[13]);
            Assert.AreEqual (15, mat[14]);
            Assert.AreEqual (16, mat[15]);
        }


        [TestMethod]
        public void Test_Determinant () {
            var a = new Matrix4x4 (1, 2, 1, 1,
                                   2, 1, 2, 1,
                                   2, 2, 2, 2,
                                   1, 1, 2, 1);
            var b = new Matrix4x4 (1, 2, -1, 1,
                                   -1, 1, -1, 1,
                                   2, -1, 1, -1,
                                   1, 1, 2, 1);

            Assert.AreEqual (2, a.Determinant, 0.001f);
            Assert.AreEqual (-3, b.Determinant, 0.001f);
        }

        [TestMethod]
        public void Test_Transpose () {
            var a = new Matrix4x4 (1, 2, 1, 1,
                                   2, 1, 2, 1,
                                   2, 2, 2, 2,
                                   1, 1, 2, 1);

            var b = new Matrix4x4 (1, 2, -1, 1,
                                   -1, 1, -1, 1,
                                   2, -1, 1, -1,
                                   1, 1, 2, 1);

            var aTra = new Matrix4x4 (1, 2, 2, 1,
                                      2, 1, 2, 1,
                                      1, 2, 2, 2,
                                      1, 1, 2, 1);
            var bTra = new Matrix4x4 (1, -1, 2, 1,
                                     2, 1, -1, 1,
                                     -1, -1, 1, 2,
                                     1, 1, -1, 1);

            Assert.AreEqual (aTra, a.Transpose ());
            Assert.AreEqual (bTra, b.Transpose ());

        }

        [TestMethod]
        public void Test_Inverse () {
            var a = new Matrix4x4 (1, 2, 1, 1,
                                   2, 1, 2, 1,
                                   2, 2, 2, 2,
                                   1, 1, 2, 1);

            var b = new Matrix4x4 (1, 2, -1, 1,
                                   -1, 1, -1, 1,
                                   2, -1, 1, -1,
                                   1, 1, 2, 1);

            var aInv = new Matrix4x4 (0, 1, 0, -1,
                                      1, 0, -0.5f, 0,
                                      0, 0, -0.5f, 1,
                                      -1, -1, 1.5f, 0);

            var bInv = new Matrix4x4 (0, 1, 1, 0,
                                      1, -3, -2, 0,
                                      0, -1, -0.6666667f, 0.3333333f,
                                      -1, 4, 2.3333333f, 0.3333333f);

            Assert.AreEqual (aInv, a.Inverse ());
            Assert.AreEqual (bInv, b.Inverse ());
        }


        [TestMethod]
        public void Test_Matrix_Operator_Matrix () {
            var a = new Matrix4x4 (1, 2, 1, 1,
                                   2, 1, 2, 1,
                                   2, 2, 2, 2,
                                   1, 1, 2, 1);

            var b = new Matrix4x4 (1, 2, -1, 1,
                                   -1, 1, -1, 1,
                                   2, -1, 1, -1,
                                   1, 1, 2, 1);


            var ab = new Matrix4x4 (2, 4, 0, 3,
                                    6, 4, 1, 2,
                                    6, 6, 2, 4,
                                    5, 2, 2, 1);
            var ba = new Matrix4x4 (4, 3, 5, 2,
                                    0, -2, 1, -1,
                                    1, 4, 0, 2,
                                    8, 8, 9, 7);
            Assert.AreEqual (ab, a * b);
            Assert.AreEqual (ba, b * a);
        }

        [TestMethod]
        public void Test_TranslationMatrix () {
            var mat = Matrix4x4.CreateFromTranslation (1, 2, 3);
            var exptected = new Matrix4x4 (1, 0, 0, 1,
                                           0, 1, 0, 2,
                                           0, 0, 1, 3,
                                           0, 0, 0, 1);

            Assert.AreEqual (exptected, mat);
        }

        [TestMethod]
        public void Test_ScalingMatrix () {
            var mat = Matrix4x4.CreateFromScale (1, 2, 3);
            var expected = new Matrix4x4 (1, 0, 0, 0,
                                          0, 2, 0, 0,
                                          0, 0, 3, 0,
                                          0, 0, 0, 1);
            Assert.AreEqual (expected, mat);
        }

        [TestMethod]
        public void Test_RotationMatrix_1 () {
            var mat = Matrix4x4.CreateFromRotation (45, 0, 0, 1);
            var s = (float)Math.Sin (45 * Math.PI / 180.0f);
            var c = (float)Math.Cos (45 * Math.PI / 180.0f);
            var expected = new Matrix4x4 (c, -s, 0, 0,
                                          s, c, 0, 0,
                                          0, 0, 1, 0,
                                          0, 0, 0, 1);
            Assert.AreEqual (expected, mat);
        }

        [TestMethod]
        public void Test_RotationMatrix_2 () {
            var mat = Matrix4x4.CreateFromRotation (new Quaternion (45, 0, 0, 1));
            var s = (float)Math.Sin (45 * Math.PI / 180.0f);
            var c = (float)Math.Cos (45 * Math.PI / 180.0f);
            var expected = new Matrix4x4 (c, -s, 0, 0,
                                          s, c, 0, 0,
                                          0, 0, 1, 0,
                                          0, 0, 0, 1);
            Assert.AreEqual (expected, mat);
        }


        [TestMethod]
        public void Test_Decompress_1 () {
            var t = Matrix4x4.CreateFromTranslation (1, 2, 3);
            var r = Matrix4x4.CreateFromRotation (45, 0, 0, 1);
            var s = Matrix4x4.CreateFromScale (1, 2, 3);
            var trs = t * r * s;

            Vector3 outT;
            Quaternion outR;
            Vector3 outS;

            trs.Decompress (out outT, out outR, out outS);

            var expectedT = new Vector3 (1, 2, 3);
            var expectedR = new Quaternion (45, 0, 0, 1);
            var expectedS = new Vector3 (1, 2, 3);

            Assert.AreEqual (expectedT, outT);
            Assert.AreEqual (expectedR, outR);
            Assert.AreEqual (expectedS, outS);
        }

        [TestMethod]
        public void Test_Decompress_2 () {
            var t = Matrix4x4.CreateFromTranslation (1, 2, 3);
            var r = Matrix4x4.CreateFromRotation (45, 0, 0, 1);
            var s = Matrix4x4.CreateFromScale (1, 2, 3);
            var trs = t * r * s;

            Vector3 outT;
            Matrix3x3 outR;
            Vector3 outS;

            trs.Decompress (out outT, out outR, out outS);

            var expectedT = new Vector3 (1, 2, 3);
            var expectedR = new Quaternion (45, 0, 0, 1).Matrix3x3;
            var expectedS = new Vector3 (1, 2, 3);

            Assert.AreEqual (expectedT, outT);
            Assert.AreEqual (expectedR, outR);
            Assert.AreEqual (expectedS, outS);
        }

        [TestMethod]
        public void Test_Apply () {

            var m = new Matrix4x4 (1, 2, 1, 1,
                                   2, 1, 2, 1,
                                   2, 2, 2, 2,
                                   1, 1, 2, 1);
            Assert.AreEqual(new Vector3(0.9f, 1.1f, 1.4f), m.Apply (1,2,3)); // w=1
            Assert.AreEqual (new Vector3 (8,10,12), m.Apply (1, 2, 3, 0));   // w=0（W除算を行わない）
        }

        [TestMethod]
        public void Test_ApplyVector () {

            var m1 = Matrix4x4.CreateFromScale (1, 2, 1);
            Assert.AreEqual(new Vector3(1,1,1), m1.ApplyDirection (1, 2, 1));

            var m2 = Matrix4x4.CreateFromTranslation (1, 1, 1);
            Assert.AreEqual (new Vector3 (1, 2, 1), m2.ApplyDirection (1, 2, 1));

            var m3 = Matrix4x4.CreateFromRotation (45, 0, 0, 1);
            Assert.AreEqual (new Vector3(-0.7071068f, 2.1213204f, 1), m3.ApplyDirection (1, 2, 1));
        }

        [TestMethod]
        public void Test_Equals () {
            var a = new Matrix4x4 (1, 2, 3, 4,
                                   5, 6, 7, 8,
                                   9, 10, 11, 12,
                                   13, 14, 15, 16);
            var delt = 0.00001f;
            var b = new Matrix4x4 (1 + delt, 2 + delt, 3 + delt, 4 + delt,
                                   5 + delt, 6 + delt, 7 + delt, 8 + delt,
                                   9 - delt, 10 - delt, 11 - delt, 12 - delt,
                                   13 - delt, 14 - delt, 15 - delt, 16 - delt);

            Assert.IsTrue (a.Equals (b));                            // 誤差を許容する
            Assert.IsFalse (a == b);                                 // 厳密な比較
            Assert.IsFalse (a.GetHashCode () == b.GetHashCode ());   // 厳密な比較に基づくハッシュ値

        }


    }
}
