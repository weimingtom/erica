using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    public class MyBalloon : Component {
        long lifeTime;
        long bornTime;

        public MyBalloon () {
            this.lifeTime = 0;
            this.bornTime = 0;
        }

        public static Node Create (string words, long lifeTime) {
            var cmp = new MyBalloon ();
            cmp.lifeTime = lifeTime;

            var spr = new Sprite (128, 64);
            spr.AddTexture (new Texture ("media/Balloon-128x64.png"));

            var label = new Label ();
            label.Text = words;
            label.Color = Color.Black;
            label.SetOffset (4, 12);

            var node = new Node ("Balloon");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (label);

            node.DrawPriority = -3;

            return node;
        }

        public override void OnUpdateInit (long msec) {
            bornTime = msec;
        }

        public override void OnUpdate (long msec) {
            if (msec - bornTime > lifeTime) {
                Destroy (this);
            }
        }
    }
}
