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
    /// スプライトは矩形・固定サイズの画像表示コンポーネントです。
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
        public Sprite (string name)
            : this () {
            this.name = name;
            LoadTexture (name);
        }
        #endregion

        #region Property
        /// <summary>
        /// 幅（ピクセル数）
        /// </summary>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// 高さ（ピクセル数）
        /// </summary>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// テクスチャー名
        /// </summary>
        public string Name {
            get { return name; }
        }

        #endregion

        #region Method
        /// <summary>
        /// テクスチャーのロード
        /// </summary>
        /// <remarks>
        /// テクスチャーをロードします。
        /// </remarks>
        /// <param name="name"></param>
        public void LoadTexture (string name) {
            this.tex = ResourceManager.GetInstance ().GetTexture (name);
        }

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            if (tex == null) {
                return;
            }
            var win = window as RenderWindow;
            var sprite = new SFML.Graphics.Sprite (tex);
            sprite.Position = new Vector2f (Node.GlobalX, Node.GlobalY);
            win.Draw (sprite);
        }

        /// <inheritdoc/>
        public override string ToString () {
            return name;
        }
        #endregion
    }
}
