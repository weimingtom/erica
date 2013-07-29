using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.ScrollSample {
    public class MyWorld : Component {
        public Sprite Sprite { get; set; }

        public MyWorld () {
        }

        public static World Create () {
            var spr = new Sprite (1920, 1200);
            spr.AddTexture (new Texture ("media/Vanity.jpg"));
            spr.AddTexture (new Texture ("media/image128x128(Green).png"));
            spr.ActiveTextureIndex = 0;

            var cmp = new MyWorld ();
            cmp.Sprite = spr;

            var wld = new World ("First Script");
            wld.Attach (cmp);
            wld.Attach (spr);
            wld.DrawPriority = 127;


            return wld;
        }

        public override void OnDraw (object window) {
            var pass = World.GetProperty("Pass", 1);

            if (pass == 0) {
                Sprite.ActiveTextureIndex = 0;
                Sprite.Color = Color.White;
            }
            if (pass == 1) {
                Sprite.ActiveTextureIndex = 1;
                Sprite.Color = new Color (255, 255, 255, 100);
            }
        }

    }
}
