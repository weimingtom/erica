using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    public class MyController : Component {
        MyTank tank;
        Node targetMarker;
        SoundClip noise;

        public Node TargetMarker { get { return targetMarker; } set { this.targetMarker = value; } }

        public MyController (MyTank tank) {
            this.tank = tank;
            this.TargetMarker = null;
            this.noise = new SoundClip("media/Sound-LoopNoise.wav");
            noise.Loop = true;
        }

        public void MoveForward () {
            var forward = Node.GlobalTransform.ApplyDirection (0, -1, 0);
            var target = (targetMarker.Position - Node.Position);
            var angle = Vector3.Angle (forward, target);
            var cross = Vector3.Cross (forward, target);
            var rot = new Quaternion (MyMath.Clamp (angle, 0, tank.AngularSpeed), cross.X, cross.Y, cross.Z);

            var v = Node.Transform.ApplyDirection (0, -1, 0) * tank.ForwardSpeed;

            if (target.Length < 20) {
                noise.Stop ();
                Destroy (TargetMarker);
                this.TargetMarker = null;
            }
            else {
                if (!noise.IsPlaying) {
                    noise.Play ();
                }
                Node.Rotate (rot);
                Node.Translate (v.X, v.Y, v.Z);
            }


        }

        public void MoveBack () {
            var forward = Node.GlobalTransform.ApplyDirection (0, 1, 0);
            var target = (targetMarker.Position - Node.Position);
            var angle = Vector3.Angle (forward, target);
            var cross = Vector3.Cross (forward, target);
            var rot = new Quaternion (MyMath.Clamp (angle, 0, tank.AngularSpeed), cross.X, cross.Y, cross.Z);

            var v = Node.Transform.ApplyDirection (0, 1, 0) * tank.ForwardSpeed;

            if (target.Length < 20) {
                noise.Stop ();
                Destroy (targetMarker);
                this.targetMarker = null;
            }
            else {
                if (!noise.IsPlaying) {
                    noise.Play ();
                }
                Node.Rotate (rot);
                Node.Translate (v.X, v.Y, v.Z);
            }

        }

        public override void OnUpdateInit (long msec) {
            Sound.AddClip (noise);
        }

        public override void OnUpdate (long msec) {

            if (TargetMarker != null) {
                var marker = targetMarker.GetComponent<MyMarker> ();
                switch (marker.Direction) {
                    case MyDirection.Forward: MoveForward (); break;
                    case MyDirection.Back: MoveBack (); break;
                }
            }
        }

    }
}
