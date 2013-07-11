using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DD.Physics;

namespace DD.PlatformSample {
    public class PlatFormSample {
        private static Node CreateFPSCounter () {
            var node = new Node ();
            node.Attach (new FPSCounter ());
            return node;
        }

        private static Node CreateMyCharacter () {
            var spr1 = new Sprite (new Texture ("media/Character-Gelato.png"), 24, 32);
            var col1 = new BoxCollisionShape (spr1.Width / 2, spr1.Height / 2, 0);
            col1.Offset = new Vector3 (spr1.Width / 2, spr1.Height / 2, 0);

            var spr2 = new Sprite (new Texture("media/image128x128(Red).png"), 24, 4);
            var col2 = new SphereCollisionShape (2);
            spr2.Offset = new Vector2 (0, spr1.Height+2);
            col2.Offset = new Vector3 (spr1.Width / 2, spr1.Height + 4, 0);

            var cmp = new MyCharacterComponent ();
            var node = new Node ();
            node.Attach (spr1);
            node.Attach (cmp);
            node.Attach (col1);
            node.Attach (spr2);
            node.Attach (col2);

            node.SetTranslation (100, 100, 0);

            return node;
        }

        private static Node CreateLabels () {
            var label1 = new Label ();
            var label2 = new Label ();
            label2.Offset = new Vector2 (0, 30);

            var node = new Node ("Label");
            node.SetTranslation (50, 50, 0);
            node.Attach (label1);
            node.Attach (label2);

            return node;
        }

        private static Node CreateWalls () {
            var spr1 = new Sprite (new Texture ("media/Rectangle-160x40.png"), 800, 40);
            var col1 = new BoxCollisionShape (spr1.Width/2, spr1.Height/2, 0);
            col1.Offset = new Vector3 (spr1.Width / 2, spr1.Height / 2, 0);

            var node1 = new Node ("Floor");
            node1.Attach (spr1);
            node1.Attach (col1);
            node1.SetTranslation (0, 560, 0);

            var spr2 = new Sprite (new Texture ("media/Rectangle-160x40.png"));
            var col2 = new BoxCollisionShape (spr2.Width / 2, spr2.Height / 2, 0);
            col2.Offset = new Vector3 (spr2.Width / 2, spr2.Height / 2, 0);

            var node2 = new Node ("Block");
            node2.Attach (spr2);
            node2.Attach (col2);
            node2.SetTranslation (400, 400, 0);
            node2.Rotate (-10, 0, 0, 1);

            var node = new Node ("Collision");
            node.AddChild (node1);
            node.AddChild (node2);

            return node;
        }

        static void Main2 (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            // ----------------------------------------

            var node1 = CreateFPSCounter ();
            var node2 = CreateMyCharacter ();
            var node3 = CreateLabels ();
            var node4 = CreateWalls ();


            // ----------------------------------------

            var wld = new DD.World ("First Script");
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);

            wld.Attach (new MyWorld ());

            var alive = true;
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                alive = true;
            };

            Console.WriteLine ("Start of Main Loop");

            g2d.SetFrameRateLimit (30);

            var watch = new Stopwatch ();
            watch.Start ();


            while (alive) {
                var msec = watch.ElapsedMilliseconds;

                wld.Animate (msec);
                wld.Update (msec);
                g2d.Dispatch (wld);
                g2d.Draw (wld);

            }

            Console.WriteLine ("End of Game");
        }
    }

}
