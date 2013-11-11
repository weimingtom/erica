using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.DonkeyKongSample {
    public class MyGoal: Component {

        public MyGoal () {
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyGoal ();

            var spr = new Sprite (128, 128);
            spr.AddTexture (Resource.GetDefaultTexture ());
            spr.Color = Color.Blue;

            var col = new BoxCollision (spr.Width / 2, spr.Height / 2, 0);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 0);

            var node = new Node ("Goal");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);

            node.Translation = pos;

            return node;
        }

        public override void OnUpdate (long msec) {
            var col = GetComponent<CollisionShape> ();
            var tra = Node.GlobalTransform;
            var playerCol = World.Find ("MyCharacter").GetComponent<Collision> ();
            var playerTra = playerCol.Node.GlobalTransform;

            var hit = Physics2D.Collide (col, tra, playerCol, playerTra);
            if (hit) {
                SendMessage ("GameClear", null);
                Node.Updatable = false;
            }
        }

    }
}
