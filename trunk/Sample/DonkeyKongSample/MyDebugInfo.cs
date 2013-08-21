using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DonkeyKongSample {
    public class MyDebugInfo: Component {

        public MyDebugInfo () {
        }

        public static Node Create (Vector3 pos) {
            var label1 = new Label ();
            var label2 = new Label ();
            var label3 = new Label ("Destroy : ");
            var label4 = new Label ("Miss : 0");
            label2.SetOffset (0, 20);
            label3.SetOffset (200, 0);
            label4.SetOffset (200, 20);

            var node = new Node ("Label");
            node.Attach (label1);
            node.Attach (label2);
            node.Attach (label3);
            node.Attach (label4);

            node.Translation = pos;

            return node;
        
        }
    }
}
