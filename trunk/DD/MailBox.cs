using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// メールボックス アクション
    /// </summary>
    /// <param name="from">メッセージの送信元（ノード）</param>
    /// <param name="to">メッセージの宛先（文字列）</param>
    /// <param name="letter">通信メッセージ</param>
    public delegate void MailBoxAction (Node from, string address, object letter);

    /// <summary>
    /// メール ボックス コンポーネント
    /// </summary>
    /// <remarks>
    /// ゲーム オブジェクトはメッセージ通信によってデータをやりとりします。
    /// メール ボックスはその通信を受け取るコンポーネントです。
    /// メールを受信すると OnMailBox が呼ばれるとともに、登録済みのメール アクション <see cref="Action"/> が実行されます。
    /// 標準では <see cref="Component.OnMailBox"/> ですが、ユーザーは自分で任意のメール ボックス アクションを登録可能です。
    /// </remarks>
    public class MailBox : Component {

        #region Field
        string address;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// メールボックスを作成し指定のアドレスのメッセージ通信を受信します。
        /// </remarks>
        /// <param name="address">アドレス（文字列）</param>
        public MailBox (string address) {
            if (address == null) {
                throw new ArgumentNullException ("Address is null");
            }
            this.address = address;
        }
        #endregion

        #region Event
        /// <summary>
        /// メール アクション
        /// </summary>
        /// <remarks>
        /// メールを受信した時に実行されるアクションです。
        /// 標準では（すべてのアドレス共通で） <see cref="Component.OnMailBox"/> が呼び出されますが、
        /// ユーザーはそれに加えてメールボックス毎に追加のアクションを登録可能です。
        /// </remarks>
        public event MailBoxAction Action;
        #endregion

        #region Property
        /// <summary>
        /// アドレス
        /// </summary>
        /// <remarks>
        /// メッセージ通信における宛先です。
        /// </remarks>
        public string Address {
            get { return address; }
        }
        #endregion


        #region Method

        /// <inheritdoc/>
        public override void OnMailBox (Node from, string address, object letter) {
            if (Action != null) {
                Action (from, address, letter);
            }
        }

        /// <inheritdoc>
        public override string ToString () {
            return "MailBox: \"" + Address + "\"";
        }
        #endregion

    }
}
