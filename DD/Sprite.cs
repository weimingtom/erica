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
    /// スプライトは複数のテクスチャーをセット可能で、そのうちの1つを選んで描画します。
    /// 有効なテクスチャーを次々に変更する事で「ぱらぱらアニメ」のような簡易アニメーションを実装可能です。
    /// スプライトはバウンディング ボックスを書き換えません。
    /// <note>
    /// 描画の際に Node.Opacity の実効値をかけるべきですが現在未実装です。
    /// </note>
    /// </remarks>
    public partial class Sprite : Component {
        #region Field
        int width;
        int height;
        Vector2 offset;
        List<Texture> texs;
        int active;
        Color color;
        Vector2 texOffset;
        bool autoScale;
        #endregion

        #region Constructor
        /// <summary>
        /// サイズを指定して <see cref="Sprite"/> コンポーネントを作成
        /// </summary>
        /// <remarks>
        /// サイズ (<paramref name="width"/>, <paramref name="height"/>)の
        /// スプライト オブジェクトを作成します。
        /// テクスチャーは未設定です。このまま描画すると指定サイズの単色ブロックが表示されます。
        /// </remarks>
        public Sprite (int width, int height) {
            this.width = width;
            this.height = height;
            this.texs = new List<Texture> ();
            this.active = 0;
            this.offset = new Vector2 (0, 0);
            this.color = Color.White;
            this.texOffset = new Vector2 (0, 0);
            this.autoScale = false;
        }

        /// <summary>
        /// サイズ未指定で <see cref="Sprite"/> コンポーネントを作成
        /// </summary>
        /// <remarks>
        /// サイズ未指定でスプライト オブジェクトを作成します。
        /// サイズはテクスチャーがセットされると自動的にそのサイズになります。
        /// テクスチャーサイズと同一のスプライトを作りたい時に便利です。
        /// テクスチャーは未設定です。
        /// このまま描画すると何も表示されません。。
        /// </remarks>
        public Sprite ()
            : this (0, 0) {
        }
        #endregion

        #region Property

        /// <summary>
        /// 現在アクティブなテクスチャー番号
        /// </summary>
        /// <remarks>
        /// 現在アクティブなテクスチャーの番号を取得または変更します。
        /// スプライトは複数のテクスチャーの中から1つを選んで描画します。
        /// </remarks>
        public int ActiveTexture {
            get { return active; }
            set {
                if (value < 0 || value > TextureCount - 1) {
                    throw new IndexOutOfRangeException ("Index is out of range");
                }
                this.active = value;
            }
        }

        /// <summary>
        /// オフセット（ピクセル座標）
        /// </summary>
        /// <remarks>
        /// このスプライトを描画する際にオフセット ピクセル分ずらして描画します。
        /// </remarks>
        public Vector2 Offset {
            get { return offset; }
            set { SetOffset (value.X, value.Y); }
        }

        /// <summary>
        /// スプライトの幅（ピクセル数）
        /// </summary>
        /// <remarks>
        /// このスプライトの幅（ピクセル数）です。
        /// </remarks>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// スプライトの高さ（ピクセル数）
        /// </summary>
        /// <remarks>
        /// このスプライトの高さ（ピクセル数）です。
        /// </remarks>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// テクスチャーの自動スケーリング
        /// </summary>
        /// <remarks>
        /// <c>true</c> の場合は1枚のテクスチャー画像をスプライトサイズに拡大･縮小して表示します。
        /// <c>false</c> の場合はテクスチャー画像サイズはそのままに繰り返しコピーでスプライト領域を埋めます。
        /// </remarks>
        public bool AutoScale {
            get { return autoScale; }
            set { this.autoScale = value; }
        }

        /// <summary>
        /// テクスチャー オフセット（ピクセル数）
        /// </summary>
        /// <remarks>
        /// テクスチャーを取得する際に画像ピクセルを (0,0) からではなく、このピクセル数分ずらした位置から取得します。
        /// 1枚のテクスチャー画像を複数のスプライトで使用したり、時間によって位置を変更してぱらぱらアニメ機能を実装するのに利用可能です。
        /// </remarks>
        public Vector2 TextureOffset {
            get { return texOffset; }
            set { this.texOffset = value; }
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
        /// オフセットの変更
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
        /// <param name="x">X方向のオフセット量（ピクセル数）</param>
        /// <param name="y">Y方向のオフセット量（ピクセル数）</param>
        public void SetOffset (float x, float y) {
            this.offset = new Vector2 (x, y);
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
        /// スプライトには複数のテクスチャーをセット可能で、
        /// その中から1つを選んで（アクティブ）描画されます。
        /// </remarks>
        /// <param name="tex">テクスチャー オブジェクト</param>
        public void AddTexture (Texture tex) {
            if (tex == null) {
                throw new ArgumentNullException ("Texture is null");
            }
            this.texs.Add (tex);
            if (width == 0 && height == 0) {
                this.width = tex.Width;
                this.height = tex.Height;
            }
        }

        /// <summary>
        /// テクスチャーの変更
        /// </summary>
        /// <param name="index">テクスチャー番号</param>
        /// <param name="tex">テクスチャー</param>
        public void SetTexture (int index, Texture tex) {
            if (index < 0 || index > TextureCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            this.texs[index] = tex;
            if (width == 0 && height == 0) {
                this.width = tex.Width;
                this.height = tex.Height;
            }
        }

        /// <summary>
        /// テクスチャーの削除
        /// </summary>
        /// <remarks>
        /// テクスチャーを削除します。
        /// アクティブ テクスチャーは変更されません。
        /// </remarks>
        /// <param name="tex">削除したいテクスチャー</param>
        /// <returns>削除したら true, そうでなければ false.</returns>
        public bool RemoveTexture (Texture tex) {
            if (tex == null) {
                return false;
            }
            return this.texs.Remove (tex);
        }

        /// <summary>
        /// テクスチャーの取得
        /// </summary>
        /// <param name="index">テクスチャー番号</param>
        /// <returns></returns>
        public Texture GetTexture (int index) {
            if (index < 0 || index > TextureCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return texs[index];
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

        /// <summary>
        /// サイズの変更
        /// </summary>
        /// <remarks>
        /// <paramref name="width"/> と <paramref name="height"/> に負の値を設定すると画像が反転します。
        /// 0を指定した場合、アクティブなテクスチャーのサイズと同じ値に設定されます。
        /// 現在アクティブなテクスチャーが存在しない場合、後でセットされます。
        /// </remarks>
        /// <param name="width">スプライトの幅（ピクセル数）</param>
        /// <param name="height">スプライトの高さ（ピクセル数）</param>
        public void Resize (int width, int height) {
            if (width == 0 && height == 0 && texs[active] != null) {
                width = texs[active].Width;
                height = texs[active].Height;
            }

            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// テクスチャー オフセット（ピクセル数）の変更
        /// </summary>
        /// <param name="x">X方向のオフセット量（ピクセル数）</param>
        /// <param name="y">Y方向のオフセット量（ピクセル数）</param>
        public void SetTextureOffset (int x, int y) {
            this.texOffset = new Vector2 (x, y);
        }



        /// <inheritdoc/>
        public override void OnDraw (object window) {
            if (width == 0 || height == 0) {
                return;
            }

            Vector3 T;
            Quaternion R;
            Vector3 S;
            Node.GlobalTransform.Decompress (out T, out R, out S);

            // クォータニオンは指定したのと等価な軸が反対で回転角度[0,180]の回転で返ってくる事がある
            // ここで回転軸(0,0,-1)のものを(0,0,1)に変換する必要がある
            var angle = R.Angle;
            var axis = R.Axis;
            var dot = Vector3.Dot (axis, new Vector3 (0, 0, 1));
            if (dot < 0) {
                angle = 360 - angle;
                axis = -axis;
            }

            var opacity = Node.Upwards.Aggregate (1.0f, (x, node) => x * node.Opacity);

            var tex = texs[active] ?? Resource.GetDefaultTexture ();
            var spr = new SFML.Graphics.Sprite ();
            spr.Texture = tex.Data;

            var posX = (width > 0) ? T.X : T.X;// -width;
            var posY = (height > 0) ? T.Y : T.Y;// -height;
            spr.Position = new Vector2f (posX, posY);
            spr.Rotation = angle;
            spr.Origin = new Vector2f (-offset.X, -offset.Y);
            spr.Color = new Color (color.R, color.G, color.B, (byte)(color.A * opacity)).ToSFML ();


            if (autoScale) {
                var texX = (int)texOffset.X;
                var texY = (int)texOffset.Y;
                var texWidth = (width > 0) ? tex.Width : -tex.Width;
                var texHeight = (height > 0) ? tex.Height : -tex.Height;
                spr.TextureRect = new IntRect (texX, texY, texWidth, texHeight);
                spr.Scale = new Vector2f (S.X * Math.Abs (width / (float)tex.Width),
                                          S.Y * Math.Abs (height / (float)tex.Height));
            }
            else {
                var texX = (int)texOffset.X;
                var texY = (int)texOffset.Y;
                var texWidth = width;
                var texHeight = height;
                spr.TextureRect = new IntRect (texX, texY, texWidth, texHeight);
                spr.Scale = new Vector2f (S.X, S.Y);
            }

            var win = window as RenderWindow;
            win.Draw (spr);
        }


        /// <inheritdoc/>
        public override string ToString () {
            var str = "";
            for (var i = 0; i < texs.Count (); i++) {
                str += string.Format ("{0}{2}: {1}, ", i, texs[i], (i == active) ? "(active)" : "");
            }
            return str;
        }

        #endregion
    }
}
