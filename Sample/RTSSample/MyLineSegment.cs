using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    public class MyLineSegment : Component {

        public static Node Create () {
            var cmp = new MyLineSegment ();

            var line = new LineSegment ();
            line.Texture =  new Texture ("media/image128x128(Red).png");
            line.Width = 10;
            line.Length = 200;
            //line.Color = new Color (0, 255, 0);
     
            var node = new Node ();
            node.Attach (cmp);
            node.Attach (line);

            node.Translate (200, 100, 0);
            return node;
        }

        public override void OnUpdate (long msec) {
            Node.Rotate (1, 0, 0, 1);
        }
    }
}
