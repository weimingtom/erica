using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestRectangle {
        [TestMethod]
        public void Test_New () {
            var bbox = new Rectangle (1, 1, 2, 2);

            Assert.AreEqual (1, bbox.X);
            Assert.AreEqual (1, bbox.Y);
            Assert.AreEqual (3, bbox.X2);
            Assert.AreEqual (3, bbox.Y2);
            Assert.AreEqual (2, bbox.Width);
            Assert.AreEqual (2, bbox.Height);
        }

        [TestMethod]
        public void Test_Contain () {
            var bbox = new Rectangle (1, 1, 2, 2);

            Assert.AreEqual (true, bbox.Contain (2, 2));

            Assert.AreEqual (true, bbox.Contain (1, 2));
            Assert.AreEqual (true, bbox.Contain (2, 1));
            Assert.AreEqual (false, bbox.Contain (2, 3));
            Assert.AreEqual (false, bbox.Contain (3, 2));

            Assert.AreEqual (true, bbox.Contain (1, 1));
            Assert.AreEqual (false, bbox.Contain (1, 3));
            Assert.AreEqual (false, bbox.Contain (3, 1));
            Assert.AreEqual (false, bbox.Contain (3, 3));
        }
    }
}
