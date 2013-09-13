using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
   public  class MyFPSCounter : Component {

       public static Node Create (Vector3 pos) {
           var cmp = new MyFPSCounter ();

           var fps = new FPSCounter ();

           var node = new Node ("FPSCounter");
           node.Attach (cmp);
           node.Attach (fps);

           node.Translation = pos;

           return node;
       }
    }
}
