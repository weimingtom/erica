using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.GUISample {
    public class MyButton : Component{
        
        public Label Label1 { get; set; }
        public Label Label2 { get; set; }

        public static Node Create () {
            var cmp = new MyButton ();
            
            var btn = new Button (ButtonType.Push);
            btn.Normal = new Texture ("media/image128x128(Red).png");
            btn.Pressed = new Texture ("media/image128x128(Green).png");

            var label1 = new Label ();
            var label2 = new Label ();
            label2.SetOffset (0, 20);
            
            var col = new BoxCollisionShape (64, 64, 0);
            col.SetOffset (64, 64, 0);

            var node = new Node ("Button");
            node.Attach (btn);
            node.Attach (cmp);
            node.Attach (label1);
            node.Attach (label2);
            node.Attach (col);

           // node.Translate (128, 128, 0);
           
            cmp.Label1 = label1;
            cmp.Label2 = label2;
            
            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
           Label1.Text = "Pressed " + button + " : " + x + ", " + y + "";
        }

        public override void OnMouseButtonReleased (MouseButton button, float x, float y) {
            Label2.Text = "Released " + button + " : " + x + ", " + y + "";
        }

    }
}
