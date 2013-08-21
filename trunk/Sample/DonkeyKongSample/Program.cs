using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DD.Physics;

namespace DD.Sample.DonkeyKongSample {
    public class Program {
        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            var p2d = DD.Physics.Physics2D.GetInstance ();
            p2d.CreateWorld (32);
            p2d.Gravity = new DD.Vector2 (0, 9.8f);

            // ----------------------------------------

            var wld = MyWorld.Create ();

            var node1 = MyPlatform.Create(new Vector3(0,0,0));
            var node2 = MyGorilla.Create (new Vector3(50, 0, 0));
            var node4 = MyCharacter.Create (new Vector3(100,500,0));
            var node5 = MyDebugInfo.Create (new Vector3(300,30,0));
            var node6 = MyDeadBox.Create(new Vector3(672, 472,0));
            var node7 = MyGoal.Create(new Vector3(50,100,0));
            var node8 = MyGameClear.Create (new Vector3 (300, 200, 0));
            
            wld.AddChild (node1);
            wld.AddChild (node2);
            //wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (node5);
            wld.AddChild (node6);
            wld.AddChild (node7);
            wld.AddChild (node8);
            
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

                g2d.Dispatch (wld);
                p2d.Step (wld, msec);

                wld.Animate (msec, 0);
                wld.Update (msec);
                wld.Deliver ();

                g2d.Draw (wld);
            }

            Console.WriteLine ("End of Game");
        }

    }
}
