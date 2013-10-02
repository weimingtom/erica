using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.GUISample {
    public class MyButton : Component{

        public MyButton () {
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyButton ();
            
            var btn = new Button (ButtonType.Push);
            btn.Normal = new Texture ("media/image128x128(Red).png");
            btn.Pressed = new Texture ("media/image128x128(Green).png");

            var label1 = new Label ();
            var label2 = new Label ();
            label2.SetOffset (0, 20);

            var col = new CollisionObject ();
            col.Shape = new BoxShape (64, 64, 100);
            col.SetOffset (64, 64, 100);

            var node = new Node ("Button");
            node.Attach (btn);
            node.Attach (cmp);
            node.Attach (label1);
            node.Attach (label2);
            node.Attach (col);

           node.Translation = pos;
           
            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var label = GetComponent<Label> (0);
            label.Text = "Pressed " + button + " : " + x + ", " + y + "";
        }

        public override void OnMouseButtonReleased (MouseButton button, float x, float y) {
            var label = GetComponent<Label> (1);
            label.Text = "Released " + button + " : " + x + ", " + y + "";
        }

    }
}
