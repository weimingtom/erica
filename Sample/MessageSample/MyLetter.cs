using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.MessageSample {
    public class MyLetter : EventArgs {
        public MyLetter (string text) {
            this.Text = text;
        }
        public string Text { get; private set; }
    }
}
