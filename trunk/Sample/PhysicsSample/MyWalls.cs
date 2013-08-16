using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;
using DD.Physics;


namespace DD.Sample.PhysicsSample {
    public class MyWalls : Component {

        public static Node Create () {
            var spr1 = new Sprite (new Texture ("media/Brick-1024x64.jpg"));
            spr1.SetOffset (-spr1.Width / 2, -spr1.Height / 2);

            var spr2 = new Sprite (new Texture ("media/Brick-1024x64.jpg"));
            spr2.SetOffset (-spr2.Width / 2, -spr2.Height / 2);

            var spr3 = new Sprite (new Texture ("media/Brick-1024x64.jpg"));
            spr3.SetOffset (-spr3.Width / 2, -spr3.Height / 2);

            var spr4 = new Sprite (new Texture ("media/Brick-1024x64.jpg"));
            spr4.SetOffset (-spr4.Width / 2, -spr4.Height / 2);

            var body1 = new PhysicsBody ();
            body1.Type = ColliderType.Static;
            body1.SetShape (new BoxCollisionShape (spr1.Width / 2, spr1.Height / 2, 0));
            body1.SetMaterial (new PhysicsMaterial ());

            var body2 = new PhysicsBody ();
            body2.Type = ColliderType.Static;
            body2.SetShape (new BoxCollisionShape (spr2.Width / 2, spr2.Height / 2, 0));
            body2.SetMaterial (new PhysicsMaterial ());

            var body3 = new PhysicsBody ();
            body3.Type = ColliderType.Static;
            body3.SetShape (new BoxCollisionShape (spr3.Width / 2, spr3.Height / 2, 0));
            body3.SetMaterial (new PhysicsMaterial ());

            var body4 = new PhysicsBody ();
            body4.Type = ColliderType.Static;
            body4.SetShape (new BoxCollisionShape (spr4.Width / 2, spr4.Height / 2, 0));
            body4.SetMaterial (new PhysicsMaterial ());

            var node1 = new Node ("Wall1");
            node1.Translate (400, 584, 0);
            node1.Attach (spr1);
            node1.Attach (body1);

            var node2 = new Node ("Wall2");
            node2.Translate (400, 16, 0);
            node2.Rotate (180, 0, 0, 1);
            node2.Attach (spr2);
            node2.Attach (body2);

            var node3 = new Node ("Wall3");
            node3.Translate (16, 300, 0);
            node3.Rotate (90, 0, 0, 1);
            node3.Attach (spr3);
            node3.Attach (body3);

            var node4 = new Node ("Wall4");
            node4.Translate (784, 300, 0);
            node4.Rotate (-90, 0, 0, 1);
            node4.Attach (spr4);
            node4.Attach (body4);

            var node = new Node ();
            node.AddChild (node1);
            node.AddChild (node2);
            node.AddChild (node3);
            node.AddChild (node4);
            return node;
        }
    }
}
