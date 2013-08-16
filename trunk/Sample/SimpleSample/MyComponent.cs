using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.SimpleSample {
    public class MyComponent : Component {
        AnimationClip clip;

        public MyComponent () {
        }

        public static Node Create () {

            var cmp = new MyComponent ();

            var spr = new Sprite (256, 128);
            spr.AddTexture (Resource.GetTexture ("media/BigExplosion.png"));
            spr.SetTextureOffset (0, 0);
            //spr.SetOffset (-128, -64);

            var track = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track.AddKeyframe (0, new Vector2 (0, 0));
            track.AddKeyframe (200, new Vector2 (256, 0));
            track.AddKeyframe (400, new Vector2 (512, 0));
            track.AddKeyframe (600, new Vector2 (0, 128));
            track.AddKeyframe (800, new Vector2 (256, 128));
            track.AddKeyframe (1000, new Vector2 (512, 128));
            track.AddKeyframe (1200, new Vector2 (0, 256));
            track.AddKeyframe (1400, new Vector2 (256, 256));
            track.AddKeyframe (1600, new Vector2 (512, 256));
            track.AddKeyframe (1800, new Vector2 (0, 384));
            track.AddKeyframe (2000, new Vector2 (256, 384));
            track.AddKeyframe (2200, new Vector2 (512, 384));

            cmp.clip = new AnimationClip (2400, "Fire-Animation");
            cmp.clip.AddTrack (spr, track);

            var sqr = new Square (256, 128, 10);
            sqr.LineWidth = 10;

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (sqr);

            return node;
        }

        public override void OnUpdateInit (long msec) {
            clip.WrapMode = WrapMode.Once;
            clip.Play ();
            clip.Speed = 0.5f;

            Animation.AddClip (clip);

            Node.Translate (100, 100, 0);
            Node.Rotate (30, 0, 0, 1);
        }

        public override void OnUpdate (long msec) {
        }
    }
}
