using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// キーフレーム構造体
    /// </summary>
    /// <remarks>
    /// アニメーションで使用されるキーフレーム データをあらわす構造体です。
    /// 値は動的型(dynamic)として宣言され、どのような型でも使用可能です。
    /// このエンジン中では標準のプリミティブ型（Int32, Single, etc.）か、
    /// Length プロパティとインデクサー（'[]'）を実装した構造体でのみ使用されます。
    /// </remarks>
    public struct Keyframe {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="time">ローカル ポジション</param>
        /// <param name="value">値</param>
        internal Keyframe (int time, dynamic value) : this () {
            this.Time = time;
            this.Value = value;
        }

        /// <summary>
        /// 時刻
        /// </summary>
        public int Time { get; private set; }

        /// <summary>
        /// 値
        /// </summary>
        public dynamic Value { get; private set; }
    }
}
