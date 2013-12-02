using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace スニペット.FocusAnimation {
    public class MySprite : Component {
        public MySprite () {

        }

        public static Node Create (Vector3 pos) {
            var cmp = new MySprite ();

            var spr = new Sprite (200,300);
            spr.AutoScale = true;
            spr.AddTexture (new Texture ("FocusAnimation/media/Character-Ako.png"));

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width / 2, spr.Height / 2, 10);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 0);

            var anim = new AnimationController ();
            var clip = new AnimationClip (1000, "FocusAnimation");
            clip.WrapMode = WrapMode.Once;
            var track = new AnimationTrack ("Translation", InterpolationType.Linear);
            track.AddKeyframe (0, pos + new Vector3 (0, 0, 0));
            track.AddKeyframe (1000, pos + new Vector3 (200, 0, 0));
            anim.AddClip (clip);

            
            var node = new Node ();
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (anim);

            clip.AddTrack (node, track);

            node.Translation = pos;

            return node;
        }


        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var clip = Animation["FocusAnimation"];

            switch (button) {
                case MouseButton.Left: {
                        clip.SetPlaybackPoisition (0, Time.CurrentTime);
                        clip.SetSpeed(1, Time.CurrentTime);
                        clip.Play ();
                        break;
                    }
                case MouseButton.Right: {
                        clip.SetPlaybackPoisition (clip.Duration, Time.CurrentTime);
                        clip.SetSpeed (-1, Time.CurrentTime);
                        clip.Play ();
                        break;
                    }
            }

        }

    }

}
