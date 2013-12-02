using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {

    /// <summary>
    /// 味方ステータスを表示するパネル コンポーネント
    /// </summary>
    public class MyMemberStatusPanel : Component {

        public MyMemberStatusPanel () {
        }

        /// <summary>
        /// 味方ステータスを表示するパネル ノードを作成
        /// </summary>
        /// <param name="pos">位置</param>
        /// <param name="team">味方チーム</param>
        /// <returns></returns>
        public static Node Create (Vector3 pos, MyMember team) {
            var cmp = new MyMemberStatusPanel ();

            var node = new Node ("味方ステータス表示パネル");
            node.Attach (cmp);

            for(var i=0; i < team.CharacterCount; i++){
                var ch = team.GetCharacter (i);
                node.AddChild (MyCharactreStatusPanel.Create (new Vector3 ((i%2)*30, i*50, 0), ch));
            }

            node.Translation = pos;

            return node;
        }



    }
}
