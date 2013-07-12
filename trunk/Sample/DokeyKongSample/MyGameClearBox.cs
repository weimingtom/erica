using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.DonkeyKongSample {
    public class MyGameClearBox : Component{
        bool gameClear;

        public MyGameClearBox () {
            this.gameClear = false;
        }

        private Node CreateGameClearMessage () {
            var spr = new Sprite (new Texture ("media/GameClear.png"));
            
            var node = new Node ();
            node.SetTranslation (400-spr.Width/2, 300-spr.Height/2, 0);
            node.Attach (spr);

            return node;
        }

        SoundClip sound;

        public override void OnUpdate (long msec) {
            var col = GetComponent<CollisionShape>();
            var tra = Node.GlobalTransform;
            var playerCol = World.Find (x => x.Name == "MyCharacter").GetComponent<CollisionShape>();
            var plyaerTra = playerCol.Node.GlobalTransform;

            var hit = Physics2D.Collide(col, tra, playerCol, plyaerTra);
            if (hit && !gameClear) {
                var spr = new Sprite (new Texture ("media/GameClear.png"));
                var node = new Node ("GameClear");
                node.Attach (spr);
                node.SetTranslation (400-spr.Width/2, 150-spr.Height/2, 0);
                node.DrawPriority = -1;

                World.AddChild (node);

                sound = new SoundClip ("media/Announce.ogg");
                sound.Volume = 0.3f;
                sound.Play ();

                this.gameClear = true;
            }
        }
    }
}
