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

            var node1 = new Node ();
            node1.Attach (new FPSCounter());

            var node2 = new Node ();
            node2.Attach (new DD.Sprite ("media/PhilosophyOfLife.png"));
            
            var script = new Script ("First Script");
            script.AddChild (node2);
            script.AddChild (node1);
            
            var director = new Director ();
            director.PushScript (script);
            g2d.OnClose += delegate (object sender, EventArgs eventArgs) {
                director.Exit ();
            };


            Console.WriteLine ("Start of Main Loop");

            while (director.IsAlive) {
                director.Update ();
                g2d.Draw (director.CurrentScript);
            }

            Console.WriteLine ("End of Game");
        }
    }

}
