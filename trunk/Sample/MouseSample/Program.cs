using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.MouseSample {
    public class Program {
        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            var wld = MyWorld.Create(800, 600);
            var node2 = MyLogger.Create (new Vector3 (10, 10, 0));
            var node3 = MySprite.Create (new Vector3 (200, 200, 0));

            wld.AddChild (node2);
            wld.AddChild (node3);
            
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
