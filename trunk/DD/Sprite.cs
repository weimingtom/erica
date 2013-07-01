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
        Texture active;
        Color color;
        Vector2 texOffset;
        #endregion

        #region Constructor
        /// <summary>
        /// デフォルトの <see cref="Sprite"/> コンポーネントを作成
        /// </summary>
        /// <remarks>
        /// テクスチャーは未定義です。
        /// アクティブなテクスチャーは null にセットされます。
        /// </remarks>
        public Sprite (int width, int height) {
            this.width = width;
            this.height = height;
            this.texs = new List<Texture> ();
            this.active = null;
            this.offset = new Vector2(0, 0);
            this.color = Color.White;
            this.texOffset = new Vector2 (0, 0);
        }

        /// <summary>
        /// テクスチャーを指定して <see cref="Sprite"/> コンポーネントを作成
        /// </summary>
        /// <remarks>
        /// テクスチャーを指定して <see cref="Sprite"/> コンポーネントを作成します。
        /// スプライトのサイズはテクスチャー画像のサイズと同一に設定されます。
        /// </remarks>
        public Sprite (Texture texture) {
            if (texture == null) {
                throw new ArgumentNullException ("Texture is null");
            }
            this.width = texture.Width;
            this.height = texture.Height;
            this.texs = new List<Texture> () {texture};
            this.active = texture;
            this.offset = new Vector2(0,0);
            this.color = Color.White;
            this.texOffset = new Vector2 (0, 0);
        }

        /// <summary>
        /// テクスチャーとサイズを指定して <see cref="Sprite"/> コンポーネントを作成
        /// </summary>
        /// <remarks>
        /// テクスチャーとサイズを指定して <see cref="Sprite"/> コンポーネントを作成します。
        /// スプライトのサイズはテクスチャー画像サイズとは無関係に引数で指定した値が使用されます。
        /// </remarks>
        /// <param name="texture">テクスチャー</param>
        /// <param name="width">幅（ピクセル数）</param>
        /// <param name="height">高さ（ピクセル数）</param>
        public Sprite (Texture texture, int width, int height) : this(texture) {
            if (width < 0 || height < 0) {
                throw new ArgumentException ("Size is invalid");
            }
            this.width = width;
            this.height = height;
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
        /// 現在アクティブなテクスチャーを取得または変更します。
        /// ここで指定できるテクスチャーはこのスプライト オブジェクトに登録済みの物に限ります。
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
        /// このスプライトが描画されるときの幅（ピクセルス数）です。
        /// 変更はできません。
        /// </remarks>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// 画像の高さ（ピクセル数）
        /// </summary>
        /// <remarks>
        /// このスプライトが描画されるときの高さ（ピクセルス数）です。
        /// 変更はできません。
        /// </remarks>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// テクスチャーオフセット（ピクセル数）
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
        /// <param name="x">X（ピクセル数）</param>
        /// <param name="y">Y（ピクセル数）</param>
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
        /// テクスチャー オフセット（ピクセル数）の変更
        /// </summary>
        /// <param name="x">X方向のオフセット量（ピクセル数）</param>
        /// <param name="y">Y方向のオフセット量（ピクセル数）</param>
        public void SetTextureOffset (int x, int y) {
            this.texOffset = new Vector2 (x, y);
        }



        /// <inheritdoc/>
        public override void OnDraw (object window) {
            if (active == null) {
                return;
            }
            var tex = active;

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

            var spr = new SFML.Graphics.Sprite (tex.Data);
            spr.Position = new Vector2f (point.X, point.Y);
            spr.Scale = new Vector2f (scale.X, scale.Y);
            spr.Rotation = angle;
            spr.TextureRect = new IntRect ((int)texOffset.X, (int)texOffset.Y, width, height);
            
            spr.Origin = new Vector2f (-offset.X, -offset.Y);
            spr.Color = color.ToSFML ();

            var win = window as RenderWindow;
            win.Draw (spr);
        }

        #endregion
    }
}
