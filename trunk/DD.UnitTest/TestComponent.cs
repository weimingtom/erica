using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestComponent {
        [TestMethod]
        public void Test_New () {
           var comp = new Component ();

           Assert.AreEqual(null, comp.Node);
           Assert.AreEqual (0, comp.OffsetX);
           Assert.AreEqual (0, comp.OffsetY);
        }

        [TestMethod]
        public void Test_XY () {
           var comp = new Component ();
           comp.OffsetX = 1;
           comp.OffsetY = 2;

           Assert.AreEqual (1, comp.OffsetX);
           Assert.AreEqual (2, comp.OffsetY);
        }
    }
}
