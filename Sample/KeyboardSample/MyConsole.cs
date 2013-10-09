using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.KeyboardSample {
    public class MyConsole: Component {
        
        const int lineCount = 10;


        public static Node Create (Vector3 pos) {
            var cmp = new MyConsole ();

            var spr = new Sprite (300, 240);
            spr.Color = new Color (0, 0, 0, 128);

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (spr);

            for (var i = 0; i < lineCount; i++) {
                var label = new Label ();
                label.SetOffset (10, 16 * i);
                label.Text = "> ";
                node.Attach (label);
            }

            node.Translation = pos;

            return node;
        }

        public override void OnKeyPressed (KeyCode key) {
            GetComponent<Label> (lineCount - 1).Text += key.ToString ();
        }

        public override void OnUpdate (long msec) {

            if (Input.GetKeyDown(KeyCode.Return)) {
                for (var i = 0; i < lineCount-1; i++) {
                    GetComponent<Label> (i).Text = GetComponent<Label> (i + 1).Text;
                }
                GetComponent<Label> (lineCount - 1).Text = "> ";
            }
            
            base.OnUpdate (msec);
        }
    }
}
