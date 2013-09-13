using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.IsometricSample {
    public class Program {
        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            //var node1 = MyTiledMap.Create ("media/GlassLand.tmx");
            var node1 = MyTiledMap.Create ("media/GlassWater.tmx");
            var node2 = MyCharacter.Create ();
            var node3 = MyCamera.Create ();
            var node4 = MyLabels.Create ();

            // ----------------------------------------

            var wld = MyWorld.Create ();
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.ActiveCamera = node3;

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

                wld.Animate (msec, 0);
                wld.Analyze ();
                wld.Update (msec);
                g2d.Dispatch (wld);
                g2d.Draw (wld);

            }

            wld.Destroy ();
            Console.WriteLine ("End of Game");

        }
    }
}
