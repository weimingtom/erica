using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    public delegate void MailBoxAction (Node from, string to, object letter);

    /// <summary>
    /// メール ボックス構造体
    /// </summary>
    /// <remarks>
    /// ゲーム オブジェクトはメッセージ通信によってデータをやりとりします。
    /// メール ボックスはその通信の受け取りポイントです。
    /// メールを受信すると登録済みのメール ボックス アクション <see cref="MailBoxAction"/> が実行されます。
    /// 標準では <see cref="Component.OnMailBox"/> ですが、ユーザーは自分で任意のメール ボックス アクションを登録可能です。
    /// </remarks>
    public struct MailBox {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="address">アドレス（文字列）</param>
        /// <param name="action">メールを受信した時のアクション</param>
        public MailBox (string address, MailBoxAction action)
            : this () {
            this.NamePlate = address;
            this.Action = action;
        }

        /// <summary>
        /// アドレス
        /// </summary>
        /// <remarks>
        /// メッセージ通信における宛先です。
        /// </remarks>
        public string NamePlate { get; private set; }

        /// <summary>
        /// メール アクション
        /// </summary>
        /// <remarks>
        /// メールを受信した時に実行されるアクションです。
        /// 標準では <see cref="Component.OnMailBox"/> が呼び出されますが、ユーザーは標準に代えて任意のアクションを登録可能です。
        /// </remarks>
        public MailBoxAction Action { get; private set; }

        /// <inheritdoc>
        public override string ToString () {
            return NamePlate;
        }

    }
}
