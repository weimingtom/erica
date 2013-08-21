using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;
using DD.Physics;
using DD;

namespace DD.Sample.DonkeyKongSample {
    public class MyCharacter : Component {

        #region InnerClass
        enum State {
            Down,
            Up,
            Right,
            Left,
        }

        enum Trigger {
            Down,
            Up,
            Right,
            Left,
        }
        #endregion

        StateMachine<State, Trigger> sm;
        int miss;
        int invincibleTime;

        SoundClip jumpSound;
        AnimationClip walkUp;
        AnimationClip walkDown;
        AnimationClip walkRight;
        AnimationClip walkLeft;

        public float Speed {            get;            set;        }
        public bool IsGrounded {            get;            set;        }
        public Vector3 gravitationalVelocity {            get;            set;        }



        public MyCharacter () {
            this.Speed = 4.0f;
            this.IsGrounded = false;
            this.gravitationalVelocity = new Vector3 (0, 1, 0);
            this.jumpSound = null;
            this.walkUp = null;
            this.walkDown = null;
            this.walkRight = null;
            this.walkLeft = null;
                
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyCharacter ();

            var spr1 = new Sprite (new Texture ("media/Character-Gelato.png"), 24, 32);
            var spr2 = new Sprite (new Texture ("media/Image128x128(Red).png"), 24, 4);
            spr2.SetOffset (0, spr1.Height);

            var body = new BoxCollisionShape (spr1.Width / 2, spr1.Height / 2, 0);
            var foot = new SphereCollisionShape (2);
            body.SetOffset (spr1.Width / 2, spr1.Height / 2, 0);
            foot.SetOffset (spr1.Width / 2, spr1.Height + 2, 0);

            var node = new Node ("MyCharacter");
            node.Attach (spr1);
            node.Attach (spr2);
            node.Attach (body);
            node.Attach (foot);
            node.Attach (cmp);

            node.DrawPriority = -1;
            node.Translation = pos;

            cmp.jumpSound = new SoundClip ("JumpSound");
            cmp.jumpSound.AddTrack (new SoundEffectTrack ("media/Jump.ogg"));

            var track1 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track1.AddKeyframe (0, new Vector2 (0, 64));
            track1.AddKeyframe (300, new Vector2 (24, 64));
            track1.AddKeyframe (600, new Vector2 (48, 64));
            cmp.walkDown = new AnimationClip (900, "MyCharacter.Down");
            cmp.walkDown.AddTrack (spr1, track1);

            var track2 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track2.AddKeyframe (0, new Vector2 (0, 0));
            track2.AddKeyframe (300, new Vector2 (24, 0));
            track2.AddKeyframe (600, new Vector2 (48, 0));
            cmp.walkUp = new AnimationClip (900, "MyCharacter.Up");
            cmp.walkUp.AddTrack (spr1, track2);

            var track3 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track3.AddKeyframe (0, new Vector2 (0, 32));
            track3.AddKeyframe (300, new Vector2 (24, 32));
            track3.AddKeyframe (600, new Vector2 (48, 32));
            cmp.walkRight = new AnimationClip (900, "MyCharacter.Right");
            cmp.walkRight.AddTrack (spr1, track3);

            var track4 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track4.AddKeyframe (0, new Vector2 (0, 96));
            track4.AddKeyframe (300, new Vector2 (24, 96));
            track4.AddKeyframe (600, new Vector2 (48, 96));
            cmp.walkLeft = new AnimationClip (900, "MyCharacter.Left");
            cmp.walkLeft.AddTrack (spr1, track4);

            return node;
        }

        public void Hit () {
            if (invincibleTime > 0) {
                return;
            }

            this.invincibleTime = 30;
            miss += 1;
            var label = World.Find (x => x.Name == "Label").GetComponent<Label> (3);
            label.Text = "Miss : " + miss;

            var node = MyPopupNumber.Create ("MISS!", -5);
            node.Translate (-10, -5, 0);
            Node.AddChild (node);

            var clip = Resource.GetSoundTrack ("media/Cancel.ogg");
            clip.Play ();
        }

