using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestTiledMapLayer {
        [TestMethod]
        public void TestMethod_New_1 () {
            var layer = new TiledMapLayer (16, 16);

            Assert.AreEqual (16, layer.Width);
            Assert.AreEqual (16, layer.Height);
            Assert.AreEqual (0, layer.TileCount);
            Assert.AreEqual (0, layer.Tiles.Count ());
        }

        [TestMethod]
        public void TestMethod_SetTile () {

            var layer = new TiledMapLayer (16, 16);
            var tile1 = new Node ();
            var tile2 = new Node ();

            layer.SetTile (tile1, 0, 0);
            layer.SetTile (tile2, 15, 15);

            Assert.AreEqual (2, layer.TileCount);
            Assert.AreEqual (2, layer.Tiles.Count ());

            Assert.AreEqual (tile1, layer.GetTile (0, 0));
            Assert.AreEqual (tile2, layer.GetTile (15, 15));
            Assert.AreEqual (null, layer.GetTile (10, 10));
        }
    }
}
