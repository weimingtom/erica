using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestBulletPhysicsExtensions {
        [TestMethod]
        public void Test_Vector3 () {
            Assert.AreEqual (new Vector3 (1, 2, 3), new BulletSharp.Vector3 (1, 2, 3).ToDD ());
            Assert.AreEqual (new BulletSharp.Vector3 (1, 2, 3), new Vector3 (1, 2, 3).ToBullet ());
        }

        [TestMethod]
        public void Test_Matrix () {
            var bullet = new BulletSharp.Matrix ();
            bullet.M11 = 1;
            bullet.M12 = 2;
            bullet.M13 = 3;
            bullet.M14 = 4;
            bullet.M21 = 5;
            bullet.M22 = 6;
            bullet.M23 = 7;
            bullet.M24 = 8;
            bullet.M31 = 9;
            bullet.M32 = 10;
            bullet.M33 = 11;
            bullet.M34 = 12;
            bullet.M41 = 13;
            bullet.M42 = 14;
            bullet.M43 = 15;
            bullet.M44 = 16;
            var dd = new Matrix4x4(1,5,9,13,
                                   2,6,10,14,
                                   3,7,11,15,
                                   4,8,12,16);

            Assert.AreEqual (dd, bullet.ToDD());
            Assert.AreEqual (bullet, dd.ToBullet ());
        }

        [TestMethod]
        public void Test_Quaternion () {
            var bullet = BulletSharp.Quaternion.RotationAxis(new BulletSharp.Vector3(1,2,3), (float)Math.PI/4.0f);
            var dd = new Quaternion (45, new Vector3 (1, 2, 3));

            Assert.AreEqual (dd, bullet.ToDD ());
            Assert.AreEqual (bullet.X, dd.ToBullet ().X, 0.001f);
            Assert.AreEqual (bullet.Y, dd.ToBullet ().Y, 0.001f);
            Assert.AreEqual (bullet.Z, dd.ToBullet ().Z, 0.001f);
            Assert.AreEqual (bullet.W, dd.ToBullet ().W, 0.001f);
        }
    }
}
