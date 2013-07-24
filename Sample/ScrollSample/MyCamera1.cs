using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.ScrollSample {
    public class MyCamera1 : Component {

        public int MoveSpeed { get; set; }
        public int RotateSpeed { get; set; }
        public float ZoomSpeed { get; set; }

        public static Node Create () {
            var cam = new Camera ();
            cam.SetScreen (0, 0, 800, 600);
            cam.SetViewport (0, 0, 1, 1);

            var cmp = new MyCamera1 ();
            cmp.MoveSpeed = 10;
            cmp.RotateSpeed = 3;
            cmp.ZoomSpeed = 0.1f;

            var node = new Node ("Camera1");
            node.Attach (cmp);
            node.Attach (cam);
            node.Translate (-400, -300, 0);

            return node;
        }


    }
}
