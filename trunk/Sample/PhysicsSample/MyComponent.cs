using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;
using DD.Physics;

namespace DD.Sample.PhysicsSample {
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

        /// <inheritdoc/>
        public override void OnMouseButtonPressed (MouseButton button, int x, int y) {
            if (button == MouseButton.Left) {
                Console.WriteLine(Node.Name + " : Left Clicked " + i++);
             }
            else if (button == MouseButton.Right) {
                Console.WriteLine (Node.Name + " : Right Clicked " + i++);
             }
        }

        /// <inheritdoc/>
        public override void OnCollisionEnter (DD.Physics.Collision cp) {
            Console.WriteLine (Node.Name + " : Collision Enter : Collidee = {0}, Point = {1}, Normal = {2}", cp.Collidee.Node.Name, cp.Point, cp.Normal);
            chime.Play ();
        }

         /// <inheritdoc/>
        public override void OnCollisionExit (PhysicsBody col) {
            Console.WriteLine (Node.Name + " : Collision Exit");
        }

        public override void OnUpdate (long msec) {
            var label = World.Find (x => x.Name == "Labels").GetComponent<Label>(0);

            var col = GetComponent<PhysicsBody> ();

            var num = col.Collisions.Count ();

            label.Text = "Col num = " + num;
        }


    }
}
