using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DonkeyKongSample {
    class MyWorld : Component {
        Node picked;
        Vector2 delta;

        public static World Create () {
            var cmp = new MyWorld ();

            var spr = new Sprite (new Texture ("media/DarkGalaxy.jpg"));

            var clip = new SoundClip ();
            clip.AddTrack (new MusicTrack ("media/BGM(Field04).ogg"));
            clip.Play ();
            clip.Volume = 0.3f;

            var wld = new World ("First Script");
            wld.Attach (cmp);
            wld.Attach (spr);
            wld.UserData.Add (clip.Name, clip);

            wld.DrawPriority = 127;

            return wld;

        }

        public override void OnAttached () {
            this.picked = null;
            this.delta = new Vector2 (0, 0);
        }

        public override void OnUpdate (long msec) {

            var g2d = Graphics2D.GetInstance ();
            var pos = g2d.GetMousePosition ();

            if (Input.GetKeyDown (KeyCode.Mouse0)) {
                var node = Graphics2D.Pick (World, pos.X, pos.Y).FirstOrDefault ();
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
