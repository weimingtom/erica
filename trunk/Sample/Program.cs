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

            var node = new Node ();
            var spr = new Sprite (1);
            var anim = new AnimationController ();
            var cmp = new MyComponent (spr);

            node.Attach (spr);
            node.Attach (anim);
            node.Attach (cmp);
            node.Move (0, 32);
            node.SetBoundingBox (0, 0, 800, 450);
            wld.AddChild (node);

            var tex = new TiledTexture ("media/Explosion.png", 4, 8, 30);
            spr.SetTexture (0, tex); 
            
            var clip = new AnimationClip ();
            clip.Duration = 1000;
            clip.WrapMode = WrapMode.Loop;
            clip.Speed = 1;

            var track1 = new AnimationTrack ("X", InterpolationType.Linear);
            track1.AddKeyframe (0, 0);
            track1.AddKeyframe (1000, 400);
            clip.AddTrack (node, track1);
            
            var track2 = new AnimationTrack ("ActiveTile", InterpolationType.Linear);
            track2.AddKeyframe (0, 0);
            track2.AddKeyframe (1000, 29);
            clip.AddTrack (tex, track2);

            anim.AddClip (clip);


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
