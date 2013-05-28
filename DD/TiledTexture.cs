using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// タイル テクスチャー クラス
    /// </summary>
    /// <remarks>
    /// 1枚のテクスチャーをサブテクスチャー（タイル）に分けて使用するテクスチャーです。
    /// 典型的には特殊効果のぱらぱらアニメに使用します。
    /// </remarks>
    public class TiledTexture : Texture {
        #region Field
        int rows;
        int columns;
        int tileWidth;
        int tileHeight;
        int active;
        int tiles;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="name">テクスチャー名</param>
        /// <param name="rows">縦方向のタイル数</param>
        /// <param name="columns">横方向のタイル数</param>
        /// <param name="tiles">有効なタイル数</param>
        public TiledTexture (string name, int rows, int columns, int tiles)
            : base (name) {
            if (rows <= 0 || columns <= 0) {
                throw new ArgumentException ("Rows or Columns is invalid");
            }
            if (tiles <= 0 || tiles > rows * columns) {
                throw new ArgumentException ("TileCount is invalid");
            }
            this.rows = rows;
            this.columns = columns;
            this.tiles = tiles;
            this.tileWidth = ImageWidth / columns;
            this.tileHeight = ImageHeight / rows;
            this.Width = tileWidth;
            this.Height = tileHeight;

            this.ActiveTile = 0;
        }
        #endregion

        #region Property
        /// <summary>
        /// 縦方向のタイル数
        /// </summary>
        public int Rows {
            get { return rows; }
        }

        /// <summary>
        /// 横方向のタイル数
        /// </summary>
        public int Columns {
            get { return columns; }
        }

        /// <summary>
        /// 有効なタイル数
        /// </summary>
        public int TileCount {
            get { return tiles; }
        }

        /// <summary>
        /// アクティブなタイル番号
        /// </summary>
        public int ActiveTile {
            get { return active; }
            set { SetActiveTile (value); }
        }

        /// <summary>
        /// アクティブタイルの変更
        /// </summary>
        /// <remarks>
        /// アクティブなテクスチャー領域を指定の番号のタイルに変更します。
        /// </remarks>
        /// <param name="index">タイル番号</param>
        public void SetActiveTile (int index) {
            if (index < 0 || index > TileCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of ragne");
            }
            this.active = index;
            var x = (index % columns) * tileWidth;
            var y = (index / columns) * tileHeight;
            this.SetOffset (x, y);
        }


        #endregion

    }
}
