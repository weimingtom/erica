﻿using System;
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
            var sce = new World ("Script1");

            Assert.AreEqual ("Script1", sce.Name);
            Assert.AreEqual (null, sce.Director);
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