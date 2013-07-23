using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace DD {
    /// <summary>
    /// ボタン コンポーネント
    /// </summary>
    /// <remarks>
    /// クリック可能なボタンを表示するコンポーネントです。
    /// フォーカスが移った時に画像を差し替える事が可能です。
    /// ボタン状態には「通常」「通常（フォーカス）」「押下」「押下（フォーカス）」の4つが存在します。
    /// 通常状態のテクスチャーの指定は必須です。
    /// 指定しない場合は単に描画しません（豆腐の方が良いか？）。
    /// </remarks>
    public class Button : Component {
        #region Field
        int width;
        int height;
        Texture normal;
        Texture normal2;
        Texture pressed;
        Texture pressed2;
        ButtonState state;
        Dictionary<Texture, string> names;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// 指定のサイズの <see cref="Button"/> オブジェクトを作成します。
        /// </remarks>
        /// <param name="width">ボタンの幅</param>
        /// <param name="height">ボタンの高さ</param>
        public Button (int width, int height) {
            if (width <= 0 || height <= 0) {
                throw new ArgumentException ("Width or Height is invalid");
            }
            this.width = width;
            this.height = height;
            this.normal = null;
            this.normal2 = null;
            this.pressed = null;
            this.pressed2 = null;
            this.state = ButtonState.Normal;
            this.names = new Dictionary<Texture, string> ();
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// テクスチャーファイルをロードし、それと同じサイズの<see cref="Button"/> オブジェクトを作成ます。
        /// </remarks>
        /// <param name="texFileName">テクスチャーファイル名</param>
        public Button (string texFileName) : this(1, 1) {
            if (texFileName == null || texFileName == "") {
                throw new ArgumentNullException ("Texture is null");
            }
            LoadTexutre (ButtonState.Normal, texFileName);
            this.width = normal.Width;
            this.height = normal.Height;
        }
        #endregion

        #region Propety

        /// <summary>
        /// ボタン状態
        /// </summary>
        public ButtonState State {
            get { return state; }
            set { this.state = value; }
        }


        /// <summary>
        /// ボタンの幅（ピクセル数）
        /// </summary>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// ボタンの高さ（ピクセル数）
        /// </summary>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// ボタン画像（通常状態）
        /// </summary>
        public string Normal {
            get { return (normal != null) ?names[normal] : null; }
        }

        /// <summary>
        /// ボタン画像（通常状態、フォーカスあり）
        /// </summary>
        public string Focused {
            get { return (normal2 != null) ? names[normal2] : null; }
        }


        /// <summary>
        /// ボタン画像（押された状態）
        /// </summary>
        public string Pressed {
            get { return (pressed != null) ? names[pressed] : null; }
        }

        /// <summary>
        /// ボタン画像（押された状態、フォーカスあり）
        /// </summary>
        public string PressedFocused {
            get { return (pressed2 != null) ? names[pressed2] : null; }
        }

        #endregion

        #region Method
        /// <summary>
        /// テクスチャーのロード
        /// </summary>
        /// <param name="state">ボタン状態</param>
        /// <param name="fileName">テクスチャー ファイル名</param>
        public void LoadTexutre (ButtonState state, string fileName) {
            if (fileName == null || fileName == "") {
                throw new ArgumentNullException ("Name is null");
            }
            var tex = Resource.GetTexture (fileName);
            switch (state) {
                case ButtonState.Normal: this.normal = tex; break;
                case ButtonState.Focused: this.normal2 = tex; break;
                case ButtonState.Pressed: this.pressed = tex; break;
                case ButtonState.PressedFocused: this.pressed2 = tex; break;
                default: throw new NotImplementedException ("Sorry");
            }
            this.names.Add (tex, fileName);
        }

        /// <inheritdoc/>
        public override void OnAttached () {
        }

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            if (normal == null) {
                return;
            }

            var win = window as RenderWindow;
            var spr = new SFML.Graphics.Sprite ();
            switch (state) {
                case ButtonState.Normal: spr.Texture = normal.Data; break;
                case ButtonState.Focused: spr.Texture = (normal2 != null) ? normal2.Data : normal.Data; break;
                case ButtonState.Pressed: spr.Texture = (pressed != null) ? pressed.Data : normal.Data; break;
                case ButtonState.PressedFocused: spr.Texture = (pressed2 != null) ? pressed2.Data :
                                                    (pressed != null) ? pressed.Data : normal.Data; break;
                default: throw new NotImplementedException ("Sorry");
            }
            spr.Position = new Vector2f (Node.Point.X, Node.Point.Y);
            win.Draw (spr);
        }

        /// <inheritdoc/>
        public override void OnMouseButtonPressed (MouseButton button, int x, int y) {
            Console.WriteLine ("Clicked");
            switch (state) {
                case ButtonState.Normal: this.state = ButtonState.Pressed; break;
                case ButtonState.Focused: this.state = ButtonState.PressedFocused; break;
                case ButtonState.Pressed: this.state = ButtonState.Normal; break;
                case ButtonState.PressedFocused: this.state = ButtonState.Focused; break;
                default: throw new NotImplementedException ("Sorry");
            }
        }

        /// <inheritdoc/>
        public override void OnMouseButtonReleased (MouseButton button, int x, int y) {
            Console.WriteLine ("Released");
        }

        /// <inheritdoc/>
        public override void OnMouseFocusIn (MouseButton button, int x, int y) {
            Console.WriteLine ("Entered");
            switch (state) {
                case ButtonState.Normal: this.state = ButtonState.Focused; break;
                case ButtonState.Pressed: this.state = ButtonState.PressedFocused; break;
                default: break;
            }
        }

        /// <inheritdoc/>
        public override void OnMouseFocusOut (MouseButton button, int x, int y) {
            Console.WriteLine ("Left");
            switch (state) {
                case ButtonState.Focused: this.state = ButtonState.Normal; break;
                case ButtonState.PressedFocused: this.state = ButtonState.Pressed; break;
                default: break;
            }
        }
        #endregion

    }
}
