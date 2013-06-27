using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    /*
    [TestClass]
    public class TestTiledTexture {
        [TestMethod]
        public void Test_New () {
            var tex = new TiledTexture ("Explosion4x8.png", 4, 8, 30);

            Assert.AreEqual (1024, tex.ImageWidth);
            Assert.AreEqual (512, tex.ImageHeight);
            Assert.AreEqual (0, tex.ActiveTile);
            Assert.AreEqual (0, tex.TextureOffset.X);
            Assert.AreEqual (0, tex.TextureOffset.Y);
            Assert.AreEqual (128, tex.TextureWidth);
            Assert.AreEqual (128, tex.TextureHeight);
        }

        [TestMethod]
        public void Test_SetActiveTile () {
            var tex = new TiledTexture ("Explosion4x8.png", 4, 8, 30);
            tex.ActiveTile = 29;

            Assert.AreEqual (1024-128*3, tex.TextureOffset.X);
            Assert.AreEqual (512-128, tex.TextureOffset.Y);
            Assert.AreEqual (128, tex.TextureWidth);
            Assert.AreEqual (128, tex.TextureHeight);
        }
    }
     * */
}
