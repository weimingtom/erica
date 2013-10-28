using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.DatabaseSample {
    public class Program2 {
        
        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");


            Resource.SetTextureDirectory ("DatabaseSample/Textures/");

            // ----------------------------------------
            var node1 = MyCharacterHolder.Create ();
            var node2 = MyCharacterViewer.Create (new Vector3 (0, 0, 0));
            var node3 = MyCharacterSelector.Create (new Vector3 (0,0,0));
        
            var wld = new World ();
            wld.AddChild (node1);
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

                g2d.Dispatch (wld);
                wld.Animate (msec, 33);
                wld.Deliver ();
                wld.CollisionUpdate ();
                wld.Update (msec);
                wld.Purge ();
                g2d.Draw (wld);
            }

            Console.WriteLine ("End of Game");
            wld.Destroy ();
        }
    }
}
