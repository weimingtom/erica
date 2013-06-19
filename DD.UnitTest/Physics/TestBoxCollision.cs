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
            var box = new BoxCollider (1, 2, 3);

            Assert.AreEqual (1, box.Width);
            Assert.AreEqual (2, box.Height);
            Assert.AreEqual (3, box.Depth);
        }
    }
}
