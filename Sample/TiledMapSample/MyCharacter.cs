using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;

namespace DD.Sample.TiledMapSample {
    public class MyCharacter : Component {

        public enum MovingState {
            Up,
            Right,
            Down,
            Left
        }

        public enum MovingTrigger {
            Up,
            Right,
            Down,
            Left
        }

        float speed = 10;
        StateMachine<MovingState, MovingTrigger> sm = new StateMachine<MovingState, MovingTrigger> (MovingState.Down);

        public MyCharacter () {
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyCharacter ();

            var spr = new Sprite (24, 32);
            spr.AddTexture (new Texture ("media/Character-Gelato.png"));
            spr.AddTexture (new Texture ("media/image128x128(Cyan).png"));

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width / 2, spr.Height / 2, 1);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 1);

            var shadow = new Sprite (24, 8);
            shadow.AddTexture (new Texture ("media/FakeShadow.png"));
            shadow.SetOffset (0, 30);

            var anim = new AnimationController ();

            var node = new Node ("MyCharacter");
            node.Attach (cmp);
            node.Attach (shadow);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (anim);

            node.DrawPriority = -1;
            node.Translation = pos;

            // アニメーション設定
            var up = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            up.AddKeyframe (0, new Vector2 (0, 0));
            up.AddKeyframe (1000, new Vector2 (24, 0));
            up.AddKeyframe (2000, new Vector2 (48, 0));
            var clip1 = new AnimationClip (3000, "Up");
            clip1.AddTrack (spr, up);
            anim.AddClip (clip1);

            var right = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            right.AddKeyframe (0, new Vector2 (0, 32));
            right.AddKeyframe (1000, new Vector2 (24, 32));
            right.AddKeyframe (2000, new Vector2 (48, 32));
            var clip2 = new AnimationClip (3000, "Right");
            clip2.AddTrack (spr, right);
            anim.AddClip (clip2);


            var down = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            down.AddKeyframe (0, new Vector2 (0, 64));
            down.AddKeyframe (1000, new Vector2 (24, 64));
            down.AddKeyframe (2000, new Vector2 (48, 64));
            var clip3 = new AnimationClip (3000, "Down");
            clip3.AddTrack (spr, down);
            anim.AddClip (clip3);

            var left = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            left.AddKeyframe (0, new Vector2 (0, 96));
            left.AddKeyframe (1000, new Vector2 (24, 96));
            left.AddKeyframe (2000, new Vector2 (48, 96));
            var clip4 = new AnimationClip (3000, "Left");
            clip4.AddTrack (spr, left);
            anim.AddClip (clip4);

            anim["Right"].Speed = 3.0f;
            anim["Left"].Speed = 3.0f;
            anim["Down"].Speed = 3.0f;
            anim["Up"].Speed = 3.0f;

            var sm = new StateMachine<MovingState, MovingTrigger> (MovingState.Up);
            sm.Configure (MovingState.Up)
                .Permit (MovingTrigger.Right, MovingState.Right)
                .Permit (MovingTrigger.Down, MovingState.Down)
                .Permit (MovingTrigger.Left, MovingState.Left)
                .Ignore (MovingTrigger.Up)
                .OnEntry (x => clip1.Play ())
                .OnExit (x => clip1.Stop ());
            sm.Configure (MovingState.Right)
                .Permit (MovingTrigger.Up, MovingState.Up)
                .Permit (MovingTrigger.Down, MovingState.Down)
                .Permit (MovingTrigger.Left, MovingState.Left)
                .Ignore (MovingTrigger.Right)
                .OnEntry (x => clip2.Play ())
                .OnExit (x => clip2.Stop ());
            sm.Configure (MovingState.Down)
                .Permit (MovingTrigger.Up, MovingState.Up)
                .Permit (MovingTrigger.Right, MovingState.Right)
                .Permit (MovingTrigger.Left, MovingState.Left)
                .Ignore (MovingTrigger.Down)
                .OnEntry (x => clip3.Play ())
                .OnExit (x => clip3.Stop ());
            sm.Configure (MovingState.Left)
                .Permit (MovingTrigger.Up, MovingState.Up)
                .Permit (MovingTrigger.Right, MovingState.Right)
                .Permit (MovingTrigger.Down, MovingState.Down)
                .Ignore (MovingTrigger.Left)
                .OnEntry (x => clip4.Play ())
                .OnExit (x => clip4.Stop ());

            sm.Fire (MovingTrigger.Down);

            cmp.sm = sm;

            return node;

        }

        public override void OnAttached () {
        }

        private void Move (Vector3 move) {
            var label1 = World.Find ("Label").GetComponent<Label> (0);
            var label2 = World.Find ("Label").GetComponent<Label> (1);

            var result = World.Sweep (Node, move);
            if (result.Hit) {
                label1.Text = "Hit: " + result.Node.Name;
                label2.Text = "Dist = " + result.Distance;
                move *= result.Fraction;
                move *= (result.Distance - 0.9f) / move.Length;
            }
            else {
                label1.Text = "Hit: なし";
            }
            Node.Translate (move.X, move.Y, move.Z);

        }

        public override void OnUpdate (long msec) {

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

            if (dx > 0) {
                sm.Fire (MovingTrigger.Right);
                Move (new Vector3 (dx, 0, 0) * speed);
            }
            if (dx < 0) {
                sm.Fire (MovingTrigger.Left);
                Move (new Vector3 (dx, 0, 0) * speed);
            }
            if (dy > 0) {
                sm.Fire (MovingTrigger.Down);
                Move (new Vector3 (0, dy, 0) * speed);
            }
            if (dy < 0) {
                sm.Fire (MovingTrigger.Up);
                Move (new Vector3 (0, dy, 0) * speed);
            }


        }


    }

}
