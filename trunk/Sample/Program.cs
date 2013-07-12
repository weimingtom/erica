using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample {
    public class Program {

        static void Main2 (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            var p2d = DD.Physics.Physics2D.GetInstance ();
            p2d.CreateWorld (32);
            p2d.Gravity = new DD.Vector2 (0, 9.8f);

            // ----------------------------------------
 
            var wld = MyWorld.Create ();
 
            // ----------------------------------------
            var active = true;

            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                active = false;
            };

            Console.WriteLine ("Start of Main Loop");

            g2d.SetFrameRateLimit (30);

            var watch = new Stopwatch ();
            watch.Start ();

            while (active) {
                var msec = watch.ElapsedMilliseconds;

                p2d.Step (wld, msec);

                wld.Animate (msec, 33);
                wld.Update (msec);

                g2d.Dispatch (wld);
                g2d.Draw (wld);

            }

            Console.WriteLine ("End of Game");
        }
    }
}
