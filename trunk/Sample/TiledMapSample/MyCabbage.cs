using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyCabbage : Component {

        public override void OnUpdate (long msec) {

            var mychar = World.Find ("MyCharacter");

            var dist = World.Distance (mychar, Node);
            if (dist < 3f) {
                SendMessage ("IamDestroyed", null);

                var node = MyPopup.Create (Node.Position);
                World.AddChild (node);
                Destroy (Node, 0);
            }
        }

    }
}
