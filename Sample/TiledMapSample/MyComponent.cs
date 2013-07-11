using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMap {
    public class MyComponent : Component {
        int x;
        int y;

        public int MyProp1 {
            get;
            set;
        }

        public GridBoard Map {
            get;
            set;
        }

        public GridBoard Collision {
            get;
            set;
        }

        public override void OnAttached () {
        }

        public override void OnUpdate (long msec) {

            if (Input != null) {
                var label1 = Node.Root.Find ("Label").GetComponent (0) as Label;
                var label2 = Node.Root.Find ("Label").GetComponent (1) as Label;
                var nx = this.x;
                var ny = this.y;

                if (Input.GetKey (KeyCode.LeftArrow)) {
                    nx = nx - 1;
                }
                if (Input.GetKey (KeyCode.RightArrow)) {
                    nx = nx + 1;
                }
                if (Input.GetKey (KeyCode.UpArrow)) {
                    ny = ny - 1;
                }
                if (Input.GetKey (KeyCode.DownArrow)) {
                    ny = ny + 1;
                }
                nx = MyMath.Clamp (nx, 0, Map.Width - 1);
                ny = MyMath.Clamp (ny, 0, Map.Height - 1);
                if (Collision.GetTile (nx, ny) == null) {
                    this.x = nx;
                    this.y = ny;
                    Node.Translation = Map[y, x].Translation;
                }

                label1.Text = "KeyCodes = " + String.Join (", ", Input.Keys.Select (code => code.ToString ()));
                label2.Text = "Mouse = " + Input.MousePosition;
            }
        }
    }
}
