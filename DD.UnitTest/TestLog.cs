using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestLog {
        [TestMethod]
        public void Test_New () {
            var log = new Log ("Node", 1, "Message");

            Assert.AreEqual ("Node", log.Node);
            Assert.AreEqual (1, log.Priority);
            Assert.AreEqual ("Message", log.Message);
        }
    }
}
