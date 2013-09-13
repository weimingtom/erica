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
            if (angle == 1) {
                body.Mass = 0;
            }
            else {
                body.Mass = 1f;

            }
            body.Material = new PhysicsMaterial ();
            body.Material.Restitution = 1f;
            body.Use2D = true;
            body.CollideWith = -1;
            body.Shape = new BoxShape (radius, radius, radius);
            
            var node = new Node ("Ball");
            node.Attach (spr);
            node.Attach (body);
            node.GroupID = -1;

            node.Translation = pos;
            node.Rotate (angle, 0, 0, 1);
            
            return node;
        }


    }
}
