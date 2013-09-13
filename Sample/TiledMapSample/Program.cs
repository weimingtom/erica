using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DD.Sample.TiledMapSample {
    public static class Program {

        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------
           
            var node2 = MyFPSCounter.Create(new Vector3(0,0,0));
            var node3 = MyLabels.Create(new Vector3(16, 32, 0));
            var node4 = MyTiledMap.Create ("media/desert.tmx");
            var node6 = MyRader.Create (new Vector3 (220, -250, 0));
            var node7 = MyGameClear.Create ();
            var mychar = MyCharacter.Create (new Vector3 (150, 120, 0));
            var cam = MyCamera.Create ();

            mychar.AddChild (cam);
            mychar.AddChild (node6);
            mychar.AddChild (node7);

            // ----------------------------------------

            var wld = new DD.World ("First Script");
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (mychar);
            wld.ActiveCamera = cam;
            
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
                wld.Deliver ();
                wld.Update (msec);
                g2d.Dispatch (wld);
                
                g2d.Draw (wld);
            }

            wld.Destroy ();
            Console.WriteLine ("End of Game");
        }
    }

}

