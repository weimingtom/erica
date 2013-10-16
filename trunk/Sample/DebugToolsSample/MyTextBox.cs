using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DD.Sample.DebugToolsSample {
    public class MyTextBox : Component {

        public static Node Create (Vector3 pos) {
            var cmp = new MyTextBox ();

            var label = new Label ();
            label.Text = "No message.";
            label.Color = Color.Black;

            var mbox = new MailBox ("CharacterChanged");

            var node = new Node ("TextBox");
            node.Attach (cmp);
            node.Attach (label);
            node.Attach (mbox);

            //node.AddChild (MyFaceWindow.Create (new Vector3 (50, 445, 0)));

            node.Translation = pos;

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo ("ja-JP");

            return node;

        }

        public override void OnMailBox (Node from, string address, object letter) {

            switch (address) {
                case "CharacterChanged": {
                        var ch = ((Node)letter).GetComponent<MyCharacter> ();

                        GetComponent<Label> ().Text = ch.Greeting;
                        Log (0, "Change TextBox to " + ch.Name);
                        break;
                    }
            }
        }
    }
}
