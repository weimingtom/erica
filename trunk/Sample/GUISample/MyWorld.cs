using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.GUISample {
    public class MyWorld : Component {
        Node mouse;

        public MyWorld () {
        }

        public static World Create () {
            var cmp = new MyWorld ();
            
            var spr = new Sprite (new Texture ("media/Vanity.jpg"));

            var col = new BoxCollisionShape (spr.Width / 2, spr.Height / 2, 0);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 0);

            var wld = new World ("World");
            wld.Attach (cmp);
            wld.Attach (spr);
            wld.Attach (col);

            return wld;
        }

        public override void OnUpdateInit (long msec) {
            this.mouse = World.Find ("MouseSelector");
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            mouse.GetComponent<MyMouseSelector>().BeginSelection ();
        }

        public override void OnMouseButtonReleased (MouseButton button, float x, float y){
            mouse.GetComponent<MyMouseSelector>().EndSelection ();
        }


    }
}
