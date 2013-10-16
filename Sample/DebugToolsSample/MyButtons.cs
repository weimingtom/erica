using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyButtons  : Component{

        public static Node Create (Vector3 pos) {
            var cmp = new MyButtons ();

            var node = new Node ("Buttons");
            node.Attach (cmp);

            node.AddChild (MyButton.Create("Button1", "A子", new Vector3(0,0,0)));
            node.AddChild (MyButton.Create ("Button2", "B子", new Vector3 (80, 0, 0)));
            node.AddChild (MyButton.Create ("Button3", "C子", new Vector3 (160, 0, 0)));

            node.Translation = pos;

            return node;
        }
    }
}
