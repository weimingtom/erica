using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyRader : Component{

        public MyRader () {
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyRader ();

            // マップの20分の1
            var spr = new Sprite (128,128);  
            spr.Color = new Color(32, 64, 128, 128);

            var node = new Node ("Rader");
            node.Attach (cmp);
            node.Attach (spr);

            node.Translation = pos;
            node.DrawPriority = -2;

            return node;
        }

        public override void OnUpdateInit (long msec) {

            var cabbages = World.Find ("CabbageMap").Downwards.Skip (1);
            foreach (var cab in cabbages) {
                var dot = MyRaderDot.Create (cab, Color.Red);
                Node.AddChild (dot);
            }
            
            var mychar = World.Find ("MyCharacter");
            if (mychar != null) {
                var dot = MyRaderDot.Create (mychar, Color.White);
                Node.AddChild (dot);
            }

        }
    }
}
