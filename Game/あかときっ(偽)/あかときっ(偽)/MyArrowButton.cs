using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
    public class MyArrowButton : Component{

        public enum ActionTarget {
            CoatLevel,
            SkirtLevel,
            InnerLevel,
            BraLevel,
            PantsLevel,
        }

        public enum ActionType {
            Up,
            Down,
        }

        ActionType type;
        ActionTarget target;
   
        public MyArrowButton (ActionTarget target, ActionType type){
            this.type = type;
            this.target = target;
        }

        public static Node Create (Vector3 pos, ActionTarget target, ActionType type) {
            var cmp = new MyArrowButton (target, type);

            var spr = new Sprite (32, 32);
            switch (type) {
                case ActionType.Down: spr.AddTexture (Resource.GetTexture ("共通/LeftArrow.png")); break;
                case ActionType.Up: spr.AddTexture (Resource.GetTexture ("共通/RightArrow.png")); break;
            }
            spr.AutoScale = true;

            var col = new CollisionObject ();
            col.Shape = new BoxShape (spr.Width / 2, spr.Height / 2, 10);
            col.SetOffset (spr.Width / 2, spr.Height / 2, 10);
            
            var node = new Node ();
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);

            node.Position = pos;

            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            switch(type){
                case ActionType.Up: SendMessage ("IncreasePartsLevel", target.ToString ()); break;
                case ActionType.Down: SendMessage ("DecreasePartsLevel", target.ToString ()); break;
            }
        }

    }
}
