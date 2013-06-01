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
        /// <param name="color">DDのカラー</param>
        /// <returns>SFMLのカラー</returns>
        public static SFML.Graphics.Color ToSFML (this DD.Color color) {
            return new SFML.Graphics.Color (color.R, color.G, color.B, color.A);
        }
    }
}
