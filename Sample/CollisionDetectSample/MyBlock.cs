using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.CollisionDetectSample {
    public class MyBlock : Component {

        public static Node Create (Vector3 pos, int collisionMask) {
            var cmp = new MyBlock ();

            var spr = new Sprite (128, 64);
            spr.AddTexture (Resource.GetDefaultTexture ());
            spr.Color = Color.Cyan;

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width/2, spr.Height/2, 1);
            col.SetOffset (spr.Width/2, spr.Height/2, 1);
            col.CollideWith = collisionMask;

            var label = new Label ();
            label.SetText ("Mask = " + collisionMask.ToString("x"));

            var node = new Node ("Block");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (label);

            node.Translation = pos;

            return node;
        }
    }
}
