using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DatabaseSample {
    public class MyCharacterButton : Component {
        string name;
        DB.Character dbCharacter;

        public MyCharacterButton (string name) {
            this.name = name;
            this.dbCharacter = new DB.AkatokiEntities1 ().Characters.Find (name);
            if (dbCharacter == null) {
                throw new InvalidOperationException (string.Format("Name({0}) does not exist in DB", name));
            }
        }

        public static Node Create (string name, Vector3 pos) {
            var cmp = new MyCharacterButton (name);

            var spr = new Sprite (128, 64);
            spr.AddTexture (Resource.GetDefaultTexture ());

            var label1 = new Label ();
            label1.Text = cmp.dbCharacter.FullName;
            label1.SetOffset (10, 10);
            label1.Color = Color.Black;

            var label2 = new Label ();
            label2.Text = cmp.dbCharacter.FullNameYomi;
            label2.SetOffset (10, 30);
            label2.Color = Color.Black;

            var col = new CollisionObject ();
            col.Shape = new BoxShape (64, 32, 100);
            col.SetOffset (64, 32, 0);

            var snd = new SoundEffectTrack ("media/PinPon.wav");
            var clip = new SoundClip ("クリック音");
            clip.AddTrack (snd);

            var node = new Node ("Button(" + name + ")");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (label1);
            node.Attach (label2);
            node.Attach (col);

            node.UserData.Add (clip.Name, clip);

            node.Translation = pos;
            node.DrawPriority = -1;

            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {

            ((SoundClip)Node.UserData["クリック音"]).Play ();

            var ch = World.Find (name);
            if (ch != null) {
                SendMessage ("ChangeCharacter", ch);
            }
        }
    }
}
