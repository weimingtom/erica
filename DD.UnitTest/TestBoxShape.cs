using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest {
    [TestClass]
    public class TestBoxShape {
        [TestMethod]
        public void Test_New_1 () {
            var box = new BoxShape (1, 2, 3);

            Assert.AreEqual (1, box.HalfWidth);
            Assert.AreEqual (2, box.HalfHeight);
            Assert.AreEqual (3, box.HalfDepth);

            Assert.AreEqual (2, box.Width);
            Assert.AreEqual (4, box.Height);
            Assert.AreEqual (6, box.Depth);
        }

        [TestMethod]
        public void Test_New_2 () {
            var box = new BoxShape (1);

            Assert.AreEqual (1, box.HalfWidth);
            Assert.AreEqual (1, box.HalfHeight);
            Assert.AreEqual (1, box.HalfDepth);

            Assert.AreEqual (2, box.Width);
            Assert.AreEqual (2, box.Height);
            Assert.AreEqual (2, box.Depth);
        }

        [TestMethod]
        public void Test_CreateBulletObject () {
            var box = new BoxShape (1, 1, 1);

            Assert.IsNotNull (box.CreateGhostObject ());
            Assert.IsNotNull (box.CreateRigidBody (1));
            Assert.IsNotNull (box.CreateBulletShape());
        }


        [TestMethod]
        public void Test_CreateShape () {
            var box = new BoxShape (1, 2, 3);
            var shp = box.CreateBulletShape () as BulletSharp.BoxShape;

            var size = shp.HalfExtentsWithMargin.ToDD() * PhysicsSimulator.PPM;

            Assert.AreEqual(new Vector3(1,2,3), size);
        }
    }
}
