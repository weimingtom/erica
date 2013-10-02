using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.CollisionDetectSample {
    public class MyCharacter : Component {

        float speed = 3.77f;
        float rotationSpeed = 3;

        /// <summary>
        /// 
        /// </summary>
        public MyCharacter () {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public static Node Create (Vector3 pos, int groupID) {
            var cmp = new MyCharacter ();

            var spr = new Sprite (64, 64);
            spr.AddTexture (Resource.GetDefaultTexture ());
            spr.Color = Color.Red;
            spr.SetOffset (-32, -32);

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width / 2, spr.Height / 2, 1);
            //col.SetOffset (spr.Width / 2, spr.Height / 2, 1);

            var label = new Label ();
            label.Text = "ID = 0x" + groupID.ToString ("x");
            label.SetOffset (-spr.Width / 2, -spr.Height / 2);

            var node = new Node ("MyCharacter");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (label);

            node.Translation = pos;
            node.GroupID = groupID;

            return node;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void Move (float x, float y, float z) {
            var move = new Vector3 (x, y, z) * speed;

            var result = World.Sweep (Node, move);
            if (result.Hit) {
                move *= result.Fraction;
                if (move.Length > 0 && move.Length > result.Distance - 1) {
                    move *= (result.Distance - 1) / move.Length;
                }
            }
       
            Node.Translate (move.X, move.Y, move.Z);
        }

        void Rotate (float angle) {

            Node.Rotate (angle * rotationSpeed, 0, 0, 1);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msec"></param>
        public override void OnUpdate (long msec) {
            var x = 0;
            var y = 0;
            var angle = 0;

            foreach (var key in Input.Keys) {
                switch (key) {
                    case KeyCode.LeftArrow: x -= 1; break;
                    case KeyCode.RightArrow: x += 1; break;
                    case KeyCode.UpArrow: y -= 1; break;
                    case KeyCode.DownArrow: y += 1; break;
                    case KeyCode.Z: angle -= 1; break;
                    case KeyCode.X: angle += 1; break;
                    case KeyCode.Space: Console.WriteLine ("Position = {0}", Node.Position); break;
                }

            }

            if (x != 0) {
                Move (x, 0, 0);
            }
            if (y != 0) {
                Move (0, y, 0);
            }
            if (angle != 0) {
                Rotate (angle);
            }

            
            var str = "Collide with = ";
            foreach(var obj in Node.CollisionObject.OverlapObjects){
                str += obj.Name + ", ";
            }
            SendMessage ("HUD", str);
        }

    }
}
