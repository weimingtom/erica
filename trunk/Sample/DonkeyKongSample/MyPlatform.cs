using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.DonkeyKongSample {
    public class MyPlatform : Component{
        public MyPlatform () {
        }

        private static Node CreateFloor (string name, int x, int y, int width, int height, Quaternion rot) {
            var spr = new Sprite (width, height);
            spr.AddTexture (Resource.GetTexture ("media/Rectangle-160x40.png"));

            var col = new BoxCollision (spr.Width / 2, spr.Height / 2, 0);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 0);

            var body = new PhysicsBody ();
            var mat = new PhysicsMaterial ();
            body.Shape = col;
            body.Material = mat;

            var node = new Node (name);
            node.SetTranslation (x, y, 0);
            node.SetRotation (rot);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (body);

            return node;
        }




        public static Node Create (Vector3 pos) {
            var cmp = new MyPlatform ();
            
            var node = new Node ("Platform");
            node.AddChild (CreateFloor ("Ground", 0, 580, 800, 20, new Quaternion (0, 0, 0, 1)));
            node.AddChild (CreateFloor ("Floor1", 10, 450, 650, 20, new Quaternion (3, 0, 0, 1)));
            node.AddChild (CreateFloor ("Floor2", 100, 350, 690, 20, new Quaternion (-3, 0, 0, 1)));
            node.AddChild (CreateFloor ("Floor3", 10, 180, 650, 20, new Quaternion (3, 0, 0, 1)));
            node.AddChild (CreateFloor ("LeftWall", 0, 0, 600, 20, new Quaternion (90, 0, 0, 1)));
            node.AddChild (CreateFloor ("RightWall1", 820, 0, 600, 20, new Quaternion (90, 0, 0, 1)));
            node.Attach (cmp);

            node.Translation = pos;

            return node;
        }
    }
}
