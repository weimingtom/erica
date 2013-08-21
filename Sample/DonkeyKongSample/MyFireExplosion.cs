﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DonkeyKongSample {
    public class MyFireExplosion : Component{
        int count;

        public MyFireExplosion () {
            this.count = 0;
        }

        public static Node Create(Vector3 pos){
            var cmp = new MyFireExplosion ();

            var spr = new Sprite (100, 100);
            spr.AddTexture(Resource.GetTexture ("media/FireExplosion.png"));
            
            var node = new Node ("FireExplosion");
            node.Attach (spr);
            node.Attach (cmp);

            node.Translation = pos - new Vector3(34, 34, 0);   // 爆発の中心を地球にあわせる

            var track = new AnimationTrack ("TextureOffset", InterpolationType.Step);
            track.AddKeyframe (0, new Vector2 (0, 0));
            track.AddKeyframe (100, new Vector2 (100, 0));
            track.AddKeyframe (200, new Vector2 (200, 0));
            track.AddKeyframe (300, new Vector2 (300, 0));
            track.AddKeyframe (400, new Vector2 (400, 0));
            track.AddKeyframe (500, new Vector2 (500, 0));
            track.AddKeyframe (600, new Vector2 (600, 0));
            track.AddKeyframe (700, new Vector2 (700, 0));
            track.AddKeyframe (800, new Vector2 (800, 0));
            track.AddKeyframe (900, new Vector2 (900, 0));
            track.AddKeyframe (1000, new Vector2 (0, 100));
            track.AddKeyframe (1100, new Vector2 (100, 100));
            track.AddKeyframe (1200, new Vector2 (200, 100));
            track.AddKeyframe (1300, new Vector2 (300, 100));
            track.AddKeyframe (1400, new Vector2 (400, 100));
            track.AddKeyframe (1500, new Vector2 (500, 100));
            track.AddKeyframe (1600, new Vector2 (600, 100));
            track.AddKeyframe (1700, new Vector2 (700, 100));
            track.AddKeyframe (1800, new Vector2 (800, 100));
            track.AddKeyframe (1900, new Vector2 (900, 100));
            track.AddKeyframe (2000, new Vector2 (0, 200));
            track.AddKeyframe (2100, new Vector2 (100, 200));
            track.AddKeyframe (2200, new Vector2 (200, 200));
            track.AddKeyframe (2300, new Vector2 (300, 200));
            track.AddKeyframe (2400, new Vector2 (400, 200));
            track.AddKeyframe (2500, new Vector2 (500, 200));
            track.AddKeyframe (2600, new Vector2 (600, 200));
            track.AddKeyframe (2700, new Vector2 (700, 200));
            track.AddKeyframe (2800, new Vector2 (800, 200));
            track.AddKeyframe (2900, new Vector2 (900, 200));

            var clip = new AnimationClip (3000, "Explosion");
            clip.AddTrack (spr, track);
            clip.WrapMode = WrapMode.Once;

            node.UserData.Add (clip.Name, clip);

            return node;
        }

        public override void OnUpdateInit (long msec) {
            var clip = Node.UserData["Explosion"] as AnimationClip;
            clip.SetPlaybackPoisition (0, msec);
            clip.Play ();
            Animation.AddClip (clip);
        }

        public override void OnUpdate (long msec) {
            count += 1;
            if (count > 90) {
                Destroy (Node);
            }
        }
    }
}
