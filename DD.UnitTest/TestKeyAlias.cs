using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestKeyAlias {
        [TestMethod]
        public void Test_New () {
            var alias = new KeyAlias ("Name", KeyCode.Space);

            Assert.AreEqual ("Name", alias.Name);
            Assert.AreEqual (KeyCode.Space, alias.KeyCode);
        }
    }
}
