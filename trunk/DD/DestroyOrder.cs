using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// 削除命令 クラス
    /// </summary>
    /// <remarks>
    /// Destroy() を使ってノードを 削除予約すると、指定時間（msec）だけ経過した後削除されます。
    /// その間ノードはシーンに存在すると共に、オブジェクト削除コンポーネント <see cref="NodeDestroyer"/> に
    /// 削除予約 <see cref="ReservedDestruction"/> が登録されます。
    /// </remarks>
    public class ReservedDestruction {
        #region Field
        Node node;
        long purgeTime;
        long lifeTime;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="node">ノード</param>
        /// <param name="purgeTime">削除予定時刻（msec）</param>
        public ReservedDestruction (Node node, long purgeTime) {
            this.node = node;
            this.purgeTime = purgeTime;
            this.lifeTime = -1;
        }
        #endregion

        #region Property
        /// <summary>
        /// ノード
        /// </summary>
        public Node Node {
            get { return node; }
        }

        /// <summary>
        /// 削除予定時刻 (msec)
        /// </summary>
        /// <remarks>
        /// ノードはこの時刻以降に削除されます。
        /// </remarks>
        public long PurgeTime {
            get { return purgeTime; }
        }
        
        /// <summary>
        /// 残り寿命（msec）
        /// </summary>
        /// <remarks>
        /// ノードの削除予定時刻までの推定寿命です。0になると削除されます。
        /// </remarks>
        /// <seealso cref="DD.Node.Destroy"/>
        /// <seealso cref="World.Purge"/>
        public long LifeTime {
            get { return lifeTime; }
        }


        #endregion

        #region Method
        /// <summary>
        /// 残り寿命の更新
        /// </summary>
        /// <remarks>
        /// 残り寿命を更新します。
        /// 内部的には最初に設定されたパージ時刻以降で削除されるので、
        /// 残り寿命は単なるユーザー向けの親切設計。
        /// </remarks>
        public void Tick (long msec) {
            this.lifeTime = purgeTime - msec;
        }

        /// <inheritdoc/>
        public override string ToString () {
            return string.Format("Name: {0}, R.I.P.", Node.Name);    
        }
        #endregion


    }

}
