using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {

    /// <summary>
    /// 味方メンバー コンポーネント
    /// </summary>
    /// <remarks>
    /// 味方メンバーのデータを一括して保持するノードです。
    /// ゲームに登場する味方データを（非表示or離脱中も含めて）全て保存します。
    /// </remarks>
    public class MyMember : Component {

        #region Field
        Node maki;
        Node nayu;
        Node sayaka;
        Node lily;
        Node rinko;
        Node mayuto;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public MyMember () {
        }
        #endregion

        #region Property
        /// <summary>
        /// 味方キャラクターの個数
        /// </summary>
        public int CharacterCount {
            get{return 6;}
        }

        /// <summary>
        /// 全ての味方キャラクター コンポーネントを列挙する列挙子
        /// </summary>
        public IEnumerable<MyCharacter> Characters {
            get { return new MyCharacter[] { maki.GetComponent<MyCharacter>(),
                                             nayu.GetComponent<MyCharacter>(),
                                             sayaka.GetComponent<MyCharacter>(),
                                             lily.GetComponent<MyCharacter>(),
                                             rinko.GetComponent<MyCharacter>(), 
                                             mayuto.GetComponent<MyCharacter>() };
            }
        }

        /// <summary>
        /// 全ての味方キャラクター ノードを列挙する列挙子
        /// </summary>
        public IEnumerable<Node> CharacterNodes {
            get { return new Node[] { maki, nayu, sayaka, lily, rinko, mayuto }; }
        }

        /// <summary>
        /// 真姫のノード
        /// </summary>
        public Node Maki {
            get { return maki; }
        }

        /// <summary>
        /// 七夕のノード
        /// </summary>
        public Node Nayu {
            get { return nayu; }
        }

        /// <summary>
        /// 爽夏のノード
        /// </summary>
        public Node Sayaka {
            get { return sayaka; }
        }

        /// <summary>
        /// リリィのノード
        /// </summary>
        public Node Lily {
            get { return lily; }
        }

        /// <summary>
        /// 凛子のノード
        /// </summary>
        public Node Rinko {
            get { return rinko; }
        }

        /// <summary>
        /// 真悠人のノード
        /// </summary>
        public Node Mayuto {
            get { return mayuto; }
        }
        #endregion

        /// <summary>
        /// 味方チーム ノードの作成
        /// </summary>
        /// <remarks>
        /// 味方チームを作成します。表示無しのデータの保存のみなので位置情報はありません。
        /// </remarks>
        /// <returns></returns>
        public static Node Create () {
            var cmp = new MyMember ();

            // 味方
            cmp.maki = MyCharacter.Create ("Maki");
            cmp.nayu = MyCharacter.Create ("Nayu");
            cmp.sayaka = MyCharacter.Create ("Sayaka");
            cmp.lily = MyCharacter.Create ("Lily");
            cmp.rinko = MyCharacter.Create ("Rinko");
            cmp.mayuto = MyCharacter.Create ("Mayuto");

            var node = new Node ("Team");
            node.Attach (cmp);

            node.AddChild (cmp.maki);
            node.AddChild (cmp.nayu);
            node.AddChild (cmp.sayaka);
            node.AddChild (cmp.lily);
            node.AddChild (cmp.rinko);
            node.AddChild (cmp.mayuto);

            return node;
        }

        /// <summary>
        /// キャラクターの取得
        /// </summary>
        /// <param name="index">キャラクター インデックス</param>
        /// <returns></returns>
        public MyCharacter GetCharacter (int index) {
            return Characters.ElementAt (index);
        }

    }
}
