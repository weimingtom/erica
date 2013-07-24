using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.ScrollSample {
    public class Program {

        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

           // ----------------------------------------
 
            var wld = MyWorld.Create ();
            var node1 = MyCharacter.Create ();
            var node3 = MyHUD.Create ();
            var cam1 = MyCamera1.Create ();
            var cam2 = MyCamera2.Create ();

            wld.AddChild (node1);
            node1.AddChild (cam1);
            cam1.AddChild (node3);
            wld.AddChild (cam2);

            wld.ActiveCamera = cam1;
            
            // ----------------------------------------
            var active = true;

            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                active = false;
            };

            Console.WriteLine ("Start of Main Loop");

            g2d.SetFrameRateLimit (60);

            var myargs = new MyDrawArgs ();
            var watch = new Stopwatch ();
            watch.Start ();

            while (active) {
                var msec = watch.ElapsedMilliseconds;

                wld.ActiveCamera = cam1;
               

                
                myargs.RenderPass = 0;
                g2d.Draw (wld, myargs, false);

                // Draw() の後でないと view が切り替わらない
                wld.Animate (msec, 33);
                wld.Update (msec);
                g2d.Dispatch (wld);

                wld.ActiveCamera = cam2;

                myargs.RenderPass = 1;
                g2d.Draw (wld, myargs, true);

            }

            Console.WriteLine ("End of Game");
        }
    }
}
