using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.MessageSample {
    public class Program {
        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            var wld = new World ();

            var node1 = MySender.Create ("Recver1", new Vector3 (100, 250, 0));
            var node2 = MyRecver.Create ("Recver1", new Vector3 (600, 150, 0));
            var node3 = MyRecver.Create ("Recver2", new Vector3 (600, 250, 0));
            var node4 = MyRecver.Create ("Recver3", new Vector3 (600, 350, 0));
            var node5 = MyRecver.Create ("All", new Vector3 (600, 500, 0));
            var node6 = MyButton.Create ("1", "Recver1", new Vector3 (50, 350, 0));
            var node7 = MyButton.Create ("2", "Recver2", new Vector3 (100, 350, 0));
            var node8 = MyButton.Create ("3", "Recver3", new Vector3 (150, 350, 0));
            var node9 = MyButton.Create ("All", "All", new Vector3 (200, 350, 0));

            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (node5);
            wld.AddChild (node6);
            wld.AddChild (node7);
            wld.AddChild (node8);
            wld.AddChild (node9);

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
                wld.Deliver ();
                g2d.Draw (wld);

            }

            Console.WriteLine ("End of Game");
        }
    }
}
