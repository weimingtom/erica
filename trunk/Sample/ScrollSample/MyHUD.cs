using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.ScrollSample {
    public class MyHUD : Component {
        public MyHUD () {
        }

        public static Node Create () {
            
            var label = new Label ("こんにちは、世界（カメラ固定）");
            label.SetOffset (10, 10);
            
            var cmp = new MyHUD ();

            var node = new Node ();
            node.Attach (label);
            node.Attach (cmp);

            return node;
        }
    }
}
