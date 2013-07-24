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
    /// ワールドはデフォルトでいくつかのコンポーネントがアタッチされています。
    /// これらのコンポーネントは使用してもしなくてもかまいません。
    /// </remarks>
    /// <seealso cref="Node"/>
    public class World : Node {
        #region Field
        Node activeCamera;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public World ()
            : this ("") {
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// スクリプト名は単なる識別用の文字列でエンジン内部では使用しません。
        /// <see cref="World"/> オブジェクトはデフォルトで以下のコンポーネントがアタッチされています。
        /// <list type="bullet">
        ///   <item><see cref="InputReceiver"/></item>
        ///   <item><see cref="AnimationController"/></item>
        ///   <item><see cref="SoundPlayer"/></item>
        /// </list>
        /// </remarks>
        /// <param name="name">シーン名</param>
        public World (string name)
            : base (name) {
                this.activeCamera = null;

            this.Attach (new InputReceiver ());
            this.Attach (new AnimationController ());
            this.Attach (new SoundPlayer ());
        }
        #endregion

        #region Property

        /// <summary>
        /// 現在アクティブなカメラ ノード
        /// </summary>
        /// <remarks>
        /// エンジンはシーン グラフの中から指定された1つのカメラを選んで描画します。
        /// 描画に使用されるカメラをアクティブなカメラと呼びます。
        /// 省略された時はデフォルトの設定で描画されます。
        /// </remarks>
        public Node ActiveCamera {
            get { return activeCamera; }
            set {
                if (!value.Is<Camera> ()) {
                    throw new ArgumentException ("Node is not Camera");
                }
                this.activeCamera = value;
            }
        }

        /// <summary>
        /// デフォルトのインプット レシーバー
        /// </summary>
        public InputReceiver InputReceiver {
            get { return GetComponent<InputReceiver> (); }
        }

        /// <summary>
        /// デフォルトのアニメーション コントローラー
        /// </summary>
        public AnimationController AnimationController {
            get { return GetComponent<AnimationController> (); }
        }

        /// <summary>
        /// デフォルトのサウンド プレイヤー
        /// </summary>
        public SoundPlayer SoundPlayer {
            get { return GetComponent<SoundPlayer> (); }
        }

        #endregion

        #region Method

        /// <summary>
        /// アニメートの実行
        /// </summary>
        /// <note>
        /// 現在はワールドの関数だが、ディレクターを作成してそちらに移動する事を考えている。
        /// </note>
        /// <param name="msec">現在時刻 (msec)</param>
        /// <param name="dtime">デルタタイム (msec)</param>
        public void Animate (long msec, long dtime) {

            var nodes = from node in Downwards
                        where node.Upwards.Aggregate (true, (x, y) => x & y.Animatable) == true
                        select node;

            foreach (var node in nodes) {
                foreach (var comp in node.Components) {
                    comp.OnAnimate (msec, dtime);
                }
            }
        }

        /// <summary>
        /// 更新処理の実行
        /// </summary>
        /// <note>
        /// 現在はワールドの関数だが、ディレクターを作成してそちらに移動する事を考えている。
        /// </note>
        /// <param name="msec">現在時刻 (msec)</param>
        public void Update (long msec) {

            var nodes = from node in Downwards
                        where node.Upwards.Aggregate (true, (x, y) => x & y.Updatable) == true
                        orderby node.UpdatePriority ascending
                        select node;

            foreach (var node in nodes) {
                // OnUpdate()の中で自分自身をデタッチする事があるので
                // ここの ToArray() は消さないように！！
                foreach (var comp in node.Components.ToArray ()) {
                    if (!comp.IsUpdateInitCalled) {
                        comp.OnUpdateInit (msec);
                        comp.IsUpdateInitCalled = true;
                    }
                    comp.OnUpdate (msec);
                }
            }
        }

        #endregion
    }
}
