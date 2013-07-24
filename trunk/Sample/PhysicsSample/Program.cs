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
    static class PhysicsProgram {

        private static Node CreateWalls () {
            var spr1 = new Sprite (new Texture ("media/Brick-1024x64.jpg"));
            spr1.SetOffset (-spr1.Width / 2, -spr1.Height / 2);

            var spr2 = new Sprite (new Texture ("media/Brick-1024x64.jpg"));
            spr2.SetOffset (-spr2.Width / 2, -spr2.Height / 2);

            var spr3 = new Sprite (new Texture ("media/Brick-1024x64.jpg"));
            spr3.SetOffset (-spr3.Width / 2, -spr3.Height / 2);

            var spr4 = new Sprite (new Texture ("media/Brick-1024x64.jpg"));
            spr4.SetOffset (-spr4.Width / 2, -spr4.Height / 2);

            var body1 = new PhysicsBody ();
            body1.Type = ColliderType.Static;
            body1.SetShape (new BoxCollisionShape (spr1.Width / 2, spr1.Height / 2, 0));
            body1.SetMaterial (new PhysicsMaterial ());
       
            var body2 = new PhysicsBody ();
            body2.Type = ColliderType.Static;
            body2.SetShape (new BoxCollisionShape (spr2.Width / 2, spr2.Height / 2, 0));
            body2.SetMaterial (new PhysicsMaterial ());
       
            var body3 = new PhysicsBody ();
            body3.Type = ColliderType.Static;
            body3.SetShape (new BoxCollisionShape (spr3.Width / 2, spr3.Height / 2, 0));
            body3.SetMaterial (new PhysicsMaterial ());
       
            var body4 = new PhysicsBody ();
            body4.Type = ColliderType.Static;
            body4.SetShape (new BoxCollisionShape (spr4.Width / 2, spr4.Height / 2, 0));
            body4.SetMaterial (new PhysicsMaterial ());
       
            var node1 = new Node ("Wall1");
            node1.Translate (400, 584, 0);
            node1.Attach (spr1);
            node1.Attach (body1);

            var node2 = new Node ("Wall2");
            node2.Translate (400, 16, 0);
            node2.Rotate (180, 0, 0, 1);
            node2.Attach (spr2);
            node2.Attach (body2);

            var node3 = new Node ("Wall3");
            node3.Translate (16, 300, 0);
            node3.Rotate (90, 0, 0, 1);
            node3.Attach (spr3);
            node3.Attach (body3);

            var node4 = new Node ("Wall4");
            node4.Translate (784, 300, 0);
            node4.Rotate (-90, 0, 0, 1);
            node4.Attach (spr4);
            node4.Attach (body4);

            var node = new Node ();
            node.AddChild (node1);
            node.AddChild (node2);
            node.AddChild (node3);
            node.AddChild (node4);
            return node;
        }

        private static Node CreateBalls () {
            var spr1 = new Sprite (new Texture ("media/Earth.png"));
            spr1.SetOffset (-spr1.Width / 2, -spr1.Height / 2);

            var spr2 = new Sprite (new Texture ("media/Moon.png"));
            spr2.SetOffset (-spr2.Width / 2, -spr2.Height / 2);

            var col1 = new SphereCollisionShape (spr1.Width / 2);
            var body1 = new PhysicsBody ();
            body1.Type = ColliderType.Dynamic;
            body1.SetShape (col1);
            body1.SetMaterial (new PhysicsMaterial ());

            var col2 = new SphereCollisionShape (spr2.Width / 2);
            var body2 = new PhysicsBody ();
            body2.Type = ColliderType.Dynamic;
            body2.SetShape (col2);
            body2.SetMaterial (new PhysicsMaterial ());
            

            var cmp1 = new MyComponent ();
            var cmp2 = new MyComponent ();

            var node1 = new Node ("Ball1");
            node1.Translate (220, 200, 0);
            node1.Attach (spr1);
            node1.Attach (col1);
            node1.Attach (body1);
            node1.Attach (cmp1);
            node1.Rotate (-135, 0, 0, 1);
            node1.GroupID = 1;

            
            var node2 = new Node ("Ball2");
            node2.Translate (200, 100, 0);
            node2.Attach (spr2);
            node2.Attach (col2);
            node2.Attach (body2);
            node2.Attach (cmp2);
            

            var node = new Node ();
            node.AddChild (node1);
            node.AddChild (node2);
            return node;
        }

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

        static World CreateWorld(){
            var spr = new Sprite (new Texture ("media/DarkGalaxy.jpg"));
            var comp = new MyWorld ();
 
            var node = new World ("First Script");
            node.Attach (spr);
            node.Attach (comp);
            node.DrawPriority = 127;

            return node;
        }

        static void PhycsSampleMain (string[] args) {

            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            var p2d = DD.Physics.Physics2D.GetInstance ();
            p2d.CreateWorld (32);
            p2d.Gravity = new DD.Vector2 (0, 9.8f);

            // ----------------------------------------

            var node1 = CreateWalls ();
            var node2 = CreateBalls ();
            var node3 = CreateFPSCounter ();
            //var node4 = CreateUpdraft ();
            var node5 = CreateLabels ();
            
            // ----------------------------------------

            var wld = CreateWorld ();
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
           // wld.AddChild (node4);
            wld.AddChild (node5);

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
                g2d.Draw (wld, null);

            }

            Console.WriteLine ("End of Game");
        }
    }

}
