using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.PhysicsSample {
    public class MyBox : Component {

        public static Node Create (Vector3 pos, float angle) {
            var cmp = new MyBox ();
            var width = 64;
            var height = 64;
            var depth = 64;

            var spr = new Sprite (width, height);
            spr.AddTexture (new Texture("media/Box-64x64.png"));
            spr.SetOffset (-width / 2, -height / 2);

            var body = new RigidBody ();
            body.AddShape( new BoxShape (width / 2, height / 2, depth / 2));
            body.Material = new PhysicsMaterial ();
            body.Material.Restitution = 1.0f;
            //body.UseCCD = true;

            var node = new Node ("Block");
            node.Attach (cmp);  
            node.Attach (spr);
            node.Attach (body);

            node.Translation = pos;
            node.Rotation = new Quaternion (angle, 0, 0, 1);

            return node;

        }
    }
}
