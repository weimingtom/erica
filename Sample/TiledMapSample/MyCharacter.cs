using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;
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

        public float Speed { get; set; }
        public Node Map { get; set; }
        public Node CollisionMap { get; set; }
        public Sprite Sprite { get; set; }
        public StateMachine<MovingState, MovingTrigger> StateMachine {get; set;}

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
            node.Translate (150, 120, 0);

            node.GroupID = 0xffffffffu;

            cmp.Sprite = spr;

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
                .OnEntry (x => clip1.Play())
                .OnExit (x => clip1.Stop());
            sm.Configure (MovingState.Right)
                .Permit (MovingTrigger.Up, MovingState.Up)
                .Permit (MovingTrigger.Down, MovingState.Down)
                .Permit (MovingTrigger.Left, MovingState.Left)
                .Ignore (MovingTrigger.Right)
                .OnEntry (x => clip2.Play())
                .OnExit (x => clip2.Stop());
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

            cmp.StateMachine = sm;

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
                StateMachine.Fire (MovingTrigger.Right);
                Move (new Vector3 (dx, 0, 0) * Speed);
            }
            if (dx < 0) {
                StateMachine.Fire (MovingTrigger.Left);
                Move (new Vector3 (dx, 0, 0) * Speed);
            }
            if (dy > 0) {
                StateMachine.Fire (MovingTrigger.Down);
                Move (new Vector3 (0, dy, 0) * Speed);
            }
            if (dy < 0) {
                StateMachine.Fire (MovingTrigger.Up);
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
