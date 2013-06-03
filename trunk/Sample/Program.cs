using System;
using System.Collections.Generic;
using System.Linq;
//using SFML.Audio;
//using SFML.Window;
//using SFML.Graphics;
using DD;

namespace Sample {
    static class Program {

        static void Main (string[] args) {

            var g2d = DD.Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "こんにちは、世界");

            var wld = new World ("First Script");
            wld.Attach (new FPSCounter ());

            var cmp = new MyComponent ();

            var line = new LineReader (800, 400);
            line.LoadFromFile ("media/HelloMiku.txt");
            line.SetFeedMode (FeedMode.Automatic);
            line.SetFeedParameter(new LineReader.FeedParameters (200, 2000));
            line.Color = Color.White;

            var sound1 = new SoundClip ("media/System41.wav");
            var sound2 = new SoundClip("media/System7.wav");
            line.Ticked += (x,y) => {
                sound1.Play ();
            };
            line.Tacked += (x, y) => {
                sound2.Play ();
            };


            var node = new Node ();
            node.Attach (cmp);
            node.Attach (line);

            node.Move (0, 200);
            node.SetBoundingBox (0, 0, 800, 400);
            wld.AddChild (node);

            var director = new Director ();
            director.PushScript (wld);
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                director.Exit ();
            };


            Console.WriteLine ("Start of Main Loop");

            while (director.IsAlive) {
                director.Animate ();
                director.Update ();
                g2d.Dispatch (director.CurrentScript);
                g2d.Draw (director.CurrentScript);
            }

            Console.WriteLine ("End of Game");
        }
    }

}
