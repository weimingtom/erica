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
        Color color;
        #endregion

        #region Constructor
        /// <summary>
        /// 標準のコンストラクター
        /// </summary>
        /// <remarks>
        /// テクスチャーは未定義です。
        /// アクティブなテクスチャーは null にセットされます。
        /// </remarks>
        public Sprite () {
            this.texs = new List<Texture> ();
            this.active = null;
            this.offsetX = 0;
            this.offsetY = 0;
            this.color = Color.White;
        }

        /// <summary>
        /// テクスチャーを指定するコンストラクター
        /// </summary>
        /// <remarks>
        /// テクスチャーを指定して <see cref="Sprite"/> コンポーネントを作成します。
        /// アクティブなテクスチャーはそれにセットされます。
        /// </remarks>
        public Sprite (Texture texture) {
            if (texture == null) {
                throw new ArgumentNullException ("Texture is null");
            }
            this.texs = new List<Texture> () {texture};
            this.active = texture;
            this.offsetX = 0;
            this.offsetY = 0;
            this.color = Color.White;
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
            get { return texs.FindIndex (x => x == active); }
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
        /// オフセット座標はスプライトを描画する時にローカル座標の原点からずらすピクセル数(x,y)です。
        /// </remarks>
        public int OffsetX {
            get { return offsetX; }
            set { SetOffset (value, offsetY); }
        }

        /// <summary>
        /// オフセット座標Y（ピクセル数）
        /// </summary>
        /// <remarks>
        /// オフセット座標はスプライトを描画する際にローカル座標の原点からずらすピクセル数(x,y)です。
        /// </remarks>
        public int OffsetY {
            get { return offsetY; }
            set { SetOffset (offsetX, value); }
        }

        /// <summary>
        /// 画像の幅（ピクセル数）
        /// </summary>
        /// <remarks>
        /// 画像の幅は現在アクティブなテクスチャーの画像領域と同じです。
        /// アクティブなテクスチャーを変更すると変わる事があります。。
        /// </remarks>
        public int Width {
            get { return (active != null) ? active.Width : 0; }
        }

        /// <summary>
        /// 画像の高さ（ピクセル数）
        /// </summary>
        /// <remarks>
        /// 画像の高さは現在アクティブなテクスチャーの画像領域と同じです。
        /// アクティブなテクスチャーを変更すると変わる事があります。。
        /// </remarks>
        public int Height {
            get { return (active != null) ? active.Height : 0; }
        }

        /// <summary>
        /// スプライトの基本色
        /// </summary>
        /// <remarks>
        /// スプライトはこの色とテクスチャーをかけ算した色で描画されます。
        /// </remarks>
        public Color Color {
            get { return color; }
            set { SetColor (value.R, value.G, value.B, value.A); }
        }

        /// <summary>
        /// オフセット座標の変更
        /// </summary>
        /// <remarks>
        /// スプライトを（ローカル座標上で）原点から (<paramref name="x"/>,<paramref name="y"/>) だけずらします。
        /// これは典型的には回転中心を画像の中心にあわせるのに使用します。
        /// <code>
        ///          var spr = new Sprite ();
        ///          spr.OffsetX = -spr.ActiveTexture.Width / 2 ;
        ///          spr.OffsetY = -spr.ActiveTexture.Height / 2;
        /// </code>
        /// </remarks>
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
            this.texs.Add (tex);
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
                this.active = texs.FirstOrDefault (x => x != tex);
            }
            return this.texs.Remove (tex);
        }

        /// <summary>
        /// テクスチャーの取得
        /// </summary>
        /// <param name="i">テクスチャー番号</param>
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
        /// <returns>テクスチャー</returns>
        public Texture GetActiveTexture () {
            return active;
        }

        /// <summary>
        /// 基本色の変更
        /// </summary>
        /// <remarks>
        /// スプライトの基本色を変更します。
        /// スプライトはこの色とテクスチャーをかけ算した色で描画されます。
        /// </remarks>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">不透明度</param>
        public void SetColor (byte r, byte g, byte b, byte a) {
            this.color = new Color (r, g, b, a);
        }



        /// <inheritdoc/>
        public override void OnDraw (object window) {
            if (active == null) {
                return;
            }
            var tex = active;
            var spr = new SFML.Graphics.Sprite (tex.Data);

            Vector3 point;
            Quaternion rotation;
            Vector3 scale;
            Node.GlobalTransform.Decompress (out point, out rotation, out scale);

            // クォータニオンは指定したのと等価な軸が反対で回転角度[0,180]の回転で返ってくる事がある
            // ここで回転軸(0,0,-1)のものを(0,0,1)に変換する必要がある
            var angle = rotation.Angle;
            var axis = rotation.Axis;
            var dot = Vector3.Dot (axis, new Vector3 (0, 0, 1));
            if (dot < 0) {
                angle = 360 - angle;
                axis = -axis;
            }

            spr.Position = new Vector2f (point.X, point.Y);
            spr.Scale = new Vector2f (scale.X, scale.Y);
            spr.Rotation = angle;
            spr.TextureRect = new IntRect (tex.OffsetX, tex.OffsetY, tex.Width, tex.Height);

            spr.Origin = new Vector2f (-offsetX, -offsetY);
            spr.Color = color.ToSFML ();

            var win = window as RenderWindow;
            win.Draw (spr);
        }

        #endregion
    }
}
