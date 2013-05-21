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
        }

    }
}
