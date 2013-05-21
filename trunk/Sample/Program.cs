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

            var btn1 = new Button (128, 128);
            btn1.LoadTexutre (ButtonState.Normal, "media/image128x128(Red).png");
            btn1.LoadTexutre (ButtonState.Focused, "media/image128x128(Green).png");
            btn1.LoadTexutre (ButtonState.Pressed, "media/image128x128(Blue).png");
            btn1.LoadTexutre (ButtonState.PressedFocused, "media/image128x128(Cyan).png");
            var node1 = new Node ("node1");
            node1.Attach (btn1);
            node1.Move(100,100);


            var script = new Script ("First Script");
            script.Attach (new Sprite ("media/PhilosophyOfLife.png"));
            script.Attach (new FPSCounter ());
            script.AddChild (node1);
            
            var director = new Director ();
            director.PushScript (script);
            g2d.OnClosed += delegate (object sender, EventArgs eventArgs) {
                director.Exit ();
            };


            Console.WriteLine ("Start of Main Loop");

            while (director.IsAlive) {
                director.Update ();
                g2d.Dispatch (director.CurrentScript);
                g2d.Draw (director.CurrentScript);
            }

            Console.WriteLine ("End of Game");
        }
    }

}
