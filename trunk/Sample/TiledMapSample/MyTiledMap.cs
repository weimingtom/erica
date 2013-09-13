using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {

    public class MyTiledMap : Component {

        public static Node Create (string fileName) {
            var cmp = new MyTiledMap ();
            var map = new TiledMapComposer ();

            var node = new Node ("TiledMap");
            node.Attach (cmp);
            node.Attach (map);

            map.LoadFromFile (fileName);

            var halfTileWidth = map.TileWidth / 2;
            var halfTileHeight = map.TileHeight / 2;

            var colMap = node.Find ("CollisionMap");
            foreach (var x in colMap.Downwards.Skip(1)) {
                var col = new CollisionObject ();
                col.Shape = new BoxShape (halfTileWidth, halfTileHeight, 1);
                col.SetOffset (halfTileWidth, halfTileHeight, 1);
                x.Attach (col);
            }

            var cabMap = node.Find ("CabbageMap");
            foreach (var nod in cabMap.Downwards.Skip (1)) {
                var comp = new MyCabbage ();

                var col = new CollisionObject ();
                col.Shape = new BoxShape (halfTileWidth, halfTileHeight, 1);
                col.SetOffset (halfTileWidth, halfTileHeight, 1);

                nod.Attach (comp);
                nod.Attach (col);

                //n.GroupID = 0xffffffffu;
            }

            var bgMap = node.Find ("BackgroundMap");
            //bgMap.GroupID = 0xffffffffu;

            return node;
        }

        public override void OnUpdate (long msec) {
            /*
            var cabMap = Node.Find ("CabbageMap");
            if (cabMap != null && cabMap.Downwards.Count () == 1) {
                var node = MyGameClear.Create ();
                node.SetTranslation (300, 120, 0);
                World.ActiveCamera.AddChild (node);
                Destroy (cabMap);
            }
        */

        }
    }
}
