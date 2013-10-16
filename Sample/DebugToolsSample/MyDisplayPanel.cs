using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyDisplayPanel : Component {

        public static Node Create (Vector3 pos) {
            var cmp = new MyDisplayPanel ();

            var spr = new Sprite (800, 220);
            spr.AddTexture (new Texture ("media/雪の舞う花の夢.png"));

            var node = new Node ("DisplayPanel");
            node.Attach (cmp);
            node.Attach (spr);

            node.AddChild(MyTextBox.Create(new Vector3(250, 70, 0)));
            node.AddChild(MyFaceWindow.Create(new Vector3(50,65,0)));
            node.AddChild (MyNamePlate.Create (new Vector3 (70, 20, 0)));

            node.DrawPriority = -10;
            foreach (var n in node.Downwards) {
                n.DrawPriority = -10;
            }

            node.Translation = pos;

            return node;
        }
    }
}
