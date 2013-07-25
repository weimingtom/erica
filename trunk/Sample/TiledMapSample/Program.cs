﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.TiledMapSample {
    public static class Program {

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

            var node = new Node ("Label");
            node.SetTranslation (10, 32, 0);
            node.Attach (label1);
            node.Attach(label2);
            node.DrawPriority = -2;

            return node;
        }

        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------
           
            var node2 = CreateFPSCounter();
            var node3 = CreateLabel ();
            var node4 = MyTiledMap.Create ("media/desert.tmx");
            var node5 = MyCharacter.Create ();
            var cam1 = MyCamera1.Create ();
            var cam2 = MyCamera2.Create ();

            var mychar = node5.GetComponent<MyCharacter> ();
            mychar.Map = node4.Find ("Ground");
            mychar.CollisionMap = node4.Find ("Collision");

            node4.AddChild (node5);

            // ----------------------------------------

            var wld = new DD.World ("First Script");
            //wld.Attach (new Sprite (new Texture ("media/DarkGalaxy.jpg")));
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (cam2);
            
            node5.AddChild (cam1);
            wld.ActiveCamera = cam1;

            var alive = true;
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                alive = false;
            };

            Console.WriteLine ("Start of Main Loop");

            g2d.SetFrameRateLimit (30);

            var watch = new Stopwatch ();
            watch.Start ();

            while (alive) {
                var msec = watch.ElapsedMilliseconds;

                wld.Properties ["Pass"] = 1;
                wld.ActiveCamera = cam1;

                wld.Animate (msec, 0);
                wld.Update (msec);
                g2d.Dispatch (wld);
                g2d.Draw (wld, null, false);

                wld.Properties ["Pass"] = 2;
                wld.ActiveCamera = cam2;
                g2d.Draw (wld, null, true);
            }

            Console.WriteLine ("End of Game");
        }
    }

}

