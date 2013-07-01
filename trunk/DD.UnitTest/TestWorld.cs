using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD;

namespace DD.UnitTest {
    [TestClass]
    public class TestWorld {

        [TestMethod]
        public void Test_New () {
            var wld = new World ("Script1");

            Assert.AreEqual ("Script1", wld.Name);
            Assert.AreEqual (null, wld.Director);
            Assert.IsNotNull (wld.InputReceiver);
            Assert.IsNotNull (wld.AnimationController);
        }

        [TestMethod]
        public void Test_SetDirector () {
            var direc = new Director ();
            var sce = new World ("Script1");

            sce.SetDirector (direc);

            Assert.AreEqual (direc, sce.Director);
        }

    }
}
