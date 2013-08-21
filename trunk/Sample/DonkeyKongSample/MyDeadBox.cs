using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;


namespace DD.Sample.DonkeyKongSample {
    public class MyDeadBox : Component {

        public static Node Create (Vector3 pos) {
            var cmp = new MyDeadBox ();

            var spr = new Sprite (128, 128);
            spr.AddTexture (new Texture ("media/Image128x128(Red).png"));
            spr.Color = Color.Gray;

            var col = new BoxCollisionShape (spr.Width / 2, spr.Height / 2, 0);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 0);

            var body = new PhysicsBody ();
            body.Shape = col;
            body.IsTrigger = true;

            var node = new Node ("DeadBox");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (body);

            node.Translation  = pos;

            return node;
        }

        public override void OnCollisionEnter (Collision cp) {
            var label = World.Find ("Label").GetComponent<Label> (2);

            // 爆発
            var node = MyFireExplosion.Create (cp.Collidee.Node.Position);
            World.AddChild (node);
  
            label.Text = "Destroy : \"" + cp.Collidee.Node.Name + "\"";
            Destroy (cp.Collidee.Node);

        }

    }
}
