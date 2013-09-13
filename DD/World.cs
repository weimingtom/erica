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
        ILookup<string, Node> allNodesGroupedByGroup;  // 全ノードのキャッシュ（グループでグルーピング済み）
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
                var cam = value.GetComponent<Camera> ();
                if (cam == null) {
                    throw new ArgumentException ("Node has no Camera");
                }
                this.activeCamera = value;

                // Viewの変更をここで行わないと
                // ピクセルとワールド座標の対応（mapPixelToCoord()）が正しい値を返さなくなる
                var g2d = Graphics2D.GetInstance ();
                var win = g2d.GetWindow ();
                cam.SetupView (win);
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
        public CollisionAnalyzer CollisionAnlyzer {
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
        /// </remarks>
        /// <param name="msec">現在時刻 (msec)</param>
        public void Update (long msec) {

            /// 検索用キャッシュの更新
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
        /// 物理演算ワールドの更新
        /// </summary>
        /// <remarks>
        /// 注意：現在の実装では時刻の指定がバグっている。
        /// ここで指定するのは現在時刻だが BulletPhysics が必要とするのはデルタ タイム。
        /// 現在の所前のフレームの時刻を保存していないので計算不可能。
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
        /// </remarks>
        public void Deliver () {
            var po = GetComponent<PostOffice> ();
            po.Deliver ();

        }


        /// <summary>
        /// コリジョンの解決
        /// </summary>
        public void Analyze() {
            var cr = GetComponent<CollisionAnalyzer> ();
            cr.Analyze ();
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

            return Finds(name).FirstOrDefault ();
        }

        /// <summary>
        /// ノードの検索（高速）
        /// </summary>
        /// <remarks>
        /// このメソッドは <see cref="Node.Find(Func<Node,bool>)"/> をキャッシュを使った高速バージョンに置き換えます。
        /// ノード キャッシュは <see cref="World.Update"/> を呼んだタイミングで更新されます。
        /// 従って新しくインスタンス化したゲーム オブジェクトは次のフレームから検索で発見されるようになります。
        /// </remarks>
        /// <param name="pred">条件式</param>
        /// <returns></returns>
        /// <seealso cref="Node.Find(Func<Node,bool>)"/>
        public Node Find (Func<Node, bool> pred) {
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
        /// このメソッドは <see cref="Node.Finds(Func<Node,bool>))"/> をキャッシュを使った高速バージョンに置き換えます。
        /// ノード キャッシュは <see cref="World.Update"/> を呼んだタイミングで更新されます。
        /// 従って新しくインスタンス化したゲーム オブジェクトは次のフレームから検索で発見されるようになります。
        /// </remarks>
        /// <param name="pred">条件式</param>
        /// <returns></returns>
        /// <seealso cref="Node.Finds(Func<Node,bool>)"/>
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
            return Distance (nodeA, nodeB) == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeA"></param>
        /// <param name="nodeB"></param>
        /// <returns></returns>
        public float Distance (Node nodeA, Node nodeB) {
            if (nodeA == null || !nodeA.Is<CollisionObject> () || nodeB == null || !nodeB.Is<CollisionObject> ()) {
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
        /// 1つのノードにレイキャスト
        /// </summary>
        /// <remarks>
        /// ノードに対してレイキャストを行いレイとノードが交差する距離を返します。
        /// 交差しない場合は 0 を返します。このメソッドが負の値を返すことはありません。
        /// レイの開始地点がコリジョン内部の場合はそのレイとノードは交差しません。
        /// ノードのどちらか一方、または両方が <c>null</c> またはコリジョン形状がアタッチされていない場合は <c>NaN</c> が返ります。
        /// </remarks>
        /// <note>
        /// 現状ではFarrseerを使用しているためZを考慮しない。いずれ変更する。
        /// </note>
        /// <param name="nodeA">ノードA</param>
        /// <param name="start">レイキャストの開始地点（グローバル座標）</param>
        /// <param name="end">レイキャストの終了地点（グローバル座標）</param>
        /// <param name="collideWith">コリジョン対象を表すビットマスク</param>
        /// <returns>0より大きな浮動小数値、または0、測定不能の時 <c>NaN</c>.</returns>
        public IEnumerable<RaycastResult> RayCast (Vector3 start, Vector3 end, int collideWith = -1) {
            return CollisionAnlyzer.RayCast (start, end, collideWith);
        }

        public Node Pick (Vector3 start, Vector3 end, int collideWith = -1) {
            return (from result in CollisionAnlyzer.RayCast (start, end, collideWith)
                   select result.Node).FirstOrDefault();
        }

        public RaycastResult Sweep (Node node, Vector3 move) {
            if (node == null) {
                return new RaycastResult ();
            }
            if (node.World != this) {
                throw new ArgumentException ("Node belongs to another world");
            }

            var colA = node.GetComponent<CollisionObject> ();

            return CollisionAnlyzer.Sweep(colA, move);
        }

        /// <summary>
        /// シーンの削除
        /// </summary>
        /// <remarks>
        /// すべてのシーン ノードおよびそこにアタッチされていたすべてのコンポーネントを安全に削除します。
        /// すべてのノードおよびコンポーネントは（実装されていれば）Dispose() されます。
        /// <note>
        /// シーンノードの削除の順番は順不同だが、微妙に下から削除した方がいい気がする。
        /// SceneDepthプロパティを実装すれば可能。
        /// Worldだけは一番最後に削除されることが保証されている（でないといろいろ困る）。
        /// </note>
        /// </remarks>
        public new void Destroy () {
            
            foreach (var node in base.Downwards.Skip(1).Reverse().ToArray ()) {
                node.Destroy ();
            }
            base.Destroy ();
        }
        #endregion
    }
}
