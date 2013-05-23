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
            wld.Attach (new Sprite ("media/PhilosophyOfLife.png"));
            wld.Attach (new FPSCounter ());

            var node = new Node ();
            var player = new LineReader (800, 150);
            player.LoadLine("media/HelloMiku.txt");
            var control = new MyComponent (player);
            node.Attach (player);
            node.Attach (control);
            node.Move (0, 450);
            wld.AddChild (node);

            var director = new Director ();
            director.PushScript (wld);
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
