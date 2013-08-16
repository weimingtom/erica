using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.GUISample {
    public class MyTarget: Component {
        public bool Selected { get; set; }

        public MyTarget () {
            this.Selected = false;
        }

        public static Node Create () {

            var spr1 = new Sprite (new Texture ("media/isometric-man-ne.png"), 64, 128);
            var spr2 = new Sprite (new Texture ("media/isometric-man-nw.png"), 64, 128);
            var spr3 = new Sprite (new Texture ("media/isometric-man-se.png"), 64, 128);
            var spr4 = new Sprite (new Texture ("media/isometric-man-sw.png"), 64, 128);

            var col1 = new BoxCollisionShape (32, 64, 0);
            var col2 = new BoxCollisionShape (32, 64, 0);
            var col3 = new BoxCollisionShape (32, 64, 0);
            var col4 = new BoxCollisionShape (32, 64, 0);
            col1.SetOffset (32, 64, 0);
            col2.SetOffset (32, 64, 0);
            col3.SetOffset (32, 64, 0);
            col4.SetOffset (32, 64, 0);

            var cmp1 = new MyTarget ();
            var cmp2 = new MyTarget ();
            var cmp3 = new MyTarget ();
            var cmp4 = new MyTarget ();

            var node1 = new Node ("Man-NE");
            var node2 = new Node ("Man-NW");
            var node3 = new Node ("Man-SE");
            var node4 = new Node ("Man-SW");

            node1.Translate (200, 100, 0);
            node2.Translate (280, 100, 0);
            node3.Translate (200, 244, 0);
            node4.Translate (280, 244, 0);

            node1.Attach (spr1);
            node2.Attach (spr2);
            node3.Attach (spr3);
            node4.Attach (spr4);

            node1.Attach (cmp1);
            node2.Attach (cmp2);
            node3.Attach (cmp3);
            node4.Attach (cmp4);

            node1.Attach (col1);
            node2.Attach (col2);
            node3.Attach (col3);
            node4.Attach (col4);

            var node = new Node ("Targets");
            node.AddChild (node1);
            node.AddChild (node2);
            node.AddChild (node3);
            node.AddChild (node4);

            return node;
        }

        public override void OnPreDraw (object window) {
            base.OnDraw (window);
            var spr = GetComponent<Sprite> ();
            if (Selected) {
                spr.SetColor (255, 0, 0, 255);
            }
            else {
                spr.SetColor (255, 255, 255, 255);
            }
            
        }

       
    }
}
