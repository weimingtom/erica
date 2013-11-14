using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
   public class MyStatusController: Component {
       public MyStatusController () {

       }

       public static Node Create (Vector3 pos) {
           var cmp = new MyStatusController ();

           var node = new Node ("StatusController");
           node.Attach (cmp);

           node.AddChild (MyArrowButton.Create (new Vector3 (0, 0, 0), MyArrowButton.ActionTarget.CoatLevel, MyArrowButton.ActionType.Down));
           node.AddChild (MyArrowButton.Create (new Vector3 (32, 0, 0), MyArrowButton.ActionTarget.CoatLevel, MyArrowButton.ActionType.Up));
           node.AddChild (MyArrowButton.Create (new Vector3 (0, 32, 0), MyArrowButton.ActionTarget.SkirtLevel, MyArrowButton.ActionType.Down));
           node.AddChild (MyArrowButton.Create (new Vector3 (32, 32, 0), MyArrowButton.ActionTarget.SkirtLevel, MyArrowButton.ActionType.Up));
           node.AddChild (MyArrowButton.Create (new Vector3 (0, 64, 0), MyArrowButton.ActionTarget.InnerLevel, MyArrowButton.ActionType.Down));
           node.AddChild (MyArrowButton.Create (new Vector3 (32, 64, 0), MyArrowButton.ActionTarget.InnerLevel, MyArrowButton.ActionType.Up));
           node.AddChild (MyArrowButton.Create (new Vector3 (0, 96, 0), MyArrowButton.ActionTarget.BraLevel, MyArrowButton.ActionType.Down));
           node.AddChild (MyArrowButton.Create (new Vector3 (32, 96, 0), MyArrowButton.ActionTarget.BraLevel, MyArrowButton.ActionType.Up));
           node.AddChild (MyArrowButton.Create (new Vector3 (0, 128, 0), MyArrowButton.ActionTarget.PantsLevel, MyArrowButton.ActionType.Down));
           node.AddChild (MyArrowButton.Create (new Vector3 (32, 128, 0), MyArrowButton.ActionTarget.PantsLevel, MyArrowButton.ActionType.Up));

           node.Position = pos;

           return node;
       }
    }
}
