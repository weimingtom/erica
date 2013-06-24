using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample {
    public static class Program {

        static Node CreateBar () {

            var spr = new Sprite (new Texture("media/Rectangle-160x40.png"));
            var cmp = new MyComponent ();
            
            var node = new Node ();
            node.Attach (spr);
            node.Attach (cmp);
            node.Translate (320, 500, 0);

            return node;
        }

        private static Node CreateFPSCounter () {
            var node = new Node ();
            node.Attach (new FPSCounter ());
            return node;
        }

        private static Node CreateLabel () {
            var label1 = new Label ("");
            var label2 = new Label ("");
            label1.SetOffset (0, 0);
            label2.SetOffset (0, 16);

            var node = new Node ();
            node.UserID = 1;
            node.SetTranslation (10, 32, 0);
            node.Attach (label1);
            node.Attach(label2);

            return node;
        }

        static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            var node1 = CreateBar ();
            var node2 = CreateFPSCounter();
            var node3 = CreateLabel ();

            // ----------------------------------------

            var wld = new DD.World ("First Script");
            wld.Attach (new Sprite (new Texture ("media/DarkGalaxy.jpg")));
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);

            var director = new Director ();
            director.PushScript (wld);
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                director.Exit ();
            };

            Console.WriteLine ("Start of Main Loop");

            //g2d.SetFrameRateLimit (30);

            var watch = new Stopwatch ();
            watch.Start ();

            while (director.IsAlive) {
                var msec = watch.ElapsedMilliseconds;

                director.Animate (msec);
                director.Update (msec);
                g2d.Dispatch (director.CurrentScript);
                g2d.Draw (director.CurrentScript);

            }

            Console.WriteLine ("End of Game");
        }
    }

}

