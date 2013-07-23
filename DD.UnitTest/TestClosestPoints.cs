using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestClosestPoints {
        [TestMethod]
        public void TestMethod_New () {
            var pointA = new Vector3(1,2,3);
            var pointB = new Vector3(4,5,6);

            var cp = new ClosestPoints (pointA, pointB);
            Assert.AreEqual (pointA, cp.PointA);
            Assert.AreEqual (pointB, cp.PointB);
            Assert.AreEqual (pointB - pointA, cp.VectorAtoB);
        }
    }
}
