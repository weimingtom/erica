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
            var sph = new SphereCollider (1.0f);

            Assert.AreEqual (1, sph.Radius);
        }
    }
}
