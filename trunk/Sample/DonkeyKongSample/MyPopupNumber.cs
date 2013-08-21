using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DonkeyKongSample {
    public class MyPopupNumber : Component {
        Label label;
        float y;
        float dy;
        int count;

        public static Node Create (string number, int dy) {
            var label = new Label (number);
            var comp = new MyPopupNumber ();
            comp.label = label;
            comp.y = 5;
            comp.dy = dy;

            var node = new Node ();
            node.Attach (label);
            node.Attach (comp);

            node.DrawPriority = -1;

            return node;
        }

        public override void OnUpdate (long msec) {
            y += dy;
            dy += 0.3f;
            count += 1;

            if (dy > 0) {
                dy = 0;
            }
            if (count > 20) {
                Destroy (Node);
            }

            label.SetOffset (0, y);
        }


    }
}
