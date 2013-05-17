using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// コンポーネント クラス
    /// </summary>
    /// <remarks>
    /// コンポーネントはノードにアタッチされて、ノードに様々なの機能を追加します。
    /// 
    /// </remarks>
    public abstract class Component {
        #region Field
        Node node;
        #endregion


        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Component () {
            this.node = null;
        }
        #endregion

        #region Property
        /// <summary>
        /// ノード
        /// </summary>
        public Node Node {
            get { return node; }
        }
        #endregion

        #region Method
        /// <summary>
        /// 親ノードの変更
        /// </summary>
        /// <param name="node">ノード</param>
        internal void SetNode (Node node) {
            this.node = node;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        /// <remarks>
        /// 更新処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// 引数の <paramref name="msec"/> はゲームが起動してからの経過時間をミリ秒単位であらわします。
        /// </remarks>
        /// <param name="msec">ゲームが起動してからの起動時間(msec)</param>
        public virtual void OnUpdate (long msec) {
        }

        /// <summary>
        /// キーボード処理
        /// </summary>
        /// <remarks>
        /// キーボード処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// </remarks>
        public virtual void OnPressed () {
        }

        /// <summary>
        /// マウス処理
        /// </summary>
        /// <remarks>
        /// マウス処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// </remarks>
        public virtual void OnClicked () {
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <remarks>
        /// 描画処理を行う仮想関数のエントリーポイント。
        /// 通常はユーザーがこの仮想関数をオーバーライドする必要はありません。
        /// この仮想関数の適切な実装を提供するのはエンジン側の責任です。
        /// </remarks>
        public virtual void OnDraw (object window) {

        }

        #endregion

    }
}
