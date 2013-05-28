using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestTexture {
        [TestMethod]
        public void Test_New () {
            var tex = new Texture ("abstract7.png");

            Assert.AreEqual ("abstract7.png", tex.Name);
            Assert.AreEqual (614, tex.ImageWidth);
            Assert.AreEqual (1024, tex.ImageHeight);
            Assert.AreEqual (0, tex.OffsetX);
            Assert.AreEqual (0, tex.OffsetY);
            Assert.AreEqual (614, tex.Width);
            Assert.AreEqual (1024, tex.Height);
        }

        [TestMethod]
        public void Test_SetOffset () {
            var tex = new Texture ("abstract7.png");

            tex.SetOffset (1, 2);
            Assert.AreEqual (1, tex.OffsetX);
            Assert.AreEqual (2, tex.OffsetY);

            tex.OffsetX = 3;
            tex.OffsetY = 4;
            Assert.AreEqual (3, tex.OffsetX);
            Assert.AreEqual (4, tex.OffsetY);
        }

        [TestMethod]
        public void Test_SetSize () {
            var tex = new Texture ("abstract7.png");

            tex.SetSize (1, 2);
            Assert.AreEqual (1, tex.Width);
            Assert.AreEqual (2, tex.Height);

            tex.Width = 3;
            tex.Height = 4;
            Assert.AreEqual (3, tex.Width);
            Assert.AreEqual (4, tex.Height);

        }
    }
}
