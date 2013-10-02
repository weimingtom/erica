using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.GUISample {
    public class MyWorld : Component {

        public MyWorld () {
        }

        public static World Create () {
            var cmp = new MyWorld ();
            
            var spr = new Sprite (new Texture ("media/Vanity.jpg"));

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width / 2, spr.Height / 2, 1);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 0);

            var wld = new World ("MyWorld");
            wld.Attach (cmp);
            wld.Attach (spr);
            wld.Attach (col);

            return wld;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var sel = World.Find ("MouseSelector");
            if (sel != null) {
                if (button == MouseButton.Right) {
                    sel.GetComponent<MyMouseSelector> ().BeginSelect (x, y);
                }
            }    
        }

        public override void OnMouseButtonReleased (MouseButton button, float x, float y) {
            var sel = World.Find ("MouseSelector");
            if (sel != null) {
                if (button == MouseButton.Right) {
                    sel.GetComponent<MyMouseSelector> ().EndSelect (x, y);
                }
            }
        }
        

    }
}
