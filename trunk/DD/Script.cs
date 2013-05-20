using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// シーン クラス
    /// </summary>
    /// <remarks>
    /// シーンはゲームを構成する基本的な単位です。
    /// 通常は1シーン1画面に相当します。
    /// </remarks>
    public class Script : Node {
        #region Field
        Director director;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Script () : this("") {
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// シーン名は単なる識別用の文字列でエンジン内部では使用しません。
        /// </remarks>
        /// <param name="name">シーン名</param>
        public Script (string name) : base(name) {
        }
        #endregion

        #region Property

        /// <summary>
        /// ディレクター
        /// </summary>
        /// <remarks>
        /// このシーンを制御するディレクター
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
