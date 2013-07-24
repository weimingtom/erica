using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace DD {

    /// <summary>
    /// ラベル コンポーネント
    /// </summary>
    /// <remarks>
    /// <see cref="Label"/> はテキストを1行表示するコンポーネントです。
    /// 
    /// </remarks>
    public class Label : Component {
        #region Field
        string text;
        int charSize;
        int shadowOffset;
        Color color;
        CharacterStyle style;
        Vector2 offset;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="text">表示文字列</param>
        public Label (string text) {
            this.text = text ?? "";
            this.charSize = 16;
            this.shadowOffset = 2;
            this.color = Color.White;
            this.style = CharacterStyle.Regular;
            this.offset = new Vector2 (0,0);
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public Label ()
            : this ("") {
        }
        #endregion

        #region Property

        /// <summary>
        /// 影のずらし具合
        /// </summary>
        public int ShadowOffset {
            get { return shadowOffset; }
            set { SetShadowOffset (value); }
        }

        /// <summary>
        /// 1文字のサイズ
        /// </summary>
        public int CharacterSize {
            get { return charSize; }
            set { SetCharacterSize (value); }
        }

        /// <summary>
        /// 文字のスタイル
        /// </summary>
        /// <remarks>
        /// 太字、斜め文字、下線付き、影付きを変更可能です。
        /// </remarks>
        public CharacterStyle Style {
            get { return style; }
            set { SetStyle (value); }
        }

        /// <summary>
        /// 表示されるテキスト
        /// </summary>
        public string Text {
            get { return text; }
            set { SetText (value); }
        }

        /// <summary>
        /// テキストの色
        /// </summary>
        public Color Color {
            get { return color; }
            set { SetColor (value.R, value.G, value.B, value.A); }
        }

        /// <summary>
        /// オフセット（ピクセル座標）
        /// </summary>
        public Vector2 Offset {
            get { return offset; }
            set { SetOffset (value.X, value.Y); }
        }
        #endregion

        #region Method
        /// <summary>
        /// テキストの変更
        /// </summary>
        /// <param name="text">文字列</param>
        public void SetText (string text) {
            this.text = text ?? "";
        }

        /// <summary>
        /// 文字サイズの変更
        /// </summary>
        /// <param name="size">文字サイズ</param>
        public void SetCharacterSize (int size) {
            if (size < 0 || size > 256) {
                throw new ArgumentException ("Size is invalid");
            }
            this.charSize = size;
        }

        /// <summary>
        /// 文字スタイルの変更
        /// </summary>
        /// <remarks>
        /// 文字スタイルを太字、斜め文字、下線付き、影付きの組み合わせの中から選択します。
        /// 複数同時に指定可能です。
        /// </remarks>
        /// <param name="style">文字スタイル</param>
        public void SetStyle (CharacterStyle style) {
            this.style = style;
        }

        /// <summary>
        /// 影付けの変更
        /// </summary>
        /// <param name="shadowOffset">影のオフセット</param>
        public void SetShadowOffset (int shadowOffset) {
            if (shadowOffset < -256 || shadowOffset > 256) {
                throw new ArgumentException ("Shadow offset is invalid");
            }
            this.shadowOffset = shadowOffset;
        }

        /// <summary>
        /// 表示色の変更
        /// </summary>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">不透明度</param>
        public void SetColor (byte r, byte g, byte b, byte a) {
            this.color = new Color (r, g, b, a);
        }

        /// <inheritdoc/>
        public override void OnDraw (object window, EventArgs args) {
            var win = (RenderWindow)window;
            var font = Resource.GetDefaultFont ();

            if (style.HasFlag (DD.CharacterStyle.Shadow)) {
                var txt2 = new Text (text, font, (uint)charSize);
                txt2.Position = new Vector2f (Node.Point.X + offset.X + shadowOffset,
                                              Node.Point.Y + offset.Y + shadowOffset);
                txt2.Color = Color.Black.ToSFML ();
                txt2.Style = style.ToSFML ();
                win.Draw (txt2);
            }

            Vector3 T;
            Quaternion R;
            Vector3 S;
            Node.GlobalTransform.Decompress (out T, out R, out S);

            // クォータニオンの性質上(0,0,1)軸まわりの回転か、
            // (0,0,-1)軸まわりの回転のどちらかが返ってくる（両者は等価）。
            // SFMLは(0,0,1)軸まわりの回転角で指定するので(0,0,-1)軸の時は反転が必要。
            var angle = R.Angle;
            if (R.Axis.Z < 0) {
                angle *= -1;
            }
 
            var txt = new Text (text, font, (uint)charSize);
            txt.Position = new Vector2f (T.X + offset.X, T.Y + offset.Y);
            txt.Scale = new Vector2f (S.X, S.Y);
            txt.Rotation = angle;
            txt.Color = color.ToSFML ();
            txt.Style = style.ToSFML ();
            
            win.Draw (txt);
        }

        /// <summary>
        /// オフセットの変更
        /// </summary>
        /// <param name="px">X方向のオフセット（ピクセル座標）</param>
        /// <param name="py">Y方向のオフセット（ピクセル座標）</param>
        public void SetOffset (float px, float py) {
            this.offset = new Vector2 (px, py);
        }

        /// <inheritdoc/>
        public override string ToString () {
            return text;
        }
        #endregion
    }
}
