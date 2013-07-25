using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.TiledMapSample {
    public class MyCabbage : Component {
        public Sprite Sprite { get; set; }
        public CollisionShape CollisionShape { get; set; }
        public Node MyCharacter { get; set; }

        public override void OnUpdateInit (long msec) {
            this.Sprite = GetComponent<Sprite> ();
            this.CollisionShape = GetComponent<CollisionShape> ();
            this.MyCharacter = World.Find ("MyCharacter");

            Sprite.AddTexture (new Texture("media/image128x128(Red).png"));
        }

        public override void OnUpdate (long msec) {

            var dist = Physics2D.Distance (CollisionShape, Node.GlobalTransform,
                                           MyCharacter.GetComponent<CollisionShape> (), MyCharacter.GlobalTransform);
            if (dist < 1) {
                var node = MyPopup.Create (Node.Position);
                World.AddChild (node);
                Destroy (this);
            }
        }

        public override void OnPreDraw (object window) {
            var pass = World.GetProperty<int> ("Pass");

            if (pass == 1) {
                Sprite.ActiveTextureIndex = 0;
            }
            if (pass == 2) {
                Sprite.ActiveTextureIndex = 1;
            }
        }
    }
}
