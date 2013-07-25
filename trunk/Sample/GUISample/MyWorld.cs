using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.GUISample {
    public class MyWorld : Component {

        public static World Create () {
            var cmp = new MyWorld ();
            var spr = new Sprite (new Texture ("media/Vanity.jpg"));

            var wld = new World ("World");
            wld.Attach (cmp);
            wld.Attach (spr);

            return wld;
        }
    }
}
