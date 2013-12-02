using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {

    /// <summary>
    /// 戦闘のメイン パネルのコンポーネント
    /// </summary>
    /// <remarks>
    /// 画面の中央に表示されるメインの表示部分です。
    /// ポーズは”標準待機”と”被弾”があります。
    /// ポーズはキャラクターではなくこのクラスが持ちます。
    /// 微妙に思えますが表示したいポーズはキャラクターのパラメーターと言うよりも、
    /// 表示するパネルが持つべきです。
    /// 現在は真姫しか表示できません。
    /// </remarks>
    public class MyMainBattlePanel : Component {

        #region Field
        Sprite base0;    // 基本の裸
        Sprite parts0;   // 部位0
        Sprite parts1;
        Sprite parts2;
        Sprite parts3;
        Sprite parts4;
        Sprite parts5;
        Sprite parts6;
        string pose;     // 標準待機or被弾絵
        #endregion

        #region Constructor
        public MyMainBattlePanel () {
            this.pose = "標準待機";
        }
        #endregion

        #region Method

        /// <summary>
        /// メイン戦闘パネル ノードの作成
        /// </summary>
        /// <returns></returns>
        public static Node Create () {
            var cmp = new MyMainBattlePanel ();

            var bg = new Sprite (800, 600);
            bg.AddTexture (Resource.GetTexture ("共通/青空.png"));
            bg.Color = new Color (200, 200, 200, 255);

            var base0 = new Sprite (800, 600);
            var parts0 = new Sprite (800, 600);
            var parts1 = new Sprite (800, 600);
            var parts2 = new Sprite (800, 600);
            var parts3 = new Sprite (800, 600);
            var parts4 = new Sprite (800, 600);
            var parts5 = new Sprite (800, 600);
            var parts6 = new Sprite (800, 600);
            base0.AddTexture (Resource.GetDefaultTexture ());
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
            node.Attach (bg);
            node.Attach (base0);
            node.Attach (parts0);
            node.Attach (parts1);
            node.Attach (parts2);
            node.Attach (parts3);
            node.Attach (parts4);
            node.Attach (parts5);
            node.Attach (parts6);
            node.Attach (mbox1);
            node.Attach (mbox2);

            cmp.base0 = base0;
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
                    base0.SetTexture (0, ch.GetTexture ("戦闘", "メイン", "基本絵", pose));
                    parts0.SetTexture (0, ch.GetTexture ("戦闘", "メイン", "パンツ", pose));
                    parts1.SetTexture (0, ch.GetTexture ("戦闘", "メイン", "ブラ", pose));
                    parts2.SetTexture (0, ch.GetTexture ("戦闘", "メイン", "インナー", pose));
                    parts3.SetTexture (0, ch.GetTexture ("戦闘", "メイン", "上腕", pose));
                    parts4.SetTexture (0, ch.GetTexture ("戦闘", "メイン", "上着", pose));
                    parts5.SetTexture (0, ch.GetTexture ("戦闘", "メイン", "スカート", pose));
                    parts6.SetTexture (0, ch.GetTexture ("戦闘", "メイン", "下腕", pose));
                }
            }
        }
        #endregion

    }
}
