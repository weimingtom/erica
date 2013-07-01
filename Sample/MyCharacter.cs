using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stateless;

namespace DD.Sample {
    public class MyCharacterComponent : Component {
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

        StateMachine<State, Trigger> sm;

        public MyCharacterComponent () {
        }

        public void Init () {
            var spr = GetComponent<Sprite> ();

            var track1 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track1.AddKeyframe (0, new Vector2 (0, 64));
            track1.AddKeyframe (300, new Vector2 (24, 64));
            track1.AddKeyframe (600, new Vector2 (48, 64));
            var clip1 = new AnimationClip (900, "MyCharacter.Down");
            clip1.AddTrack (spr, track1);

            var track2 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track2.AddKeyframe (0, new Vector2 (0, 0));
            track2.AddKeyframe (300, new Vector2 (24, 0));
            track2.AddKeyframe (600, new Vector2 (48, 0));
            var clip2 = new AnimationClip (900, "MyCharacter.Up");
            clip2.AddTrack (spr, track2);

            var track3 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track3.AddKeyframe (0, new Vector2 (0, 32));
            track3.AddKeyframe (300, new Vector2 (24, 32));
            track3.AddKeyframe (600, new Vector2 (48, 32));
            var clip3 = new AnimationClip (900, "MyCharacter.Right");
            clip3.AddTrack (spr, track3);

            var track4 = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track4.AddKeyframe (0, new Vector2 (0, 96));
            track4.AddKeyframe (300, new Vector2 (24, 96));
            track4.AddKeyframe (600, new Vector2 (48, 96));
            var clip4 = new AnimationClip (900, "MyCharacter.Left");
            clip4.AddTrack (spr, track4);

            Animation.AddClip (clip1);
            Animation.AddClip (clip2);
            Animation.AddClip (clip3);
            Animation.AddClip (clip4);
            
            
            sm = new StateMachine<State, Trigger> (State.Down);
            clip1.Play ();

            sm.Configure (State.Down)
                .Ignore (Trigger.Down)
                .Permit (Trigger.Up, State.Up)
                .Permit (Trigger.Right, State.Right)
                .Permit (Trigger.Left, State.Left)
                .OnExit (x => clip1.Stop ())
                .OnEntry (x => clip1.Play ());

            sm.Configure (State.Up)
                .Permit (Trigger.Down, State.Down)
                .Ignore (Trigger.Up)
                .Permit (Trigger.Right, State.Right)
                .Permit (Trigger.Left, State.Left)
                .OnExit (x => clip2.Stop ())
                .OnEntry (x => clip2.Play ());

            sm.Configure (State.Right)
                .Permit (Trigger.Down, State.Down)
                .Permit (Trigger.Up, State.Up)
                .Ignore (Trigger.Right)
                .Permit (Trigger.Left, State.Left)
                .OnExit (x => clip3.Stop ())
                .OnEntry (x => clip3.Play ());

            sm.Configure (State.Left)
                .Permit (Trigger.Down, State.Down)
                .Permit (Trigger.Up, State.Up)
                .Permit (Trigger.Right, State.Right)
                .Ignore (Trigger.Left)
                .OnExit (x => clip4.Stop ())
                .OnEntry (x => clip4.Play ());
        }

        bool onlyOnce;

        public override void OnUpdate (long msec) {
            if (!onlyOnce) {
                Init ();
                onlyOnce = true;
            }
            var label = World.Find (n => n.Name == "Label").GetComponent<Label>();
            if (label != null) {
                label.Text = "Keys = " + String.Join(", ", Input.Keys);
            }

            var x = 0;
            var y = 0;

            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.DownArrow : { y = y + 2; sm.Fire (Trigger.Down) ; break; }
                    case KeyCode.UpArrow   : { y = y - 2; sm.Fire (Trigger.Up)   ; break; }
                    case KeyCode.RightArrow: { x = x + 2; sm.Fire (Trigger.Right); break; }
                    case KeyCode.LeftArrow : { x = x - 2; sm.Fire (Trigger.Left) ; break; }
                }
            }
            var pos = Node.Translation;
            var v = new Vector3 (x, y, 0);
            if (v.Length2 > 0) {
                pos += v.Normalize () * 4;

            }

        }
    }
}
