using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.MouseSample {
    public class MyLogger : Component {
        const int lineCount = 10;
        
        public MyLogger () {
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyLogger ();

            var mbox = new MailBox ("Logger");

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (mbox);

            for (var i = 0; i < lineCount; i++) {
                var line = new Label ("No Message");
                line.SetOffset (0, 16 * i);
                node.Attach (line);
            }
            
            node.Translation = pos;

            return node;
        }

        public override void OnMailBox (Node from, string address, object letter) {

            for (var i = 0; i < lineCount-1; i++) {
                GetComponent<Label> (i).Text = GetComponent<Label> (i + 1).Text;
            }
            GetComponent<Label> (lineCount - 1).Text = (string)letter;
        }
    }
}
