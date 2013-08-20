using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestColor {
        [TestMethod]
        public void Test_New_1 () {
            var col = new Color (1, 2, 3);
            Assert.AreEqual (1, col.R);
            Assert.AreEqual (2, col.G);
            Assert.AreEqual (3, col.B);
            Assert.AreEqual (255, col.A);
        }

        public void Test_New_2 () {
            var col = new Color (1, 2, 3, 4);
            Assert.AreEqual (1, col.R);
            Assert.AreEqual (2, col.G);
            Assert.AreEqual (3, col.B);
            Assert.AreEqual (4, col.A);
        }

        [TestMethod]
        public void Test_Predefined_Color () {
            Assert.AreEqual (new Color(255,255,255,255), Color.White);
            Assert.AreEqual (new Color (128, 128, 128, 255), Color.Gray);
            Assert.AreEqual (new Color (0, 0, 0, 255), Color.Black);
            Assert.AreEqual (new Color (255, 0, 0, 255), Color.Red);
            Assert.AreEqual (new Color (0, 255, 0, 255), Color.Green);
            Assert.AreEqual (new Color (0, 0, 255, 255), Color.Blue);
            Assert.AreEqual (new Color (255, 255, 0, 255), Color.Yellow);
            Assert.AreEqual (new Color (255, 0, 255, 255), Color.Purple);
            Assert.AreEqual (new Color (0, 255, 255, 255), Color.Cyan);
        }

        [TestMethod]
        public void Test_Equals () {
            var a = new Color (1, 2, 3, 4);
            var b = new Color (1, 2, 3, 4);
            var c = new Color (1, 2, 3, 0);

            Assert.IsTrue (a.Equals (b));
            Assert.IsTrue (a == b);
            Assert.AreEqual (a.GetHashCode (), b.GetHashCode ());

            Assert.IsFalse (a.Equals (c));
            Assert.IsFalse (a == c);
            Assert.AreNotEqual (a.GetHashCode (), c.GetHashCode ());

        }
    }
}
