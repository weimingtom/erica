using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyBackground : Component{
        public MyBackground () {

        }

        public static Node Create () {
            var cmp = new MyBackground ();

            var spr = new Sprite (800, 600);
            spr.AddTexture (new Texture ("media/TatamiRoom.png"));
            
            var node = new Node ("Background");
            node.Attach (cmp);
            node.Attach (spr);

            node.DrawPriority = 10;

            return node;
        }
    }
}
