using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestPoint {
        [TestMethod]
        public void Test_New () {
            var p = new Point (1, 2);

            Assert.AreEqual (1, p.X);
            Assert.AreEqual (2, p.Y);
        }

        [TestMethod]
        public void Test_Indexer () {
            var p = new Point (1, 2);
            p[0] = 3;
            p[1] = 4;

            Assert.AreEqual (3, p[0]);
            Assert.AreEqual (4, p[1]);
        }
    }
}
