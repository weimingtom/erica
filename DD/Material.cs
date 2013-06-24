using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// マテリアル クラス
    /// </summary>
    /// <remarks>
    /// すべてのマテリアルの基底になります。
    /// 中身は実は空です。
    /// </remarks>
    public abstract class Material {

        /// <summary>
        /// ハッシュ値の取得
        /// </summary>
        /// <remarks>
        /// すべてのフィールドを元にハッシュ値を計算します。
        /// ハッシュ値はこのオブジェクトが変更されたかどうかを判定するのに使用します。
        /// <see cref="object.GetHashCode"/> とは全く異なる概念です。
        /// リフレクションを使用する比較的遅いメソッドなので、
        /// 派生クラスはこのメソッドをオーバーライドして独自の実装を提供する事が望ましい。
        /// </remarks>
        /// <returns>ハッシュ値</returns>
        public int GetHashValue () {
            int hash = 0;
            var type = this.GetType();
            foreach (var field in type.GetFields()) {
                var value = field.GetValue (this);
                hash = hash ^ value.GetHashCode ();
            }
            return hash;
        }
    }
}
