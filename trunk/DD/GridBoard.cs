using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {

    /// <summary>
    /// グリッド ボード コンポーネント
    /// </summary>
    /// <remarks>
    /// 将棋盤やチェス盤のような2次元格子状のノードの集合を管理するコンポーネントです。
    /// 
    /// </remarks>
    public class GridMap : Component {

        #region Field
        int width;
        int height;
        Node[,] tiles;
        int tileWidth;
        int tileHeight;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="width">X方向のタイルの総数</param>
        /// <param name="height">Y方向のタイルの総数</param>
        public GridMap (int width, int height) {
            if (width < 0 || height < 0) {
                throw new ArgumentException ("Grid Size is invalid");
            }
            this.width = width;
            this.height = height;
            this.tiles = new Node[height, width];
        }
        #endregion

        #region Property
        /// <summary>
        /// 横方向のタイル数
        /// </summary>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// 縦方向のタイル数
        /// </summary>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// 横方向のマップ サイズ（ピクセル数）
        /// </summary>
        public int MapWidth {
            get { return width * tileWidth; }
        }

        /// <summary>
        /// 縦方向のマップ サイズ（ピクセル数）
        /// </summary>
        public int MapHeight {
            get { return height * tileHeight; }
        }

        /// <summary>
        /// タイル1枚の横幅（ピクセル数）
        /// </summary>
        public int TileWidth {
            get { return tileWidth; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("TileWidth is invalid");
                }
                this.tileWidth = value;
            }
        }

        /// <summary>
        /// タイル1枚の縦幅（ピクセル数）
        /// </summary>
        public int TileHeight {
            get { return tileHeight; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("TileHeight is invalid");
                }
                this.tileHeight = value;
            }
        }

        /// <summary>
        /// グリッドの総数
        /// </summary>
        /// <remarks>
        /// マス目の個数です。かならず <see cref="Width"/> x <see cref="Height"/> に等しくなります。
        /// </remarks>
        public int GridCount {
            get { return width * height; }
        }

        /// <summary>
        /// タイルの総数
        /// </summary>
        /// <remarks>
        /// マス目にセットされたタイル ノードの数です。
        /// マス目は最大<see cref="Width"/> x <see cref="Height"/> 個ですが、
        /// すべてのマス目にタイル ノードがセットされているとは限りません。
        /// 従って <see cref="TileCount"/> \lte; <see cref="GridCount"/> です。
        /// </remarks>
        public int TileCount {
            get { return Tiles.Count (); }
        }

        /// <summary>
        /// タイルを列挙する列挙子
        /// </summary>
        /// <remarks>
        /// タイルとはマス目にセットされた <c>null</c> でないノードです。
        /// </remarks>
        public IEnumerable<Node> Tiles {
            get { return tiles.Cast<Node> ().Where (x => x != null); }
        }

        /// <summary>
        /// タイルにアクセスするアクセッサー
        /// </summary>
        /// <remarks>
        /// このインデクサーはそれぞれ <see cref="GetTile"/>, <see cref="SetTile"/> と等価です。
        /// </remarks>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public Node this[int y, int x] {
            get { return GetTile(x, y); }
            set { SetTile(value, x, y); }
        }
        #endregion

        #region Method

        /// <summary>
        /// タイルの取得
        /// </summary>
        /// <param name="x">X方向のタイル番号</param>
        /// <param name="y">X方向のタイル番号</param>
        /// <returns>タイル ノード</returns>
        public Node GetTile (int x, int y) {
            if (x < 0 || x > width - 1) {
                throw new IndexOutOfRangeException ("Index is out of ragnge");
            }
            if (y < 0 || y > height - 1) {
                throw new IndexOutOfRangeException ("Index is out of ragnge");
            }
            return tiles[y, x];
        }

        /// <summary>
        /// タイルの変更
        /// </summary>
        /// <remarks>
        /// タイル ノードには <c>null</c> も可能です。
        /// </remarks>
        /// <param name="node">タイル ノード</param>
        /// <param name="x">X方向のタイル番号</param>
        /// <param name="y">X方向のタイル番号</param>
        public void SetTile (Node node, int x, int y) {
            if (x < 0 || x > width - 1) {
                throw new IndexOutOfRangeException ("Index is out of ragnge");
            }
            if (y < 0 || y > height - 1) {
                throw new IndexOutOfRangeException ("Index is out of ragnge");
            }
            this.tiles[y, x] = node;
        }
        #endregion
    }
}
