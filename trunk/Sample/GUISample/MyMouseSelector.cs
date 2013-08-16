using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.GUISample {
    public class MyMouseSelector : Component {
        Sprite spr;
        Vector2 start;
        public bool Selecting { get; set; }

        public MyMouseSelector () {
        }

        public static Node Create () {
            var cmp = new MyMouseSelector ();

            cmp.spr = new Sprite (1, 1);
            cmp.spr.AddTexture (new Texture("media/image128x128(Purple).png"));
            cmp.spr.SetColor (255, 255, 255, 64);

            var node = new Node ("MouseSelector");
            node.Attach (cmp);
            node.Attach (cmp.spr);
            node.DrawPriority = -1;
            node.Drawable = false;

            return node;
        }

        public void BeginSelection () {
            this.start = Graphics2D.GetInstance ().GetMousePosition ();
            this.Selecting = true;
            Node.SetGlobalTranslation (start.X, start.Y, 0);
            Node.Drawable = true;

            var nodes = World.Find ("Targets");
            foreach (var node in nodes.Downwards) {
                var tar = node.GetComponent<MyTarget> ();
                if (tar != null) {
                    tar.Selected = false;
                }
            }

        }

        public void EndSelection () {
            var pos = Graphics2D.GetInstance ().GetMousePosition ();
            var width = pos.X - start.X;
            var height = pos.Y - start.Y;

            Console.WriteLine ("Width={0}, Height={1}", width, height);

            width = (width >= 1) ? width : 1;
            height = (height >= 1) ? height : 1;

            if (width > 0 && height > 0) {
                var mycol = new BoxCollisionShape (width / 2, height / 2, 0);
                mycol.SetOffset (width / 2, height / 2, 0);
                var mytra = Node.GlobalTransform;

                var targets = World.Find ("Targets");
                var nodes = from node in targets.Downwards
                            let col = node.GetComponent<CollisionShape> ()
                            let myspr = node.GetComponent<MyTarget> ()
                            where col != null
                            let tra = node.GlobalTransform
                            where Physics2D.Distance (col, tra, mycol, mytra) == 0
                            select node;
                foreach (var node in nodes) {
                    var tar = node.GetComponent<MyTarget> ();
                    if (tar != null) {
                        tar.Selected = true;
                    }
                }
            }

            this.Selecting = false;
            Node.Drawable = false;
        }

        public override void OnPreDraw (object window) {
            if (spr != null) {
                var pos = Graphics2D.GetInstance ().GetMousePosition ();
                var width = (int)(pos.X - start.X);
                var height = (int)(pos.Y - start.Y);
                spr.Resize (width, height);
            }
        }

    }
}
