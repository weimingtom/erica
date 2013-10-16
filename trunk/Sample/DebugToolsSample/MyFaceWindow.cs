using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyFaceWindow : Component {

        public static Node Create (Vector3 pos) {
            var cmp = new MyFaceWindow ();

            var spr = new Sprite (128, 128);
            spr.AddTexture (Resource.GetDefaultTexture ());

            var mbox = new MailBox ("CharacterChanged");

            var node = new Node ("FaceWindow");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (mbox);

            node.Translation = pos;

            return node;
        }

        public override void OnMailBox (Node from, string address, object letter) {

            var node = letter as Node;

            switch (address) {
                case "CharacterChanged": {
                        var ch = (letter as Node).GetComponent<MyCharacter> ();
                        GetComponent<Sprite> ().SetTexture (0, ch.FaceTexture);
                        Log (0, "Change face to " + ch.Name);
                        break;
                    }
            }
        }
    }
}
