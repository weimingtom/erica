using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
   public class MyStatusViewer : Component{
       Label coat;
       Label skirt;
       Label bra;
       Label inner;
       Label pants;
       string name;
       MyCharacter ch;

       /// <summary>
       /// コンストラクター
       /// </summary>
       /// <param name="uniqueName"></param>
       public MyStatusViewer (string uniqueName) {
           this.name = uniqueName;
       }

       /// <summary>
       /// ノードの作成
       /// </summary>
       /// <param name="pos"></param>
       /// <param name="uniqueName"></param>
       /// <returns></returns>
       public static Node Create (Vector3 pos, string uniqueName) {
           var cmp = new MyStatusViewer (uniqueName);

           var spr = new Sprite (70, 70);
           spr.AddTexture (Resource.GetDefaultTexture ());
           spr.Color = Color.Gray;

           var coat = new Label ();
           var skirt = new Label ();
           var inner = new Label ();
           var bra = new Label ();
           var pants = new Label ();
           
           coat.SetOffset (0, 0);
           skirt.SetOffset (0, 12);
           inner.SetOffset (0, 24);
           bra.SetOffset (0, 36);
           pants.SetOffset (0, 48);

           var node = new Node ("CharacterViewer");
           node.Attach (cmp);
           node.Attach (spr);
           node.Attach (coat);
           node.Attach (skirt);
           node.Attach (inner);
           node.Attach (bra);
           node.Attach (pants);


           node.Translation = pos;

           cmp.coat = coat;
           cmp.skirt = skirt;
           cmp.inner = inner;
           cmp.bra = bra;
           cmp.pants = pants;

           return node;
       }

       public override void OnUpdateInit (long msec) {
           ch = World.Find (name).GetComponent<MyCharacter>();
       }


       public override void OnUpdate (long msec) {
           coat.Text = string.Format ("Coat : {0}", ch.CoatLevel);
           skirt.Text = string.Format ("Skirt : {0}", ch.SkirtLevel);
           inner.Text = string.Format ("Inner : {0}", ch.InnerLevel);
           bra.Text = string.Format ("Bra : {0}", ch.BraLevel);
           pants.Text = string.Format ("Pants : {0}", ch.PantsLevel);
       }
    }
}
