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

        #region Const
        /// <summary>
        /// 配達記録の最大保持個数
        /// </summary>
        public const int MaxRecordCount = 100;
        #endregion
        
        #region Field
        Queue<DeliveryRecord> records;   // 配達済み
        List<Mail> mails;               // 未配達
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public PostOffice () {
            this.mails = new List<Mail> ();
            this.records = new Queue<DeliveryRecord> ();
        }
        #endregion

        #region Property
        /// <summary>
        /// メールの総数
        /// </summary>
        public int MailCount {
            get { return mails.Count (); }
        }

        /// <summary>
        /// すべてのメールを列挙する列挙子
        /// </summary>
        public IEnumerable<Mail> Mails {
            get { return mails; }
        }

        /// <summary>
        /// 配達記録の総数
        /// </summary>
        /// <remarks>
        /// 配達記録は最大 <see cref="MaxRecordCount"/> 個だけ保存され、それを超える分は古い方から削除されます。
        /// </remarks>
        public int DeliveryRecordCount {
            get { return records.Count (); }
        }

        /// <summary>
        /// すべての配達記録を列挙する列挙子
        /// </summary>
        public IEnumerable<DeliveryRecord> DeliveryRecords {
            get { return records; }
        }
        #endregion

        #region Method
        /// <summary>
        /// メールの送信
        /// </summary>
        /// <param name="from">送信元ノード</param>
        /// <param name="to">送信先アドレス（文字列）</param>
        /// <param name="letter">通信メッセージ</param>
        public void Post (Node from, string to, object letter) {
            if (from == null) {
                throw new ArgumentNullException ("Mail From is null");
            }
            if (to == null) {
                throw new ArgumentNullException ("Mail To is null");
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
        /// (*1) 次の foreach の中で（ユーザーが）メールボックスを削除できるようにこの ToArray() は消さないように
        public void Deliver () {
            
            // 配達ログ
            foreach (var mail in mails) {
                records.Enqueue (new DeliveryRecord (Time.CurrentTime, mail));
                if(records.Count() > MaxRecordCount){
                     records.Dequeue ();
                }
            }

            var deliveries = (from mail in mails
                              from node in World.Downwards
                              where node.Deliverable == true
                              from mailbox in node.MailBoxs
                              where mail.Address == mailbox.Address || mail.Address == "All" || mailbox.Address == "All"
                              select new { mail, node, mailbox }).ToArray ();   // (*1)

            mails.Clear ();

            foreach (var delivery in deliveries) {
                var from = delivery.mail.From;
                var addr = delivery.mail.Address;
                var letter = delivery.mail.Letter;
                foreach (var cmp in delivery.node.Components) {
                    cmp.OnMailBox (from, addr, letter);
                }

                //mails.Remove (delivery.mail);
            }

        }


        public void ClearRecords () {
            this.records.Clear ();
        }

        #endregion

    }
}
