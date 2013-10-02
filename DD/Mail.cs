using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// メール構造体
    /// </summary>
    /// <remarks>
    /// メール（またはメッセージ）はゲームオブジェクト同士の通信に使用される基本通信単位です。
    /// 送信元、送信先、通信メッセージの3つからなります。
    /// メールの宛先はノードではなくメールボックスの名前（文字列）である事に注意して下さい。
    /// これにより柔軟にメールの送信先が制御可能になっています。
    /// </remarks>
    public struct Mail {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="from">送信元ノード</param>
        /// <param name="address">送信先アドレス（文字列）</param>
        /// <param name="letter">通信内容</param>
        public Mail (Node from, string address, object letter)
            : this () {
            this.Address = address;
            this.From = from;
            this.Letter = letter;
        }

        /// <summary>
        /// 送信ノード
        /// </summary>
        public Node From { get; private set; }

        /// <summary>
        /// 送信先アドレス
        /// </summary>
        /// <remarks>
        /// 送信先アドレスはメールボックスに付けられた宛先 <see cref="MailBox.Address"/> （文字列）です。
        /// ユーザーはメールボックスを何個でも任意のアドレスで作成可能です。
        /// またメールはシーン全体にブロードキャストするので同名のアドレスを持つノードが複数存在する場合、すべてのノードに送信されます。
        /// </remarks>
        public string Address { get; private set; }

        /// <summary>
        /// 通信メッセージ
        /// </summary>
        /// <remarks>
        /// 通信内容を表す任意のオブジェクトです。
        /// </remarks>
        public object Letter { get; private set; }

        /// <inheritdoc/>
        public override string ToString () {
            return string.Format("{0} --> \"{1}\"", From.ToString(), Address);
        }
    }

}
