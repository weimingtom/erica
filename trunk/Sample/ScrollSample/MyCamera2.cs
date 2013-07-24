using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.ScrollSample {
    public class MyCamera2 : Component{

        public static Node Create () {

            var cam = new Camera ();
            cam.ClearEnabled = false;
            cam.SetScreen (0, 0, 1920, 1200);
            cam.SetViewport (0.75f, 0.05f, 0.2f, 0.2f);

            var cmp = new MyCamera1 ();

            var node = new Node ("Camera2");
            node.Attach (cmp);
            node.Attach (cam);

            return node;
        }
    }
}
