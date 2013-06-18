﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;
using DD.Physics;

namespace DD.Sample {
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
        bool picked;
        float mx;
        float my;
        float px;
        float py;

        public MyComponent () {
            this.chime = new SoundClip ("media/PinPon.wav");
            this.picked = false;
        }

        public override void OnUpdate (long msec) {
        }

        /// <inheritdoc/>
        public override void OnMouseButtonPressed (MouseButton button, int x, int y) {
            if (button == MouseButton.Left) {
                Console.WriteLine(Node.Name + " : Left Clicked " + i++);
                this.picked = true;
            }
            else if (button == MouseButton.Right) {
                Console.WriteLine (Node.Name + " : Right Clicked " + i++);
                this.picked = false;
            }
        }

        /// <inheritdoc/>
        public override void OnCollisionEnter (DD.Physics.ContactPoint cp) {
            Console.WriteLine (Node.Name + " : Collision Enter : Collidee = {0}, Point = {1}, Normal = {2}", cp.Collidee.Node.Name, cp.Point, cp.Normal);
            chime.Play ();
        }

         /// <inheritdoc/>
        public override void OnCollisionExit (Collider col) {
            Console.WriteLine (Node.Name + " : Collision Exit");
        }


    }
}
