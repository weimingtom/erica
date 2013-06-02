﻿using System;
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
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="text"></param>
        public Label (string text) {
            this.text = text ?? "";
            this.charSize = 16;
            this.shadowOffset = 2;
            this.color = Color.White;
            this.style = CharacterStyle.Regular;
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
        public override void OnDraw (object window) {
            var win = (RenderWindow)window;
            var font = Resource.GetDefaultFont ();

            if (style.HasFlag (DD.CharacterStyle.Shadow)) {
                var txt2 = new Text (text, font, (uint)charSize);
                txt2.Position = new Vector2f (Node.WindowX + shadowOffset, Node.WindowY + shadowOffset);
                txt2.Color = Color.Black.ToSFML ();
                txt2.Style = style.ToSFML ();
                win.Draw (txt2);
            }

            var txt = new Text (text, font, (uint)charSize);
            txt.Position = new Vector2f (Node.WindowX, Node.WindowY);
            txt.Color = color.ToSFML ();
            txt.Style = style.ToSFML ();
            win.Draw (txt);
        }

        /// <inheritdoc/>
        public override string ToString () {
            return text;
        }
        #endregion
    }
}