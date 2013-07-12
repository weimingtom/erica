using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.TiledMap {
    public static class Program {

         private static Node CreateFPSCounter () {
            var node = new Node ();
            node.Attach (new FPSCounter ());
            return node;
        }

        private static Node CreateTiledMap () {
            var cmp = new TiledMapComposer ();
            var node = new Node ();
            node.Attach (cmp);

            cmp.LoadFromFile ("media/desert.tmx");

            return node;
        }

        private static Node CreateMyCharacter () {

            var cmp = new MyComponent ();

            var spr = new Sprite (new Texture("media/Earth.png"));

            var node = new Node ();
            node.DrawPriority = -1;

            node.Attach (cmp);
            node.Attach (spr);

            return node;
        }

        private static Node CreateLabel () {
            var label1 = new Label ("");
            var label2 = new Label ("");
            label1.SetOffset (0, 0);
            label2.SetOffset (0, 16);

            var node = new Node ("Label");
            node.SetTranslation (10, 32, 0);
            node.Attach (label1);
            node.Attach(label2);

            return node;
        }

        static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------
                  
            var node2 = CreateFPSCounter();
            var node3 = CreateLabel ();
            var node4 = CreateTiledMap ();
            var node5 = CreateMyCharacter ();

            node4.AddChild (node5);

            var my = node5.GetComponent<MyComponent> ();
            var map = node4.Find (x => x.Name == "Ground").GetComponent<GridBoard> ();
            var coll = node4.Find (x => x.Name == "Collision").GetComponent<GridBoard> ();
            my.Map = map;
            my.Collision = coll;


            // ----------------------------------------

            var wld = new DD.World ("First Script");
            //wld.Attach (new Sprite (new Texture ("media/DarkGalaxy.jpg")));
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);


            var alive = true;
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                alive = false;
            };

            Console.WriteLine ("Start of Main Loop");

            //g2d.SetFrameRateLimit (30);

            var watch = new Stopwatch ();
            watch.Start ();

            while (alive) {
                var msec = watch.ElapsedMilliseconds;

                wld.Animate (msec, 0);
                wld.Update (msec);
                g2d.Dispatch (wld);
                g2d.Draw (wld);

            }

            Console.WriteLine ("End of Game");
        }
    }

}

