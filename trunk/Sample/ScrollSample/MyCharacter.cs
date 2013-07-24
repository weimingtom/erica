using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.ScrollSample {
    public class MyCharacter : Component {

        public int Speed { get; set; }
        public Sprite Sprite { get; set; }

        public static Node Create () {
            var spr = new Sprite (24, 32);
            spr.AddTexture(new Texture("media/Character-Gelato.png"));
            spr.AddTexture(new Texture("media/image128x128(Red).png"));
            spr.ActiveTextureIndex = 0;

            var cmp = new MyCharacter ();
            cmp.Speed = 10;
            cmp.Sprite = spr;

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (spr);

           return node;
        }

        private void Move (float x, float y, float z) {
            Node.Translate (x, y, z);
        }


        public override void OnUpdate (long msec) {
            var g2d = Graphics2D.GetInstance ();

            var x = 0;
            var y = 0;

            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.RightArrow: x += 1;  break;
                    case KeyCode.LeftArrow: x -= 1; break;
                    case KeyCode.UpArrow: y -= 1; break;
                    case KeyCode.DownArrow: y += 1; break;
                    case KeyCode.Mouse0: {
                            var pos = g2d.GetMousePosition ();
                            var node = MyPopupNumber.Create ();
                            node.Translate (pos.X, pos.Y, 0);
                            World.AddChild (node);
                            Console.WriteLine ("(x,y) = " + pos);
                            break;
                        }

                }
            }

            Move (x * Speed, 0, 0);
            Move (0, y * Speed, 0);
        
        }

        public override void OnDraw (object window, EventArgs args) {
            var pass = ((MyDrawArgs)args).RenderPass;

            if (pass == 0) {
                Sprite.ActiveTextureIndex = 0;
            }
            if (pass == 1) {
                Sprite.ActiveTextureIndex = 1;
            }
        }

    }
}
