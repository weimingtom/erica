using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.KeyboardSample {
    public class MyWorld : Component{

        public static World Create (int width, int height, string texName) {
            var cmp = new MyWorld ();

            var spr = new Sprite (width, height);
            spr.AddTexture (new Texture (texName));

            var wld = new World ();

            wld.Attach (cmp);
            wld.Attach (spr);

            return wld;
        }
    }
}
