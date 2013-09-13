using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.PlatformSample {
    public class MyLabel : Component{

        public static Node Create (Vector3 pos) {
            var cmp = new MyLabel ();

            var label1 = new Label ();
            var label2 = new Label ();

            var node = new Node ("Label");
            node.Attach (cmp);
            node.Attach (label1);
            node.Attach (label2);

            return node;
        }
    }
}
