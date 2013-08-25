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
            var mbox = new MailBox ("Address");

            Assert.AreEqual ("Address", mbox.Address);
        }

        [TestMethod]
        public void Test_Action () {
            var mbox = new MailBox ("Address");
            var recv = 0;

            mbox.Action += new MailBoxAction ((from, adress, letter) => {
                recv += 1;
            }); 
            mbox.Action += new MailBoxAction ((from, adress, letter) => {
                recv += 1;
            });
            
            // 登録済みアクションの起動
            mbox.OnMailBox (null, "address", null);

            Assert.AreEqual (2, recv);
        }
    }
}
