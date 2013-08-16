using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    public class MyCharacter : Component {

        string charName;
        string tankName;
        string weaponName;
        Texture faceTexture;
        Node mycard;

        public string CharacterName {
            get { return charName; }
        }
        public string TankName {
            get { return tankName; }
            set { this.tankName = value; }
        }
        public string WeaponName {
            get { return weaponName; }
            set { this.weaponName = value; }
        }
        public Texture FaceTexture {
            get { return faceTexture; }
        }
        public Node MyCard {
            get { return mycard; }
            set { this.mycard = value; }
        }

        public MyCharacter (string charName) {
            this.charName = charName;
            this.tankName = "default";
            this.weaponName = "default";
            this.faceTexture = Resource.GetDefaultTexture ();
            switch (charName) {
                case "Ako": this.faceTexture = new Texture ("media/Face-Ako.png"); break;
                case "Bko": this.faceTexture = new Texture ("media/Face-Bko.png"); break;
                case "Cko": this.faceTexture = new Texture ("media/Face-Cko.png"); break;
                default: throw new NotImplementedException ("Unknown character name");
            }
        }


        public void Say (BalloonMessage msg) {
            switch (msg) {
                case BalloonMessage.Fire: {
                        var rnd = new Random ();
                        if (rnd.Next (2) == 0) {
                            Speak ("撃てーーー！", 500);
                        }
                        var clip = new SoundClip ("media/0001_さとうささら_撃てーーー.wav");
                        Sound.AddClip (clip, true);
                        break;
                    }
                case BalloonMessage.Destroy: {
                        Speak ("一気撃破！！", 2000);
                        var clip = new SoundClip ("media/0000_さとうささら_一機、撃破.wav");
                        Sound.AddClip (clip, true);
                        break;
                    }
                case BalloonMessage.Complete: {
                        Speak ("敵殲滅を確認", 30000);
                        var clip = new SoundClip ("media/0002_さとうささら_敵機殲滅を確認しました.wav");
                        Sound.AddClip (clip, true);
                        break;
                    }
                default: Speak ("default", 3000); break; ;
            }
        }

        private void Speak (string words, long lifeTime) {
            var node = MyBalloon.Create (words, lifeTime);

            node.SetTranslation (50, -70, 0);

            mycard.AddChild (node);
        }

        public static Node Create (string charName) {
            var cmp = new MyCharacter (charName);

            var node = new Node ("Character");
            node.Attach (cmp);

            return node;
        }

        public override void OnUpdateInit (long msec) {
            Speak ("こんにちは、世界", 1000);
        }

    }
}
