using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.PlatformSample {
    public class MyBlock : Component{
        public MyBlock () {

        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyBlock ();

            var spr = new Sprite (160, 40);
            spr.AddTexture (new Texture ("media/Rectangle-160x40.png"));

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width / 2, spr.Height / 2, 1);
            col.Offset = new Vector3 (spr.Width / 2, spr.Height / 2, 1);

            var node = new Node ("Block");
            node.Attach (spr);
            node.Attach (col);

            node.Translation = pos;
            node.Rotate (-10, 0, 0, 1);
            node.GroupID = (int)MyGroup.Platform;

            return node;
        }
    }
}
