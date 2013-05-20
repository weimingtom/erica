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
            g2d.CreateWindow (800, 450, "こんにちは、世界");

            var spr = new Sprite ("media/image32x32.png");
            var node = new Node ();
            node.Attach (spr);
            node.SetBoundingBox (0, 0, spr.Width, spr.Height);
            node.X = 100;
            node.Y = 100;
            
            var script = new Script ("First Script");
            script.Attach (new FPSCounter());
            script.Attach (new Sprite ("media/PhilosophyOfLife.png"));
            script.AddChild (node);
            
            var director = new Director ();
            director.PushScript (script);
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                director.Exit ();
            };


            Console.WriteLine ("Start of Main Loop");

            while (director.IsAlive) {
                director.Update ();
                g2d.Dispatch (director.CurrentScript);
                g2d.Draw (director.CurrentScript);
            }

            Console.WriteLine ("End of Game");
        }
    }

}
