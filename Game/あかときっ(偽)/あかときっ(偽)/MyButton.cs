using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
    public class MyButton : Component {
        public enum Pose {
            標準待機,
            被弾,
        }
        Pose pose;

        /// <summary>
        /// コンストラクター
        /// </summary>
        public MyButton (Pose pose) {
            this.pose = pose;
        }

        public static Node Create (Vector3 pos, Pose pose) {
            var cmp = new MyButton (pose);

            var spr = new Sprite ();
            switch (pose) {
                case Pose.標準待機: spr.AddTexture (Resource.GetTexture ("共通/ButtonBlue.png")); break;
                case Pose.被弾: spr.AddTexture (Resource.GetTexture ("共通/ButtonGreen.png")); break;
            }

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width / 2, spr.Height / 2, 10);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 10);

            var label = new Label ();
            switch (pose) {
                case Pose.標準待機: label.Text = "標準待機画像"; break;
                case Pose.被弾: label.Text = "被弾画像"; break;
            }

            var clip = new SoundClip ();
            clip.AddTrack (Resource.GetSoundTrack ("Data/共通/PinPon.wav"));

            var node = new Node ("Button");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (label);
            node.AddUserData ("ClickSound", clip);

            node.Translation = pos;

            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var clip = GetUserData<SoundClip> ("ClickSound");
            clip.Play ();
            
            var ch = World.Find("Maki");
            if (ch != null) {
                switch (pose) {
                    case Pose.標準待機: SendMessage("DisplayCharacter", ch); break;
                    case Pose.被弾: SendMessage("DisplayDamagedCharacter", ch); break;
                }
            }
        }
    }
}
