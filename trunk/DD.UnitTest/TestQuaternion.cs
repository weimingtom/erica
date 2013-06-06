using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestQuaternion {
        [TestMethod]
        public void Test_New_1 () {
            var q = new Quaternion ();

            Assert.AreEqual (4, q.ComponentCount);
            Assert.AreEqual (0, q.X);
            Assert.AreEqual (0, q.Y);
            Assert.AreEqual (0, q.Z);
            Assert.AreEqual (0, q.W);
        }

        [TestMethod]
        public void Test_New_2 () {
            var q1 = new Quaternion (0, 0, 0, 1);
            var q2 = new Quaternion (45, 0, 0, 1);

            Assert.AreEqual (0, q1.X);
            Assert.AreEqual (0, q1.Y);
            Assert.AreEqual (0, q1.Z);
            Assert.AreEqual (1, q1.W);
            Assert.AreEqual (0, q2.X);
            Assert.AreEqual (0, q2.Y);
            Assert.AreEqual (0.38265f, q2.Z, 0.0001f);
            Assert.AreEqual (0.92385f, q2.W, 0.0001f);
        }

        [TestMethod]
        public void Test_Identity () {
            var q = Quaternion.Identity;

            Assert.AreEqual (0, q.X);
            Assert.AreEqual (0, q.Y);
            Assert.AreEqual (0, q.Z);
            Assert.AreEqual (1, q.W);
        }

        [TestMethod]
        public void Test_Set () {
            var q1 = Quaternion.Set (1, 2, 3, 4, false);
            var q2 = Quaternion.Set (1, 2, 3, 4, true);

            Assert.AreEqual (1, q1.X);
            Assert.AreEqual (2, q1.Y);
            Assert.AreEqual (3, q1.Z);
            Assert.AreEqual (4, q1.W);

            Assert.AreEqual (0.1825742f, q2.X, 0.0001f);
            Assert.AreEqual (0.3651484f, q2.Y, 0.0001f);
            Assert.AreEqual (0.5477225f, q2.Z, 0.0001f);
            Assert.AreEqual (0.7302967f, q2.W, 0.0001f);
        }

        [TestMethod]
        public void Test_Indexer () {
            var q = Quaternion.Set (1, 2, 3, 4, false);

            Assert.AreEqual (1, q[0]);
            Assert.AreEqual (2, q[1]);
            Assert.AreEqual (3, q[2]);
            Assert.AreEqual (4, q[3]);
        }


        [TestMethod]
        public void Test_Conversion () {
            var q = Quaternion.Set (1, 2, 3, 4, false);
            var expected = new float[] { 1, 2, 3, 4 };

            CollectionAssert.AreEqual (expected, (float[])q);
        }

        [TestMethod]
        public void Test_Length () {
            var q = new Quaternion (45, 1, 2, 3);

            Assert.AreEqual (1, q.Length, 0.0001f);
            Assert.AreEqual (1, q.Length2, 0.0001f);

        }

        [TestMethod]
        public void Test_AngleAxis () {
            var q1 = new Quaternion (45, 0, 0, 1);
            var q2 = new Quaternion (-90, 0, 1, 0);
            var q3 = new Quaternion (270, 1, 0, 0);

            Assert.AreEqual (45f, q1.Angle, 0.0001f);
            Assert.AreEqual (new Vector3 (0, 0, 1), q1.Axis);

            Assert.AreEqual (90f, q2.Angle, 0.0001f);
            Assert.AreEqual (new Vector3 (0, -1, 0), q2.Axis);

            Assert.AreEqual (270f, q3.Angle, 0.0001f);
            Assert.AreEqual (new Vector3 (1, 0, 0), q3.Axis);

        }

        [TestMethod]
        public void Test_Normalize () {
            var q1 = Quaternion.Set (1, 2, 3, 4, false);
            var q2 = q1.Normalize ();

            Assert.AreEqual (1.0f, q2.Length, 0.0001f);
        }


        [TestMethod]
        public void Test_Inverse () {
            var q1 = new Quaternion (45, 0, 1, 0);
            var q2 = new Quaternion (270, 0, -1, 0);

            var qInv1 = q1.Inverse ();
            var qInv2 = q2.Inverse ();


            var expected1 = new Quaternion (-45, 0, 1, 0);
            var expected2 = new Quaternion (-270, 0, -1, 0);

            Assert.AreEqual (expected1, qInv1);
            Assert.AreEqual (expected2, qInv2);
        }

        [TestMethod]
        public void Test_Conjugate () {
            var q = new Quaternion (180, 1, 0, 0);
            var qConj = q.Conjugate ();
            var expected = new Quaternion (180, -1, 0, 0);

            Assert.AreEqual (expected, qConj);
        }

        [TestMethod]
        public void Test_Log () {
            var q = new Quaternion (270, -1, 0, 0);
            var qLog = q.Log ().Normalize ();
            var expected = new Quaternion (180, -1, 0, 0);

            Assert.AreEqual (expected, qLog);
        }

        [TestMethod]
        public void Test_Dot () {
            var q1 = new Quaternion (270, 1, 1, 0);
            var q2 = new Quaternion (180, 0, 1, 1);
            var dot = Quaternion.Dot (q1, q2);

            Assert.AreEqual (0.3535534f, dot, 0.0001f);
        }


        [TestMethod]
        public void Test_Quaternion_Operator () {
            var q1 = Quaternion.Set (1, 2, 3, 4, false); ;
            var q2 = Quaternion.Set (1, 2, 3, 4, false); ;
            var q3 = q1 * 2;
            var q4 = 2 * q1;
            var q5 = q1 / 2;

            Assert.AreEqual (Quaternion.Set (2, 4, 6, 8, false), q3);
            Assert.AreEqual (Quaternion.Set (2, 4, 6, 8, false), q4);
            Assert.AreEqual (Quaternion.Set (0.5f, 1f, 1.5f, 2f, false), q5);
        }

        [TestMethod]
        public void Test_Quaternion_Quaternion_Operator () {
            var q1 = Quaternion.Set (1, 2, 3, 4, false);
            var q2 = Quaternion.Set (1, 2, 3, 4, false);
            var q3 = q1 * q2;
            var q4 = q1 + q2;

            Assert.AreEqual (Quaternion.Set (8,16,24,2, false), q3);
            Assert.AreEqual (Quaternion.Set (2, 4, 6, 8, false), q4);
        }

        [TestMethod]
        public void Test_Slerp () {
            var q1 = new Quaternion (0, 0, 0, 1);
            var q2 = new Quaternion (90, 0, 0, 1);
            var q3 = Quaternion.Slerp (0, q1, q2);
            var q4 = Quaternion.Slerp (0.5f, q1, q2);
            var q5 = Quaternion.Slerp (1, q1, q2);

            var expected3 = new Quaternion (0, 0, 0, 1);
            var expected4 = new Quaternion (45, 0, 0, 1);
            var expected5 = new Quaternion (90, 0, 0, 1);

            Assert.AreEqual (expected3, q3);
            Assert.AreEqual (expected4, q4);
            Assert.AreEqual (expected5, q5);
        }

        [TestMethod]
        public void Test_Apply () {
            var q1 = new Quaternion (90, 0, 0, 1);
            var q2 = new Quaternion (90, 0, 1, 0);
            var q3 = new Quaternion (90, 1, 0, 0);
            var v1 = q1.Apply (new Vector3 (1, 0, 0));
            var v2 = q2.Apply (new Vector3 (0, 1, 0));
            var v3 = q3.Apply (new Vector3 (0, 0, 1));
            

            Assert.AreEqual (new Vector3 (0, 1, 0), v1);
            Assert.AreEqual (new Vector3 (0, 1, 0), v2);
            Assert.AreEqual (new Vector3 (0, -1, 0), v3);
        }


    }
}
