using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestLogger {
        [TestMethod]
        public void Test_New () {
            var log = new Logger ();

            Assert.AreEqual (0, log.LogCount);
            Assert.AreEqual (0, log.Logs.Count ());
        }

        [TestMethod]
        public void Test_Write () {
            var node = new Node ("Node");
            var log = new Logger ();
            log.Write (node, 1, "Hello World");

            Assert.AreEqual (1, log.LogCount);
            Assert.AreEqual (1, log.Logs.Count ());
            Assert.AreEqual ("Node", log.Logs.ElementAt (0).Node);
            Assert.AreEqual (1, log.Logs.ElementAt (0).Priority);
            Assert.AreEqual ("Hello World", log.Logs.ElementAt (0).Message);
        }

        [TestMethod]
        public void Test_Clear () {
            var node = new Node ("Node");
            var log = new Logger ();

            log.Write (node, 1, "Hello World");
            Assert.AreEqual (1, log.LogCount);

            log.Clear ();
            Assert.AreEqual (0, log.LogCount);
        }

    }
}
