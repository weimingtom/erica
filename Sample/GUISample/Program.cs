﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.GUISample {
    public class Program {
        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            var wld = MyWorld.Create ();
            var node1 = MyCamera.Create ();
            var node2 = MyMouseSelector.Create ();
            var node3 = MyButton.Create (new Vector3 (400, 200, 0));
            var node4 = MyTarget.Create ("Man-NE", "media/isometric-man-ne.png", new Vector3 (200, 100, 0));
            var node5 = MyTarget.Create ("Man-NW", "media/isometric-man-nw.png", new Vector3 (280, 100, 0));
            var node6 = MyTarget.Create ("Man-SE", "media/isometric-man-se.png", new Vector3 (200, 244, 0));
            var node7 = MyTarget.Create ("Man-SW", "media/isometric-man-sw.png", new Vector3 (280, 244, 0));



            wld.ActiveCamera = node1;
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (node5);
            wld.AddChild (node6);
            wld.AddChild (node7);

            // ----------------------------------------
            var active = true;

            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                active = false;
            };

            Console.WriteLine ("Start of Main Loop");

            g2d.SetFrameRateLimit (60);

            var watch = new Stopwatch ();
            watch.Start ();

            while (active) {
                var msec = watch.ElapsedMilliseconds;

                // Draw() の後でないと view が切り替わらない
                wld.Animate (msec, 33);
                wld.Deliver ();
                wld.CollisionUpdate ();
                wld.Update (msec);
                g2d.Dispatch (wld);

                g2d.Draw (wld);
            }

            Console.WriteLine ("End of Game");
            wld.Destroy ();
        }
    }

}
