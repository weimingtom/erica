using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.DonkeyKongSample {
    public class MyGameClear : Component {

        public MyGameClear () {
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyGameClear ();

            var spr = new Sprite (200, 160);
            spr.AddTexture (new Texture ("media/GameClear.png"));

            var clip = new SoundClip ("SoundClip");
            clip.AddTrack (new SoundEffectTrack ("media/Announce.ogg"));
            clip.Volume = 0.3f;

            var node = new Node ("GameClear");
            node.Attach (cmp);
            node.Attach (spr);
            node.AddMailBox (node.Name);
            node.UserData.Add (clip.Name, clip);

            node.Drawable = false;
            node.Translation = pos;

            return node;
        }

        public override void OnMailBox (Node from, string address, object letter) {
            Node.Drawable = true;
            Node.RemoveMailBox ("GameClear");
            (Node.UserData["SoundClip"] as SoundClip).Play ();
        }


    }
}
