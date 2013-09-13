using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestRaycastResult {
        [TestMethod]
        public void TestMethod1 () {
            var frac = 1;
            var dist = 2;
            var node = new Node("");
            var point = new Vector3(1,2,3);
            var normal = new Vector3(1,2,3);
            
            var hit = new RaycastResult(frac, dist, node, point, normal);
            Assert.AreEqual (true, hit.Hit);
            Assert.AreEqual(frac, hit.Fraction);
            Assert.AreEqual (dist, hit.Distance);
            Assert.AreEqual (node, hit.Node);
            Assert.AreEqual (point, hit.Point);
            Assert.AreEqual (normal, hit.Normal);

            var nohit = new RaycastResult ();
            Assert.AreEqual (false, nohit.Hit);
        }
    }
}
