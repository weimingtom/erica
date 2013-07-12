using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;


namespace DD.Sample.DonkeyKongSample {
    public class MyDeadBox : Component {

        public override void OnCollisionEnter (Collision cp) {
            var label = World.Find (x => x.Name == "Label").GetComponent<Label> (2);

            var node = MyFireExplosion.CreateInstance ();
            node.Translation = cp.Collidee.Node.Translation;
            node.Rotation= cp.Collidee.Node.Rotation;
            node.Scale = cp.Collidee.Node.Scale;
            cp.Collidee.Node.Parent.AddChild (node);
  

            label.Text = "Destroy : \"" + cp.Collidee.Node.Name + "\"";
            Destroy (cp.Collidee.Node);

        }

    }
}
