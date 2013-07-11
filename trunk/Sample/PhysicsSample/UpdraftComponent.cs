using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.PhysicsSample {
    public class UpdraftComponent : Component {

        public override void OnCollisionEnter (Collision cp) {
        }

        public override void OnUpdate (long msec) {
            var col = GetComponent<PhysicsBody> ();

            foreach (var c in col.Collisions) {
                c.Collidee.ApplyForce (-10000, -100000, 0);
            }
        }
    }
}
