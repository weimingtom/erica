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


namespace DD.Sample {
    static class Program {

        private static Node CreateWalls () {
            var spr1 = new Sprite ();
            spr1.AddTexture (new Texture ("media/Brick-1024x64.jpg"));
            spr1.SetOffset (-spr1.Width / 2, -spr1.Height / 2);

            var spr2 = new Sprite ();
            spr2.AddTexture (new Texture ("media/Brick-1024x64.jpg"));
            spr2.SetOffset (-spr2.Width / 2, -spr2.Height / 2);

            var spr3 = new Sprite ();
            spr3.AddTexture (new Texture ("media/Brick-1024x64.jpg"));
            spr3.SetOffset (-spr3.Width / 2, -spr3.Height / 2);

            var spr4 = new Sprite ();
            spr4.AddTexture (new Texture ("media/Brick-1024x64.jpg"));
            spr4.SetOffset (-spr4.Width / 2, -spr4.Height / 2);

            var col1 = new Collider ();
            col1.Type = ColliderType.Static;
            col1.SetShape (new BoxCollider (spr1.Width, spr1.Height, 0));
            col1.SetMaterial (new PhysicsMaterial ());
       
            var col2 = new Collider ();
            col2.Type = ColliderType.Static;
            col2.SetShape (new BoxCollider (spr2.Width, spr2.Height, 0));
            col2.SetMaterial (new PhysicsMaterial ());
       
            var col3 = new Collider ();
            col3.Type = ColliderType.Static;
            col3.SetShape (new BoxCollider (spr3.Width, spr3.Height, 0));
            col3.SetMaterial (new PhysicsMaterial ());
       
            var col4 = new Collider ();
            col4.Type = ColliderType.Static;
            col4.SetShape (new BoxCollider (spr4.Width, spr4.Height, 0));
            col4.SetMaterial (new PhysicsMaterial ());
       
            var node1 = new Node ("Wall1");
            node1.Translate (400, 584, 0);
            node1.Attach (spr1);
            node1.Attach (col1);

            var node2 = new Node ("Wall2");
            node2.Translate (400, 16, 0);
            node2.Rotate (180, 0, 0, 1);
            node2.Attach (spr2);
            node2.Attach (col2);

            var node3 = new Node ("Wall3");
            node3.Translate (16, 300, 0);
            node3.Rotate (90, 0, 0, 1);
            node3.Attach (spr3);
            node3.Attach (col3);

            var node4 = new Node ("Wall4");
            node4.Translate (784, 300, 0);
            node4.Rotate (-90, 0, 0, 1);
            node4.Attach (spr4);
            node4.Attach (col4);

            var node = new Node ();
            node.AddChild (node1);
            node.AddChild (node2);
            node.AddChild (node3);
            node.AddChild (node4);
            return node;
        }

        private static Node CreateBalls () {
            var spr1 = new Sprite ();
            spr1.AddTexture (new Texture ("media/Earth.png"));
            spr1.SetOffset (-spr1.Width / 2, -spr1.Height / 2);

            var spr2 = new Sprite ();
            spr2.AddTexture (new Texture ("media/Moon.png"));
            spr2.SetOffset (-spr2.Width / 2, -spr2.Height / 2);

            var col1 = new Collider ();
            col1.Type = ColliderType.Dynamic;
            col1.SetShape (new SphereCollider (spr1.Width / 2));
            col1.SetMaterial (new PhysicsMaterial ());


            var col2 = new Collider ();
            col2.Type = ColliderType.Dynamic;
            col2.SetShape (new SphereCollider (spr2.Width / 2));
            col2.SetMaterial (new PhysicsMaterial ());
            
            var cmp1 = new MyComponent ();

            //var cmp2 = new MyComponent ();

            var node1 = new Node ("Ball1");
            node1.SetBoundingBox (-spr1.Width / 2, -spr1.Height / 2, spr1.Width, spr1.Height);
            node1.Translate (220, 200, 0);
            node1.Attach (spr1);
            node1.Attach (col1);
            node1.Attach (cmp1);
            node1.Rotate (-135, 0, 0, 1);
            node1.GroupID = 1;

            var node2 = new Node ("Ball2");
            node2.SetBoundingBox (-spr2.Width / 2, -spr2.Height / 2, spr2.Width, spr2.Height);
            node2.Translate (200, 100, 0);
            node2.Attach (spr2);
            node2.Attach (col2);
            //node2.Attach (cmp2);


            var node = new Node ();
            node.AddChild (node1);
            node.AddChild (node2);
            return node;
        }

        private static Node CreateUpdraft () {

            var spr = new Sprite(new Texture("media/Arrow.png"));
            spr.SetOffset(-spr.Width/2, -spr.Height/2);

            var col = new Collider ();
            col.Type = ColliderType.Static;
            col.SetShape (new BoxCollider (spr.Width*2, 600, 0));
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

        static void Main (string[] args) {

            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            var p2d = DD.Physics.Physics2D.GetInstance ();
            p2d.CreateWorld (800, 600, 0);
            p2d.Gravity = new DD.Vector3 (0, 9.8f, 0);

            // ----------------------------------------

            var node1 = CreateWalls ();
            var node2 = CreateBalls ();
            var node3 = CreateFPSCounter ();
            var node4 = CreateUpdraft ();

            // ----------------------------------------


            var wld = new DD.World ("First Script");
            wld.Attach (new Sprite (new Texture ("media/DarkGalaxy.jpg")));
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);

            var director = new Director ();
            director.PushScript (wld);
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                director.Exit ();
            };

            Console.WriteLine ("Start of Main Loop");

            g2d.SetFrameRateLimit (30);

            var watch = new Stopwatch ();
            watch.Start ();
            
            while (director.IsAlive) {
                var msec = watch.ElapsedMilliseconds;

                p2d.Step (director.CurrentScript, msec);

                director.Animate (msec);
                director.Update (msec);
                g2d.Dispatch (director.CurrentScript);
                g2d.Draw (director.CurrentScript);

            }

            Console.WriteLine ("End of Game");
        }
    }

}
