using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestGrave {
        [TestMethod]
        public void Test_New () {
            var node = new Node ("DeadSoul");
            var dor = new ReservedDestruction (node, 100);

            Assert.AreEqual (node, dor.Node);
            Assert.AreEqual (-1, dor.LifeTime);
            Assert.AreEqual (100, dor.PurgeTime);
        }

        [TestMethod]
        public void Test_Tick () {
            var node = new Node ("DeadSoul");
            var dor = new ReservedDestruction (node, 100);

            dor.Tick (1);

            Assert.AreEqual (100, dor.PurgeTime);
            Assert.AreEqual (99, dor.LifeTime);
        }
    }
}
