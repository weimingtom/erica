using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyPopup : Component {

        public Label Label { get; set; }
        public AnimationTrack Track { get; set; }
        public float DeltaY { get; set; }

        public MyPopup () {
            this.DeltaY = -10;
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyPopup ();
            
            var label = new Label ("キャベツ！");
            label.Color = Color.Red;

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (label);

            node.Translation = pos + new Vector3(0,-10,0);
            node.DrawPriority = -1;

            return node;
        }

        public override void OnUpdateInit (long msec) {
            var clip = new SoundClip ("media/PinPon.wav");
            clip.Volume = 0.2f;
            Sound.AddClip (clip, true);
        }

        public override void OnUpdate (long msec) {
            Node.Translate (0, DeltaY, 0);
            DeltaY += 1;
            if (DeltaY > 0) {
                Destroy (Node, 0);
            }
        }
    }
}
