using System;
using System.Collections.Generic;
using System.Linq;
//using SFML.Audio;
//using SFML.Window;
//using SFML.Graphics;
using DD;
using DD.Physics;
using System.Diagnostics;

using BulletSharp;


namespace DD.Sample.PhysicsSample {
    static class Program {

        public static void Main (string[] args) {


            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            PhysicsSimulator.PPM = 64;

            // ----------------------------------------
            var node1 = MyBall.Create ("media/Earth.png", new Vector3 (100, 100, 0), 0);
            var node2 = MyBall.Create ("media/Earth.png", new Vector3 (100, 500, 0), 1);
            //var node2 = MyWall.Create (800, 50, 100, new Vector3 (400, 275, 0));

            // ----------------------------------------

            var wld = MyWorld.Create ();
            wld.AddChild (node1);
            //wld.AddChild (node2);

            wld.PhysicsSimulator.SetGravity (0, 9.8f, 0);

            // -----------------------------------------

            var alive = true;
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                alive = false;
            };

            Console.WriteLine ("Start of Main Loop");

            //g2d.SetFrameRateLimit (30);

            var watch = new Stopwatch ();
            watch.Start ();

            var j = 0;
            var start = watch.ElapsedMilliseconds;
            while (alive) {
                var msec = watch.ElapsedMilliseconds;
                if (msec - start > 1000) {
                    // 1秒終了
                    //break;
                }

                g2d.Dispatch (wld);
                wld.Animate (msec, 0);
                
                wld.PhysicsUpdate (16);
                wld.Update (msec);
                g2d.Draw (wld);

                if (j++ % 100 == 0) {
                    Console.WriteLine ("Position = " + node1.Position);
                }
            }

            wld.Destroy ();
            Console.WriteLine ("End of Game");
        }
    }

}
