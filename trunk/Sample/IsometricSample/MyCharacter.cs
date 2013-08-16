using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;
using DD.Physics;

namespace DD.Sample.IsometricSample {
    public class MyCharacter : Component {
        float x = 0;
        float y = 0;

        public static Node Create () {
            var spr = new Sprite (64, 128);
            spr.AddTexture (new Texture ("media/isometric-foot.png"));
            spr.AddTexture (new Texture ("media/isometric-man-ne.png"));
            spr.AddTexture (new Texture ("media/isometric-man-nw.png"));
            spr.AddTexture (new Texture ("media/isometric-man-se.png"));
            spr.AddTexture (new Texture ("media/isometric-man-sw.png"));
            spr.ActiveTextureIndex = 1;

            var col = new BoxCollisionShape (32, 64, 0);
            col.SetOffset (32, 64, 0);

            var clicker = new MyClicker (spr);
            
            var cmp = new MyCharacter ();

            var node = new Node ("Character");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (clicker);

            node.DrawPriority = -1;

            cmp.collision = col;
            cmp.sprite = spr;

            return node;
        }


        private void Move (float dx, float dy, float dz) {
            var label1 = labelNode.GetComponent<Label> (0);
            var label2 = labelNode.GetComponent<Label> (1);

            var pos = Node.Position;
            var vel = new Vector3 (dx, dy, 0);

            // Tile = 64x32
            var pointA = pos + new Vector3 (0, 0, 0);
            var pointB = pos + new Vector3 (64, 0, 0);
            var pointC = pos + new Vector3 (0, 32, 0);
            var pointD = pos + new Vector3 (64, 32, 0);
            var rayA = new Ray (pointA, pointA + vel, 1.0f);
            var rayB = new Ray (pointB, pointB + vel, 1.0f);
            var rayC = new Ray (pointC, pointC + vel, 1.0f);
            var rayD = new Ray (pointD, pointD + vel, 1.0f);

            RayIntersection riA = new RayIntersection ();
            RayIntersection riB = new RayIntersection ();
            RayIntersection riC = new RayIntersection ();
            RayIntersection riD = new RayIntersection ();
            
            var hit = (from node in colNode.Downwards.Skip (1)
                       let col = node.GetComponent<CollisionShape> ()
                       where col != null
                       let hitA = Physics2D.RayCast (col, rayA, out riA)
                       let hitB = Physics2D.RayCast (col, rayB, out riB)
                       let hitC = Physics2D.RayCast (col, rayC, out riC)
                       let hitD = Physics2D.RayCast (col, rayD, out riD)
                       select new[] { riA, riB, riC, riD } into inters
                       from ri in inters
                       where ri.Hit == true
                       orderby ri.Distance
                       select new { ri.Node, ri.Distance, ri.Normal }).FirstOrDefault ();
            if (hit == null) {
                label1.Text = "No Hit : ";
                Node.Translate (vel.X, vel.Y, 0);
            }
            else {
                label1.Text = "Hit : ";
                if (hit.Distance > 1) {
                    vel *= (hit.Distance / vel.Length) * 0.9f;
                    Node.Translate (vel.X, vel.Y, 0);
                }
            }
            


        }

        Node mapNode;
        Node colNode;
        TiledMapComposer tiled;
        Node labelNode;
        Sprite sprite;
        CollisionShape collision;

        public override void OnUpdateInit (long msec) {
            this.mapNode = World.Find (x => x.Is<TiledMapComposer> ());
            this.colNode = mapNode.Find ("Collision");
            this.tiled = mapNode.GetComponent<TiledMapComposer> ();
            this.labelNode = World.Find ("Label");

        }

        float speed = 10f;

        public override void OnUpdate (long msec) {

            var dx = 0;
            var dy = 0;

            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.RightArrow: dy -= 1; sprite.ActiveTextureIndex = 1;  break;
                    case KeyCode.LeftArrow: dy += 1; sprite.ActiveTextureIndex = 4; break;
                    case KeyCode.DownArrow: dx += 1; sprite.ActiveTextureIndex = 3; break;
                    case KeyCode.UpArrow: dx -= 1; sprite.ActiveTextureIndex = 2; break;
                }
            }
            if (dx != 0) {
                Move (dx * speed * 2, 0, 0);
            }
            if (dy != 0) {
                Move (0, dy * speed, 0);
            }

        }

        public override void OnPreDraw (object window) {
            var offset = tiled.OrthogonalToIsometric (Node.Translation);
            
            // 96は接地ラインをタイルの接地ラインにあわせるため

            sprite.SetOffset (offset.X, offset.Y - 96);
            collision.SetOffset (32 + offset.X, 64 + offset.Y - 96, 0);
        }
    }
}
