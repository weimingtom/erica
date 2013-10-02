using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.TiledMapSample {
    public class MyGameClear  : Component{
        public MyGameClear () {

        }

        public static Node Create () {
            var cmp = new MyGameClear ();

            var spr = new Sprite (200, 160);
            spr.AddTexture (new Texture("media/GameClear.png"));

            var node = new Node ("GameClear");
            node.Attach (cmp);
            node.Attach (spr);

            node.DrawPriority = -1;
            node.Visible = false;
            
            return node;
        }
        public override void OnUpdateInit (long msec) {
            base.OnUpdateInit (msec);
        }

        public override void OnUpdate (long msec) {
        
            // ここ本当は World.Finds("Cabbage") にするつもりだったが
            // タイルにプロパティを設定すると TiledSharp が落ちるため

            var cabs = World.Find ("CabbageMap").Downwards.Skip(1);
            if (cabs.Count () == 0) {
                Node.Visible = true;
                Node.Updatable = false;
            }
        }
    }
}
