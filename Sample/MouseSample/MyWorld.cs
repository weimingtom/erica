using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.MouseSample {
    public class MyWorld : Component {

        public static World Create (float width, float height) {
            var cmp = new MyWorld ();

            var spr = new Sprite ();
            spr.AddTexture(new Texture ("media/Vanity.jpg"));

            var col = new CollisionObject ();
            col.Shape = new BoxShape (width / 2, height / 2, 1);
            col.SetOffset (width / 2, height / 2, 0);

            var node = new World ("MyWorld");
            node.Attach (cmp);
            node.Attach (col);
            node.Attach (spr);

            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var selector = MyMouseSelector.Create (new Vector3(x,y,0));
            Node.AddChild (selector);
        }

        public override void OnMouseButtonReleased (MouseButton button, float x, float y) {
            var selector = Node.Find ("MouseSelector");
            Destroy (selector, 0);
        }
    }
}
