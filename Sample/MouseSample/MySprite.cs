using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.MouseSample {
    public class MySprite : Component {

        public MySprite () {
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MySprite ();

            var spr = new Sprite (64, 64);
            spr.AddTexture (new Texture ("media/Box-64x64.png"));

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width / 2, spr.Height / 2, 100);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 0);

            var mbox1 = new MailBox ("MouseSelect");
            var mbxo2 = new MailBox ("MouseDeselect");

            var node = new Node ("MySprite");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (mbox1);
            node.Attach (mbxo2);

            node.Translation = pos;

            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            SendMessage ("Logger", string.Format ("Pressed : {0}, {1}, {2}", button, x, y));
        }

        public override void OnMouseButtonReleased (MouseButton button, float x, float y) {
            SendMessage ("Logger", string.Format ("Released : {0}, {1}, {2}", button, x, y));
        }

        public override void OnMouseFocusIn (float x, float y) {
            SendMessage ("Logger", string.Format ("Forcus in"));
        }

        public override void OnMouseFocusOut (float x, float y) {
            SendMessage ("Logger", string.Format ("Forcus out"));
        }

        public override void OnMailBox (Node from, string address, object letter) {
            var spr = GetComponent<Sprite> ();
            switch (address) {
                case "MouseSelect": {
                        var colA = GetComponent<CollisionObject> ();
                        var colB = (CollisionObject)letter;
                        if (CollisionAnalyzer.Overlapped (colA, colB)) {
                            spr.Color = Color.Red;
                            SendMessage ("Logger", string.Format ("Mouse Selected"));
                        }
                        break;
                    }
                case "MouseDeselect": {
                        spr.Color = Color.White;
                        SendMessage ("Logger", string.Format ("Mouse DeSelected"));
                        break;
                    }
            }
        }
    }
}
