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
        int i;
        SoundClip chime;

        public MyComponent () {
            this.chime = new SoundClip ("media/PinPon.wav");
        }

        public override void OnUpdate (long msec) {
        }
        /// <inheritdoc/>
        public override void OnMouseButtonPressed (MouseButton button, int x, int y) {
            if (button == MouseButton.Left) {
                Console.WriteLine("Clicked " + i++);
                var line = GetComponent<LineReader> ();
                line.Next ();
            }
            else if (button == MouseButton.Right) {
                var line = GetComponent<LineReader> ();
                line.Prev ();
            }
        }


    }
}
