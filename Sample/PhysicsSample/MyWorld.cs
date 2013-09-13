using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.PhysicsSample {
    public class MyWorld : Component{


        public static World Create () {
            var cmp = new MyWorld ();
            
            var wld = new World ();
            wld.Attach (cmp);

            return wld;
        }
    }
}
