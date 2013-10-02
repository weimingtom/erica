using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.GUISample {
    public class MyTarget: Component {
        bool selected;

        public MyTarget () {
            this.selected = false;
        }
        
        public static Node Create (string name, string texture, Vector3 pos) {
            var cmp = new MyTarget ();
       
            var spr = new Sprite (64, 128);
            spr.AddTexture (new Texture (texture));
       
            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width/2, spr.Height/2, 100);
            col.SetOffset (spr.Width/2, spr.Height/2, 0);

            var mbox1 = new MailBox ("MouseSelect");
            var mbox2 = new MailBox ("MouseDeselect");

            var node = new Node (name);
            node.Attach (cmp);
            node.Attach (col);
            node.Attach (spr);
            node.Attach (mbox1);
            node.Attach (mbox2);

            node.Translation = pos;

            return node;
        }

        public override void OnMailBox (Node from, string address, object letter) {
            if (address == "MouseSelect") {
                var colA = GetComponent<CollisionObject> ();
                var colB = (CollisionObject)letter;
                if (CollisionAnalyzer.Overlapped (colA, colB)){
                    this.selected = true;
                }
            }
            if (address == "MouseDeselect") {
                this.selected = false;
            }
        }

        public override void OnPreDraw (object window) {
            var spr = GetComponent<Sprite> ();

            spr.Color = (selected) ? Color.Red : Color.White;
        }

       
    }
}
