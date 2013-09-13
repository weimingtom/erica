using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using DD.Physics;

namespace DD.Sample.MessageSample {
    public class MyButton : Component{
        string address;

        public MyButton () {
        }

        public static Node Create (string name, string address, Vector3 pos) {
            var cmp = new MyButton ();
            cmp.address = address;

            var spr = new Sprite (32, 32);
            spr.AddTexture (Resource.GetDefaultTexture ());
            
            var label = new Label ();
            label.Text = name;
            label.Color = Color.Black;
            label.SetOffset (8, 8);

            var col = new CollisionObject ();
            col.Shape = new BoxShape(16, 16, 16);
            col.SetOffset (16, 16, 16);

            var node = new Node (name);
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (label);
            node.Attach (col);

            node.Translation = pos;

            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var node = World.Find ("Sender");
            if (node != null) {
                node.GetComponent<MySender> ().Address = address;
            }
        }
    }
}
