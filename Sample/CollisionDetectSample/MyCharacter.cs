using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.CollisionDetectSample {
    public class MyCharacter: Component {

        float speed = 3.77f;

        public static Node Create (Vector3 pos, int groupID) {
            var cmp = new MyCharacter ();

            var spr = new Sprite (64, 64);
            spr.AddTexture (Resource.GetDefaultTexture ());
            spr.Color = Color.Red;
            //spr.SetOffset (-32, -32);

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width/2, spr.Height/2, 1);
            col.SetOffset (spr.Width/2, spr.Height/2, 1);

            var label = new Label ();
            label.SetText ("ID = " + groupID.ToString ("x"));

            var node = new Node ("MyCharacter");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (label);

            node.Translation = pos;
            node.GroupID = groupID;

            return node;
        }

        void Move (float x, float y, float z) {
            var move = new Vector3 (x, y, z) * speed;

            var result = World.Sweep (Node, move);
            if (result.Hit) {
                move *= result.Fraction;
                if (move.Length > result.Distance - 1) {
                    move *= (result.Distance - 1) / move.Length;
                }
            }
            Node.Translate (move.X, move.Y, move.Z);
        }

        public override void OnUpdate (long msec) {
            var x = 0;
            var y = 0;
            foreach (var key in Input.Keys) { 
            switch (key) {
                case KeyCode.LeftArrow: x -= 1; break;
                case KeyCode.RightArrow: x += 1; break;
                case KeyCode.UpArrow: y -= 1; break;
                case KeyCode.DownArrow: y += 1; break;
            }

            }

            if (x != 0) {
                Move (x, 0, 0);
            }
            if (y != 0) {
                Move (0, y, 0);
            }

        }
    }
}
