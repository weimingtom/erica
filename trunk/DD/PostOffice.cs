using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {

    /// <summary>
    /// 郵便局コンポーネント
    /// </summary>
    /// <remarks>
    /// ゲーム オブジェクト間のメッセージ通信を扱う郵便局コンポーネントです。
    /// <see cref="World"/> オブジェクトはデフォルトでこのコンポーネントを1つ保有します。
    /// ユーザーがこのコンポーネントを作成することは決してありません。
    /// キューに保存されたメールを配信するには <see cref="World.Deliver"/> メソッドを使用してください。
    /// </remarks>
    public class PostOffice : Component {

        #region Field
        List<Mail> mails;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public PostOffice () {
            this.mails = new List<Mail> ();
        }
        #endregion

        /// <summary>
        /// すべてのメールを列挙する列挙子
        /// </summary>
        public IEnumerable<Mail> Mails {
            get { return mails; }
        }

        /// <summary>
        /// メールの総数
        /// </summary>
        public int MailCount {
            get { return mails.Count (); }
        }

        /// <summary>
        /// メールの送信
        /// </summary>
        /// <param name="from">送信元ノード</param>
        /// <param name="to">送信先アドレス（文字列）</param>
        /// <param name="letter">通信メッセージ</param>
        public void Post (Node from, string to, object letter) {
            if(from == null){
                throw new ArgumentNullException ("Mail From is null");
            }
            if (to == null){
                throw new ArgumentNullException("Mail To is null");
            }
            this.mails.Add (new Mail (from, to, letter));
        }

        /// <summary>
        /// メールの発送
        /// </summary>
        /// <remarks>
        /// キューに保存しているすべての未配達のメールを送信します。
        /// このメソッドが帰るとキューのサイズは0です。
        /// </remarks>
        /// (*1) foreachの中でメールボックスを削除できるようにするためこの ToArray() は消さないように
        public void Deliver () {
            var deliveries = (from mail in mails
                            from node in World.Downwards
                            where node.Deliverable == true
                            from mailbox in node.MailBoxs
                            where mail.Address == mailbox.Address || mail.Address == "All" || mailbox.Address=="All"
                            select new {mail, node, mailbox}).ToArray();   // (*1)

            foreach (var delivery in deliveries) {
                var from = delivery.mail.From;
                var addr = delivery.mail.Address;
                var letter = delivery.mail.Letter;
                foreach (var cmp in delivery.node.Components) {
                    cmp.OnMailBox (from, addr, letter);
                }
            }

            this.mails.Clear ();
        }

    }
}
