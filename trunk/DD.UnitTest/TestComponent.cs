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

           Assert.AreEqual (null, comp.Node);
        }

        [TestMethod]
        public void Test_Node () {
            var comp = new Component ();
            var node = new Node ();
            node.Attach (comp);
            node.UserID = 1;
            node.GroupID = 2;

            Assert.AreEqual (node, comp.Node);
            Assert.AreEqual (1, comp.UserID);
            Assert.AreEqual (2u, comp.GroupID);
        }

    }
}
