using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample {
    public class MyFireExplosion : Component{
        AnimationClip clip;
        bool onlyOnce;
        int count;

        public static Node CreateInstance(){
            var spr = new Sprite (Resource.GetTexture ("media/FireExplosion.png"), 32, 32);
            
            var track = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            for (var i = 0; i < 30; i++) {
                track.AddKeyframe (1000*i/30, new Vector2 (32*i, 0));
            }

            var comp = new MyFireExplosion ();
            comp.clip = new AnimationClip (1000, "Explosion");
            comp.clip.AddTrack (spr, track);
            

            var node = new Node ("FireExplosion");
            node.Attach (spr);
            node.Attach (comp);

            return node;
        }

        public override void OnUpdate (long msec) {
            if (!onlyOnce) {
                Animation.AddClip (clip);
                clip.Play ();
                this.onlyOnce = true;
            }

            count += 1;
            if (count > 30) {
                Destroy (Node);
            }
        }
    }
}
