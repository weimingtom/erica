using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyNamePlate: Component {

        public static Node Create (Vector3 pos) {
            var cmp = new MyNamePlate ();

            var label = new Label ();
            label.Text = "No name";
            label.Color = Color.Black;
            label.CharacterSize = 24;

            var mbox = new MailBox ("CharacterChanged");
            
            var node = new Node("NamePlate");
            node.Attach (cmp);
            node.Attach (label);
            node.Attach (mbox);

            node.Translation = pos;

            return node;
        }

        public override void OnMailBox (Node from, string address, object letter) {
            switch (address) {
                case "CharacterChanged":{
                    var ch = ((Node)letter).GetComponent<MyCharacter> ();
                    GetComponent<Label> ().Text = ch.Name;
                    Log (0, "Change name plate to " + ch.Name);
                    break;
                }
            }    
        }
    }
}
