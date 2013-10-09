using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {

    /// <summary>
    /// オブジェクト破壊 コンポーネント
    /// </summary>
    /// <remarks>
    /// <see cref="Node.Destroy"/> されたオブジェクトは直ちには削除されず指定の msec だけ経過した後削除されます。
    /// その間オブジェクトはシーンに留まると共にこのオブジェクト破壊コンポーネントにも登録されます。
    /// そして寿命が尽きたオブジェクトから順番に <see cref="World.Purge"/> のタイミングで削除されます。
    /// ユーザーがこのコンポーネントをインスタンス化することはありません。
    /// またユーザーが直接このコンポーネントのメソッドを呼び出すこともありません。
    /// </remarks>
    public class NodeDestroyer : Component {
        #region Field
        List<ReservedDestruction> reservs;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public NodeDestroyer () {
            this.reservs = new List<ReservedDestruction> ();
        }
        #endregion

        #region Property
        /// <summary>
        /// （未処理の）削除予約の数
        /// </summary>
        public int ReservationCount {
            get { return reservs.Count(); }
        }

        /// <summary>
        /// すべての削除予約を列挙する列挙子
        /// </summary>
        public IEnumerable<ReservedDestruction> Reserves {
            get { return reservs; }
        }
        #endregion

        #region Method
        /// <summary>
        /// 削除予約の取得
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns></returns>
        public ReservedDestruction GetReservation (int index) {
            if (index < 0 || index > ReservationCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return reservs[index];
        }

        /// <summary>
        /// ノード削除の予約
        /// </summary>
        /// <remarks>
        /// 指定のノード <paramref name="node"/> を指定の時刻 <paramref name="purgeTime"/> にシーンから削除するよう予約します。
        /// ノードは指定時刻に達するまで有効な状態のままシーンに留まります。
        /// 詳しい説明はこのクラスの解説を参照して下しい。
        /// </remarks>
        /// <param name="node">ノード</param>
        /// <param name="purgeTime">削除予定時刻（msec）</param>
        public void Reserve (Node node, long purgeTime) {
            if (node == null) {
                throw new ArgumentNullException ("Node is null");
            }
            if (purgeTime < 0) {
                throw new ArgumentException ("PurgeTime is invalid");
            }

            this.reservs.Add (new ReservedDestruction (node, purgeTime));
        }

        /// <summary>
        /// 削除の実行
        /// </summary>
        /// <remarks>
        /// 削除予約時間に達したノードをすべて削除します。
        /// メモ：残り寿命の更新は <see cref="OnUpdate"/> が呼ばれた時。
        /// 従ってこのクラスにクロックを供給しないと削除されない。
        /// </remarks>
        public void Purge () {

            var expired = from grave in reservs
                          where grave.LifeTime <= 0
                          select grave.Node;

            foreach (var node in expired) {
                node.FinalizeNode ();
            }

            this.reservs.RemoveAll (x => x.LifeTime <= 0);
        }

        /// <inheritdoc/>
        public override void OnUpdate (long msec) {
            foreach (var grave in reservs) {
                grave.Tick (msec);
            }
        }

        /// <inheritdoc/>
        public override string ToString () {
            return string.Format("Reserves : {0}", ReservationCount);
        }
        #endregion



    }
}
