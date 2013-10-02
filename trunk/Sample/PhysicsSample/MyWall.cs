using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.PhysicsSample {
    public class MyWall : Component{

        public static Node Create (Vector3 size, Vector3 pos, float angle) {
            var width = (int)size.X;
            var height = (int)size.Y;
            var depth = (int)size.Z;
            
            var cmp = new MyWall ();

            var spr = new Sprite (width, height);
            spr.SetOffset (-width / 2, -height / 2);
            spr.Color = Color.White;

            var body = new RigidBody ();
            body.Mass = 0;
            body.Material = new PhysicsMaterial ();
            body.Material.Restitution = 1;
            body.Use2D = true;
            body.AddShape (new BoxShape (width / 2, height / 2, depth/2));
     
            var node = new Node ("Wall");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (body);

            node.Translation = pos;
            node.Rotate (angle, 0, 0, 1);

            return node;
        }

    }
}
