using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
    public class MainBattleWindow : Component {
        Sprite spr;
        Sprite parts0;
        Sprite parts1;
        Sprite parts2;
        Sprite parts3;
        Sprite parts4;
        Sprite parts5;
        Sprite parts6;
        string pose;

        public MainBattleWindow () {
            this.pose = "標準待機";
        }

        public static Node Create () {
            var cmp = new MainBattleWindow ();

            var spr = new Sprite (800, 600);
            var parts0 = new Sprite (800, 600);
            var parts1 = new Sprite (800, 600);
            var parts2 = new Sprite (800, 600);
            var parts3 = new Sprite (800, 600);
            var parts4 = new Sprite (800, 600);
            var parts5 = new Sprite (800, 600);
            var parts6 = new Sprite (800, 600);
            spr.AddTexture (Resource.GetDefaultTexture ());
            parts0.AddTexture (Resource.GetDefaultTexture ());
            parts1.AddTexture (Resource.GetDefaultTexture ());
            parts2.AddTexture (Resource.GetDefaultTexture ());
            parts3.AddTexture (Resource.GetDefaultTexture ());
            parts4.AddTexture (Resource.GetDefaultTexture ());
            parts5.AddTexture (Resource.GetDefaultTexture ());
            parts6.AddTexture (Resource.GetDefaultTexture ());

            var mbox1 = new MailBox ("DisplayCharacter");
            var mbox2 = new MailBox ("DisplayDamagedCharacter");

            var node = new Node ("MainBattleWindow");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (parts0);
            node.Attach (parts1);
            node.Attach (parts2);
            node.Attach (parts3);
            node.Attach (parts4);
            node.Attach (parts5);
            node.Attach (parts6);
            node.Attach (mbox1);
            node.Attach (mbox2);

            cmp.spr = spr;
            cmp.parts0 = parts0;
            cmp.parts1 = parts1;
            cmp.parts2 = parts2;
            cmp.parts3 = parts3;
            cmp.parts4 = parts4;
            cmp.parts5 = parts5;
            cmp.parts6 = parts6;

            return node;
        }

        public override void OnMailBox (Node from, string address, object letter) {
            switch (address) {
                case "DisplayCharacter": this.pose = "標準待機"; break;
                case "DisplayDamagedCharacter": this.pose = "被弾"; break;
            }
        }

        public override void OnUpdate (long msec) {
            var node = World.Find ("Maki");
            if (node != null) {
                var ch = node.GetComponent<MyCharacter> ();
                if (ch != null) {
                    spr.SetTexture (0, ch.GetTexture ("戦闘", "メイン", pose));
                    parts0.SetTexture (0, ch.GetPartsTexture ("戦闘", "メイン", pose, "パンツ"));
                    parts1.SetTexture (0, ch.GetPartsTexture ("戦闘", "メイン", pose, "ブラ"));
                    parts2.SetTexture (0, ch.GetPartsTexture ("戦闘", "メイン", pose, "インナー"));
                    parts3.SetTexture (0, ch.GetPartsTexture ("戦闘", "メイン", pose, "上腕"));
                    parts4.SetTexture (0, ch.GetPartsTexture ("戦闘", "メイン", pose, "上着"));
                    parts5.SetTexture (0, ch.GetPartsTexture ("戦闘", "メイン", pose, "スカート"));
                    parts6.SetTexture (0, ch.GetPartsTexture ("戦闘", "メイン", pose, "下腕"));
                }
            }
        }

    }
}
