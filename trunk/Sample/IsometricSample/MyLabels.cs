using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.IsometricSample {
    public class MyLabels : Component {

        public static Node Create () {
            var cmp = new MyLabels ();

            var label1 = new Label ();
            var label2 = new Label ();
            label2.SetOffset (0, 24);

            var node = new Node ("Label");
            node.Attach (cmp);
            node.Attach (label1);
            node.Attach (label2);
            node.DrawPriority = -1;

            return node;
        }
    }
}
