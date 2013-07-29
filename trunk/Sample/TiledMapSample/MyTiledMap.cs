using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.TiledMapSample {

    public class MyTiledMap : Component {

        public static Node Create (string fileName) {
            var cmp = new MyTiledMap ();
            var map = new TiledMapComposer ();
            var node = new Node ();
            node.Attach (cmp);
            node.Attach (map);

            map.LoadFromFile (fileName);

            var halfTileWidth = map.TileWidth / 2;
            var halfTileHeight = map.TileHeight / 2;

            var colMap = node.Find ("Collision");
            foreach (var x in colMap.Downwards.Skip(1)) {
                var col = new BoxCollisionShape (halfTileWidth, halfTileHeight, 0);
                col.Offset = new Vector3 (halfTileWidth, halfTileHeight, 0);
                x.Attach (col);
            }

            var cabMap = node.Find ("Cabbage");
            foreach (var n in cabMap.Downwards.Skip (1)) {
                var col = new BoxCollisionShape (halfTileWidth, halfTileHeight, 0);
                col.Offset = new Vector3 (halfTileWidth, halfTileHeight, 0);
                n.Attach (col);

                var cab = new MyCabbage ();
                n.Attach (cab);

                n.GroupID = 0xffffffffu;
            }

            var bgMap = node.Find ("Background");
            bgMap.GroupID = 0xffffffffu;

            return node;
        }

        public override void OnUpdate (long msec) {

            var cabMap = Node.Find ("Cabbage");
            if (cabMap != null && cabMap.Downwards.Count () == 1) {
                var node = MyGameClear.Create ();
                node.SetTranslation (300, 120, 0);
                World.ActiveCamera.AddChild (node);
                Destroy (cabMap);
            }
        
        }
    }
}
