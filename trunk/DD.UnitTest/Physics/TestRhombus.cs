using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestRhombus {
        [TestMethod]
        public void TestMethod_New () {
            var rhm = new RhombusCollisionShape (1, 2);

            Assert.AreEqual (ShapeType.Polygon, rhm.Type);
            Assert.AreEqual (true, rhm.IsPolygon);
            Assert.AreEqual (2.0f, rhm.Width);
            Assert.AreEqual (4.0f, rhm.Height);
        }

        [TestMethod]
        public void Test_CreateShape () {
            var box = new RhombusCollisionShape (1, 2);
            box.Offset = new Vector3 (1, 2, 3);

            Assert.IsNotNull (box.CreateShapeBody (1.0f));
            Assert.AreEqual (new Vector3 (1, 2, 3), box.Offset);
        }


    }
}
