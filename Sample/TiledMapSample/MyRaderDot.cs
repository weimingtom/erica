using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyRaderDot : Component {
        Node target;

        public MyRaderDot () {
            this.target = null;
        }

        public static Node Create (Node target, Color color) {
            var cmp = new MyRaderDot ();
            cmp.target = target;

            var spr = new Sprite (2, 2);
            spr.Color = color;

            var mbox = new MailBox ("IamDestroyed");

            var node = new Node ("RaderDot");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (mbox);

            node.DrawPriority = -3;

            return node;
        }

        public override void OnMailBox (Node from, string address, object letter) {
            if (from == target) {
                Destroy (this);
            } 
        }

        public override void OnUpdate (long msec) {

            Node.Translation = target.Position / 10;
        }
    }

}
