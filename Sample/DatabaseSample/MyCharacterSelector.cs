using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DatabaseSample {
    public class MyCharacterSelector : Component {

        public static Node Create (Vector3 pos) {
            var cmp = new MyCharacterSelector ();

            var node = new Node ("CharacterSelector");
            node.Attach (cmp);

            node.AddChild (MyCharacterButton.Create ("Nayu", new Vector3 (50, 100, 0)));
            node.AddChild (MyCharacterButton.Create ("Sayaka", new Vector3 (50, 180, 0)));
            node.AddChild (MyCharacterButton.Create ("Maki", new Vector3 (50, 260, 0)));
            node.AddChild (MyCharacterButton.Create ("Lily", new Vector3 (50, 340, 0)));
            node.AddChild (MyCharacterButton.Create ("Rinko", new Vector3 (50, 420, 0)));

            node.Translation = pos;

            return node;
        }
    }
}
