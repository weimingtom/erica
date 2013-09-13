using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.IsometricSample {
    
    public class MyTiledMap : Component {

        public static Node Create (string fileName) {
            var cmp = new MyTiledMap ();

            var tiledMap = new TiledMapComposer ();

            var node = new Node ("TiledMap");
            node.Attach (tiledMap);
            node.Attach (cmp);

            tiledMap.LoadFromFile (fileName);
            
            var halfWidth = tiledMap.TileWidth / 2;
            var halfHeight = tiledMap.TileHeight / 2;

           
            // コリジョンなどのゲームロジックはすべて
            // 仮想の2D直交座標系で作成する
            // 表示だけアイソメトリック
            var colMap = node.Find("CollisionMap");
            foreach (var tile in colMap.Downwards.Skip(1)) {
                var col = new CollisionObject();
                col.Shape = new BoxShape (halfWidth, halfHeight, 1);
                col.SetOffset (halfWidth, halfHeight, 1);

                tile.Attach (col);
            }
            
            return node;
        }
    }
}
