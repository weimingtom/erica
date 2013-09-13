using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.PlatformSample {
    public class MyFloor : Component{
        Node picked;
        Vector2 delta;

        public static Node Create () {
            var cmp = new MyFloor ();

            var spr = new Sprite (800, 40);
            spr.AddTexture(new Texture("media/Rectangle-160x40.png"));

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width / 2, spr.Height / 2, 1);
            col.Offset = new Vector3 (spr.Width / 2, spr.Height / 2, 1);
            
            var node = new Node ("Floor");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
           
            node.SetTranslation (0, 560, 0);

            node.GroupID = (int)MyGroup.Platform;

            return node;
        }

        public override void OnAttached () {
            this.picked = null;
            this.delta = new Vector2 (0, 0);
        }

        public override void OnUpdate (long msec) {

            var g2d = Graphics2D.GetInstance ();
            var pos = g2d.GetMousePosition ();

            if (Input.GetKeyDown (KeyCode.Mouse0)) {
                var start = new Vector3 (pos.X, pos.Y, 1000);
                var end = new Vector3 (pos.X, pos.Y, -1000);
                var node = World.Pick (start, end);
                if (node != null) {
                    this.picked = node;
                    this.delta = pos - new Vector2 (node.Position.X, node.Position.Y);
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0)) {
                this.picked = null;
            }

            if (picked != null) {
                var t = pos - delta;
                picked.Translation = new Vector3(t.X, t.Y, 0);
            }
            
            base.OnUpdate (msec);
        }
    }
}
