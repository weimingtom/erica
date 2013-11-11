using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.RTSSample {
    public class MyTullet : Component {
        float angularSpeed;
        long fireInterval;
        long fireTime;
        float effectiveRange;

        public MyTullet () {
            this.angularSpeed = 1;
            this.fireInterval = 1000;
            this.fireTime = 0;
            this.effectiveRange = 500;
        }


        public static Node Create () {
            var cmp = new MyTullet ();

            var spr = new Sprite (40, 72);
            spr.SetOffset (-20, -52);

            var col = new SphereCollision (cmp.effectiveRange);
            //col.DrawEnabled = true;

            var node = new Node ("Tullet");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);

            node.DrawPriority = -2;
            node.Translate (0, -8, 0);

            return node;
        }


        public void Fire () {
            var pos = Node.GlobalTransform.Apply (0, -80, 0);

            Vector3 T;
            Quaternion R;
            Vector3 S;
            Node.GlobalTransform.Decompress (out T, out R, out S);

            var node = MyProjectile.Create (pos, R);
            World.AddChild (node);

            var clip = new SoundClip ("media/Sound-Shot.wav");
            Sound.AddClip (clip, true);

            var tank = Node.Parent.GetComponent<MyTank>();
            var mychar = tank.MyCharacter.GetComponent<MyCharacter>();
            mychar.Say (BalloonMessage.Fire);

            var proj = node.GetComponent<MyProjectile> ();
            proj.Sender = tank.MyCharacter;
        }

        void Aim (Node target, long msec) {
            var dir = (target.Position - Node.Position);
            var forward = Node.GlobalTransform.ApplyDirection (0, -1, 0);
            var angle = Vector3.Angle (forward, dir);
            var axis = Vector3.Cross (forward, dir);

            Node.Rotate (MyMath.Clamp (angle, 0, angularSpeed), axis.X, axis.Y, axis.Z);

            if (angle < 0.1f && (msec - fireTime > fireInterval)) {
                Fire ();
                this.fireTime = msec;
            }
        }

        public override void OnUpdate (long msec) {
            if (!IsUnderWorld) {
                return;
            }
            if (Node.Parent.Is<MyEnemyAI> ()) {
                return;
            }

            //Node.Rotate (1, 0, 0, 1);
            var mycol = GetComponent<CollisionShape> ();
            var mytra = Node.GlobalTransform;

            var target = (from node in World.Downwards
                          where node != null
                          where node.Name == "EnemyTank"
                          let col = node.GetComponent<Collision> ()
                          let tra = node.GlobalTransform
                          let dist = Physics2D.Distance (col, tra, mycol, mytra)
                          where dist == 0
                          select node).FirstOrDefault ();
            if (target != null) {
                Aim (target, msec);
            }

            // Node.Rotate (1, 0, 0, 1);
        }

        public override void OnPreDraw (object window) {
            var spr = GetComponent<Sprite> ();

            var tank = Node.Parent.GetComponent<MyTank> ();

            spr.Color = tank.Selected ? Color.Red : Color.White;
        }

    }
}
