using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;
using DD.Physics;

namespace DD.Sample.IsometricSample {
    
    public class MyMap : Component {

        public static Node Create (string fileName) {
            var tiled = new TiledMapComposer ();
            var cmp = new MyMap ();

            var node = new Node ("Map");
            node.Attach (tiled);
            node.Attach (cmp);

            tiled.LoadFromFile (fileName);
            
            var halfWidth = tiled.TileWidth / 2;
            var halfHeight = tiled.TileHeight / 2;

            
                var obs = node.Find ("ForeObject");
            foreach (var tile in obs.Downwards) {
                tile.DrawPriority = -2;
            }
            
            // コリジョンは仮想2D直交座標系で作成する
            
            var colMap = node.Find("Collision");
            foreach (var tile in colMap.Downwards.Skip(1)) {
                var col = new BoxCollisionShape (halfWidth, halfHeight, 0.0f);
                col.SetOffset (halfWidth, halfHeight, 0);

                tile.Attach (col);
            }
            

            return node;
        }
    }
}
