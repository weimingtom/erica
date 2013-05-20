using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace DD {
    /// <summary>
    /// スプライト クラス
    /// </summary>
    /// <remarks>
    /// スプライトは矩形・固定サイズの画像を表示します。
    /// </remarks>
    public partial class Sprite : Component {
        #region Field
        string name;
        int width;
        int height;
        Texture tex;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Sprite () {
            this.name = "";
            this.width = 0;
            this.height = 0;
            this.tex = null;
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// インスタンスを作成し、指定のテクスチャーをロードします。
        /// </remarks>
        /// <param name="name">テクスチャー ファイル名</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> が <c>null</c></exception>
        public Sprite (string name)
            : this () {
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null");
            }
            this.name = name;
            LoadTexture (name);
        }
        #endregion

        #region Property

        /// <summary>
        /// 画像の幅（ピクセル数）
        /// </summary>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// 画像の高さ（ピクセル数）
        /// </summary>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// テクスチャー名
        /// </summary>
        public string TextureName {
            get { return name; }
        }

        #endregion

        #region Method
        /// <summary>
        /// テクスチャーのロード
        /// </summary>
        /// <remarks>
        /// テクスチャーを指定の画像ファイルからロードします。
        /// スプライトのサイズはテクスチャー画像のサイズと同一です。
        /// </remarks>
        /// <param name="name">テクスチャー ファイル名</param>
        public void LoadTexture (string name) {
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null");
            }
            this.tex = ResourceManager.GetInstance ().GetTexture (name);
            this.name = name;
            this.width = (int)tex.Size.X;
            this.height = (int)tex.Size.Y;
        }

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            if (tex == null) {
                return;
            }
            var sprite = new SFML.Graphics.Sprite (tex);
            sprite.Position = new Vector2f (Node.X, Node.Y);

            var win = window as RenderWindow;
            win.Draw (sprite);
        }

        /// <inheritdoc/>
        public override void OnMouseButtonPressed (MouseButton button, int x, int y) {
            Console.WriteLine ("Mouse button is pressed, x=" + x + ", y=" + y);

        }

        /// <inheritdoc/>
        public override void OnMouseButtonReleased (MouseButton button, int x, int y) {
            Console.WriteLine ("Mouse button is released, x=" + x + ", y=" + y);
        }

        /// <inheritdoc/>
        public override string ToString () {
            return name;
        }
        #endregion
    }
}
