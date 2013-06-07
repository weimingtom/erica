using System;
using System.Collections.Generic;
using System.Linq;
//using SFML.Audio;
//using SFML.Window;
//using SFML.Graphics;
using DD;

namespace Sample {
    static class Program {

        static void Main (string[] args) {

            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            var wld = new World ("First Script");
            wld.Attach (new FPSCounter ());

            //------------

            var cmp = new MyComponent ();

            var spr = new Sprite ();
            spr.AddTexture (new TiledTexture ("media/Explosion.png", 4, 8, 30));
            spr.OffsetX = -spr.ActiveTexture.Width / 2 ;
            spr.OffsetY = -spr.ActiveTexture.Height / 2;
            var tex = (TiledTexture)spr.ActiveTexture;
            tex.ActiveTile = 15;

            //--------------
            //var node1 = new Node ();
            //node1.Translation = new Vector3 (100, 100, 0);

            var node2 = new Node ();
            node2.Attach (cmp);
            node2.Attach (spr);
            //node2.Scale = new Vector3 (0.5f, 0.5f, 0.5f);

            //node.Expand (3, 3, 1);
            node2.Translate (200, 200, 0);
            //node.Rotate (45, 0, 0, 1);
            
            //---------------

            var track = new AnimationTrack ("Rotation", InterpolationType.SLerp);
            track.AddKeyframe (0, new Quaternion (0, 0, 0, 1));
            track.AddKeyframe (500, new Quaternion (180, 0, 0, 1));
            track.AddKeyframe (1000, new Quaternion (360, 0, 0, 1));

            var track2 = new AnimationTrack ("Scale", InterpolationType.Linear);
            track2.AddKeyframe (0, new Vector3 (1, 1, 1));
            track2.AddKeyframe (1000, new Vector3 (5, 5, 5));

            var clip = new AnimationClip ();
            clip.Duration = 1000;
            clip.Speed = 0.5f;
            clip.AddTrack (node2, track);
            clip.AddTrack (node2, track2);

            var track3 = new AnimationTrack ("ActiveTile", InterpolationType.Linear);
            track3.AddKeyframe (0, 0);
            track3.AddKeyframe (1000 * 10, 29);

            var clip2 = new AnimationClip ();
            clip2.Duration = 1000 * 10;
            clip2.AddTrack (tex, track3);
            clip2.Speed = 5;

            var ctr = new AnimationController ();
            ctr.AddClip (clip);
            ctr.AddClip (clip2);
            node2.Attach (ctr);
            
            //---------------------------------

            wld.AddChild (node2);
            //node1.AddChild (node2);
            
            var director = new Director ();
            director.PushScript (wld);
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                director.Exit ();
            };
            
            Console.WriteLine ("Start of Main Loop");

            while (director.IsAlive) {
                director.Animate ();
                director.Update ();
                g2d.Dispatch (director.CurrentScript);
                g2d.Draw (director.CurrentScript);
            }

            Console.WriteLine ("End of Game");
        }
    }

}
