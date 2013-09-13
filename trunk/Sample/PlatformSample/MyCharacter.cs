using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;


namespace DD.Sample.PlatformSample {
    public class MyCharacter : Component {
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
            Left
        }

        float speed = 10;
        float gravity = 1;
        float jumpingVelocity = 0;
        bool isGrounded = false;
        StateMachine<State, Trigger> sm = new StateMachine<State, Trigger> (State.Down);

        public MyCharacter () {
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyCharacter ();

            // Body
            var spr1 = new Sprite (24, 32);
            spr1.AddTexture (new Texture ("media/Character-Gelato.png"));

            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (spr1.Width / 2, spr1.Height / 2, 1);
            col1.Offset = new Vector3 (spr1.Width / 2, spr1.Height / 2, 1);
            col1.IgnoreWith = (int)MyGroup.Character;

            // Foot
            var spr2 = new Sprite (24, 4);
            spr2.AddTexture (new Texture ("media/image128x128(Red).png"));

            var col2 = new CollisionObject ();
            col2.Shape = new BoxShape (2,2, 2);
            spr2.Offset = new Vector2 (0, spr1.Height + 2);
            col2.Offset = new Vector3 (spr1.Width / 2, spr1.Height + 2 + 4/2, 1);
            col2.IgnoreWith = (int)MyGroup.Character;

            var node = new Node ("MyCharacter");
            node.Attach (cmp);
            node.Attach (spr1);
            node.Attach (col1);
            node.Attach (spr2);
            node.Attach (col2);

            node.Translation = pos;
            node.DrawPriority = -1;
            node.GroupID = (int)MyGroup.Character;

            // アニメーションの設定
            var track1 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track1.AddKeyframe (0, new Vector2 (0, 64));
            track1.AddKeyframe (300, new Vector2 (24, 64));
            track1.AddKeyframe (600, new Vector2 (48, 64));
            var clip1 = new AnimationClip (900, "MyCharacter.Down");
            clip1.AddTrack (spr1, track1);

            var track2 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track2.AddKeyframe (0, new Vector2 (0, 0));
            track2.AddKeyframe (300, new Vector2 (24, 0));
            track2.AddKeyframe (600, new Vector2 (48, 0));
            var clip2 = new AnimationClip (900, "MyCharacter.Up");
            clip2.AddTrack (spr1, track2);

            var track3 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track3.AddKeyframe (0, new Vector2 (0, 32));
            track3.AddKeyframe (300, new Vector2 (24, 32));
            track3.AddKeyframe (600, new Vector2 (48, 32));
            var clip3 = new AnimationClip (900, "MyCharacter.Right");
            clip3.AddTrack (spr1, track3);

            var track4 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track4.AddKeyframe (0, new Vector2 (0, 96));
            track4.AddKeyframe (300, new Vector2 (24, 96));
            track4.AddKeyframe (600, new Vector2 (48, 96));
            var clip4 = new AnimationClip (900, "MyCharacter.Left");
            clip4.AddTrack (spr1, track4);

            node.UserData.Add (clip1.Name, clip1);
            node.UserData.Add (clip2.Name, clip2);
            node.UserData.Add (clip3.Name, clip3);
            node.UserData.Add (clip4.Name, clip4);

            // ステート管理
            cmp.sm.Configure (State.Down)
                    .Ignore (Trigger.Down)
                    .Permit (Trigger.Up, State.Up)
                    .Permit (Trigger.Right, State.Right)
                    .Permit (Trigger.Left, State.Left)
                    .OnExit (x => clip1.Stop ())
                    .OnEntry (x => clip1.Play ());

            cmp.sm.Configure (State.Up)
                    .Permit (Trigger.Down, State.Down)
                    .Ignore (Trigger.Up)
                    .Permit (Trigger.Right, State.Right)
                    .Permit (Trigger.Left, State.Left)
                    .OnExit (x => clip2.Stop ())
                    .OnEntry (x => clip2.Play ());

            cmp.sm.Configure (State.Right)
                    .Permit (Trigger.Down, State.Down)
                    .Permit (Trigger.Up, State.Up)
                    .Ignore (Trigger.Right)
                    .Permit (Trigger.Left, State.Left)
                    .OnExit (x => clip3.Stop ())
                    .OnEntry (x => clip3.Play ());

            cmp.sm.Configure (State.Left)
                    .Permit (Trigger.Down, State.Down)
                    .Permit (Trigger.Up, State.Up)
                    .Permit (Trigger.Right, State.Right)
                    .Ignore (Trigger.Left)
                    .OnExit (x => clip4.Stop ())
                    .OnEntry (x => clip4.Play ());

            return node;
        }

        public override void OnUpdateInit (long msec) {
            var clip = Node.UserData["MyCharacter.Down"] as AnimationClip;
            clip.Play ();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// <note>
        /// <see cref="World.Sweep"/> で使用されるコリジョンは最初のもの(Body)だけ。
        /// Footは使用されない。
        /// </note>
        /// </remarks>
        /// <param name="move"></param>
        void Move (Vector3 move) {

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

            var body = GetComponent<CollisionObject> (0);
            var foot = GetComponent<CollisionObject> (1);

            var x = 0;

            // 着地
            if (foot.OverlapCount > 0) {
                this.isGrounded = true;
                this.jumpingVelocity = 0;
            }
            else {
                this.isGrounded = false;
            }

            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.RightArrow: x += 1; sm.Fire (Trigger.Right); break;
                    case KeyCode.LeftArrow: x -= 1; sm.Fire (Trigger.Left); break;
                    case KeyCode.Space: {
                        // ジャンプ
                        this.isGrounded = false;
                        this.jumpingVelocity = -10;
                        break;
                    }
                }
            }
            if (Input.Shift) {
                x *= 3;
            }

            // 床面に沿った移動
            if (x != 0) {
                var move = new Vector3 (x, 0, 0) * speed;
                var node = foot.Overlaps.FirstOrDefault ();
                if (node != null) {
                    Console.WriteLine ("壁面 = " + node);
                    move = node.GlobalTransform.ApplyDirection (move);
                }
                Move (move);
            }

            // ジャンプ中の落下
            if (!isGrounded) {
                var move = new Vector3 (0, jumpingVelocity, 0);
                Move (move);
                this.jumpingVelocity += gravity;
            }

        }
    }
}
