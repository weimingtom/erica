using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
   public class MyHpBar : Component {
       float maxHp;
       float maxMp;
       float hp;
       float mp;

       public MyHpBar () {
           this.hp = 100;
           this.mp = 100;
           this.MaxHp = 100;
           this.MaxMp = 100;
       }

       public float MaxHp {
           get { return maxHp; }
           set { this.maxHp = value; }
       }

       public float MaxMp {
           get { return maxMp; }
           set { this.maxMp = value; }
       }

       public float Hp {
           get { return hp; }
           set { this.hp = value; }
       }

       public float Mp {
           get { return mp; }
           set { this.mp = value; }
       }

       public static Node Create (Vector3 pos) {
           var cmp = new MyHpBar ();

           var spr = new Sprite ();
           spr.AddTexture (Resource.GetTexture ("共通/Hpバー.png"));

           var maxHp = new Sprite (98, 6);
           maxHp.AddTexture (Resource.GetDefaultTexture ());
           maxHp.SetOffset (5, 4);
           maxHp.Color = new Color (255, 109, 48); // 橙

           var hp = new Sprite (98, 6);
           hp.AddTexture (Resource.GetDefaultTexture ());
           hp.SetOffset (5, 4);
           hp.Color = new Color (255, 255, 0); // 黄

           var maxMp = new Sprite (98, 6);
           maxMp.AddTexture (Resource.GetDefaultTexture ());
           maxMp.SetOffset (5, 13);
           maxMp.Color = new Color (240, 119, 160); // ピンク

           var mp = new Sprite (98, 6);
           mp.AddTexture (Resource.GetDefaultTexture ());
           mp.SetOffset (5, 13);
           mp.Color = new Color (56,202,127); // 青緑

           var node = new Node ("HpBar");
           node.Attach (cmp);
           node.Attach (maxHp);
           node.Attach (hp);
           node.Attach (maxMp);
           node.Attach (mp);
           node.Attach (spr);

           node.AddUserData ("HP", hp);
           node.AddUserData ("MP", mp);
       
           node.Translation = pos;

           cmp.hp = 50;
           cmp.mp = 70;

           return node;
       }

       public override void OnUpdate (long msec) {
           var hp = GetUserData<Sprite> ("HP");
           var mp = GetUserData<Sprite> ("MP");

           hp.Resize((int)(98*Hp / MaxHp), 6);
           mp.Resize ((int)(98 * Mp / MaxMp), 6);
       }
    }
}
