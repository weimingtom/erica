using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.RTSSample {
    public class MyMouseSelector : Component {
        Vector2 start;

        public MyMouseSelector () {
            this.start = new Vector2 (0, 0);
        }

        public static Node Create () {
            var cmp = new MyMouseSelector ();

            var spr = new Sprite (1, 1);
            spr.AddTexture (new Texture ("media/image128x128(Purple).png"));
            spr.SetColor (255, 255, 255, 64);

            var col = new BoxCollisionShape (600, 900, 0);
            col.SetOffset (600, 900, 0);

            var node = new Node ("MouseSelector");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.DrawPriority = -1;

            node.Drawable = false;

            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var pos = new Vector3 (x, y, 0);
            if (button == MouseButton.Left) {
                var nodes = from node in World.Downwards
                            where node.Name == "MyTank"
                            let tank = node.GetComponent<MyTank> ()
                            let ctrl = node.GetComponent<MyController> ()
                            where tank.Selected == true
                            select new { tank, ctrl };
                var dir = Input.Shift ? MyDirection.Back : MyDirection.Forward;

                // 現在選択中のタンクに対して移動マーカーの設定
                foreach (var node in nodes) {
                    var marker = MyMarker.Create (pos, dir);
                    World.AddChild (marker);

                    node.tank.Selected = false;
                    node.ctrl.TargetMarker = marker;
                }
                this.start = new Vector2 (x, y);
                Node.Drawable = true;
            }

        }

        public override void OnMouseButtonReleased (MouseButton button, float x, float y) {

            var pos = Graphics2D.GetInstance ().GetMousePosition ();
            if (button == MouseButton.Left) {
                x = Math.Min (start.X, pos.X);
                y = Math.Min (start.Y, pos.Y);
                var width = (int)MyMath.Clamp (Math.Abs (pos.X - start.X), 2, 1200);
                var height = (int)MyMath.Clamp (Math.Abs (pos.Y - start.Y), 2, 1800);

                var mycol = new BoxCollisionShape (width / 2, height / 2, 0);
                mycol.SetOffset (x + width / 2, y + height / 2, 0);
                var mytra = Node.GlobalTransform;

                var tanks = from node in World.Downwards
                            where node.Name == "MyTank"
                            let tank = node.GetComponent<MyTank> ()
                            let col = node.GetComponent<CollisionShape> ()
                            let tra = node.GlobalTransform
                            where col != null
                            where Physics2D.Distance (col, tra, mycol, mytra) == 0
                            select tank;
                foreach (var tank in tanks) {
                    tank.Selected = true;
                }

                this.start = new Vector2 (0, 0);
                Node.Drawable = false;
            }


        }


        public override void OnUpdateInit (long msec) {
        }

        public override void OnUpdate (long msec) {
            if (Node.Drawable) {
                var pos = Graphics2D.GetInstance ().GetMousePosition ();

                var x = Math.Min (start.X, pos.X);
                var y = Math.Min (start.Y, pos.Y);
                var width = (int)MyMath.Clamp (Math.Abs (pos.X - start.X), 1, 1200);
                var height = (int)MyMath.Clamp (Math.Abs (pos.Y - start.Y), 1, 1800);

                var spr = GetComponent<Sprite> ();
                spr.SetOffset (x, y);
                spr.Resize (width, height);
            }
        }

    }
}
