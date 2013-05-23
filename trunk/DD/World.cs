using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// ワールド クラス
    /// </summary>
    /// <remarks>
    /// シーンは <see cref="World"/> を頂点とする <see cref="Node"/> の木構造のグラフです。
    /// ゲームのあらゆる要素はノードに所属し、シーンを通して更新されます。
    /// 多くのゲームでは1シーンが1画面に相当します。
    /// </remarks>
    /// <seealso cref="Node"/>
    public class World : Node {
        #region Field
        Director director;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public World () : this("") {
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// スクリプト名は単なる識別用の文字列でエンジン内部では使用しません。
        /// </remarks>
        /// <param name="name">シーン名</param>
        public World (string name) : base(name) {
        }
        #endregion

        #region Property

        /// <summary>
        /// ディレクター
        /// </summary>
        /// <remarks>
        /// このスクリプトを制御するディレクター
        /// </remarks>
        public Director Director {
            get { return director; }
        }
        #endregion

        #region Method
        /// <summary>
        /// ディレクターの設定
        /// </summary>
        /// <param name="dirc">ディレクター</param>
        public void SetDirector (Director dirc) {
            this.director = dirc;
        }
        

        #endregion
    }
}
