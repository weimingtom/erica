using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestLineSegment {
        [TestMethod]
        public void Test_New () {
            var line = new LineSegment ();

            Assert.AreEqual (1.0f, line.Width);
            Assert.AreEqual (10, line.Length);
            Assert.AreEqual (Color.White, line.Color);
            Assert.AreEqual (null, line.Texture);
        }

        [TestMethod]
        public void Test_SetLineWidth () {
            var line = new LineSegment ();
            line.Width = 2.0f;

            Assert.AreEqual (2.0f, line.Width);
        }

        [TestMethod]
        public void Test_SetLineLength () {
            var line = new LineSegment ();
            line.Length = 2;
            
            Assert.AreEqual (2.0f, line.Length);
        }

        [TestMethod]
        public void Test_SetColor () {
            var line = new LineSegment ();

            line.Color = Color.Blue;
            Assert.AreEqual (Color.Blue, line.Color);

            line.SetColor (255, 0, 0, 255);
            Assert.AreEqual (Color.Red, line.Color);
        }

        [TestMethod]
        public void Test_SetTexture () {
            var line = new LineSegment ();
            var tex = new Texture("abstract7.png");

            line.Texture = tex;
            Assert.AreEqual (tex, line.Texture);
        }

    }
}
