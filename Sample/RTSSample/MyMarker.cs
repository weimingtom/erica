using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    public enum MyDirection {
        Forward,
        Back
    }
    
    public class MyMarker : Component {



        public MyDirection Direction { get; set; }
        
        public static Node Create (Vector3 pos, MyDirection dir) {
            var cmp = new MyMarker ();
            cmp.Direction = dir;

            var spr = new Sprite (128, 128);
            spr.AddTexture (new Texture ("media/MedicalIconLight.png"));
            spr.SetOffset (-64, -128);

            var node = new Node ("Marker");
            node.Attach (cmp);
            node.Attach (spr);
            node.DrawPriority = -1;

            node.Translation = pos;

            return node;
        }

        public override void OnDraw (object window) {
            base.OnDraw (window);
        }

        public override void OnUpdate (long msec) {
            base.OnUpdate (msec);
        }
    }
}
