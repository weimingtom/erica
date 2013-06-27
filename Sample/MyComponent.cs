using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample {
    public class MyComponent : Component {

        public int MyProp1 {
            get;
            set;
        }

        public override void OnUpdate (long msec) {

            if (Input != null) {
                var label1 = Node.Root.Find (1).GetComponent (0) as Label;
                var label2 = Node.Root.Find (1).GetComponent (1) as Label;

                if (Input.GetKey (KeyCode.LeftArrow)) {
                    Node.Translate (-1, 0, 0);
                }
                if (Input.GetKey (KeyCode.RightArrow)) {
                    Node.Translate (+1, 0, 0);
                }
                if (Input.GetKey (KeyCode.MouseWheeleUp)) {
                    Node.Translate (-10, 0, 0);
                }
                if (Input.GetKey (KeyCode.MouseWheeleDown)) {
                    Node.Translate (+10, 0, 0);
                }
                label1.Text = "KeyCodes = " + String.Join (", ", Input.Keys.Select (x => x.ToString ()));
                label2.Text = "Mouse = " + Input.MousePosition;
            }
        }
    }
}
