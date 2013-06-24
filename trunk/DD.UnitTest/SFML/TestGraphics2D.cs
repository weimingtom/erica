using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestGraphics2D {
        [TestMethod]
        public void Test_GetInstance () {
            var g2d = Graphics2D.GetInstance ();

            Assert.IsNotNull (g2d);
        }

        [TestMethod]
        public void Test_GetWindow () {
            var g2d = Graphics2D.GetInstance ();
            var win = g2d.GetWindow ();

            Assert.IsNotNull (win);
        }

        [TestMethod]
        public void Test_GetKeys () {
            var g2d = Graphics2D.GetInstance ();
            var keys = g2d.GetKeys ();

            Assert.AreEqual (0, keys.Count());
        }


        [TestMethod]
        public void Test_GetMousePosition () {
            var g2d = Graphics2D.GetInstance ();
            var mpos = g2d.GetMousePosition ();

            Assert.AreEqual (new Vector2 (0, 0), mpos);
        }

        [TestMethod]
        public void Test_GetMouseWheele () {
            var g2d = Graphics2D.GetInstance ();
            var mwhel = g2d.GetMouseWheele ();

            Assert.AreEqual (0f, mwhel);
        }
    
    }
}
