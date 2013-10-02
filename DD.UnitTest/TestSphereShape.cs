using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest {
    [TestClass]
    public class TestSphereShape {
        [TestMethod]
        public void Test_New () {
            var sph = new SphereShape (1);
            
            Assert.AreEqual (1, sph.Radius);
            Assert.AreEqual (2, sph.Diameter);
        }

        [TestMethod]
        public void Test_CreateBulletObject () {
            var sph = new SphereShape (1);

            Assert.IsNotNull (sph.CreateGhostObject ());
            Assert.IsNotNull (sph.CreateRigidBody (1));
            Assert.IsNotNull (sph.CreateBulletShape ());
        }


        [TestMethod]
        public void Test_CreateShape () {
            var box = new SphereShape (1);
            var shp = box.CreateBulletShape () as BulletSharp.SphereShape;

            var radius = shp.Radius * PhysicsSimulator.PPM;

            Assert.AreEqual (1, radius);
        }

    }
}
