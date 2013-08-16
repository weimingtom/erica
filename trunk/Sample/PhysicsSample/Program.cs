using System;
using System.Collections.Generic;
using System.Linq;
//using SFML.Audio;
//using SFML.Window;
//using SFML.Graphics;
using DD;
using DD.Physics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using System.Diagnostics;


namespace DD.Sample.PhysicsSample {
    static class Program {



        private static Node CreateUpdraft () {

            var spr = new Sprite(new Texture("media/Arrow.png"));
            spr.SetOffset(-spr.Width/2, -spr.Height/2);

            var col = new PhysicsBody ();
            col.Type = ColliderType.Static;
            col.SetShape (new BoxCollisionShape (spr.Width*2, 600, 0));
            col.SetMaterial (new PhysicsMaterial ());
            col.IsTrigger = true;


            var cmp = new UpdraftComponent ();

            var node = new Node ();
            node.Attach (col);
            node.Attach(spr);
            node.Attach (cmp);
            node.Translate (720, 300, 0);

            return node;
        }


        private static Node CreateFPSCounter () {
            var node = new Node ();
            node.Attach (new FPSCounter ());
            return node;
        }

        private static Node CreateLabels () {
            var node = new Node ("Labels");

            var label1 = new Label ();
            var label2 = new Label ();
            label2.SetOffset (0, 20);

            node.Attach (label1);
            node.Attach (label2);
            node.DrawPriority = -1;
            node.SetTranslation (10, 50, 0);

            return node;
        }


        public static void Main (string[] args) {

            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            var p2d = DD.Physics.Physics2D.GetInstance ();
            p2d.CreateWorld (32);
            p2d.Gravity = new DD.Vector2 (0, 9.8f);

            // ----------------------------------------

            var node1 = MyWalls.Create ();
            var node2 = MySphere.Create ("media/earth.png", new Vector3(250, 100, 0), 40);
            var node3 = CreateFPSCounter ();
            var node4 = MySphere.Create ("media/moon.png", new Vector3(200, 300, 0), 0);
            var node5 = CreateLabels ();
            var node6 = MyRhombus.Create ("media/rhombus.png", new Vector3 (230, 200, 0), 0);
            
            // ----------------------------------------

            var wld = MyWorld.Create ();
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (node5);
            wld.AddChild (node6);

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

                p2d.Step (wld, msec);

                wld.Animate (msec, 0);
                wld.Update (msec);
                g2d.Dispatch (wld);
                g2d.Draw (wld);

            }

            Console.WriteLine ("End of Game");
        }
    }

}
