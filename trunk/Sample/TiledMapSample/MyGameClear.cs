using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyGameClear  : Component{

        public static Node Create () {

            var spr = new Sprite (200, 160);
            spr.AddTexture (new Texture("media/GameClear.png"));

            var node = new Node ();
            node.Attach (spr);
            node.DrawPriority = -1;
  
            return node;
        }
    }
}
