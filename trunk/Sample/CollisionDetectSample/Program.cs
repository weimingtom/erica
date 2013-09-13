using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.CollisionDetectSample {
    public class Program {
        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            var wld = new World ();

            var node1 = MyCharacter.Create (new Vector3 (200, 200, 0), 16);    // GroupID = 1
            var node2 = MyBlock.Create (new Vector3 (100, 300, 0), -1);        // CollideWith = All
            var node3 = MyBlock.Create (new Vector3 (300, 300, 0), 16);        // CollideWith = 1
            var node4 = MyBlock.Create (new Vector3 (500, 300, 0), 0);         // CollideWith = None
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);

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
                wld.Animate (msec, 33);
                wld.Analyze ();
                wld.Update (msec);
                g2d.Draw (wld);
            }

            wld.Destroy ();
            Console.WriteLine ("End of Game");
        }
    }
}
