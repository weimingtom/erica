using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {

    /// <summary>
    /// 攻撃シーケンス コンポーネント
    /// </summary>
    /// <remarks>
    /// 攻撃を行う一連の手順（ターゲット選択、部位選択、スキル選択）をまとめた最上位のコンポーネント。
    /// 攻撃を行う時はこのノードを作成して下さい。
    /// 現在の所選択完了後の処理は実装されてない。
    /// </remarks>
    public class MyAttackSequence : Component {


        public MyAttackSequence () {
        }

        /// <summary>
        /// 攻撃シーケンス ノードの作成
        /// </summary>
        /// <param name="pos">位置</param>
        /// <returns></returns>
        public static Node Create (Vector3 pos) {
            var cmp = new MyAttackSequence ();

            var label = new Label ();

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (label);

            var targetNames = new string[] { "真姫", "クロペン", "シロペン" };
            var partsNames = new string[] { "おまかせ", "上着", "インナー", "スカート", "パンツ" };
            var skillNames = new string[] { "スキル1", "スキル2", "スキル3", "スキル4", "スキル5" };

            var node1 = MyTargetSelector.Create (new Vector3 (0, 100, 0), targetNames, true);
            var node2 = MyPartsSelector.Create (new Vector3 (0, 100, 0), partsNames, false);
            var node3 = MySkillSelector.Create (new Vector3 (0, 100, 0), skillNames, false);
            {
                var panel1 = node1.GetComponent<MyPanel> ();
                var panel2 = node2.GetComponent<MyPanel> ();
                var panel3 = node3.GetComponent<MyPanel> ();

                panel1.SetNext (panel2);
                panel2.SetNext (panel3);
                panel3.Completed += delegate (object sender, EventArgs e) {
                    label.Text = string.Format("選択完了 : {0}, {1}, {2}", panel1.Selected, panel2.Selected, panel3.Selected);
                };
            }


            node.AddChild (node1);
            node.AddChild (node2);
            node.AddChild (node3);


            node.Translation = pos;

            return node;
        }


    }
}
