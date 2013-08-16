using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.IsometricSample {
    public class MyCamera : Component {
        float speed = 10;

        public static Node Create (){

            var cam = new Camera ();
            cam.SetScreen (-400, -300+160, 800, 600);
            //cam.SetScreen (0, 0, 800, 600);
            cam.SetViewport (0, 0, 1, 1);
           
            var cmp = new MyCamera ();

            var node = new Node ();
            node.Attach (cam);
            node.Attach (cmp);

            return node;
        }
        /*
        public override void OnUpdate (long msec) {
            var x = 0;
            var y = 0;
            foreach(var key in Input.Keys) {
                switch(key){
                    case KeyCode.RightArrow: x += 1; break;
                    case KeyCode.LeftArrow: x -= 1; break;
                    case KeyCode.DownArrow: y += 1; break;
                    case KeyCode.UpArrow: y -= 1; break;
                }
            }

            Node.Translate (x * speed, 0, 0);
            Node.Translate (0, y * speed, 0);
        }
         * */

    }
}
