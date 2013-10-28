using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// ログ構造体
    /// </summary>
    /// <remarks>
    /// ログ1行に付きこの構造体が1つ
    /// </remarks>
    public struct Log {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="node">記録元ノード</param>
        /// <param name="priority">優先度</param>
        /// <param name="message">メッセージ</param>
        public Log (string node, int priority, string message)
            : this () {
                this.Node = node;
            this.Priority = priority;
            this.Message = message;
        }
        /// <summary>
        /// ログの記録元
        /// </summary>
        /// <remarks>
        /// ログの記録元を文字列で保管します。
        /// ノードそのものではありません。
        /// </remarks>
        public string Node { get; private set; }

        /// <summary>
        /// 優先度
        /// </summary>
        public int Priority { get; private set; }

        /// <summary>
        /// ログメッセージ
        /// </summary>
        public string Message { get; private set; }
    }

}
