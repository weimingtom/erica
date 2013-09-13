using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyLabels : Component{

        public static Node Create (Vector3 pos) {
            var cmp = new MyLabels ();

            var label1 = new Label ("");
            var label2 = new Label ("");
            label1.SetOffset (0, 0);
            label2.SetOffset (0, 16);

            var node = new Node ("Label");
            node.Attach (cmp);
            node.Attach (label1);
            node.Attach (label2);

            node.DrawPriority = -1;
            node.Translation = pos;

            return node;
        }
    }
}
