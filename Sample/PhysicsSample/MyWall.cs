using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.PhysicsSample {
    public class MyWall : Component{

        public static Node Create (int width, int height, int depth, Vector3 pos) {
            var cmp = new MyWall ();

            var spr = new Sprite (width, height);
            spr.SetOffset (-width / 2, -height / 2);
            spr.Color = Color.White;

            
            var body = new RigidBody ();
            body.Mass = 0;
            body.Material = new PhysicsMaterial ();
            body.Material.Restitution = 1;
            //body.Use2D = true;
            body.CollideWith = -1;
            body.Shape = new BoxShape (width / 2, height / 2, depth/2);
            
            var node = new Node ("Wall");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (body);
            node.GroupID = -1;

            node.Translation = pos;

            return node;
        }

    }
}
