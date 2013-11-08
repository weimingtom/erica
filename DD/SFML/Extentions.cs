using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace DD {
    /// <summary>
    /// DDをSFMLに対応させるための拡張メソッド
    /// </summary>
    /// <remarks>
    /// DDに enum を中心にいろいろ便利関数が追加されます。
    /// 拡張メソッドの名前は ToSFML() が基本です。
    /// </remarks>
    public static class Extentions {
        /// <summary>
        /// 文字スタイルをSFML形式に変更
        /// </summary>
        /// <remarks>
        /// 影付き（Shadow）スタイルはSFMLのスタイルに含まれずDDが直接処理するので変換されません。
        /// </remarks>
        /// <param name="style">DDの文字スタイル</param>
        /// <returns>SFMLの文字スタイル</returns>
        public static SFML.Graphics.Text.Styles ToSFML (this DD.CharacterStyle style) {
            var st = SFML.Graphics.Text.Styles.Regular;
            if (style.HasFlag (DD.CharacterStyle.Bold)) {
                st |= SFML.Graphics.Text.Styles.Bold;
            }
            if (style.HasFlag (DD.CharacterStyle.Italic)) {
                st |= SFML.Graphics.Text.Styles.Italic;
            }
            if (style.HasFlag (DD.CharacterStyle.Underlined)) {
                st |= SFML.Graphics.Text.Styles.Underlined;
            }
            return st;
        }

        /// <summary>
        /// カラーをSFML形式に変更
        /// </summary>
        /// <param name="c">DDのカラー</param>
        /// <returns>SFMLのカラー</returns>
        public static SFML.Graphics.Color ToSFML (this DD.Color c) {
            return new SFML.Graphics.Color (c.R, c.G, c.B, c.A);
        }

        /// <summary>
        /// SMFLのキーコードをDDのキーコードに変更
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DD.KeyCode ToDD (this SFML.Window.Keyboard.Key key) {
            switch (key) {
                case Keyboard.Key.A: return KeyCode.A;
                case Keyboard.Key.Add: return KeyCode.Plus;
                case Keyboard.Key.B: return KeyCode.B;
                case Keyboard.Key.Back: return KeyCode.Backspace;
                case Keyboard.Key.BackSlash: return KeyCode.Backslash;
                case Keyboard.Key.C: return KeyCode.C;
                case Keyboard.Key.Comma: return KeyCode.Comma;
                case Keyboard.Key.D: return KeyCode.D;
                case Keyboard.Key.Dash: return KeyCode.Minus;
                case Keyboard.Key.Delete: return KeyCode.Delete;
                case Keyboard.Key.Divide: return KeyCode.Slash;
                case Keyboard.Key.Down: return KeyCode.DownArrow;
                case Keyboard.Key.E: return KeyCode.E;
                case Keyboard.Key.End: return KeyCode.End;
                case Keyboard.Key.Equal: return KeyCode.Equals;
                case Keyboard.Key.Escape: return KeyCode.Escape;
                case Keyboard.Key.F: return KeyCode.F;
                case Keyboard.Key.F1: return KeyCode.F1;
                case Keyboard.Key.F10: return KeyCode.F10;
                case Keyboard.Key.F11: return KeyCode.F11;
                case Keyboard.Key.F12: return KeyCode.F12;
                case Keyboard.Key.F13: return KeyCode.F13;
                case Keyboard.Key.F14: return KeyCode.F14;
                case Keyboard.Key.F15: return KeyCode.F15;
                case Keyboard.Key.F2: return KeyCode.F2;
                case Keyboard.Key.F3: return KeyCode.F3;
                case Keyboard.Key.F4: return KeyCode.F4;
                case Keyboard.Key.F5: return KeyCode.F5;
                case Keyboard.Key.F6: return KeyCode.F6;
                case Keyboard.Key.F7: return KeyCode.F7;
                case Keyboard.Key.F8: return KeyCode.F8;
                case Keyboard.Key.F9: return KeyCode.F9;
                case Keyboard.Key.G: return KeyCode.G;
                case Keyboard.Key.H: return KeyCode.H;
                case Keyboard.Key.Home: return KeyCode.Home;
                case Keyboard.Key.I: return KeyCode.I;
                case Keyboard.Key.Insert: return KeyCode.Insert;
                case Keyboard.Key.J: return KeyCode.J;
                case Keyboard.Key.K: return KeyCode.K;
                case Keyboard.Key.L: return KeyCode.L;
                case Keyboard.Key.LAlt: return KeyCode.LeftAlt;
                case Keyboard.Key.LBracket: return KeyCode.LeftBracket;
                case Keyboard.Key.LControl: return KeyCode.LeftControl;
                case Keyboard.Key.Left: return KeyCode.LeftArrow;
                case Keyboard.Key.LShift: return KeyCode.LeftShift;
                case Keyboard.Key.LSystem: return KeyCode.SysReq;
                case Keyboard.Key.M: return KeyCode.M;
                case Keyboard.Key.Menu: return KeyCode.Menu;
                case Keyboard.Key.Multiply: return KeyCode.Asterisk;
                case Keyboard.Key.N: return KeyCode.N;
                case Keyboard.Key.Num0: return KeyCode.Alpha0;
                case Keyboard.Key.Num1: return KeyCode.Alpha1;
                case Keyboard.Key.Num2: return KeyCode.Alpha2;
                case Keyboard.Key.Num3: return KeyCode.Alpha3;
                case Keyboard.Key.Num4: return KeyCode.Alpha4;
                case Keyboard.Key.Num5: return KeyCode.Alpha5;
                case Keyboard.Key.Num6: return KeyCode.Alpha6;
                case Keyboard.Key.Num7: return KeyCode.Alpha7;
                case Keyboard.Key.Num8: return KeyCode.Alpha8;
                case Keyboard.Key.Num9: return KeyCode.Alpha9;
                case Keyboard.Key.Numpad0: return KeyCode.Keypad0;
                case Keyboard.Key.Numpad1: return KeyCode.Keypad1;
                case Keyboard.Key.Numpad2: return KeyCode.Keypad2;
                case Keyboard.Key.Numpad3: return KeyCode.Keypad3;
                case Keyboard.Key.Numpad4: return KeyCode.Keypad4;
                case Keyboard.Key.Numpad5: return KeyCode.Keypad5;
                case Keyboard.Key.Numpad6: return KeyCode.Keypad6;
                case Keyboard.Key.Numpad7: return KeyCode.Keypad7;
                case Keyboard.Key.Numpad8: return KeyCode.Keypad8;
                case Keyboard.Key.Numpad9: return KeyCode.Keypad9;
                case Keyboard.Key.O: return KeyCode.O;
                case Keyboard.Key.P: return KeyCode.P;
                case Keyboard.Key.PageDown: return KeyCode.PageDown;
                case Keyboard.Key.PageUp: return KeyCode.PageUp;
                case Keyboard.Key.Pause: return KeyCode.Pause;
                case Keyboard.Key.Period: return KeyCode.Period;
                case Keyboard.Key.Q: return KeyCode.Q;
                case Keyboard.Key.Quote: return KeyCode.Quote;
                case Keyboard.Key.R: return KeyCode.R;
                case Keyboard.Key.RAlt: return KeyCode.RightAlt;
                case Keyboard.Key.RBracket: return KeyCode.RightBracket;
                case Keyboard.Key.RControl: return KeyCode.RightControl;
                case Keyboard.Key.Return: return KeyCode.Return;
                case Keyboard.Key.Right: return KeyCode.RightArrow;
                case Keyboard.Key.RShift: return KeyCode.RightShift;
                case Keyboard.Key.RSystem: return KeyCode.SysReq;
                case Keyboard.Key.S: return KeyCode.S;
                case Keyboard.Key.SemiColon: return KeyCode.Semicolon;
                case Keyboard.Key.Slash: return KeyCode.Slash;
                case Keyboard.Key.Space: return KeyCode.Space;
                case Keyboard.Key.Subtract: return KeyCode.Minus;
                case Keyboard.Key.T: return KeyCode.T;
                case Keyboard.Key.Tab: return KeyCode.Tab;
                case Keyboard.Key.Tilde: return KeyCode.Caret;
                case Keyboard.Key.U: return KeyCode.U;
                case Keyboard.Key.Unknown: return KeyCode.Unknown;
                case Keyboard.Key.Up: return KeyCode.UpArrow;
                case Keyboard.Key.V: return KeyCode.V;
                case Keyboard.Key.W: return KeyCode.W;
                case Keyboard.Key.X: return KeyCode.X;
                case Keyboard.Key.Y: return KeyCode.Y;
                case Keyboard.Key.Z: return KeyCode.Z;
                default: throw new NotImplementedException ("Sorry");

            }
        }

        /// <summary>
        /// SFMLのマウスボタンをDDのマウス ボタンに変換
        /// </summary>
        /// <remarks>
        /// キーコードに変換するには ToDD_KeyCode() を使用する。
        /// </remarks>
        /// <param name="button">ボタン</param>
        /// <returns>キー コード</returns>
        public static DD.MouseButton ToDD (this SFML.Window.Mouse.Button button) {
            switch (button) {
                case Mouse.Button.Left: return MouseButton.Left;
                case Mouse.Button.Right: return MouseButton.Right;
                case Mouse.Button.Middle: return MouseButton.Middle;
                case Mouse.Button.XButton1: return MouseButton.XButton1;
                case Mouse.Button.XButton2: return MouseButton.XButton2;
                default: throw new NotImplementedException ("Sorry");
            }
        }

        /// <summary>
        /// SFMLのマウスボタンをDDのキー コードに変換
        /// </summary>
        /// <param name="button">ボタン</param>
        /// <returns>キー コード</returns>
        public static DD.KeyCode ToDD_KeyCode (this SFML.Window.Mouse.Button button) {
            switch (button) {
                case Mouse.Button.Left: return KeyCode.Mouse0;
                case Mouse.Button.Right: return KeyCode.Mouse1;
                case Mouse.Button.Middle: return KeyCode.Mouse2;
                case Mouse.Button.XButton1: return KeyCode.Mouse3;
                case Mouse.Button.XButton2: return KeyCode.Mouse4;
                default: throw new NotImplementedException ("Sorry");
            }
        }


        /// <summary>
        /// SFMLの<see cref="SFML.Window.Vector2f"/> 型をDDの <see cref="DD.Vector2"/> 型に変換
        /// </summary>
        /// <param name="v">ベクトル</param>
        public static DD.Vector2 ToDD (this SFML.Window.Vector2f v) {
            return new Vector2 (v.X, v.Y);
        }


        /// <summary>
        /// DDの <see cref="DD.Vector2"/> 型をSFMLの<see cref="SFML.Window.Vector2f"/> 型に変換
        /// </summary>
        /// <param name="v">ベクトル</param>
        public static SFML.Window.Vector2f ToSFML (this  DD.Vector2 v) {
            return new SFML.Window.Vector2f (v.X, v.Y);
        }
    }
}
