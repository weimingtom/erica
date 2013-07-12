using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample {
    public class MyWorld : Component{

        public MyWorld () {

        }

        public static World Create () {
            var spr = new Sprite (new Texture ("media/DarkGalaxy.jpg"));
            var cmp = new MyWorld ();

            var node = new World ("First Script");
            node.Attach (spr);
            node.Attach (cmp);
            node.DrawPriority = 127;

            return node;
        }

        public override void OnUpdate (long msec) {
            var g2d = Graphics2D.GetInstance ();

            if (Input.GetKeyDown (KeyCode.Mouse0)) {
                var pos = g2d.GetMousePosition ();

                var node = new Node ();
                var comp = new MyPopupNumber ();
                node.Attach (comp);
                node.Translation = new Vector3 (pos.X, pos.Y, 0);
                Node.AddChild (node);
            }
        }
    }
}
