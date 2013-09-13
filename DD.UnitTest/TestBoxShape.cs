using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestBoxShape {
        [TestMethod]
        public void Test_New () {
            var col = new BoxShape (1, 1, 1);
            var rb = col.CreateRigidBody (1);

            Assert.AreEqual (2, col.Width);
            Assert.AreEqual (2, col.Height);
            Assert.AreEqual (2, col.Depth);
        }

        [TestMethod]
        public void Test_CreateBulletObject () {
            var box = new BoxShape (1, 1, 1);

            Assert.IsNotNull (box.CreateGhostObject ());
            Assert.IsNotNull (box.CreateRigidBody (1));
        }

    
    }
}
