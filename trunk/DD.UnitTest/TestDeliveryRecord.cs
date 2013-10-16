using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestDeliveryRecord {
        [TestMethod]
        public void Test_New () {
            var mail = new Mail (new Node ("From"), "Address", "Letter");
            var rec = new DeliveryRecord (1, mail);

            Assert.AreEqual (1, rec.Time);
            Assert.AreEqual ("From", rec.From);
            Assert.AreEqual ("Address", rec.Address);
            Assert.AreEqual ("String", rec.LetterType);
            Assert.AreEqual ("Letter", rec.Letter);
        }

    }
}
