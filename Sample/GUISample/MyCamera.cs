using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.GUISample {
    public class MyCamera : Component {
        public float Speed { get; set; }

        public MyCamera () {
            this.Speed = 10;
        }

        public static Node Create () {
            var cmp = new MyCamera ();
            var cam = new Camera ();
            cam.SetScreen (0, 0, 800, 600);
            cam.SetViewport (0, 0, 1, 1);

            var node = new Node ("Camera");
            node.Attach (cmp);
            node.Attach (cam);

            return node;
        }

        void Move (float x, float y, float z) {
            Node.Translate (x, y, z);
        }

        public override void OnUpdate (long msec) {
            var x = 0;
            var y = 0;

            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.RightArrow: x += 1; break;
                    case KeyCode.LeftArrow: x -= 1; break;
                    case KeyCode.UpArrow: y -= 1; break;
                    case KeyCode.DownArrow: y += 1; break;
                }
            }

            Move (x * Speed, 0, 0);
            Move (0, y * Speed, 0);
        }
    }
}
