using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyCamera: Component {
        public MyCamera () {

        }

        public static Node Create () {
            var cmp = new MyCamera ();
            
            var cam = new Camera ();
            cam.SetScreen (0, 0, 800, 600);
            cam.SetViewport (0, 0, 1, 1);

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (cam);

            node.Translation = new Vector3 (-400, -300, 0);

            return node;
        }
    }
}
