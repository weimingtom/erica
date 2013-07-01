using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestMyMath {
        [TestMethod]
        public void Test_Clamp_1 () {
            Assert.AreEqual (1.0f, MyMath.Clamp (0.5f, 1.0f, 2.0f));
            Assert.AreEqual (1.5f, MyMath.Clamp (1.5f, 1.0f, 2.0f));
            Assert.AreEqual (2.0f, MyMath.Clamp (2.5f, 1.0f, 2.0f));

        }

        [TestMethod]
        public void Test_Clamp_2 () {
            Assert.AreEqual (10, MyMath.Clamp (5, 10, 20));
            Assert.AreEqual (15, MyMath.Clamp (15, 10, 20));
            Assert.AreEqual (20, MyMath.Clamp (25, 10, 20));
        }
    }
}
