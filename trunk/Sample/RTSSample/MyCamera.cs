using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    public class MyCamera : Component {

        public float Speed { get; set; }

        public MyCamera () {
            this.Speed = 8;
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyCamera ();
            var cam = new Camera ();
            cam.SetScreen (0, 0, 800, 600 + 128);
            cam.SetViewport (0, 0, 1, 1);
            var node = new Node ("Camera");
            node.Attach (cmp);
            node.Attach (cam);

            node.Translation=pos;

            return node;
        }

        public override void OnUpdate (long msec) {
            var dx = 0;
            var dy = 0;

            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.RightArrow: dx += 1; break;
                    case KeyCode.LeftArrow: dx -= 1; break;
                    case KeyCode.UpArrow: dy -= 1; break;
                    case KeyCode.DownArrow: dy += 1; break;
                }
            }

            if (dx != 0) {
                Node.Translate (dx * Speed, 0, 0);
            }
            if (dy != 0) {
                Node.Translate (0, dy * Speed, 0);
            }

        }
    }
}
