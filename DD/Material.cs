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
        /// すべてのパブリック プロパティを元にハッシュ値を計算します（プライベートおよびフィールド値は考慮しません）。
        /// ハッシュ値はこのオブジェクトが変更されたかどうかを判定するのに使用します。
        /// <see cref="object.GetHashCode"/> とは全く異なる概念です。
        /// リフレクションを使用する比較的遅いメソッドなので、
        /// 派生クラスはこのメソッドをオーバーライドして独自の実装を提供して下さい。
        /// </remarks>
        /// <returns>ハッシュ値</returns>
        public virtual int GetHashValue () {
            int hash = 0;
            var type = this.GetType();
            
            foreach (var prop in type.GetProperties()) {
                var value = prop.GetValue (this, null);
                hash = hash ^ value.GetHashCode ();
            }
            return hash;
        }

    }
}
