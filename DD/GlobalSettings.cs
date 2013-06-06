using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// エンジン全体設定
    /// </summary>
    /// <remarks>
    /// エンジン全体に関する設定値を保存しています。
    /// 通常は一切変更する必要はありません。
    /// </remarks>
    public static class GlobalSettings {
        /// <summary>
        /// 浮動小数の等値性の比較に使用される許容誤差
        /// </summary>
        /// <remarks>
        /// 2つの浮動小数の差がこの値以下の場合「等しい」と見なされます。
        /// 許容誤差は Equals() を使った比較でのみ使用され、==, !=演算子と GetHashCode() では使用されません。
        /// 従ってキーとして使う場合は厳密に値を判定します。
        /// </remarks>
        public static float Torrelance;

        /// <summary>
        /// スタティック初期化
        /// </summary>
        static GlobalSettings () {
            Torrelance = 0.0001f;
        }
    }
}
