using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;
using DD.Physics;

namespace DD.Sample.PhysicsSample {
    public class MyBall : Component {
        SoundClip chime;

        public MyBall () {
            this.chime = new SoundClip ("media/PinPon.wav");
        }


        public static Node Create (string texName, Vector3 pos, float angle) {
            var cmp = new MyBall ();

            var tex = new Texture (texName);
            var radius = tex.Width / 2;

            var spr = new Sprite (tex.Width, tex.Height);
            spr.AddTexture (tex);
            spr.SetOffset (-tex.Width/2, -tex.Height/2);

            var body = new RigidBody ();
            body.AddShape (new SphereShape (radius));
            body.Mass = 1;
            body.Material = new PhysicsMaterial ();
            body.Material.Restitution = 1f;
            body.Use2D = true;

            var node = new Node ("Ball");
            node.Attach (spr);
            node.Attach (body);

            node.Translation = pos;
            node.Rotate (angle, 0, 0, 1);
            
            return node;
        }


    }
}
