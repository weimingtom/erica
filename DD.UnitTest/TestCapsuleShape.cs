using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest {
    [TestClass]
    public class TestCapsuleShape {
        [TestMethod]
        public void Test_New () {
            var cap = new CapsuleShape (1, 2);

            Assert.AreEqual (1, cap.Radius);
            Assert.AreEqual (2, cap.HalfHeight);
        }

        [TestMethod]
        public void Test_CreateBulletObject () {
            var cap = new CapsuleShape (1, 2);

            Assert.IsNotNull (cap.CreateGhostObject ());
            Assert.IsNotNull (cap.CreateRigidBody (1));
            Assert.IsNotNull (cap.CreateBulletShape ());
        }

        [TestMethod]
        public void Test_CreateShape () {
            var box = new CapsuleShape (1, 2);
            var shp = box.CreateBulletShape () as BulletSharp.CapsuleShape;

            var height = shp.HalfHeight * PhysicsSimulator.PPM;
            var radius = shp.Radius * PhysicsSimulator.PPM;

            Assert.AreEqual (1, radius);
            Assert.AreEqual (1, height);
        }

    }
}
