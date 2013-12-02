using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;
using System.Diagnostics;

namespace スニペット.FocusAnimation {
    public class Program {

        public static void Main (string[] args) {
            var g2d = Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            //--------------------------------------
            var wld = new World ();

            var node1 = MySprite.Create(new Vector3(100,100,0));

            wld.AddChild (node1);

            //--------------------------------------

            var timer = new Stopwatch ();
            timer.Start ();

            var active = true;
            g2d.OnClosed +=delegate(object sender, EventArgs ea){
                active = false;
            };

            g2d.SetFrameRateLimit (30);

            while(active){
                var msec = timer.ElapsedMilliseconds;

                g2d.Dispatch (wld);

                wld.Deliver ();
                wld.Animate (msec, 33);
                wld.CollisionUpdate ();
                wld.Update (msec);
                wld.Purge ();
                g2d.Draw (wld);
            }

            wld.Destroy();
            Console.WriteLine ("End of Game");
        }
    }
}
