using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    public class MyBGM : Component{
        public MyBGM () {
        }

        public static Node Create () {
            var clip = new SoundClip ("media/BGM-BattleScene.ogg", true);
            clip.Play ();
            clip.Volume = 0.5f;

            var ply = new SoundPlayer ();
            ply.AddClip (clip);
            
            var node = new Node ("BGM");
            node.Attach (ply);

            return node;
        }
    }
}
