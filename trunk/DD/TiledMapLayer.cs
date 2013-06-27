using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TiledSharp;

namespace DD {
    public class TiledMapLayer : Component {
        int width;
        int height;
        Node[,] tiles;
        
        #region Constructor
        public TiledMapLayer (int width, int height) {
            this.width = width;
            this.height = height;
            this.tiles = new Node[height, width];
        }
        #endregion


        #region Property
        public int Width {
            get { return width; }
        }

        public int Height {
            get { return height; }
        }

        public int TileCount {
            get{return Tiles.Count();}
        }


        // これいいんだ・・・
        public IEnumerable<Node> Tiles {
            get { return tiles.Cast<Node>().Where(x => x != null); }
        }
        #endregion


        #region Method
        public Node GetTile (int x, int y) {
            return tiles[y, x];
        }

        public void SetTile (Node tile, int x, int y) {
            this.tiles[y, x] = tile;
        }
        #endregion
    }
}
