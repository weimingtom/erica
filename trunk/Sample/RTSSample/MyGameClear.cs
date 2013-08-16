using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    public class MyGameClear : Component {
        public MyGameClear () {
        }

        public static Node Create () {
            var cmp = new MyGameClear ();

            var spr = new Sprite (200, 160);
            spr.AddTexture(new Texture("media/GameClear.png"));

            var node = new Node("GameClear");
            node.Attach(cmp);
            node.Attach(spr);

            node.Drawable = false;
            node.DrawPriority = -3;
            node.SetTranslation (300, 220, 0);
            
            return node;
        }

        public override void OnUpdate (long msec) {
            var node = World.Find ("EnemyTank");
            if (node == null) {
                var mychar = World.Find ("Character").GetComponent<MyCharacter>();
                mychar.Say (BalloonMessage.Complete);

                Node.Drawable = true;
                Node.Updatable = false;
            }
        }

    }
}
