using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.DonkeyKongSample {
    public class MyGorilla : Component {
        long prevTime;
        long interval;
        int index;

        public MyGorilla () {
            this.index = 0;
            this.interval = 1000*5;
            this.prevTime = -interval;
        }

        private Node CreateBall () {
            var spr = new Sprite (Resource.GetTexture ("media/Earth-32x32.png"), 32, 32);
            var comp = new MyBall ();

            var col = new SphereCollisionShape (16);
            col.SetOffset (col.Radius, col.Radius, 0);

            var body = new PhysicsBody ();
            body.Type = ColliderType.Dynamic;
            body.SetShape (col);
            body.SetMaterial (new PhysicsMaterial ());
            body.ApplyForce (50000, 0, 0);

            var node = new Node ("Ball " + index++);
            node.Attach (spr);
            node.Attach (comp);
            node.Attach (col);
            node.Attach (body);
            
            node.SetTranslation (50, 150, 0);

            return node;
        }

        public override void OnUpdate (long msec) {
            if (msec > prevTime + interval) {
                var ball = CreateBall ();
                Node.AddChild(ball);
                this.prevTime = msec;
            }
        }


    }
}
