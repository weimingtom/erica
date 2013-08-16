using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace DD.Sample.RTSSample {
    public class Program {
        public static void Main (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 728, "こんにちは、世界");

            Thread.Sleep (1000);

            // ----------------------------------------

            var wld = new World ();

            var node1 = MyCamera.Create (new Vector3(-100, 1300, 0));
            var node2 = MyBackground.Create ();
            var node3 = MyTank.CreateFriend ("T-34", new Vector3 (200, 1600, 0), Quaternion.Identity);
            var node4 = MyTank.CreateFriend ("PanzerIV", new Vector3 (400, 1600, 0), Quaternion.Identity);
            var node5 = MyTank.CreateFriend ("Panther", new Vector3 (600, 1600, 0), Quaternion.Identity);
            var node6 = MyTank.CreateEnemy ("Hotchkiss", new Vector3 (100, 1200, 0), new Quaternion(180, 0,0,1));
            var node7 = MyCharacter.Create ("Ako");
            var node8 = MyCharacter.Create ("Bko");
            var node9 = MyCharacter.Create ("Cko");
            var node10 = MyCard.Create (0, node7, node3);
            var node11 = MyCard.Create (1, node8, node4);
            var node12 = MyCard.Create (2, node9, node5);
            var node13 = MyRader.Create (new Vector3(800 - 128 - 16, 16, 0));
            var node14 = MyMouseSelector.Create ();
            var node15 = MyBGM.Create ();
            var node16 = MyGameClear.Create ();

            // Tank --> MyCharacter
            node3.GetComponent<MyTank> ().MyCharacter = node7;
            node4.GetComponent<MyTank> ().MyCharacter = node8;
            node5.GetComponent<MyTank> ().MyCharacter = node9;

            var rnd = new Random ();
            for (var i = 0; i < 10; i++) {
                var pos = new Vector3 (100 + rnd.Next (600), 1000 - rnd.Next (2000), 0);
                var rot = new Quaternion (180, 0, 0, 1);
                wld.AddChild(MyTank.CreateEnemy ("Hotchkiss", pos, rot));
            }

            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (node5);
            wld.AddChild (node6);
            wld.AddChild (node7);
            wld.AddChild (node8);
            wld.AddChild (node9);
            node1.AddChild (node10);
            node1.AddChild (node11);
            node1.AddChild (node12);
            node1.AddChild (node13);
            node1.AddChild (node16);
            wld.AddChild (node14);
            wld.AddChild (node15);
            wld.ActiveCamera = node1;

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

                wld.Animate (msec, 33);
                g2d.Dispatch (wld);
                wld.Update (msec);
                g2d.Draw (wld);
            }

            Console.WriteLine ("End of Game");
        }

    }
}
