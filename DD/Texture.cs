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
    /// 画像は全体を使用するか一部だけを選らんで描画可能です。
    /// </remarks>
    public class Texture : IDisposable {
        #region Field
        string name;
        int width;
        int height;
        Rectangle region;
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
            this.width = (int)data.Size.X;
            this.height = (int)data.Size.Y;
            this.region = new Rectangle (0, 0, width, height);
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
        /// テクスチャー画像の幅（ピクセル数）。
        /// </remarks>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// テクスチャー画像の高さ（ピクセル数）
        /// </summary>
        /// <remarks>
        /// テクスチャー画像の高さ（ピクセル数）。
        /// </remarks>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// テクスチャーの有効表示領域
        /// </summary>
        /// <remarks>
        /// テクスチャーは画像全体か、もしくは一部を選んで描画されます。
        /// </remarks>
        public Rectangle ActiveRegion  {
            get { return region; }
            set { SetActiveRegion (value.X, value.Y, value.Width, value.Height); }
        }
        #endregion

        #region Method
        /// <summary>
        /// アクティブ領域の変更
        /// </summary>
        /// <remarks>
        /// <note>
        /// アクティブ領域は画像サイズに限定すべき？（有益？）
        /// 現状では特にチェックしていない。
        /// </note>
        /// </remarks>
        /// <param name="x">アクティブ領域の左上のX座標（ピクセル単位）</param>
        /// <param name="y">アクティブ領域の左上のY座標（ピクセル単位）</param>
        /// <param name="width">アクティブ領域の横幅（ピクセル単位）</param>
        /// <param name="height">アクティブ領域の高さ（ピクセル単位）</param>
        public void SetActiveRegion (int x, int y, int width, int height) {
            if (x < 0 || x > Width-1) {
                throw new ArgumentException ("X is invlaid");
            }
            if (y < 0 || y > Height - 1) {
                throw new ArgumentException ("Y is invlaid");
            }
            if (width < 0 || x + width > Width) {
                throw new ArgumentException ("Width is invalid");
            }
            if (height < 0 || y + height> Height) {
                throw new ArgumentException ("Height is invalid");
            }
            this.region = new Rectangle (x, y, width, height);
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
