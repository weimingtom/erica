using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;

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
        Dictionary<string, object> prop;
        IEnumerable<Node> allNodes;                    // 全ノードのキャッシュ
        ILookup<string, Node> allNodesGroupedByName;   // 全ノードのキャッシュ（名前でグルーピング済み）
        #endregion


        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public World ()
            : this ("") {
            // 省略時 name=""でいい
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// スクリプト名は単なる識別用の文字列でエンジン内部では使用しません。
        /// <see cref="World"/> オブジェクトはデフォルトで以下のコンポーネントが自動でアタッチされています。
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
            this.prop = new Dictionary<string, object> ();

            this.Attach (new InputReceiver ());
            this.Attach (new AnimationController ());
            this.Attach (new SoundPlayer ());
            this.Attach (new PostOffice ());
            this.Attach (new CollisionAnalyzer ());
            this.Attach (new Physics.PhysicsSimulator ());
            this.Attach (new NodeDestroyer ());
            this.Attach (new ClockTower ());

            // キャッシュ
            this.allNodes = null;
            this.allNodesGroupedByName = null;
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
                if (!value.Has<Camera>()) {
                    throw new ArgumentException ("This node has no Camera");
                }
                this.activeCamera = value;

                var cam = value.GetComponent<Camera> ();

                // Viewの変更をここで行わないと
                // ピクセルとワールド座標の対応（mapPixelToCoord()）が正しい値を返さなくなる
                var g2d = Graphics2D.GetInstance ();
                cam.SetupView (g2d.GetWindow ());
            }
        }

        /// <summary>
        /// グローバル プロパティの取得
        /// </summary>
        public Dictionary<string, object> Properties {
            get { return prop; }
        }


        /// <summary>
        /// デフォルトのインプット レシーバー
        /// </summary>
        public InputReceiver InputReceiver {
            get { return GetComponent<InputReceiver> (); }
        }

        /// <summary>
        /// デフォルトの時計塔
        /// </summary>
        public ClockTower ClockTower {
            get { return GetComponent<ClockTower> (); }
        }

        /// <summary>
        /// デフォルトのノード削除コンポーネント
        /// </summary>
        public NodeDestroyer NodeDestroyer {
            get { return GetComponent<NodeDestroyer> (); }
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

        /// <summary>
        /// デフォルトの郵便局
        /// </summary>
        public PostOffice PostOffice {
            get { return GetComponent<PostOffice> (); }
        }

        /// <summary>
        /// デフォルトのコリジョン分析機
        /// </summary>
        public CollisionAnalyzer CollisionAnalyzer {
            get { return GetComponent<CollisionAnalyzer> (); }
        }

        /// <summary>
        /// デフォルトの物理演算機
        /// </summary>
        public Physics.PhysicsSimulator PhysicsSimulator {
            get { return GetComponent<Physics.PhysicsSimulator> (); }
        }

        /// <summary>
        /// 全ノードを列挙する列挙子（高速）
        /// </summary>
        /// <remarks>
        /// このメソッドはプロパティを <see cref="Node.Downwards"/> をキャッシュを使った高速バージョンに置き換えます。
        /// ノード キャッシュは <see cref="World.Update"/> を呼んだタイミングで更新されます。
        /// 従って新しくインスタンス化したゲーム オブジェクトは次のフレームから検索で発見されるようになります。
        /// </remarks>
        /// <seealso cref="Node.Downwards"/>
        public new IEnumerable<Node> Downwards {
            get {
                if (allNodes == null) {
                    StoreNodeCache ();
                }
                return allNodes;
            }
        }


        #endregion

        #region Method

        /// <summary>
        /// グローバル プロパティの設定
        /// </summary>
        /// <remarks>
        /// 登録済みの名前の場合、新しい値で置き換えます。
        /// </remarks>
        /// <typeparam name="T">プロパティ値の型</typeparam>
        /// <param name="name">プロパティの名前</param>
        /// <param name="value">プロパティの値</param>
        public void SetProperty<T> (string name, T value) {
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null");
            }
            this.prop[name] = value;
        }

        /// <summary>
        /// グローバル プロパティの取得
        /// </summary>
        /// <remarks>
        /// 指定の名前のプロパティが見つからなかった場合、基底のデフォルト値を返します。
        /// </remarks>
        /// <typeparam name="T">プロパティの型</typeparam>
        /// <param name="name">プロパティの名前</param>
        /// <returns></returns>
        public T GetProperty<T> (string name) {
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null");
            }
            object value;
            prop.TryGetValue (name, out value);
            return (T)(value ?? default (T));
        }

        /// <summary>
        /// グローバル プロパティの取得
        /// </summary>
        /// <remarks>
        /// 指定の名前のプロパティが見つからなかった場合、ユーザー指定のデフォルト値を返します。
        /// </remarks>
        /// <typeparam name="T">プロパティの型</typeparam>
        /// <param name="name">プロパティの名前</param>
        /// <param name="defaultValue">デフォルト値</param>
        /// <returns></returns>
        public T GetProperty<T> (string name, T defaultValue) {
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null");
            }
            if (!prop.ContainsKey (name)) {
                return defaultValue;
            }
            return (T)prop[name];
        }

        /// <summary>
        /// アニメートの実行
        /// </summary>
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
        /// <remarks>
        /// ゲーム ロジックを更新します。
        /// またこのメソッドに入るタイミングでノード キャッシュを更新します。
        /// 原則として1フレームに1回呼び出して下さい。
        /// </remarks>
        /// <param name="msec">現在時刻 (msec)</param>
        public void Update (long msec) {

            // 検索用キャッシュの更新
            StoreNodeCache ();

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

        /// <summary>
        /// コリジョンの解決
        /// </summary>
        /// <remarks>
        /// コリジョン解析機を使ってシーンのコリジョンを解決し、コリジョン状態の変化に応じて
        /// <see cref="Component.OnCollisionEnter"/> と <see cref="Component.OnCollisionExit"/> を呼び出します。 
        /// 原則として1フレームに1回呼び出して下さい。
        /// </remarks>
        public void CollisionUpdate () {
            var nodes = from node in Downwards
                        where node.Upwards.Aggregate (true, (x, y) => x & y.Updatable) == true
                        orderby node.UpdatePriority ascending
                        select node;

            // OnCollisionUpdateInit() の呼び出し
            foreach (var node in nodes) {
                foreach (var comp in node.Components.ToArray ()) {
                    if (!comp.IsCollisionUpdateInitCalled) {
                        comp.OnCollisionUpdateInit (0);
                        comp.IsCollisionUpdateInitCalled = true;
                    }
                }
            }

            // OnCollisionUpdate() の呼び出し
            foreach (var node in nodes) {
                foreach (var comp in node.Components.ToArray ()) {
                    comp.OnCollisionUpdate (0);
                }
            }

            // コリジョンの解析と Enter, Exit の呼び出し
            var ca = GetComponent<CollisionAnalyzer> ();
            ca.Analyze ();

        }



        /// <summary>
        /// 物理演算ワールドの更新
        /// </summary>
        /// <remarks>
        /// 物理演算を使用する場合は、原則として1フレームに1回呼び出して下さい。
        /// <note>
        /// 注意：現在の実装では時刻の指定がバグっている。
        /// ここで指定するのは現在時刻だが BulletPhysics が必要とするのはデルタ タイム。
        /// 現在の所前のフレームの時刻を保存していないので計算不可能。
        /// </note>
        /// </remarks>
        /// <param name="msec">現在時刻 (msec)</param>
        public void PhysicsUpdate (long msec) {

            var nodes = from node in Downwards
                        where node.Upwards.Aggregate (true, (x, y) => x & y.Updatable) == true
                        orderby node.UpdatePriority ascending
                        select node;

            // OnPhysicsUpdateInit() の呼び出し
            foreach (var node in nodes) {
                foreach (var comp in node.Components.ToArray ()) {
                    if (!comp.IsPhysicsUpdateInitCalled) {
                        comp.OnPhysicsUpdateInit (msec);
                        comp.IsPhysicsUpdateInitCalled = true;
                    }
                }
            }

            // 物理シミュレーションの実行
            var pa = GetComponent<Physics.PhysicsSimulator> ();
            pa.Step (msec);

            // OnPhysicsUpdate() の呼び出し
            foreach (var node in nodes) {
                foreach (var comp in node.Components.ToArray ()) {
                    comp.OnPhysicsUpdate (msec);
                }
            }
        }

        /// <summary>
        /// 配達処理の実行
        /// </summary>
        /// <remarks>
        /// ゲームオブジェクト間のメッセージ通信を処理します。
        /// 原則として1フレームに1回呼び出して下さい。
        /// </remarks>
        public void Deliver () {
            var po = GetComponent<PostOffice> ();
            po.Deliver ();

        }


        /// <summary>
        /// ノードキャッシュの更新
        /// </summary>
        private void StoreNodeCache () {
            this.allNodes = base.Downwards;
            this.allNodesGroupedByName = allNodes.ToLookup (x => x.Name);
        }

        /// <summary>
        /// ノードの検索（高速）
        /// </summary>
        /// このメソッドは <see cref="Node.Find(string)"/> をキャッシュを使った高速バージョンに置き換えます。
        /// ノード キャッシュは <see cref="World.Update"/> を呼んだタイミングで更新されます。
        /// 従って新しくインスタンス化したゲーム オブジェクトは次のフレームから検索で発見されるようになります。
        /// <param name="name">ノード名</param>
        /// <returns></returns>
        /// <seealso cref="Node.Find(string)"/>
        public new Node Find (string name) {
            if (allNodesGroupedByName == null) {
                StoreNodeCache ();
            }

            return Finds (name).FirstOrDefault ();
        }

        /// <summary>
        /// ノードの検索（高速）
        /// </summary>
        /// <remarks>
        /// このメソッドは <see cref="Node.Find(Func{Node,bool})"/> をキャッシュを使った高速バージョンに置き換えます。
        /// ノード キャッシュは <see cref="World.Update"/> を呼んだタイミングで更新されます。
        /// 従って新しくインスタンス化したゲーム オブジェクトは次のフレームから検索で発見されるようになります。
        /// </remarks>
        /// <param name="pred">条件式</param>
        /// <returns></returns>
        /// <seealso cref="Node.Find(Func{Node,bool})"/>
        public new Node Find (Func<Node, bool> pred) {
            return Finds (pred).FirstOrDefault ();
        }


        /// <summary>
        /// ノードの検索（高速）
        /// </summary>
        /// <remarks>
        /// このメソッドは <see cref="Node.Finds(string)"/> をキャッシュを使った高速バージョンに置き換えます。
        /// ノード キャッシュは <see cref="World.Update"/> を呼んだタイミングで更新されます。
        /// 従って新しくインスタンス化したゲーム オブジェクトは次のフレームから検索で発見されるようになります。
        /// </remarks>
        /// <param name="name">ノード名</param>
        /// <returns></returns>
        /// <seealso cref="Node.Finds(string)"/>
        public new IEnumerable<Node> Finds (string name) {
            if (allNodesGroupedByName == null) {
                StoreNodeCache ();
            }

            return allNodesGroupedByName[name];
        }


        /// <summary>
        /// ノードの検索（高速）
        /// </summary>
        /// <remarks>
        /// このメソッドは <see cref="Node.Finds(Func{Node,bool})"/> をキャッシュを使った高速バージョンに置き換えます。
        /// ノード キャッシュは <see cref="World.Update"/> を呼んだタイミングで更新されます。
        /// 従って新しくインスタンス化したゲーム オブジェクトは次のフレームから検索で発見されるようになります。
        /// </remarks>
        /// <param name="pred">条件式</param>
        /// <returns></returns>
        /// <seealso cref="Node.Finds(Func{Node,bool})"/>
        public new IEnumerable<Node> Finds (Func<Node, bool> pred) {
            return from node in Downwards
                   where pred (node) == true
                   select node;
        }



        /// <summary>
        /// 2つのノードのコリジョンの重複判定
        /// </summary>
        /// <remarks>
        /// ノードのどちらか一方、または両方が <c>null</c> またはコリジョン形状がアタッチされていない場合は <c>false</c> が返ります。
        /// このメソッドは NaN を返しません。
        /// </remarks>
        /// <param name="nodeA">ノードA</param>
        /// <param name="nodeB">ノードA</param>
        /// <returns>重複している時 <c>true</c>, そうでないとき <c>false</c></returns>
        public bool Overlap (Node nodeA, Node nodeB) {
            if (nodeA == null || !nodeA.Has<CollisionObject> () || nodeB == null || !nodeB.Has<CollisionObject> ()) {
                return false;
            }
            var maskA = nodeA.CollisionObject.CollideWith & ~nodeA.CollisionObject.IgnoreWith;
            var maskB = nodeB.CollisionObject.CollideWith & ~nodeB.CollisionObject.IgnoreWith;
            if ((maskA & nodeB.GroupID) == 0 || (maskB & nodeA.GroupID) == 0) {
                return false;
            }
            if (nodeA.World != this || nodeB.World != this) {
                throw new ArgumentException ("Node belongs to another world");
            }


            return Distance (nodeA, nodeB) == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        public float Distance (Node nodeA, Node nodeB) {
            if (nodeA == null || !nodeA.Has<CollisionObject> () || nodeB == null || !nodeB.Has<CollisionObject> ()) {
                return Single.NaN;
            }
            if (nodeA.World != this || nodeB.World != this) {
                throw new ArgumentException ("Node belongs to another world");
            }

            var colA = nodeA.GetComponent<CollisionObject> ();
            var colB = nodeB.GetComponent<CollisionObject> ();

            return CollisionAnalyzer.Distance (colA, colB);
        }

        /// <summary>
        /// シーン全体からレイキャストを行いヒットするすべてのノードを列挙する
        /// </summary>
        /// <remarks>
        /// シーン全体に対してレイキャストを行いヒットするノードを近い順に列挙します。
        /// 内部にレイの開始地点を持つコリジョンは含まれません。
        /// </remarks>
        /// <param name="start">レイキャストの開始地点（グローバル座標）</param>
        /// <param name="end">レイキャストの終了地点（グローバル座標）</param>
        /// <param name="collideWith">コリジョン対象を表すビットマスク</param>
        /// <returns>0より大きな浮動小数値、または0、測定不能の時 <c>NaN</c>.</returns>
        public IEnumerable<RaycastResult> RayCast (Vector3 start, Vector3 end, int collideWith = -1) {
            return CollisionAnalyzer.RayCast (start, end, collideWith);
        }

        /// <summary>
        /// シーン全体から指定のレイキャストでヒットする一番手前のノードを選択する
        /// </summary>
        /// <param name="start">開始地点（ワールド座標）</param>
        /// <param name="end">終了地点（ワールド座標）</param>
        /// <param name="collideWith">コリジョン対象のビットマスク</param>
        /// <returns></returns>
        public Node Pick (Vector3 start, Vector3 end, int collideWith = -1) {
            return (from result in CollisionAnalyzer.RayCast (start, end, collideWith)
                    select result.Node).FirstOrDefault ();
        }

        /// <summary>
        /// ノードのスィープ テスト
        /// </summary>
        /// <param name="node">対象ノード</param>
        /// <param name="move">移動ベクトル</param>
        /// <remarks>
        /// コリジョン形状がアタッチされている対象ノードを指定ベクトル分移動して、衝突判定を行います。
        /// 自分自身にはヒットしませんが、このノードに重なっているノードがあった場合距離 0 が返ることがあります。
        /// コリジョンマスクは <paramref name="node"/> のそれが使用されます。
        /// ノードが <c>null</c> または <see cref="CollisionObject"/> がアタッチ背廷内場合は例外は発生せずヒット無しが返ります。
        /// </remarks>
        /// <returns></returns>
        public RaycastResult Sweep (Node node, Vector3 move) {
            if (node == null || node.CollisionObject == null) {
                return new RaycastResult ();
            }
            if (node.World != this) {
                throw new ArgumentException ("Node belongs to another world");
            }

            return CollisionAnalyzer.Sweep (node.CollisionObject, move);
        }

        /// <summary>
        /// シーン全ての即時削除
        /// </summary>
        /// <remarks>
        /// このシーンの全てのノードを即座に削除します。
        /// </remarks>
        public void Destroy () {

            // このメソッドは Node.Destroy() を上書きして
            // すべてのノードを直ちに処分する
            foreach (var node in base.Downwards.Skip (1).Reverse ().ToArray ()) {
                node.Destroy (-1);
            }
            this.Destroy (-1);
        }

        /// <summary>
        /// シーンのパージ
        /// </summary>
        /// <remarks>
        /// シーンから最終的にノードを削除します。
        /// <see cref="Node.Destroy"/> されたノードは指定時刻以降このメソッドを呼ぶと削除されます。
        /// それまでノードはシーン中に有効なまま存在します。
        /// </remarks>
        public void Purge () {
            NodeDestroyer.Purge ();
        }

        #endregion
    }
}
