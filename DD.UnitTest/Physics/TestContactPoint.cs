using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestContactPoint {
        [TestMethod]
        public void Test_New () {
            var coll = new Collider (ColliderType.Dynamic);
            var p = new Vector3 (1, 2, 3);
            var n = new Vector3 (4, 5, 6);
            var cp = new ContactPoint (coll, p, n);

            Assert.AreEqual (coll, cp.Collidee);
            Assert.AreEqual (p, cp.Point);
            Assert.AreEqual (n, cp.Normal);
        }
    }
}
