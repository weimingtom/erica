using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyBodySprite : Component {


        public static Node Create (Vector3 pos) {
            var cmp = new MyBodySprite ();

            var spr = new Sprite (400, 600);
            spr.AddTexture (Resource.GetDefaultTexture ());

            var mbox = new MailBox ("CharacterChanged");

            var node = new Node ("BodySprite");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (mbox);

            node.Translation = pos;

            return node;
        }

        public override void OnMailBox (Node from, string address, object letter) {
            switch (address) {
                case "CharacterChanged": {
                        var ch = ((Node)letter).GetComponent<MyCharacter> ();
                        GetComponent<Sprite> ().SetTexture (0, ch.StandingTexture);
                        Log (0, "Change body sprite to " + ch.Name);
                        break;
                    }
            }
        }
    }
}
