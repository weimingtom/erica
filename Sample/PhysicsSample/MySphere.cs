using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;
using DD.Physics;

namespace DD.Sample.PhysicsSample {
    public class MySphere : Component {
        SoundClip chime;

        public MySphere () {
            this.chime = new SoundClip ("media/PinPon.wav");
        }


        public static Node Create (string texName, Vector3 pos, float angle) {
            var spr1 = new Sprite (new Texture (texName));
            spr1.SetOffset (-spr1.Width / 2, -spr1.Height / 2);

            var col1 = new SphereCollisionShape (spr1.Width / 2);
            var body = new PhysicsBody ();
            body.Type = ColliderType.Dynamic;
            body.SetShape (col1);
            body.SetMaterial (new PhysicsMaterial ());
            body.IsEnabled = true;
            body.IsBullet = true;

            var cmp1 = new MySphere ();

            var node1 = new Node ("Ball");
            node1.Translate (pos.X, pos.Y, 0);
            node1.Attach (spr1);
            node1.Attach (col1);
            node1.Attach (body);
            node1.Attach (cmp1);
            node1.Rotate (angle, 0, 0, 1);
            node1.GroupID = 1;

            return node1;
        }

        /// <inheritdoc/>
        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
             Console.WriteLine (Node.Name + " : Clicked, {0} ", button);
        }

        /// <inheritdoc/>
        public override void OnCollisionEnter (DD.Physics.Collision cp) {
            //Console.WriteLine (Node.Name + " : Collision Enter : Collidee = {0}, Point = {1}, Normal = {2}", cp.Collidee.Node.Name, cp.Point, cp.Normal);
            chime.Play ();
        }

        /// <inheritdoc/>
        public override void OnCollisionExit (PhysicsBody col) {
            Console.WriteLine (Node.Name + " : Collision Exit");
        }

        public override void OnUpdate (long msec) {
            var label = World.Find ("Labels").GetComponent<Label> (0);

            var col = GetComponent<PhysicsBody> ();
            var num = col.Collisions.Count ();

            label.Text = "Col num = " + num;
        }

    }
}
