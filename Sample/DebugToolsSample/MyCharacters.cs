using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyCharacters: Component {

        public static Node Create(){
            var cmp = new MyCharacters();

            var node = new Node("Characters");
            node.Attach(cmp);

            node.AddChild (MyCharacter.Create ("A子"));
            node.AddChild (MyCharacter.Create ("B子"));
            node.AddChild (MyCharacter.Create ("C子"));


            return node;
        }
    }
}
