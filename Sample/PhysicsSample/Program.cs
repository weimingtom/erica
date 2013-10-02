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


            // ----------------------------------------
            var node0 = MyBall.Create ("media/Earth.png", new Vector3 (400, 100, 0), 0);
            var node1 = MyBall.Create ("media/Moon.png", new Vector3 (450, 200, 0), 0);
            var node2 = MyBox.Create (new Vector3 (425, 400, 0), 10);
            var node3 = MyWall.Create (new Vector3 (800, 25, 100), new Vector3 (400, 25, 0), 0);
            var node4 = MyWall.Create (new Vector3 (800, 25, 100), new Vector3 (400, 575, 0), 0);
            var node5 = MyWall.Create (new Vector3 (25, 600, 100), new Vector3 (25, 300, 0), 0);
            var node6 = MyWall.Create (new Vector3 (25, 600, 100), new Vector3 (775, 300, 0), 0);

            // ----------------------------------------

            var wld = MyWorld.Create ();
            wld.AddChild (node0);
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (node5);
            wld.AddChild (node6);
            
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

            var prev = watch.ElapsedMilliseconds;
            var i = 0;

            while(alive){
                    var current = watch.ElapsedMilliseconds;
                    var delta = current - prev;

                    g2d.Dispatch (wld);
                    wld.Animate (current, 0);
                    wld.PhysicsUpdate (delta);

                    wld.Update (current);
                    g2d.Draw (wld);

                    prev = current;

                    if (i++ % 10 == 0) {
                       // Console.WriteLine ("Pos = {0}", node0.Position);
                    }
            }

            wld.Destroy ();
            Console.WriteLine ("End of Game");
        }
    }

}
