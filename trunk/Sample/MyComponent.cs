using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace Sample {
    public class MyEventArgs : EventArgs {
        public MyEventArgs ()
            : base () {
        }

        public int X {
            get;
            set;
        }
        public int Y {
            get;
            set;
        }
    }

    public class MyComponent : Component {
        LineReader player;
        public MyComponent (LineReader linePlayer) {
            this.player = linePlayer;
        }

        /// <inheritdoc/>
        public override void OnMouseButtonPressed (MouseButton button, int x, int y) {
            if (button == MouseButton.Left) {
                player.Next ();
            }
            if (button == MouseButton.Right) {
                player.Prev ();
            }
        }

        /// <inheritdoc/>
        public override void OnLineEvent (Line line, object args) {
            Console.WriteLine ("Event !!");
            Console.WriteLine ("Words = " + line.Words);
            Console.WriteLine ("Events = " + line.Event);
            var my = args as MyEventArgs;
            Console.WriteLine ("X = " + my.X);
            Console.WriteLine ("Y = " + my.Y);
        }

    }
}
