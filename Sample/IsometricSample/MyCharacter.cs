using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// 1つのタイルは64x32で40x40ブロック
// キャラクターは64x128で作成されている
// コリジョンは(0,0)-(64,32).

namespace DD.Sample.IsometricSample {
    public class MyCharacter : Component {
        float speed = 10f;

        public static Node Create () {
            var cmp = new MyCharacter ();

            var spr = new Sprite (64, 128);
            spr.AddTexture (new Texture ("media/isometric-foot.png"));
            spr.AddTexture (new Texture ("media/isometric-man-ne.png"));
            spr.AddTexture (new Texture ("media/isometric-man-nw.png"));
            spr.AddTexture (new Texture ("media/isometric-man-se.png"));
            spr.AddTexture (new Texture ("media/isometric-man-sw.png"));
            spr.ActiveTextureIndex = 1;

            var col = new CollisionObject ();
            col.Shape = new BoxShape (32, 16, 1);
            col.SetOffset (32, 16, 1);

            var clicker = new MyClicker (spr);
            
            var node = new Node ("Character");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (clicker);

            node.DrawPriority = -1;

            return node;
        }


        private void Move (float dx, float dy, float dz) {
            var label = World.Find ("Label").GetComponent<Label> (0);

             var move = new Vector3 (dx, dy, dz);

            var output = World.Sweep (Node, move);
            if (output.Hit) {
                move *= output.Fraction;
                move *= (move.Length - 1) / move.Length;
                label.Text = "Hit : " + output.Node.Name;
            }
            else {
                label.Text = "NoHit";
            }
            Node.Translate (move.X, move.Y, move.Z);


        }


        public override void OnUpdate (long msec) {

            var spr = GetComponent<Sprite> ();

            var dx = 0;
            var dy = 0;

            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.RightArrow: dy -= 1; spr.ActiveTextureIndex = 1; break;
                    case KeyCode.LeftArrow: dy += 1; spr.ActiveTextureIndex = 4; break;
                    case KeyCode.DownArrow: dx += 1; spr.ActiveTextureIndex = 3; break;
                    case KeyCode.UpArrow: dx -= 1; spr.ActiveTextureIndex = 2; break;
                }
            }
            if (dx != 0) {
                Move (dx * speed *2, 0, 0);
            }
            if (dy != 0) {
                Move (0, dy * speed, 0);
            }

        }

        public override void OnPreDraw (object window) {
            var tiled = World.Find ("TiledMap").GetComponent<TiledMapComposer>();
            var spr = GetComponent<Sprite> ();
            var col = GetComponent<CollisionObject> ();

            // -96はキャラクターの足下の菱形をタイルに一致させるため
            // +16は元の画像キャラクターの足下が少し高すぎる分の補正（目視で適当に決めた）
            var convert = tiled.OrthogonalToIsometricOffset (Node.Translation);
            spr.SetOffset (convert.X, convert.Y - 96 + 16);
        }
    }
}
