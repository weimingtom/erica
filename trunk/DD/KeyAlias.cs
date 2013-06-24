using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// キー エイリアス構造体
    /// </summary>
    /// <remarks>
    /// キー コード <see cref="KeyCode"/> に別名（エイリアス）を付けます。
    /// <seealso cref="InputReceiver"/>
    /// </remarks>
    public struct KeyAlias {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="name">別名</param>
        /// <param name="key">キー コード</param>
        internal KeyAlias (string name, KeyCode key)
            : this () {
            this.Name = name;
            this.KeyCode = key;
        }

        /// <summary>
        /// 別名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// キー コード
        /// </summary>
        public KeyCode KeyCode { get; private set; }
    }

}
