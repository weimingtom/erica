using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest {
    [TestClass]
    public class TestRayCast {
        /*
        [TestMethod]
        public void Test_New_1 () {
            var pointA = new Vector3(0,0,0);
            var pointB = new Vector3(2,0,0);
            var frac = 10f;

            var ray = new Ray (pointA, pointB, frac);

            Assert.AreEqual (new Vector3(0,0,0), ray.PointA);
            Assert.AreEqual (new Vector3(2,0,0), ray.PointB);
            Assert.AreEqual (10, ray.Fraction, 0.0001f);

            Assert.AreEqual (new Vector3 (0, 0, 0), ray.Origin);
            Assert.AreEqual (new Vector3 (1, 0, 0), ray.Direction);
            Assert.AreEqual (2, ray.UnitLength, 0.0001f);
            Assert.AreEqual (20, ray.Length, 0.0001f);
        }

        [TestMethod]
        public void Test_New_2 () {
            var node = new Node ();
            var normal = new Vector3 (1,2,3);
            var frac = 10;
            var ray = new Ray (new Vector3 (0,0,0), new Vector3 (2,0,0), 10);

            var output = new RayIntersection (true, node, normal, frac, ray);

            Assert.AreEqual (true, output.Hit);
            Assert.AreEqual (node, output.Node);
            Assert.AreEqual (new Vector3(1,2,3), output.Normal);
            Assert.AreEqual (10.0f, output.Fraction, 0.0001f);
            Assert.AreEqual (ray, output.Ray);

            Assert.AreEqual (20.0f, output.Distance, 0.0001f);
        }
        */
    }
}
