using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.MouseSample {
    public class MyMouseSelector : Component {
        Vector2 start;

        public MyMouseSelector () {
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyMouseSelector ();
            cmp.start = Graphics2D.GetInstance ().GetMousePosition ();

            var spr = new Sprite (1, 1);
            spr.SetColor (255, 64, 64, 64);

            var col = new CollisionObject ();
            col.Shape = new BoxShape (1, 1, 1);

            var node = new Node ("MouseSelector");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);

            node.DrawPriority = -1;
            node.Translation = pos;

            return node;
        }

        public override void OnUpdate (long msec) {
            var end = Graphics2D.GetInstance ().GetMousePosition ();
            var center = end / 2;
            var width = end.X - start.X;
            var height = end.Y - start.Y;

            if (width > 1 && height > 1) {
                var spr = GetComponent<Sprite> ();
                spr.Resize ((int)width, (int)height);

                var col = GetComponent<CollisionObject> ();
                col.Offset = new Vector3 (center.X, center.Y, 0);
                col.Shape = new BoxShape (width / 2, height / 2, 1000);

                SendMessage ("MouseSelect", col);
            }
        }

        public override void OnDetached () {

            SendMessage ("MouseDeselect", null);
        }



    }
}
