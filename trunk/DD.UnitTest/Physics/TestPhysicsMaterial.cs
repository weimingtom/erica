using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestPhysicsMaterial {
        [TestMethod]
        public void Test_New () {
            var mat = new PhysicsMaterial ();

            Assert.AreEqual (1, mat.Restitution);
            Assert.AreEqual (0.5f, mat.Friction);
            Assert.AreEqual (0, mat.RollingFriction);
            Assert.AreEqual (0, mat.LinearDamping);
            Assert.AreEqual (0, mat.AngularDamping);
        }
    }
}
