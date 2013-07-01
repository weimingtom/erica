using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestGridBoard {
        [TestMethod]
        public void TestMethod_New () {
            var grid = new GridBoard (1, 2);

            Assert.AreEqual (1, grid.Width);
            Assert.AreEqual (2, grid.Height);
            Assert.AreEqual (0, grid.TileWidth);
            Assert.AreEqual (0, grid.TileHeight);
            Assert.AreEqual (0, grid.MapWidth);
            Assert.AreEqual (0, grid.MapHeight);
            Assert.AreEqual (0, grid.TileCount);
            Assert.AreEqual (2, grid.GridCount);
        }

        [TestMethod]
        public void TestMethod_SetTileSize () {
            var grid = new GridBoard (2, 2);

            grid.TileWidth = 1;
            grid.TileHeight = 2;

            Assert.AreEqual (1, grid.TileWidth);
            Assert.AreEqual (2, grid.TileHeight);
            Assert.AreEqual (2, grid.MapWidth);
            Assert.AreEqual (4, grid.MapHeight);
            Assert.AreEqual (0, grid.TileCount);
            Assert.AreEqual (4, grid.GridCount);
        }

        [TestMethod]
        public void TestMethod_SetTile () {
            var grid = new GridBoard (2, 2);

            grid[0, 0] = new Node ("1");
            grid[0, 1] = new Node ("2");
            grid[1, 0] = new Node ("3");
            grid[1, 1] = new Node ("4");

            Assert.AreEqual (4, grid.TileCount);
            Assert.AreEqual ("1", grid[0, 0].Name);
            Assert.AreEqual ("2", grid[0, 1].Name);
            Assert.AreEqual ("3", grid[1, 0].Name);
            Assert.AreEqual ("4", grid[1, 1].Name);
            Assert.AreEqual (4, grid.TileCount);
            Assert.AreEqual (4, grid.GridCount);

            grid.SetTile (null, 0, 0);
            grid.SetTile (null, 0, 1);
            grid.SetTile (null, 1, 0);
            grid.SetTile (null, 1, 1);

            Assert.AreEqual (null, grid[0, 0]);
            Assert.AreEqual (null, grid[0, 1]);
            Assert.AreEqual (null, grid[1, 0]);
            Assert.AreEqual (null, grid[1, 1]);
            Assert.AreEqual (0, grid.TileCount);
            Assert.AreEqual (4, grid.GridCount);
        }
    }
}
