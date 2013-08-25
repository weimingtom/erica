using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestSphereCollision {
        [TestMethod]
        public void Test_New () {
            var sph = new SphereCollision (1.0f);

            Assert.AreEqual (ShapeType.Sphere, sph.Type);
            Assert.AreEqual (true, sph.IsCircle);
            Assert.AreEqual (1, sph.Radius);
        }

        [TestMethod]
        public void Test_CreateShape () {
            var sph = new SphereCollision (1.0f);
            sph.Offset = new Vector3 (1, 2, 3);

            Assert.IsNotNull (sph.CreateShapeBody (1.0f));
            Assert.AreEqual (new Vector3 (1, 2, 3), sph.Offset);
        }


    }
}
