using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.DonkeyKongSample {
    public class MyGorilla : Component {
        long prevTime;
        long interval;
        int ballIndex;

        public MyGorilla () {
            this.ballIndex = 0;
            this.interval = 1000*5;
            this.prevTime = -interval;
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyGorilla ();
            
            var spr = new Sprite (Resource.GetTexture ("media/Gorilla.png"));
   
            var node = new Node ("Gorilla");
            node.Attach (spr);
            node.Attach (cmp);
            
            node.Translation = pos;

            return node;
        }

        private Node CreateBall (Vector3 pos) {
            var cmp = new MyBall ();
            
            var spr = new Sprite (Resource.GetTexture ("media/Earth-32x32.png"), 32, 32);
        
            var col = new SphereCollisionShape (16);
            col.SetOffset (col.Radius, col.Radius, 0);

            var body = new PhysicsBody ();
            body.Type = ColliderType.Dynamic;
            body.SetShape (col);
            body.SetMaterial (new PhysicsMaterial ());
            body.ApplyForce (50000, 0, 0);

            var node = new Node ("Ball " + ballIndex++);
            node.Attach (spr);
            node.Attach (cmp);
            node.Attach (col);
            node.Attach (body);
            
            node.Translation = pos;

            return node;
        }

        public override void OnUpdate (long msec) {
            if (msec > prevTime + interval) {
                var ball = CreateBall (new Vector3(50, 150, 0));
                Node.AddChild(ball);
                this.prevTime = msec;
            }
        }


    }
}
