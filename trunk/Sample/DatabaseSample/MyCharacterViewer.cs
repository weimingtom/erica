using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DatabaseSample {
   public class MyCharacterViewer : Component{
       
       public MyCharacterViewer () {
       }

       public static Node Create (Vector3 pos) {
           var cmp = new MyCharacterViewer ();

           var spr = new Sprite (800, 600);
           spr.AddTexture (Resource.GetDefaultTexture ());
           spr.AutoScale = true;

           var mbox = new MailBox ("ChangeCharacter");

           var node = new Node ("CharacterViewer");
           node.Attach (cmp);
           node.Attach (spr);
           node.Attach (mbox);

           node.Translation = pos;
           node.Visible = false;

           return node;
       }

       public override void OnMailBox (Node from, string address, object letter) {
           switch (address) {
               case "ChangeCharacter": {
                   var chr = ((Node)letter).GetComponent<MyCharacter> ();
                   var tex = chr.GetTexture("キャラクター", "紹介");
                   GetComponent<Sprite> ().SetTexture (0, tex);
                   Node.Visible = true;
                   break;
               }
           }
       
       }
    }
}
