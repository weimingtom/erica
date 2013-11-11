using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.RTSSample {
    public class MyBackground : Component {

        public static Node Create () {
            var cmp = new MyBackground ();
            var spr = new Sprite (1200, 1800);
            spr.AddTexture (Resource.GetTexture ("media/Background-Glass.png"));

            var col = new BoxCollision (spr.Width/2, spr.Height/2, 0);
            col.SetOffset (spr.Width/2, spr.Height/2, 0);

            var node = new Node ("Ground");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.DrawPriority = 1;

            return node;
        }

        
        

    }
}
