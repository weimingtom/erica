using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// ログ記録 コンポーネント
    /// </summary>
    /// <remarks>
    /// DDのログ機能を使ってログを記録します。
    /// ログはデバッグツールのログビュー を使って参照することが可能です。
    /// </remarks>
    public class Logger : Component {

        #region Const
        const int MaxLogLine = 100;
        #endregion

        #region Field
        Queue<Log> logs;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Logger () {
            this.logs = new Queue<Log> ();
        }
        #endregion

        #region Property
        /// <summary>
        /// 記録されたログの行数
        /// </summary>
        /// <remarks>
        /// </remarks>
        public int LogCount {
            get { return logs.Count (); }
        }

        /// <summary>
        /// 記録されたログを列挙する列挙子
        /// </summary>
        public IEnumerable<Log> Logs {
            get { return logs; }
        }
        #endregion

        /// <summary>
        /// ログの記録
        /// </summary>
        /// <remarks>
        /// ログの優先度 <paramref name="priority"/> はマイナスの値ほど優先度が高く、
        /// プラスの値は優先度が低いです。0が標準です。
        /// 現在の所優先度には特に意味はありません。
        /// 現在の仕様だとログの行数に制限はなく全て保管します。
        /// </remarks>
        /// <param name="node">ログを記録したノード</param>
        /// <param name="priority">優先度</param>
        /// <param name="message">ログメッセージ</param>
        public void Write (Node node, int priority, string message) {
            var name = (node != null) ? node.Name : "Null";
            this.logs.Enqueue (new Log (name, priority, message));
            if (logs.Count () > MaxLogLine) {
                logs.Dequeue ();
            }
        }

        /// <summary>
        /// ログの全削除
        /// </summary>
        public void Clear () {
            this.logs.Clear ();
        }
    }
}
