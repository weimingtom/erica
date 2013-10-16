using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    public struct DeliveryRecord {
        public DeliveryRecord (long time, Mail mail) : this(){
            this.Time = time;
            this.From = mail.From.ToString ();
            this.Address = mail.Address;
            this.LetterType = (mail.Letter == null) ? "Null" : mail.Letter.GetType().Name;
            this.Letter = (mail.Letter == null) ? "Nothing" : mail.Letter.ToString ();
        }
        public long Time { get; private set; }
        public string From { get; private set; }
        public string Address { get; private set; }
        public string LetterType { get; private set; }
        public string Letter { get; private set; }

        public override string ToString () {
            return string.Format ("{0} : From={1} --> Address=\"{2}\" : LetterType={3}, Letter={4}"
                , Time, From, Address, LetterType, Letter);
        }
    }
}
