using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace DD {
    /// <summary>
    /// スプライト コンポーネント
    /// </summary>
    /// <remarks>
    /// スプライトは矩形・固定サイズの画像を表示します。
    /// スプライトは複数のテクスチャーをセット可能で、そのうちの1つだけを選んで有効化します。
    /// 有効なテクスチャーを次々に変更する事で「ぱらぱらアニメ」のような簡易アニメーションを実装可能です。
    /// スプライトはバウンディング ボックスを書き換えません。
    /// </remarks>
    public partial class Sprite : Component {
        #region Field
        Texture[] texs;
        int active;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// テクスチャーを最大 <paramref name="count"/> 枚持つ <see cref="Sprite"/> オブジェクトのインスタンスを生成します。
        /// アクティブなテクスチャーは0番にセットされます。
        /// </remarks>
        /// <param name="count">テクスチャーの最大数</param>
        public Sprite (int count) {
            if (count <= 0) {
                throw new ArgumentException ("Texture count is invalid");
            }
            this.texs = new Texture[count];
            this.active = 0;
        }
        #endregion

        #region Property

        /// <summary>
        /// 現在アクティブなテクスチャー番号
        /// </summary>
        /// <remarks>
        /// 現在アクティブなテクスチャーの番号を取得または変更します。
        /// </remarks>
        public int ActiveTexture {
            get { return active; }
            set { this.active = value; }
        }

        /// <summary>
        /// テクスチャーの個数
        /// </summary>
        public int TextureCount {
            get { return texs.Count (); }
        }

        /// <summary>
        /// すべてのテクスチャーを列挙する列挙子
        /// </summary>
        public IEnumerable<Texture> Textures {
            get { return texs; }
        }

        #endregion

        #region Method
        /// <summary>
        /// テクスチャーの変更
        /// </summary>
        /// <param name="index">テクスチャー番号</param>
        /// <param name="tex">テクスチャー オブジェクト</param>
        public void SetTexture (int index, Texture tex) {
            if (tex == null) {
                throw new ArgumentNullException ("Texture is null");
            }
            if (index < 0 || index > TextureCount - 1) {
                throw new ArgumentException ("Index is out of range.");
            }

            this.texs[index] = tex;
        }

        /// <summary>
        /// テクスチャーの取得
        /// </summary>
        /// <param name="i">インデックス</param>
        /// <returns></returns>
        public Texture GetTexture (int i) {
            if (i < 0 || i > TextureCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return texs[i];
        }

        /// <summary>
        /// アクティブなテクスチャーの取得
        /// </summary>
        /// <returns></returns>
        public Texture GetActiveTexture () {
            return texs[active];
        }

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            var tex = texs[active];
            if (tex == null) {
                return;
            }
            var spr = new SFML.Graphics.Sprite (tex.Data);
            spr.Position = new Vector2f (Node.WindowX, Node.WindowY);

            var rec = tex.ActiveRegion;
            spr.TextureRect = new IntRect (rec.X, rec.Y, rec.Width, rec.Height);

            var win = window as RenderWindow;
            win.Draw (spr);
        }

        #endregion
    }
}
