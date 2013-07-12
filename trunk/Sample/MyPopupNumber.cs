using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample {
    public class MyPopupNumber : Component{

        public MyPopupNumber () {
        }

        public Label Label {
            get;
            set;
        }

        public override void OnAttached () {
            var label = new Label ("こんにちは、世界");
            Node.Attach (label);
        }

        public override void OnUpdateInit (long msec) {
            var snd = new SoundClip ("media/PinPon.wav");
            snd.Volume = 0.2f;

            var track = new AnimationTrack ("Translation", InterpolationType.Linear);
            var pos = Node.Translation;
            track.AddKeyframe (0, new Vector3 (pos.X, pos.Y, 0));
            track.AddKeyframe (3000, new Vector3 (pos.X, pos.Y-100, 0));

            var clip = new AnimationClip (3000);
            clip.Play ();
            clip.WrapMode = WrapMode.Loop;
            clip.SetPlaybackPoisition (0, msec);

            clip.AddTrack (Node, track);
            clip.AddEvent (10, (sender, args) => snd.Play (), null);
            clip.AddEvent (2900, (sender, args) => {
                Animation.RemoveClip (clip);
                Destroy (this);
            }, null);

            Animation.AddClip (clip);
        }

        public override void OnDestroyed () {
            Console.WriteLine ("Destroyed");
        }


        public override void OnUpdate (long msec) {

        }
    }
}
