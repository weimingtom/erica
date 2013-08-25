using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestCollisionShape {
        [TestMethod]
        public void Test_New () {
            var shape = new SphereCollision (1);
            
            Assert.AreEqual (Vector3.Zero, shape.Offset);
        }

        [TestMethod]
        public void Test_SetOffset () {
            var shape = new SphereCollision (1);

            shape.Offset = new Vector3 (1, 2, 3);

            Assert.AreEqual (new Vector3(1,2,3), shape.Offset);

            shape.SetOffset (4, 5, 6);

            Assert.AreEqual (new Vector3 (4,5,6), shape.Offset);

        }

    }
}
