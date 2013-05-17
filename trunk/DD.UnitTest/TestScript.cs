using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD;

namespace DD.UnitTest {
    [TestClass]
    public class TestScript {

        [TestMethod]
        public void Test_New () {
            var sce = new Script ("Script1");

            Assert.AreEqual ("Script1", sce.Name);
            Assert.AreEqual (null, sce.Director);
        }

        [TestMethod]
        public void Test_SetDirector () {
            var direc = new Director ();
            var sce = new Script ("Script1");

            sce.SetDirector (direc);

            Assert.AreEqual (direc, sce.Director);
        }


    }
}
