using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;

namespace DD.UnitTest {
    [TestClass]
    public class TestTexture {
        [TestMethod]
        public void Test_New_1_FromFile () {
            var tex = new Texture ("abstract7.png");

            Assert.AreEqual ("abstract7.png", tex.Name);
            Assert.AreEqual (614, tex.Width);
            Assert.AreEqual (1024, tex.Height);
        }

        [TestMethod]
        public void Test_New_2_FromBitmap () {
            var bitmap = new Bitmap ("abstract7.png");
            var tex = new Texture (bitmap, "abstract7.png");

            Assert.AreEqual ("abstract7.png", tex.Name);
            Assert.AreEqual (614, tex.Width);
            Assert.AreEqual (1024, tex.Height);
        }

        [TestMethod]
        public void Test_New_3_FromStream () {

            var bytes = File.ReadAllBytes ("abstract7.png");
            var tex = new Texture (new MemoryStream(bytes), "abstract7.png");

            Assert.AreEqual ("abstract7.png", tex.Name);
            Assert.AreEqual (614, tex.Width);
            Assert.AreEqual (1024, tex.Height);
        }

    }
}
