using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.RTSSample {
    public class MyProjectile : Component {
        float speed;
        long bornTime;
        long lifeTime;
        int attackPower;
        Node sender;

        public Node Sender { 
            get { return sender; } 
            set { this.sender = value; }
        }

        public MyProjectile () {
            this.speed = 5;
            this.bornTime = 0;
            this.lifeTime = 3000;
            this.attackPower = 40;
            this.sender = null;
        }

        public static Node Create (Vector3 pos, Quaternion rot) {
            var cmp = new MyProjectile ();

            var spr = new Sprite (16, 32);
            spr.AddTexture (Resource.GetTexture("media/Projectile.png"));
            spr.SetOffset (-8, 16);

            var col = new SphereCollision (10);
            //col.DrawEnabled = true;

            var node = new Node ("Projectile");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);

            node.Translation = pos;
            node.Rotation = rot;

            return node;
        }

        public override void OnUpdateInit (long msec) {
            this.bornTime = msec;
        }

        public void Hit (Node hit) {
            var enemy = hit.GetComponent<MyTank> ();
            enemy.Damage (attackPower, sender);

            var node = MyExplosion.Create (Node.Position);
            World.AddChild (node);
        }

        public override void OnUpdate (long msec) {
            if (msec - bornTime > lifeTime) {
                Destroy (this);
                return;
            }

            var mycol = GetComponent<CollisionShape> ();
            var mytra = Node.GlobalTransform;

            var hit = (from node in World.Downwards
                       where node.Name == "EnemyTank"
                       let col = node.GetComponent<Collision> ()
                       let tra = node.GlobalTransform
                       where Physics2D.Distance (col, tra, mycol, mytra) == 0
                       select node).FirstOrDefault ();
            if (hit != null) {
                Hit (hit);
                Destroy (this);
                return;
            }

            var v = Node.Transform.ApplyDirection (0, -1, 0) * speed;
            Node.Translate (v.X, v.Y, v.Z);
        }
    }
}
