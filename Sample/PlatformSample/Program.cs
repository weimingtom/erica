using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.PlatformSample {
    public class Program {

        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            var node0 = MyBlock.Create (new Vector3(100, 400, 0));
            var node1 = MyFloor.Create ();
            var node2 = MyCharacter.Create (new Vector3 (100, 100, 0));
            var node3 = MyLabel.Create (new Vector3 (200, 50, 0));
            var node4 = MyFpsCounter.Create (new Vector3 (10, 10, 0));

            // ----------------------------------------

            var wld = new DD.World ("First Script");
            wld.AddChild (node0);
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);

            wld.Attach (new MyFloor ());

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
