using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.TiledMapSample {

    public class MyTiledMap : Component {

        public static Node Create (string fileName) {
            var cmp = new Component ();
            var map = new TiledMapComposer ();
            var node = new Node ();
            node.Attach (cmp);
            node.Attach (map);

            map.LoadFromFile (fileName);

            var halfWidth = map.TileWidth / 2;
            var halfHeight = map.TileHeight / 2;
            var colMap = node.Find (x => x.Name == "Collision");
            foreach (var x in colMap.Downwards.Skip(1)) {
                var col = new BoxCollisionShape (halfWidth, halfHeight, 0);
                col.Offset = new Vector3 (halfWidth, halfHeight, 0);
                x.Attach (col);
            }

            return node;

        }
    }
}
