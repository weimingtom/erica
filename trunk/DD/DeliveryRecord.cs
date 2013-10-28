using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// 配達記録構造体
    /// </summary>
    /// <remarks>
    /// メッセージ送信 <see cref="PostOffice"/> で配達されたメッセージの記録を保持します。
    /// この構造体のインスタンスをユーザーが作成することはありません。
    /// </remarks>
    public struct DeliveryRecord {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="time">配達時刻 (msec)</param>
        /// <param name="mail">メール（のコピー）</param>
        public DeliveryRecord (long time, Mail mail) : this(){
            this.Time = time;
            this.From = mail.From.ToString ();
            this.Address = mail.Address;
            this.LetterType = (mail.Letter == null) ? "Null" : mail.Letter.GetType().Name;
            this.Letter = (mail.Letter == null) ? "Nothing" : mail.Letter.ToString ();
        }
        /// <summary>
        /// 配達時刻 (msec)
        /// </summary>
        public long Time { get; private set; }

        /// <summary>
        /// メッセージの送信元ノード
        /// </summary>
        public string From { get; private set; }

        /// <summary>
        /// メッセージの宛先
        /// </summary>
        public string Address { get; private set; }

        /// <summary>
        /// メッセージ本体の型情報
        /// </summary>
        public string LetterType { get; private set; }

        /// <summary>
        /// メッセージ本体
        /// </summary>
        /// <remarks>
        /// メッセージを ToString() で文字列にした物を保管します。
        /// メッセージ本体そのものではありません。
        /// </remarks>
        public string Letter { get; private set; }

        /// <inheritdoc/>
        public override string ToString () {
            return string.Format ("{0} : From={1} --> Address=\"{2}\" : LetterType={3}, Letter={4}"
                , Time, From, Address, LetterType, Letter);
        }
    }
}
