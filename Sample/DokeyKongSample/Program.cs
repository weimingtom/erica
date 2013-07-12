using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DD.Physics;

namespace DD.Sample.DonkeyKongSample {
    public class Program {
        private static Node CreateFloor (string name, int x, int y, int width, int height, Quaternion rot) {
            var spr = new Sprite (Resource.GetTexture ("media/Rectangle-160x40.png"), width, height);
            
            var col = new BoxCollisionShape (spr.Width / 2, spr.Height / 2, 0);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 0);

            var body = new PhysicsBody ();
            var mat = new PhysicsMaterial ();
            body.Shape = col;
            body.Material = mat;

            var node = new Node (name);
            node.SetTranslation (x, y, 0);
            node.SetRotation (rot);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (body);

            return node;
        }

        private static Node CreateGorilla () {
            var spr = new Sprite(Resource.GetTexture("media/Gorilla.png"));
            var cmp = new MyGorilla ();

            var node = new Node("Gorilla");
            node.Attach (spr);
            node.Attach (cmp);
            node.SetTranslation (50, 0, 0);
       
            return node;
        }

        private static Node CreatePlatform () {
            var node = new Node ("Platform");
            node.AddChild (CreateFloor ("Ground", 0, 580, 800, 20, new Quaternion (0, 0, 0, 1)));
            node.AddChild (CreateFloor ("Floor1", 10, 450, 650, 20, new Quaternion(3, 0,0,1)));
            node.AddChild (CreateFloor ("Floor2", 100, 350, 690, 20, new Quaternion (-3, 0, 0, 1)));
            node.AddChild (CreateFloor ("Floor3", 10, 180, 650, 20, new Quaternion (3, 0, 0, 1)));
            //node.AddChild (CreateFloor ("Floor4", 10, 100, 200, 20, new Quaternion (0, 0, 0, 1)));
            node.AddChild (CreateFloor ("LeftWall", 0, 0, 600, 20, new Quaternion (90, 0, 0, 1)));
            node.AddChild (CreateFloor ("RightWall1", 820, 0, 600, 20, new Quaternion (90, 0, 0, 1)));
            return node;
        }

        private static World CreateWorld () {
            var spr = new Sprite (new Texture ("media/DarkGalaxy.jpg"));
            var cmp = new MyWorld ();

            var clip = new SoundClip ("media/BGM(Field04).ogg", true);
            clip.Play();
            clip.Volume = 0.3f;

            var sound = new SoundPlayer();
            sound.AddClip(clip);
            

            var wld = new World ("First Script");
            wld.Attach (cmp);
            wld.Attach (spr);
            wld.Attach (sound);
            wld.DrawPriority = 127;
            return wld;
        }

        private static Node CreateMyCharacter () {
            var spr = new Sprite (new Texture ("media/Character-Gelato.png"), 24, 32);
            var spr2 = new Sprite (new Texture ("media/Image128x128(Red).png"), 24, 4);
            spr2.SetOffset (0, spr.Height);

            var body = new BoxCollisionShape (spr.Width / 2, spr.Height / 2, 0);
            var foot = new SphereCollisionShape (2);
            body.SetOffset (spr.Width / 2, spr.Height / 2, 0);
            foot.SetOffset (spr.Width / 2, spr.Height + 2, 0);

            var comp = new MyCharacterComponent ();

            var node = new Node ("MyCharacter");
            node.Attach (spr);
            node.Attach (spr2);
            node.Attach (body);
            node.Attach (foot);
            node.Attach (comp);
            node.SetTranslation (100, 500, 0);

            return node;
        }

        private static Node CreateLabel () {
            var label1 = new Label ();
            var label2 = new Label ();
            var label3 = new Label ("Destroy : ");
            var label4 = new Label ("Miss : 0");
            label2.SetOffset (0, 20);
            label3.SetOffset (200, 0);
            label4.SetOffset (200, 20);

            var node = new Node ("Label");
            node.Attach (label1);
            node.Attach (label2);
            node.Attach (label3);
            node.Attach (label4);
            node.SetTranslation (300, 30, 0);
           
            return node;
        }

        private static Node CreateDeadBox () {
            var spr = new Sprite(new Texture("media/Image128x128(Red).png"));
            spr.Color = new Color (255, 255, 255, 64);
           
            var comp = new MyDeadBox ();

            var col = new BoxCollisionShape (spr.Width/2, spr.Height / 2, 0);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 0);

            var body = new PhysicsBody ();
            body.Shape = col;
            body.IsTrigger = true;

            var node = new Node ("DeadBox");
            node.Attach (spr);
            node.Attach (comp);
            node.Attach (col);
            node.Attach (body);
            node.SetTranslation(800-spr.Width, 600-spr.Height/2, 0);

            return node;
        }

        private static Node CreateGameClearBox () {
            var spr = new Sprite(new Texture("media/Image128x128(Blue).png"));
            spr.Color = new Color(255,255,255,128);
           
            var comp = new MyGameClearBox();

            var col = new BoxCollisionShape(spr.Width/2, spr.Height/2, 0);
            col.SetOffset (spr.Width/2, spr.Height/2, 0);


            var node = new Node("GameClearBox");
            node.Attach(spr);
            node.Attach(comp);
            node.Attach(col);

            node.SetTranslation (50,60, 0);
            node.DrawPriority = 1;

            return node;
        }

        static void DonkeyKongMain (string[] args) {
            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            var p2d = DD.Physics.Physics2D.GetInstance ();
            p2d.CreateWorld (32);
            p2d.Gravity = new DD.Vector2 (0, 9.8f);

            // ----------------------------------------

            var node1 = CreatePlatform ();
            var node2 = CreateGorilla ();
            //var node3 = CreateBall ();
            var node4 = CreateMyCharacter ();
            var node5 = CreateLabel ();
            var node6 = CreateDeadBox ();
            var node7 = CreateGameClearBox ();

            var wld = CreateWorld ();
            wld.AddChild (node1);
            wld.AddChild (node2);
            //wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (node5);
            wld.AddChild (node6);
            wld.AddChild (node7);

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
