using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.IsometricSample {
    public class MyClicker : Component{
        Sprite spr;

        public MyClicker (Sprite spr) {
            this.spr = spr;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            spr.SetColor (255, 0, 0, 255);
        }
        public override void OnMouseButtonReleased (MouseButton button, float x, float y) {
            spr.SetColor (255, 255, 255, 255);
        }
      
    }
}
