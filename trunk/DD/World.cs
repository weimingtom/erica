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
        Dictionary<string, object> prop;
        IEnumerable<Node> allNodes;                                           // 全ノードのキャッシュ
        IOrderedEnumerable<IGrouping<string, Node>> allNodesGroupedByName;    // 全ノードのキャッシュ（名前でグルーピング化）
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
                this.prop = new Dictionary<string, object> ();

            this.Attach (new InputReceiver ());
            this.Attach (new AnimationController ());
            this.Attach (new SoundPlayer ());
            this.Attach (new PostOffice ());

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
                    StoreNodeCache();
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
        /// ノードキャッシュの更新
        /// </summary>
        void StoreNodeCache () {
            this.allNodes = base.Downwards;
            this.allNodesGroupedByName = from node in allNodes
                                         group node by node.Name into groupedNode
                                         orderby groupedNode.Key
                                         select groupedNode;
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

            return (from nodeGroup in allNodesGroupedByName
                    where nodeGroup.Key == name
                    from node in nodeGroup
                    select node).FirstOrDefault ();
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
            
            return from nodeGroup in allNodesGroupedByName
                    where nodeGroup.Key == name
                    from node in nodeGroup
                    select node;
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

        /*
        /// <summary>
        /// ノードのピック
        /// </summary>
        /// <remarks>
        /// 指定の点を内部に持つノードをピックアップします。複数のノードが該当する場合一番 <see cref="DrawPriority"/> が高いノードがピックされます。
        /// （注意）引数のピック位置はグローバル座標です。
        /// 線分を使ったピックは現在未実装です。
        /// </remarks>
        /// <note>
        /// このメソッドは時代遅れ。1点のピックとかあり得ん。
        /// レイキャスト方式でピックするように変更する。
        /// </note>
        /// <param name="x">ピック位置X（グローバル座標）</param>
        /// <param name="y">ピック位置Y（グローバル座標）</param>
        /// <param name="y">ピック位置Z（グローバル座標）</param>
        /// <returns></returns>
        public Node Pick (float x, float y, float z) {

            return (from node in Downwards
                         where Node.Contain (node, x, y, z)
                         orderby DrawPriority
                         select node).FirstOrDefault ();
        }

        /// <summary>
        /// ノードのピック
        /// </summary>
        /// <param name="start">開始地点（グローバル座標）</param>
        /// <param name="end">終了地点（グローバル座標）</param>
        /// <returns></returns>
        public IEnumerable<Node> RayCast (Vector3 start, Vector3 end) {
            return from node in Downwards
                    let f = Node.RayCast (node, start, end)
                    where f > 0
                    orderby f
                    orderby node.DrawPriority
                    select node;
        }
        */

        #endregion
    }
}
