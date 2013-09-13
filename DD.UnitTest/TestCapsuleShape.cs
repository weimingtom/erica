using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestCapsuleShape {
        [TestMethod]
        public void Test_New () {
            var cap = new CapsuleShape (1, 2);

            Assert.AreEqual (1, cap.Radius);
            Assert.AreEqual (2, cap.Height);
        }

        [TestMethod]
        public void Test_CreateBulletObject () {
            var cap = new CapsuleShape (1, 2);

            Assert.IsNotNull (cap.CreateGhostObject ());
            Assert.IsNotNull (cap.CreateRigidBody (1));
        }

    }
}
