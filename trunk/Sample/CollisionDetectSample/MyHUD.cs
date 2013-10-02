using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.CollisionDetectSample {
    public class MyHUD : Component {

        public override void OnMailBox (Node from, string address, object letter) {
            GetComponent<Label> ().Text = letter as string;
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyHUD ();

            var label = new Label ();
            label.Text = "No message";

            var mbox = new MailBox ("HUD");
            
            var node = new Node ("HUD");
            node.Attach (cmp);
            node.Attach (label);
            node.Attach (mbox);

            node.Translation = pos;

            return node;
        }
    }
}
