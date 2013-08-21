using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestMail {
        [TestMethod]
        public void Test_New () {
            var from = new Node ("Node1");
            var mail = new Mail (from, "Node2", "Hello World");

            Assert.AreEqual (from, mail.From);
            Assert.AreEqual ("Node2", mail.Address);
            Assert.AreEqual ("Hello World", mail.Letter);
        }
    }
}
