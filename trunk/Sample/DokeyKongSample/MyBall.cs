using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.DonkeyKongSample {
    public class MyBall : Component {


        public override void OnUpdate (long msec) {

            var col = GetComponent<CollisionShape> ();
            var tra = Node.GlobalTransform;

            var mycol = World.Find (x => x.Name == "MyCharacter").GetComponent<CollisionShape> ();
            var mytra = mycol.Node.GlobalTransform;

            var hit = Physics2D.Collide (col, tra, mycol, mytra);
            if (hit) {
                var mycomp = mycol.Node.GetComponent<MyCharacterComponent> ();
                mycomp.Hit ();
            }
        }
    }
}
