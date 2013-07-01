using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample {
    class Program {
        private static Node CreateFPSCounter () {
            var node = new Node ();
            node.Attach (new FPSCounter ());
            return node;
        }

        private static Node CreateMyCharacter () {
            var spr = new Sprite (new Texture ("media/Character-Gelato.png"), 24, 32);

            var cmp = new MyCharacterComponent ();
            var node = new Node ();
            node.Attach (spr);
            node.Attach (cmp);
            node.SetTranslation (100, 100, 0);

            return node;
        }

        private static Node CreateLabels () {
            var label = new Label ();
            var node = new Node ("Label");

            node.Attach (label);
            node.SetTranslation (50, 100, 0);

            return node;
        }

        private static Node CreateWalls () {
            var spr = new Sprite (new Texture ("media/Rectangle-160x40.png"));
            var node = new Node ();
            node.Attach (spr);
            node.SetTranslation (200, 150, 0);
            node.SetBoundingBox (0, 0, spr.Width, spr.Height);

            return node;
        }

        static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            var node1 = CreateFPSCounter ();
            var node2 = CreateMyCharacter ();
            var node3 = CreateLabels ();
            var node4 = CreateWalls ();



            // ----------------------------------------

            var wld = new DD.World ("First Script");
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);

            var director = new Director ();
            director.PushScript (wld);
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                director.Exit ();
            };

            Console.WriteLine ("Start of Main Loop");

            g2d.SetFrameRateLimit (30);

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
