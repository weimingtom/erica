using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;
using DD.Physics;

namespace DD.Sample.PhysicsSample {
    public class MyRhombus : Component{

        public static Node Create (string texName, Vector3 pos, float angle) {
            var spr = new Sprite (new Texture (texName));
            spr.SetOffset (-spr.Width / 2, -spr.Height / 2);

            var col = new RhombusCollisionShape (spr.Width / 2, spr.Height/2);
            var body = new PhysicsBody ();
            body.Type = ColliderType.Dynamic;
            body.SetShape (col);
            body.SetMaterial (new PhysicsMaterial ());
            body.IsEnabled = true;

            var cmp1 = new MyRhombus ();

            var node = new Node ("Rhombus");
            node.Translate (pos.X, pos.Y, 0);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (body);
            node.Attach (cmp1);
            node.Rotate (angle, 0, 0, 1);
            node.GroupID = 1;

            return node;
        }

        int i = 0;

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            Console.WriteLine ("Mouse pressed : {0}, {1}", button, i++);
        }
    }
}
