using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// --------------------------------------------------------------------------------------
// TiledSharp の TiledCore.cs : var asm = Assembly.GetEntryAssembly(); の所で
// エラーが出るためコメントアウト
// さてどうしたものか

namespace DD.UnitTest {

    [TestClass]
    public class TestTiledMapComposer {


        /*
        // DD.UnitTest.TestTiledMapComposer+MyComponent, DD.UnitTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
        public class MyComponent : Component {
            public MyComponent ()
                : base () {
            }
            public int ObjectProperty1 { get; set; }
            public float ObjectProperty2 { get; set; }
            public string ObjectProperty3 { get; set; }
            public bool ObjectProperty4 { get; set; }
        }

        [TestMethod]
        public void Test_New () {
            var map = new TiledMapComposer ();

            Assert.AreEqual (0, map.Width);
            Assert.AreEqual (0, map.Height);
            Assert.AreEqual (0, map.TileWidth);
            Assert.AreEqual (0, map.TileHeight);
            Assert.AreEqual (0, map.LayerCount);
            Assert.AreEqual (0, map.MapLayerCount);
            Assert.AreEqual (0, map.ObjectLayerCount);
            Assert.AreEqual (0, map.ImageLayerCount);
        }

        [TestMethod]
        public void Test_OrthoToIsometric () {

        }

        [TestMethod]
        public void Test_LoadFromFile () {
            var node = new Node ();
            var map = new TiledMapComposer ();
            node.Attach (map);
            map.LoadFromFile ("desert.tmx");

            Assert.AreEqual ("desert.tmx", node.Name);
            Assert.AreEqual (40, map.Width);
            Assert.AreEqual (40, map.Height);
            Assert.AreEqual (32, map.TileWidth);
            Assert.AreEqual (32, map.TileHeight);
            Assert.AreEqual (1, map.MapLayerCount);
            Assert.AreEqual (1, map.ObjectLayerCount);
            Assert.AreEqual (1, map.ImageLayerCount);
            Assert.AreEqual (3, map.LayerCount);
            Assert.AreEqual (3, map.Node.ChildCount);
        }

        [TestMethod]
        public void Test_Layer () {
            var node = new Node ();
            var map = new TiledMapComposer ();
            node.Attach (map);
            map.LoadFromFile ("desert.tmx");

            Assert.AreEqual (1, map.MapLayerCount);

            var layer = map.GetLayer(0).GetComponent<TiledMapLayer>();

            Assert.AreEqual ("Ground", layer.Node.Name);
            Assert.AreEqual (40, layer.Height);
            Assert.AreEqual (40, layer.Width);
            Assert.AreEqual (1600, layer.TileCount);
            Assert.AreEqual (1600, layer.Tiles.Count());
            Assert.AreEqual (1600, layer.Node.ChildCount);
        }


        [TestMethod]
        public void Test_Tile () {
            var node = new Node ();
            var map = new TiledMapComposer ();
            node.Attach (map);
            map.LoadFromFile ("desert.tmx");

            var layer = map.GetLayer(0).GetComponent<TiledMapLayer>();

            Assert.AreEqual (1600, layer.TileCount);

            var tile = layer.GetTile (1, 1);

            Assert.AreEqual ("Ground[1,1]", tile.Name);
            Assert.AreEqual (32, tile.X);
            Assert.AreEqual (32, tile.Y);
            Assert.AreEqual (0, tile.BoundingBox.X);
            Assert.AreEqual (0, tile.BoundingBox.Y);
            Assert.AreEqual (32, tile.BoundingBox.Width);
            Assert.AreEqual (32, tile.BoundingBox.Height);
        }

        [TestMethod]
        public void Test_ObjectGroup () {
            var node = new Node ();
            var map = new TiledMapComposer ();
            node.Attach (map);
            map.LoadFromFile ("desert.tmx");

            Assert.AreEqual (1, map.ObjectLayerCount);
            Assert.AreEqual (1, map.ObjectLayers.Count ());

            var layer = node.Find (x => x.Name == "Objects");

            Assert.AreEqual ("Objects", layer.Name);
            Assert.AreEqual (2, layer.ChildCount);
        }

        [TestMethod]
        public void Test_Object () {
            var a = typeof (MyComponent);
            var b = a.AssemblyQualifiedName;
            var c = a.FullName;
            var fff = Type.GetType ("DD.UnitTest.MyComponent, DD.UnitTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");

            var node = new Node ();
            var map = new TiledMapComposer ();
            node.Attach (map);
            map.LoadFromFile ("desert.tmx");

            var obj1 = node.Find (x => x.Name == "Object1");
            var objComp1 = obj1.GetComponent<MyComponent> ();

            Assert.AreEqual ("Object1", obj1.Name);
            Assert.AreEqual (192f, obj1.X, 0.0001f);
            Assert.AreEqual (64f, obj1.Y, 0.0001f);
            Assert.AreEqual (0, obj1.BoundingBox.X);
            Assert.AreEqual (0, obj1.BoundingBox.Y);
            Assert.AreEqual (48, obj1.BoundingBox.Width);
            Assert.AreEqual (48, obj1.BoundingBox.Height);
            Assert.AreEqual (100, objComp1.ObjectProperty1);
            Assert.AreEqual (100.5f, objComp1.ObjectProperty2);
            Assert.AreEqual ("Hello Tiled 1", objComp1.ObjectProperty3);
            Assert.AreEqual (true, objComp1.ObjectProperty4);

            var obj2 = node.Find (x => x.Name == "Object2");
            var objComp2 = obj2.GetComponent<MyComponent> ();

            Assert.AreEqual ("Object2", obj2.Name);
            Assert.AreEqual (72f, obj2.X, 0.0001f);
            Assert.AreEqual (121f, obj2.Y, 0.0001f);
            Assert.AreEqual (0, obj2.BoundingBox.X);
            Assert.AreEqual (0, obj2.BoundingBox.Y);
            Assert.AreEqual (52, obj2.BoundingBox.Width);
            Assert.AreEqual (52, obj2.BoundingBox.Height);
            Assert.AreEqual (200, objComp2.ObjectProperty1);
            Assert.AreEqual (200.5f, objComp2.ObjectProperty2);
            Assert.AreEqual ("Hello Tiled 2", objComp2.ObjectProperty3);
            Assert.AreEqual (true, objComp2.ObjectProperty4);
        }
         * * */
    }
         
}
