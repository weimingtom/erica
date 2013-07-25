using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.TiledMapSample {
    public class MyCharacter : Component {
        public float Speed { get; set; }
        public Node Map { get; set; }
        public Node CollisionMap { get; set; }
        public Sprite Sprite { get; set; }

        public MyCharacter () {
            this.Speed = 10;
        }

        public static Node Create () {
            var cmp = new MyCharacter ();
            var spr = new Sprite (24, 32);
            spr.AddTexture(new Texture ("media/Character-Gelato.png"));
            spr.AddTexture (new Texture ("media/image128x128(Cyan).png"));

            var col = new BoxCollisionShape (spr.Width / 2, spr.Height / 2, 0);
            col.Offset = new Vector3 (spr.Width / 2, spr.Height / 2, 0);

            var node = new Node ("MyCharacter");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.DrawPriority = -1;
            node.Translate (150, 120, 0);

            node.GroupID = 0xffffffffu;

            cmp.Sprite = spr;

            return node;

        }

        public override void OnAttached () {
        }

        private void Move (Vector3 v) {
            var label1 = World.Find ("Label").GetComponent<Label> (0);
            var label2 = World.Find ("Label").GetComponent<Label> (1);

            var pointA = new Vector3 (Node.Position.X, Node.Position.Y, 0);
            var pointB = new Vector3 (Node.Position.X + 24, Node.Position.Y, 0);
            var pointC = new Vector3 (Node.Position.X, Node.Position.Y + 32, 0);
            var pointD = new Vector3 (Node.Position.X + 24, Node.Position.Y + 32, 0);
            var rayA = new Ray (pointA, pointA + v, 1f);
            var rayB = new Ray (pointB, pointB + v, 1f);
            var rayC = new Ray (pointC, pointC + v, 1f);
            var rayD = new Ray (pointD, pointD + v, 1f);
            var outA = new RayIntersection ();
            var outB = new RayIntersection ();
            var outC = new RayIntersection ();
            var outD = new RayIntersection ();

            var hit = (from node in CollisionMap.Downwards
                       let shp = node.GetComponent<CollisionShape> ()
                       where shp != null
                       let hitA = Physics2D.RayCast (shp, rayA, out outA)
                       let hitB = Physics2D.RayCast (shp, rayB, out outB)
                       let hitC = Physics2D.RayCast (shp, rayC, out outC)
                       let hitD = Physics2D.RayCast (shp, rayD, out outD)
                       select new[] { outA, outB, outC, outD } into hitResults
                       from x in hitResults
                       where x.Hit
                       orderby x.Distance
                       select x
                       ).FirstOrDefault ();
            if (!hit.Hit) {
                label1.Text = "Hit: なし";
                Node.Translate (v.X, v.Y, 0);
            }
            else {
                label1.Text = "Hit: " + hit.Node.Name;
                label2.Text = "Dist = " + hit.Distance;
                if (hit.Distance > 1) {
                    v *= (hit.Fraction * 0.9f);
                    Node.Translate (v.X, v.Y, 0);
                }
            }

        }

        public override void OnUpdate (long msec) {
            var label1 = World.Find ("Label").GetComponent<Label> (0);
            var label2 = World.Find ("Label").GetComponent<Label> (1);
            label2.Text = Node.Translation.ToString ();

            var dx = 0;
            var dy = 0;

            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.LeftArrow: dx = dx - 1; break;
                    case KeyCode.RightArrow: dx = dx + 1; break;
                    case KeyCode.UpArrow: dy = dy - 1; break;
                    case KeyCode.DownArrow: dy = dy + 1; break;
                }
            }

            if (dx != 0) {
                Move (new Vector3 (dx, 0, 0) * Speed);
            }
            if (dy != 0) {
                Move (new Vector3 (0, dy, 0) * Speed);
            }


        }

        public override void OnPreDraw (object window) {
            var pass = World.GetProperty<int> ("Pass");

            if (pass == 1) {
                Sprite.ActiveTextureIndex = 0;
            }
            if (pass == 2) {
                Sprite.ActiveTextureIndex = 1;
            }
        }



    }

}
