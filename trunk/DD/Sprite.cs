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
        List<Texture> texs;
        Texture active;
        int offsetX;
        int offsetY;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// アクティブなテクスチャーは null にセットされます。
        /// </remarks>
        public Sprite () {
            this.texs = new List<Texture>();
            this.active = null;
            this.offsetX = 0;
            this.offsetY = 0;
        }
        #endregion

        #region Property

        /// <summary>
        /// 現在アクティブなテクスチャー番号
        /// </summary>
        /// <remarks>
        /// 現在アクティブなテクスチャーの番号を取得または変更します。
        /// このプロパティはアニメーション システムが値を書き換えてアクティブ テクスチャーを変更する為に存在しています。
        /// 配列型のプロパティのアニメーションに対応したら消すかもしれません。
        /// </remarks>
        public int ActiveTextureIndex {
            get { return texs.FindIndex(x => x == active); }
            set {
                if (value < 0 || value > TextureCount - 1) {
                    throw new IndexOutOfRangeException ("Index is out of range");
                }
                this.active = texs[value];
            }
        }

        /// <summary>
        /// 現在アクティブなテクスチャー
        /// </summary>
        /// <remarks>
        /// 現在アクティブなテクスチャーの番号を取得または変更します。
        /// </remarks>
        public Texture ActiveTexture {
            get { return active; }
            set {
                if (!texs.Contains (value)) {
                    throw new ArgumentException ("Texture is not in this Sprite");
                }
                this.active = value;
            }
        }

        /// <summary>
        /// オフセット座標X（ピクセル数）
        /// </summary>
        /// <remarks>
        /// オフセット座標はスプライトを描画する時にローカル座標位置の原点から(x,y)だけ動かします。
        /// </remarks>
        public int OffsetX {
            get { return offsetX; }
            set { SetOffset (value, offsetX); }
        }

        /// <summary>
        /// オフセット座標Y（ピクセル数）
        /// </summary>
        /// <remarks>
        /// オフセット座標はスプライトを描画する時にローカル座標位置の原点から(x,y)だけ動かします。
        /// </remarks>
        public int OffsetY {
            get { return offsetY; }
            set { SetOffset (offsetX, value); }
        }

        /// <summary>
        /// オフセット座標の変更
        /// </summary>
        /// <param name="x">X（ピクセル数）</param>
        /// <param name="y">Y（ピクセル数）</param>
        /// <returns></returns>
        public void SetOffset (int x, int y) {
            this.offsetX = x;
            this.offsetY = y;
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
        /// テクスチャーの追加
        /// </summary>
        /// <remarks>
        /// このスプライトで使用するテクスチャーを追加します。
        /// セットされたテクスチャーの中から1つを選んで（アクティブ テクスチャー）描画されます。
        /// </remarks>
        /// <param name="tex">テクスチャー オブジェクト</param>
        public void AddTexture (Texture tex) {
            if (tex == null) {
                throw new ArgumentNullException ("Texture is null");
            }
            this.texs.Add(tex);
            if (active == null) {
                this.active = tex;
            }
        }

        /// <summary>
        /// テクスチャーの削除
        /// </summary>
        /// <remarks>
        /// テクスチャーを削除します。もし削除されたテクスチャーがアクティブだった場合、
        /// 既存のテクスチャーの中から1つがアクティブ テクスチャーとして再選択されます。
        /// どれが選ばれるかは未定義です。
        /// </remarks>
        /// <param name="tex">削除したいテクスチャー</param>
        /// <returns>削除したら true, そうでなければ false.</returns>
        public bool RemoveTexture (Texture tex) {
            if (active == tex) {
                this.active = (TextureCount > 0) ? texs[0] : null;
            }
            return this.texs.Remove (tex);
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
            return active;
        }

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            if (active == null) {
                return;
            }
            var tex = active;
            var spr = new SFML.Graphics.Sprite (tex.Data);

            spr.Position = new Vector2f (Node.GlobalTranslation.X + offsetX,
                                         Node.GlobalTranslation.Y + offsetY);
            spr.Scale = new Vector2f (Node.GlobalScale.X, Node.GlobalScale.Y);
            spr.Rotation = Node.GlobalRotation.Angle;

            spr.TextureRect = new IntRect (tex.OffsetX, tex.OffsetY, tex.Width, tex.Height);

            var win = window as RenderWindow;
            win.Draw (spr);
        }

        #endregion
    }
}
