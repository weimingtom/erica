using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyCamera1 : Component {

        public static Node Create () {
            var cmp = new MyCamera1 ();
            var cam = new Camera ();
            cam.SetScreen (0, 0, 800, 600);
            cam.SetViewport (0, 0, 1, 1);

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (cam);
            node.Translate (-400, -300, 0);

            node.GroupID = 0xffffffff;

            return node;
        }
    }
}
