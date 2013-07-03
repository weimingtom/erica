using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestBoxCollision {
        [TestMethod]
        public void Test_New () {
            var box = new BoxCollisionShape (1, 2, 3);

            Assert.AreEqual (DD.ShapeType.Box, box.Type);
            Assert.AreEqual (2, box.Width);
            Assert.AreEqual (4, box.Height);
            Assert.AreEqual (6, box.Depth);
            Assert.AreEqual (Vector3.Zero, box.Offset);
        }

        [TestMethod]
        public void Test_CreateShape () {
            var box = new BoxCollisionShape (1, 2, 3);
            box.Offset = new Vector3 (1, 2, 3);

            Assert.IsNotNull(box.CreateShapeBody (1.0f));
            Assert.AreEqual (new Vector3 (1, 2, 3), box.Offset);
        }

    }
}
