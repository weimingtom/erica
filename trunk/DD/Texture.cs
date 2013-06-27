using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using System.Drawing.Imaging;
using System.IO;

namespace DD {
    /// <summary>
    /// テクスチャー クラス
    /// </summary>
    /// <remarks>
    /// テクスチャーは1枚の画像を表します。
    /// </remarks>
    public class Texture :  IDisposable {
        #region Field
        string name;
        int width;
        int height;
        SFML.Graphics.Texture data;
        #endregion
      

        #region Constructor

        /// <summary>
        /// テクスチャー オブジェクトを画像ファイルから作成
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public Texture (string fileName){
            if (fileName == null || fileName == "") {
                throw new ArgumentNullException ("FileName is null or empty");
            }

            this.name = fileName;
            this.data = new SFML.Graphics.Texture (fileName);
            this.width = (int)this.data.Size.X;
            this.height = (int)this.data.Size.Y;

            this.data.Repeated = true;
        }

        /// <summary>
        /// テクスチャー オブジェクトをビットマップ画像から作成
        /// </summary>
        /// <param name="bitmap">ビットマップ</param>
        /// <param name="name">テクスチャー名</param>
        public Texture (System.Drawing.Bitmap bitmap, string name) {
            if (bitmap == null) {
                throw new ArgumentNullException ("Bitmap is null");
            }
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null or empty");
            }

            var tmp = new MemoryStream ();
            bitmap.Save (tmp, ImageFormat.Png);
            var mem = new MemoryStream (tmp.GetBuffer());

            this.name = name;
            this.data = new SFML.Graphics.Texture (mem);
            this.width = (int)this.data.Size.X;
            this.height = (int)this.data.Size.Y;

            this.data.Repeated = true;
        }

        /// <summary>
        /// テクスチャー オブジェクトをメモリー上の画像ファイルから作成
        /// </summary>
        /// <param name="memory">メモリー ストリーム</param>
        /// <param name="name">テクスチャー名</param>
        public Texture (System.IO.MemoryStream memory, string name) {
            if (memory == null) {
                throw new ArgumentNullException ("Memory stream is null");
            }
            if (!memory.CanRead) {
                throw new ArgumentException ("Memory stream can't be read");
            }
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null or empty");
            }

            this.name = name;
            this.data = new SFML.Graphics.Texture (memory);
            this.width = (int)this.data.Size.X;
            this.height = (int)this.data.Size.Y;

            this.data.Repeated = true;
        }

        #endregion


        #region Property
        /// <summary>
        /// 内部使用のテクスチャー データへのアクセッサー
        /// </summary>
        /// <remarks>
        /// 戻り値の型は object の方が良かったかも。
        /// </remarks>
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
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// テクスチャー画像の高さ（ピクセル数）
        /// </summary>
        /// <remarks>
        /// このテクスチャーの元になった画像の高さ（ピクセル数）。
        /// </remarks>
        public int Height {
            get { return height; }
        }

        #endregion

        #region Method

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
