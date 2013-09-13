using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.PhysicsSample {
    public class MyFpsCounter : Component{

        public static Node Crate (Vector3 pos) {
            var cmp = new MyFpsCounter ();

            var fps = new FPSCounter ();

            var node = new Node ("FpsCounter");
            node.Attach (cmp);
            node.Attach (fps);

            node.DrawPriority = -1;
        node.Translation = pos;

        return node;
        }
    }
}
