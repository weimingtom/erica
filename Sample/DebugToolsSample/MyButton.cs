using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyButton : Component{

        string target;

        public MyButton () {
        }

        public string Target  {
            get { return target; }
        }

        public static Node Create (string name, string target, Vector3 pos) {
            var cmp = new MyButton ();
            cmp.target = target;

            var spr = new Sprite (64, 64);
            spr.AddTexture (new Texture("media/ButtonRed-Active-64x64.png"));
            spr.AddTexture (new Texture ("media/ButtonGreen-Active-64x64.png"));
            spr.AddTexture (new Texture ("media/ButtonBlue-Active-64x64.png"));
            switch (target) {
                case "A子": spr.ActiveTextureIndex = 0; break;
                case "B子": spr.ActiveTextureIndex = 1; break;
                case "C子": spr.ActiveTextureIndex = 2; break;
            }

            var col = new CollisionObject ();
            col.Shape = new BoxShape (40, 25, 1);
            col.SetOffset (40, 25, 0);

            var node = new Node (name);
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);

            node.UserData.Add ("PinPon", new SoundEffectTrack ("media/PinPon.wav"));
             
            node.Translation = pos;



            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            
            var sound = Node.UserData["PinPon"] as SoundEffectTrack;
            sound.Play ();

            var node = World.Find (target);
            if (node != null) {
                Log (-1, "Request to change character = " + target);
                SendMessage ("CharacterChanged", node);
            }
        }

        public override string ToString () {
            return "Button : " + target;
        }
    }
}
