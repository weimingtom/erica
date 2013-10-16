﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using DD.Physics;

namespace DD.Sample.SimpleSample {
    public class MyComponent : Component {

        public MyComponent () {
        }

        public static Node Create (Vector3 pos) {

            var cmp = new MyComponent ();

            var clip = new SoundClip ("Sound");
            clip.AddTrack (new MusicTrack ("media/BGM(Field04).ogg"));
            clip.AddTrack (new SoundEffectTrack ("media/PinPon.wav"));

            var spr = new Sprite (64, 64);
            spr.Color = Color.Red;

            var label = new Label ();
            label.Text = "ここにはA子のキャラクター紹介文が入ります。";

            var col = new CollisionObject();
            col.Shape  = new BoxShape(32, 32, 32);
            col.SetOffset (32, 32, 0);

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (label);
            
            node.UserData.Add (clip.Name, clip);

            node.Translation = pos;

            return node;
        }

        public override void OnUpdateInit (long msec) {
            var clip = Node.UserData["Sound"] as SoundClip;
            clip.Play ();
        }


        public override void OnUpdate (long msec) {
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var clip = Node.UserData["Sound"] as SoundClip;
            clip.Play ();
        }
    
    
    }
}
