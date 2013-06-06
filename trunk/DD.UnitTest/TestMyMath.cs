using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestMyMath {
        [TestMethod]
        public void Test_Clamp () {
            Assert.AreEqual (1.0f, MyMath.Clamp (0.5f, 1.0f, 2.0f));
            Assert.AreEqual (1.5f, MyMath.Clamp (1.5f, 1.0f, 2.0f));
            Assert.AreEqual (2.0f, MyMath.Clamp (2.5f, 1.0f, 2.0f));
        }
    }
}
