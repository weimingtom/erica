using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.MessageSample {
    public class MyRecver : Component {
        public MyRecver () {
        }

        public static Node Create (string name, Vector3 pos) {
            var cmp = new MyRecver ();

            var spr = new Sprite (64, 64);
            spr.AddTexture (Resource.GetDefaultTexture ());
            spr.Color = Color.Cyan;

            var label1 = new Label ();
            label1.Text = name;
            label1.SetOffset (4, 16);

            var label2 = new Label ();
            label2.Text = "None";
            label2.SetOffset (0, -24);

            var node = new Node (name);
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (label1);
            node.Attach (label2);

            node.AddMailBox (name);

            node.Translation = pos;

            return node;
        }

        public override void OnMailBox (Node from, string to, object letter) {
            var msg = letter as MyLetter;

            GetComponent<Label> (1).Text = (string)letter;
        }

        public override void OnUpdate (long msec) {
        }
    }
}
