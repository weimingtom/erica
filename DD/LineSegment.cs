using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;
using SFML.Graphics;

namespace DD {
    /// <summary>
    /// ライン コンポーネント
    /// </summary>
    /// <remarks>
    /// 画面上に直線（ライン）を引きます。
    /// ラインは追加で色とテクスチャーを変更可能です。
    /// </remarks>
    public class LineSegment : Component {
        #region Field
        float length;
        float width;
        Color color;
        Texture texture;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// 初期値は、
        /// <remarks>
        /// <list type="bullet">
        ///   <item>幅 = 1ピクセル</item>
        ///   <item>長さ = 10ピクセル</item>
        ///   <item>カラー = 白</item>
        /// </list>
        /// です。
        /// </remarks>
        public LineSegment () {
            this.length = 10;
            this.width = 1;
            this.color = Color.White;
            this.texture = null;
        }
        #endregion

        #region Property
        /// <summary>
        /// 線幅（ピクセル数）
        /// </summary>
        public float Width {
            get { return width; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("LineWidth is invalid");
                }
                this.width = value;
            }
        }

        /// <summary>
        /// 線の長さ（ピクセル数）
        /// </summary>
        public float Length {
            get { return length; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("Length is invalid");
                }
                this.length = value;
            }
        }

        /// <summary>
        /// テクスチャー
        /// </summary>
        public Texture Texture {
            get { return texture; }
            set { this.texture = value; }
        }

        /// <summary>
        /// 線の基本色
        /// </summary>
        public Color Color {
            get { return color; }
            set { this.color = value; }
        }
        #endregion

        #region Method
        /// <summary>
        /// 線の基本色の変更
        /// </summary>
        /// <param name="r">赤</param>
        /// <param name="g">青</param>
        /// <param name="b">緑</param>
        /// <param name="a">不当明度</param>
        public void SetColor (byte r, byte g, byte b, byte a) {
            this.color = new Color (r, g, b, a);
        }

        /// <inheritdoc/>
        public override void OnDraw (object window) {

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
                axis *= -1;
            }

            var tex = texture ?? Resource.GetDefaultTexture ();
            var spr = new SFML.Graphics.Sprite ();

            spr.Texture = tex.Data;
            spr.TextureRect = new IntRect (0, 0, (int)length, (int)width);
            spr.Origin = new Vector2f (0, width / 2);

            spr.Position = new Vector2f (T.X, T.Y);
            spr.Scale = new Vector2f (S.X, S.Y);
            spr.Rotation = angle;


            var opacity = Node.Upwards.Aggregate (1.0f, (x, node) => x * node.Opacity);
            var col = new Color (color.R, color.G, color.B, (byte)(color.A * opacity));

            spr.Color = col.ToSFML ();

            var win = window as RenderWindow;
            win.Draw (spr);
        }
        #endregion

    }
}