        public override void OnUpdateInit (long msec) {

            Animation.AddClip (walkUp);
            Animation.AddClip (walkDown);
            Animation.AddClip (walkRight);
            Animation.AddClip (walkLeft);


            sm = new StateMachine<State, Trigger> (State.Down);
            walkDown.Play ();

            sm.Configure (State.Down)
                .Ignore (Trigger.Down)
                .Permit (Trigger.Up, State.Up)
                .Permit (Trigger.Right, State.Right)
                .Permit (Trigger.Left, State.Left)
                .OnExit (x => walkDown.Stop ())
                .OnEntry (x => walkDown.Play ());

            sm.Configure (State.Up)
                .Permit (Trigger.Down, State.Down)
                .Ignore (Trigger.Up)
                .Permit (Trigger.Right, State.Right)
                .Permit (Trigger.Left, State.Left)
                .OnExit (x => walkUp.Stop ())
                .OnEntry (x => walkUp.Play ());

            sm.Configure (State.Right)
                .Permit (Trigger.Down, State.Down)
                .Permit (Trigger.Up, State.Up)
                .Ignore (Trigger.Right)
                .Permit (Trigger.Left, State.Left)
                .OnExit (x => walkRight.Stop ())
                .OnEntry (x => walkRight.Play ());

            sm.Configure (State.Left)
                .Permit (Trigger.Down, State.Down)
                .Permit (Trigger.Up, State.Up)
                .Permit (Trigger.Right, State.Right)
                .Ignore (Trigger.Left)
                .OnExit (x => walkLeft.Stop ())
                .OnEntry (x => walkLeft.Play ());

        }

        public override void OnUpdate (long msec) {
            this.invincibleTime += -1;

            var label1 = World.Find ("Label").GetComponent<Label> (0);
            var label2 = World.Find ("Label").GetComponent<Label> (1);
            var map = World.Find ("Platform");
            var body = GetComponent<CollisionShape> (0);
            var foot = GetComponent<CollisionShape> (1);
            var footTra = foot.Node.GlobalTransform;

            var right = new Vector3 (1, 0, 0);

            // 床面の検出
            {
                var hit = (from n in map.Downwards
                           let col = n.GetComponent<CollisionShape> ()
                           let colTra = n.GlobalTransform
                           where col != null && Physics2D.Collide (foot, footTra, col, colTra)
                           let colRight = colTra.ApplyDirection (1, 0, 0)
                           select new { n, right = colRight }).FirstOrDefault ();
                if (hit != null) {
                    label2.Text = "Hit: right = " + hit.right;
                    right = hit.right;
                }
                else {
                    label2.Text = "Hit: None";
                }
            }

            var x = 0;
       
            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.RightArrow: {
                            x = x + 1;
                            this.sm.Fire (Trigger.Right);
                            break;
                        }
                    case KeyCode.LeftArrow: {
                            x = x - 1;
                            this.sm.Fire (Trigger.Left);
                            break;
                        }
                    case KeyCode.Space: {
                            if (Input.GetKeyDown (KeyCode.Space)) {
                                this.IsGrounded = false;
                                this.gravitationalVelocity = new Vector3 (0, -11, 0);
                                jumpSound.Play ();
                            }
                            break;
                        }
                }
            }
            if (Input.Keys.Contains (KeyCode.LeftShift)) {
                x *= 3;
            }

            // ユーザーによる操作
            var v = x * right * Speed;
            if (v.Length2 > 0) {
                var currTra = Node.GlobalTransform;
                var deltTra = Matrix4x4.CreateFromTranslation (v.X, v.Y, 0);
                var nextTra = deltTra * currTra;

                var hit = (from n in map.Downwards
                           let comp = n.GetComponent<CollisionShape> ()
                           let tra = n.GlobalTransform
                           where comp != null && Physics2D.Collide (body, nextTra, comp, tra)
                           let dist = Physics2D.Distance (body, currTra, comp, tra)
                           select new { n, dist }).FirstOrDefault ();
                if (hit != null) {
                    label1.Text = "Hit: dist = " + hit.dist;
                    v = v.Normalize () * hit.dist;
                    Node.Translate (v.X, v.Y, v.Z);
                }
                else {
                    label1.Text = "Hit: None";
                    Node.Translate (v.X, v.Y, v.Z);
                }
            }

            // 重力による落下
            v = gravitationalVelocity;
            if (true) {
                var currTra = Node.GlobalTransform;
                var deltTra = Matrix4x4.CreateFromTranslation (v.X, v.Y, 0);
                var nextTra = deltTra * currTra;

                var hit = (from n in map.Downwards
                           let comp = n.GetComponent<CollisionShape> ()
                           let tra = n.GlobalTransform
                           where comp != null && Physics2D.Collide (body, nextTra, comp, tra)
                           let dist = Physics2D.Distance (body, currTra, comp, tra)
                           select new { n, dist }).FirstOrDefault ();
                if (hit != null) {
                    label1.Text = "Hit: dist = " + hit.dist;
                    if (hit.dist > 1) {
                        v = v.Normalize () * hit.dist * 0.8f;
                        Node.Translate (v.X, v.Y, v.Z);
                    }
                    gravitationalVelocity = new Vector3 (0, 1, 0);
                    IsGrounded = true;
                }
                else {
                    label1.Text = "Hit: None";
                    Node.Translate (v.X, v.Y, v.Z);
                    this.gravitationalVelocity += new Vector3 (0, 1, 0);
                    IsGrounded = false;
                }
            }
        }
    }
}
