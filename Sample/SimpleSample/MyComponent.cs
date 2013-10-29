using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using DD.Physics;

namespace DD.Sample.SimpleSample {
    public class MyComponent : Component {

        public MyComponent () {
        }

        public static Node Create (Vector3 pos) {

            var cmp = new MyComponent ();

            var spr = new Sprite (480, 300);
            spr.AddTexture (new Texture ("media/Vanity.jpg"));
            spr.AddTexture (new Texture ("media/Tanks.png"));
            spr.AddTexture (new Texture ("media/TatamiRoom.png"));
            spr.AutoScale = true;

            Console.WriteLine ("tex = " + spr.GetTexture (0));
            Console.WriteLine ("spr = " + spr);

            var col = new CollisionObject();
            col.Shape  = new BoxShape(spr.Width/2, spr.Height/2, 100);
            col.SetOffset (spr.Width/2, spr.Height/2, 0);

            var ctr = new AnimationController ();

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (ctr);
            
            node.Translation = pos;

            var clip = new SoundClip ("Sound");
            clip.AddTrack (new SoundEffectTrack ("media/PinPon.wav"));

            node.UserData.Add (clip.Name, clip);


            return node;
        }

        public override void OnUpdateInit (long msec) {
        }


        public override void OnUpdate (long msec) {
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var clip = Node.UserData["Sound"] as SoundClip;
            clip.Play ();
        }
    
    
    }
}
