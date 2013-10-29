using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.SimpleSample {
    public class Program {
        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            var wld = new World ();

            var node1 = MyComponent.Create (new Vector3(100,100,0));
            wld.AddChild (node1);

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

                g2d.Dispatch (wld);
                wld.Animate (msec, 16);
                wld.CollisionUpdate ();
                wld.Update (msec);
                g2d.Draw (wld);
            }

            Console.WriteLine ("End of Game");
            wld.Destroy ();
        }
    }
}
