using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyCamera2 : Component {
        public static Node Create () {
            var mapWidth = 40 * 32;
            var mapHeight = 40 * 32;

            var cmp = new MyCamera2 ();
            var cam = new Camera ();
            cam.SetScreen (0, 0, mapWidth, mapHeight);
            cam.SetViewport (0.75f, 0.05f, 0.2f, 0.2f);
            cam.ClearEnabled = false;

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (cam);
            //node.Translate (-mapWidth/2, -mapHeight/2, 0);

            node.GroupID = 0xffff0000;

            return node;
        }
    }
}
