using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestTiledTexture {
        [TestMethod]
        public void Test_New () {
            var tex = new TiledTexture ("Explosion4x8.png", 4, 8, 30);

            Assert.AreEqual (1024, tex.Width);
            Assert.AreEqual (512, tex.Height);
            Assert.AreEqual (0, tex.ActiveTile);
            Assert.AreEqual (new Rectangle (0, 0, 128, 128), tex.ActiveRegion);
        }

        [TestMethod]
        public void Test_GetTileRegion(){
            var tex = new TiledTexture ("Explosion4x8.png", 4, 8, 30);

            var rect0 = tex.GetTileRegion (0);
            Assert.AreEqual (0, rect0.X);
            Assert.AreEqual (0, rect0.Y);
            Assert.AreEqual (128, rect0.Width);
            Assert.AreEqual (128, rect0.Height);

            var rect1 = tex.GetTileRegion (31);
            Assert.AreEqual (896, rect1.X);
            Assert.AreEqual (384, rect1.Y);
            Assert.AreEqual (128, rect1.Width);
            Assert.AreEqual (128, rect1.Height);
        }

        [TestMethod]
        public void Test_SetActiveTile () {
            var tex = new TiledTexture ("Explosion4x8.png", 4, 8, 30);
            tex.SetActiveTile (29);

            var rect = tex.ActiveRegion;
            Assert.AreEqual (1024-128*3, rect.X);
            Assert.AreEqual (512-128, rect.Y);
            Assert.AreEqual (128, rect.Width);
            Assert.AreEqual (128, rect.Height);
        }
    }
}
