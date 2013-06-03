using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestColor {
        [TestMethod]
        public void Test_New () {
            var col = new Color (1, 2, 3, 4);
            Assert.AreEqual (1, col.R);
            Assert.AreEqual (2, col.G);
            Assert.AreEqual (3, col.B);
            Assert.AreEqual (4, col.A);
        }

        [TestMethod]
        public void Test_Predefined () {
            Assert.AreEqual (new Color(255,255,255,255), Color.White);
            Assert.AreEqual (new Color(0,0,0,255), Color.Black);
        }
    }
}
