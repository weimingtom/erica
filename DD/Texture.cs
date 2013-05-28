using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace DD {
    /// <summary>
    /// テクスチャー クラス
    /// </summary>
    /// <remarks>
    /// テクスチャーは1枚の画像を表します。
    /// テクスチャーは画像全体から一部の表示領域を選んで表示可能です。
    /// </remarks>
    public class Texture :  IDisposable {
        #region Field
        string name;
        int imageWidth;
        int imageHeight;
        int offsetX;
        int offsetY;
        int width;
        int height;
        SFML.Graphics.Texture data;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="fileName">テクスチャー ファイル名</param>
        public Texture (string fileName) {
            this.name = fileName;
            this.data = new SFML.Graphics.Texture (fileName);
            this.imageWidth = (int)data.Size.X;
            this.imageHeight = (int)data.Size.Y;
            this.offsetX = 0;
            this.offsetY = 0;
            this.width = imageWidth;
            this.height = imageHeight;

            this.data.Repeated = true;
        }
        #endregion

        #region Property
        /// <summary>
        /// 内部使用のテクスチャー データへのアクセッサー
        /// </summary>
        public SFML.Graphics.Texture Data {
            get { return data; }
        }


        /// <summary>
        /// テクスチャー名
        /// </summary>
        /// <remarks>
        /// 現在選択されているテクスチャー画像の名前
        /// </remarks>
        public string Name {
            get { return name; }
        }
       
        /// <summary>
        /// テクスチャー画像の幅（ピクセル数）
        /// </summary>
        /// <remarks>
        /// このテクスチャーの元になった画像の幅（ピクセル数）。
        /// </remarks>
        public int ImageWidth {
            get { return imageWidth; }
        }

        /// <summary>
        /// テクスチャー画像の高さ（ピクセル数）
        /// </summary>
        /// <remarks>
        /// このテクスチャーの元になった画像の高さ（ピクセル数）。
        /// </remarks>
        public int ImageHeight {
            get { return imageHeight; }
        }

        /// <summary>
        /// テクスチャー オフセットのX座標
        /// </summary>
        public int OffsetX {
            get { return offsetX; }
            set { SetOffset (value, offsetY); }
        }

        /// <summary>
        /// テクスチャー オフセットのY座標
        /// </summary>
        public int OffsetY {
            get { return offsetY; }
            set { SetOffset (offsetX, value); }
        }

        /// <summary>
        /// テクスチャー領域の幅（ピクセル数）
        /// </summary>
        public int Width {
            get { return width; }
            set { SetSize (value, height); }
        }

        /// <summary>
        /// テクスチャー領域の高さ（ピクセル数）
        /// </summary>
        public int Height {
            get { return height; }
            set { SetSize (width, value); }
        }

        #endregion

        #region Method

        /// <summary>
        /// テクスチャーオフセット位置の変更
        /// </summary>
        /// <remarks>
        /// テクスチャーの表示領域を変更します。領域は元の画像サイズとは無関係に指定可能です。
        /// 範囲外の場合はテクスチャーが無限に繰り返されていると仮定して描画されます。
        /// </remarks>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        public void SetOffset (int x, int y) {
            this.offsetX = x;
            this.offsetY = y;
        }

        /// <summary>
        /// テクスチャー サイズの変更
        /// </summary>
        /// <remarks>
        /// テクスチャーの表示領域を変更します。領域は元の画像サイズとは無関係に指定可能です。
        /// 範囲外の場合はテクスチャーが無限に繰り返されていると仮定して描画されます。
        /// </remarks>
        /// <param name="width">幅（ピクセル数）</param>
        /// <param name="height">高さ（ピクセル数）</param>
        public void SetSize (int width, int height) {
            if (width < 0) {
                throw new ArgumentException ("Width is invalid");
            }
            if (height < 0) {
                throw new ArgumentException ("Height is invalid");
            }
            this.width = width;
            this.height = height;
        }


        /// <inheritdoc/>
        public void Dispose () {
            if (data != null) {
                data.Dispose ();
                data = null;
            }
        }
        #endregion
    }
}
