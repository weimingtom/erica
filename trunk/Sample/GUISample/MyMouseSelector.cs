using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.GUISample {
    public class MyMouseSelector : Component {
        Vector2 start;
        bool selecting;

        public MyMouseSelector () {
        }

        public static Node Create () {
            var cmp = new MyMouseSelector ();

            var spr = new Sprite (1, 1);
            spr.SetColor (255, 64, 64, 64);

            var col = new CollisionObject ();
            col.Shape = new BoxShape (1, 1, 1);

            var node = new Node ("MouseSelector");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);

            node.DrawPriority = -1;
            node.Visible = false;

            return node;
        }

        public void BeginSelect (float x, float y) {
            this.selecting = true;
            this.start = new Vector2 (x, y);

            Node.Visible = false;
        }

        public void EndSelect (float x, float y) {
            this.selecting = false;

            SendMessage ("MouseDeselect", null);

            Node.Visible = false;
        }

        public override void OnUpdate (long msec) {
            if (selecting) {
                var end = Graphics2D.GetInstance ().GetMousePosition ();
                var width = Math.Abs (end.X - start.X);
                var height = Math.Abs (end.Y - start.Y);
                var depth = 2000;
                var center = (start + end) / 2;

                // 原点はそのまま画面の左上でオフセットのみ変更する
                // コリジョン形状って登録後に変えられたっけ？
                if (width > 1 && height > 1) {
                    Node.Visible = true;

                    var spr = GetComponent<Sprite> ();
                    spr.Offset = start;
                    spr.Resize ((int)width, (int)height);

                    var col = GetComponent<CollisionObject> ();
                    col.Offset = new Vector3 (center, 0);
                    col.Shape = new BoxShape (width / 2, height / 2, depth / 2);

                    SendMessage ("MouseSelect", col);
                }
            }
        }




    }
}




