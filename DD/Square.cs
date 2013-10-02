using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;
using SFML.Graphics;

namespace DD {

    /// <summary>
    /// 枠表示コンポーネント
    /// </summary>
    /// <remarks>
    /// 4角形の枠を表示するコンポーネントです。ライン <see cref="Line"/> の4角形版に相当します。
    /// 4角形の中身は塗りつぶしません。
    /// あまり使い道のなさそうなコンポーネントですが結構便利です。
    /// </remarks>
    public class Square : Component {

        #region 
        float width;
        float height;
        float lineWidth;
        Color color;
        Vector3 offset;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// 枠線は縦横サイズの内側に表示されます。
        /// </remarks>
        /// <param name="width">横サイズ</param>
        /// <param name="height">縦サイズ</param>
        /// <param name="lineWidth">ライン幅</param>
        public Square (float width, float height, float lineWidth) {
            this.width = width;
            this.height = height;
            this.lineWidth = lineWidth;
            this.color = Color.White;
        }
        #endregion

        #region Property
        /// <summary>
        /// 横サイズ
        /// </summary>
        public float Width {
            get { return width; }
            set {
                if (value <= 0) {
                    throw new ArgumentException ("Width is invalid");
                }
                this.width = value;
            }
        }

        /// <summary>
        /// 縦サイズ
        /// </summary>
        public float Height{
            get{return height;}
            set {
                if (value <= 0) {
                    throw new ArgumentException ("Height is invalid");
                }
                this.height = value;
            }
        }

        /// <summary>
        /// 線幅
        /// </summary>
        public float LineWidth {
            get { return lineWidth; }
            set {
                if (value <= 0) {
                    throw new ArgumentException ("LineWidth is invalid");
                }
                this.lineWidth = value;
            }
        }

        /// <summary>
        /// 線色
        /// </summary>
        /// <remarks>
        /// デフォルトは白色です。
        /// </remarks>
        public Color Color {
            get { return color; }
            set { SetColor (value.R, value.G, value.B, value.A); }
        }

        /// <summary>
        /// オフセット
        /// </summary>
        public Vector3 Offset {
            get { return offset; }
            set { this.offset = value; }
        }
        #endregion

        #region Method
        /// <summary>
        /// サイズの変更
        /// </summary>
        /// <param name="width">横サイズ</param>
        /// <param name="height">縦サイズ</param>
        public void Resize (float width, float height) {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// 線色の変更
        /// </summary>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">不透明度</param>
        public void SetColor (byte r, byte g, byte b, byte a) {
            this.color = new Color (r, g, b, a);
        }

        /// <summary>
        /// オフセットの変更
        /// </summary>
        /// <param name="x">オフセットX</param>
        /// <param name="y">オフセットY</param>
        /// <param name="z">オフセットZ</param>
        public void SetOffset (float x, float y, float z) {
            this.offset = new Vector3 (x, y, z);
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

            var opacity = Node.Upwards.Aggregate (1.0f, (x, node) => x * node.Opacity);
            var col = new Color (color.R, color.G, color.B, (byte)(color.A * opacity));

            var spr1 = new SFML.Graphics.Sprite ();
            spr1.Texture = Resource.GetDefaultTexture ().Data;
            spr1.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)width, (int)lineWidth);
            spr1.Position = new Vector2f (T.X + offset.X, T.Y + offset.Y);
            spr1.Scale = new Vector2f (S.X, S.Y);
            spr1.Rotation = angle;
            spr1.Color = color.ToSFML();

            var spr2 = new SFML.Graphics.Sprite ();
            spr2.Texture = Resource.GetDefaultTexture ().Data;
            spr2.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)width, (int)lineWidth);
            spr2.Origin = new Vector2f (0, -(height - lineWidth));
            spr2.Position = new Vector2f (T.X + offset.X, T.Y + offset.Y);
            spr2.Scale = new Vector2f (S.X, S.Y);
            spr2.Rotation = angle;
            spr2.Color = color.ToSFML ();

            var spr3 = new SFML.Graphics.Sprite ();
            spr3.Texture = Resource.GetDefaultTexture ().Data;
            spr3.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)lineWidth, (int)height);
            spr3.Position = new Vector2f (T.X + offset.X, T.Y + offset.Y);
            spr3.Scale = new Vector2f (S.X, S.Y);
            spr3.Rotation = angle;
            spr3.Color = color.ToSFML ();

            var spr4 = new SFML.Graphics.Sprite ();
            spr4.Texture = Resource.GetDefaultTexture ().Data;
            spr4.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)lineWidth, (int)height);
            spr4.Origin = new Vector2f (-(width - LineWidth), 0);
            spr4.Position = new Vector2f (T.X + offset.X, T.Y + offset.Y);
            spr4.Scale = new Vector2f (S.X, S.Y);
            spr4.Rotation = angle;
            spr4.Color = color.ToSFML ();


            var win = window as RenderWindow;
            win.Draw (spr1);
            win.Draw (spr2);
            win.Draw (spr3);
            win.Draw (spr4);
        }
        #endregion


    }
}
