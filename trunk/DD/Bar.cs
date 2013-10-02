using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace DD {

    /// <summary>
    /// バー コンポーネント
    /// </summary>
    /// <remarks>
    /// HPなどを表示するのに便利なバー コンポーネントです。
    /// バーは [0,最大値] の範囲の値をとり、現在値に応じて伸張します。
    /// 背景カラーとテクスチャー、前景カラーとテクスチャーを変更可能です。
    /// </remarks>
    public class Bar : Component {

        #region Field
        BarOrientation orientation;
        int width;
        int length;
        Color bgColor;
        Color fgColor;
        Texture backgroundTexture;
        Texture foregroundTexture;
        float currentValue;
        float maxValue;
        Vector2 offset;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="width">横幅（ピクセル数）</param>
        /// <param name="length">縦幅（ピクセル数）</param>
        /// <param name="ori">バーの伸びる方向</param>
        public Bar (int width, int length, BarOrientation ori) {
            if (width < 0 || length < 0) {
                throw new ArgumentException ("Bar size is invalie");
            }
            this.width = width;
            this.length = length;
            this.orientation = ori;
            this.bgColor = Color.Black;
            this.fgColor = Color.Green;
            this.backgroundTexture = null;
            this.foregroundTexture = null;
            this.maxValue = 0;
            this.currentValue = 0;
            this.offset = new Vector2 (0, 0);
        }
        #endregion

        #region Property
        /// <summary>
        /// バーの最大値
        /// </summary>
        public float MaxValue {
            get { return maxValue; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("MaxValue is invalid");
                }
                this.maxValue = value;
            }
        }

        /// <summary>
        /// 現在値
        /// </summary>
        public float CurrentValue {
            get { return currentValue; }
            set {
                this.currentValue = MyMath.Clamp (value, 0, maxValue);
            }
        }

        /// <summary>
        /// 現在値の最大値に対する比率 [0,1]
        /// </summary>
        public float CurrentRate {
            get { return (maxValue==0) ? 1 : currentValue / maxValue; }

        }

        /// <summary>
        /// 横幅（ピクセル数）
        /// </summary>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// 縦幅（ピクセル数）
        /// </summary>
        public int Height {
            get { return length; }
        }

        /// <summary>
        /// オフセット（ピクセル数）
        /// </summary>
        public Vector2 Offset {
            get { return offset; }
            set { this.offset = value; }
        }

        /// <summary>
        /// バーの伸びる方向
        /// </summary>
        public BarOrientation Orientation {
            get { return orientation; }
        }

        /// <summary>
        /// 背景色
        /// </summary>
        /// <remarks>
        /// デフォルト黒
        /// </remarks>
        public Color BackgroundColor {
            get { return bgColor; }
            set { this.bgColor = value; }
        }
        
        /// <summary>
        /// バーの色
        /// </summary>
        /// <remarks>
        /// デフォルト緑
        /// </remarks>
        public Color ForegroundColor {
            get { return fgColor; }
            set { this.fgColor = value; }
        }

        /// <summary>
        /// 背景テクスチャー
        /// </summary>
        /// <remarks>
        /// デフォルトなし
        /// </remarks>
        public Texture BackgroundTexture {
            get { return backgroundTexture; }
            set { this.backgroundTexture = value; }
        }

        /// <summary>
        /// バーのテクスチャー
        /// </summary>
        /// <remarks>
        /// デフォルト無し
        /// </remarks>
        public Texture ForegroundTexture {
            get { return foregroundTexture; }
            set { this.foregroundTexture = value; }
        }

        #endregion

        #region Method
        /// <summary>
        /// オフセットの変更
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetOffset (float x, float y) {
            this.offset = new Vector2 (x, y);
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

            var spr1 = new SFML.Graphics.Sprite ();
            spr1.Texture = (backgroundTexture ?? Resource.GetDefaultTexture ()).Data;
            spr1.TextureRect = (orientation == BarOrientation.Horizontal) ? new IntRect (0, 0, (int)length, (int)width)
                                                                          : new IntRect (0, 0, (int)width, (int)length);
            spr1.Origin = new Vector2f (-offset.X, -offset.Y);
            spr1.Position = new Vector2f (T.X, T.Y);
            spr1.Scale = new Vector2f (S.X, S.Y);
            spr1.Rotation = angle;
            
            spr1.Color = new Color (bgColor.R, bgColor.G, bgColor.B, (byte)(bgColor.A * opacity)).ToSFML ();
        

            var spr2 = new SFML.Graphics.Sprite ();
            spr2.Texture = (foregroundTexture ?? Resource.GetDefaultTexture ()).Data;
            spr2.TextureRect = (orientation == BarOrientation.Horizontal) ? new IntRect (0, 0, (int)(length * CurrentRate), (int)width)
                                                                          : new IntRect (0, 0, (int)width, (int)(length * CurrentRate));
            spr2.Origin = new Vector2f (-offset.X, -offset.Y);
            spr2.Position = new Vector2f (T.X, T.Y);
            spr2.Scale = new Vector2f (S.X, S.Y);
            spr2.Rotation = angle;

            spr2.Color = new Color (fgColor.R, fgColor.G, fgColor.B, (byte)(fgColor.A * opacity)).ToSFML ();

            var win = window as RenderWindow;
            win.Draw (spr1);
            win.Draw (spr2);
        }

        /// <inheritdoc/>
        public override string ToString () {
            return string.Format("Value : {0}/{1}", currentValue, maxValue);
        }
        #endregion


    }
}
