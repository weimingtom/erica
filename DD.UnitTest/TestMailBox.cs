using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestMailBox {

        [TestMethod]
        public void Test_New () {
            var action = new MailBoxAction ((from, address, letter) => { });
            var mbox = new MailBox ("Node1", action);

            Assert.AreEqual ("Node1", mbox.NamePlate);
            Assert.AreEqual (action, mbox.Action);
        }
    }
}
