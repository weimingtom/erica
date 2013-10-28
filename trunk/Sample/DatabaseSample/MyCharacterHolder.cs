using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DatabaseSample {
   public class MyCharacterHolder : Component{

       public static Node Create () {
           var cmp = new MyCharacterHolder ();

           var node = new Node ("Characters");
           node.Attach (cmp);

           node.AddChild (MyCharacter.Create ("Mayuto", new Vector3 (0, 0, 0)));
           node.AddChild (MyCharacter.Create ("Nayu", new Vector3 (0, 0, 0)));
           node.AddChild (MyCharacter.Create ("Sayaka", new Vector3 (0, 0, 0)));
           node.AddChild (MyCharacter.Create ("Maki", new Vector3 (0, 0, 0)));
           node.AddChild (MyCharacter.Create ("Lily", new Vector3 (0, 0, 0)));
           node.AddChild (MyCharacter.Create ("Rinko", new Vector3 (0, 0, 0)));

           return node;
       }
    }
}
