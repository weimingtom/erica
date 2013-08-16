using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {

   public class MyContinuousTrack:Component {
       long bornTime;
       long lifeTime;

       public MyContinuousTrack () {
           this.bornTime = 0;
           this.lifeTime = 500;
       }

       public static Node Create (Vector3 pos) {
           var cmp = new MyContinuousTrack ();

           var spr = new Sprite (40, 20);
           spr.AddTexture (Resource.GetTexture ("media/ContinuousTrack.png"));
           spr.SetOffset (-20, -20);

           var node = new Node ("ContinuousTrack");
           node.Attach (cmp);
           node.Attach (spr);

           node.DrawPriority = -1;
           node.Translation = pos;

           return node;
       }

       public override void OnUpdateInit (long msec) {
           this.bornTime = msec;
       }

       public override void OnUpdate (long msec) {
          Node.Opacity = MyMath.Clamp(1 - (msec - bornTime) / lifeTime - 0.5f, 0, 1);

           if (Node.Opacity == 0) {
               Destroy (this);
           }
       }
   }
}
