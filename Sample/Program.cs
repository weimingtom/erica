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
            spr.AddTexture (new Texture ("media/Arrow.png"));
            spr.OffsetX = -spr.ActiveTexture.Width / 2 ;
            spr.OffsetY = -spr.ActiveTexture.Height / 2;

            //--------------
            var node1 = new Node ();
            node1.Translation = new Vector3 (100, 100, 0);

            var node2 = new Node ();
            node2.Attach (cmp);
            node2.Attach (spr);

            //node.Expand (3, 3, 1);
            node2.Move (100, 100, 0);
            node2.Expand (2, 2, 1);
            //node.Rotate (45, 0, 0, 1);
            
            //---------------

            var track = new AnimationTrack ("Rotation", InterpolationType.SLerp);
            track.AddKeyframe (0, new Quaternion (0, 0, 0, 1));
            track.AddKeyframe (1000 * 10, new Quaternion (350, 0, 0, 1));
            var clip = new AnimationClip ();
            clip.Duration = 1000 * 10;
            clip.Speed = 4.0f;
            clip.AddTrack (node2, track);

            var ctr = new AnimationController ();
            ctr.AddClip (clip);
            node2.Attach (ctr);
            
            //---------------------------------

            wld.AddChild (node1);
            node1.AddChild (node2);
            
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
