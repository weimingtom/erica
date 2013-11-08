using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;
using System.Data.Entity;

namespace DD {
    /// <summary>
    /// コンポーネント クラス
    /// </summary>
    /// <remarks>
    /// コンポーネントはノードにアタッチされて、ノードに様々なの機能を追加します。
    /// </remarks>
    public class Component {
        #region Field
        Node node;
        bool updateInitIsCalled;
        bool physicsUpdateInitIsCalled;
        bool collisionUpdateInitIsCalled;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Component () {
            this.node = null;
            this.updateInitIsCalled = false;
            this.physicsUpdateInitIsCalled = false;
            this.collisionUpdateInitIsCalled = false;
        }
        #endregion

        #region Property
        /// <summary>
        /// 所属ノード
        /// </summary>
        public Node Node {
            get { return node; }
            internal set { this.node = value; }
        }

        /// <summary>
        /// UpdateInit()が呼ばれた事があるかどうかのフラグ
        /// </summary>
        internal bool IsUpdateInitCalled {
            get { return updateInitIsCalled; }
            set { this.updateInitIsCalled = value; }
        }

        /// <summary>
        /// PhysicsUpdateInit()が呼ばれた事があるかどうかのフラグ
        /// </summary>
        internal bool IsPhysicsUpdateInitCalled {
            get { return physicsUpdateInitIsCalled; }
            set { this.physicsUpdateInitIsCalled = value; }
        }

        /// <summary>
        /// CollisionUpdateInit()が呼ばれた事があるかどうかのフラグ
        /// </summary>
        internal bool IsCollisionUpdateInitCalled {
            get { return collisionUpdateInitIsCalled; }
            set { this.collisionUpdateInitIsCalled = value; }
        }


        /// <summary>
        /// ワールド ノード
        /// </summary>
        /// <remarks>
        /// シーン ツリーの一番上のノード <see cref="World"/> を取得するプロパティです。
        /// </remarks>
        public World World {
            get {
                if (node == null) {
                    return null;
                }
                return node.Root as World;
            }
        }

        /// <summary>
        /// 標準のインプット レシーバー
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされたローカルのインプット レシーバー <see cref="InputReceiver"/> または、それが存在しない場合は
        /// <see cref="World"/> のインプット レシーバー  <see cref="InputReceiver"/> を返します。
        /// <see cref="World"/> クラスは必ずデフォルトのインプット レシーバーを1つ保持しています。
        /// </remarks>
        protected internal InputReceiver Input {
            get {
                return GetComponent<InputReceiver>() ?? World.InputReceiver;
            }
        }

        /// <summary>
        /// 標準の時計塔
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされたローカルの時計塔 <see cref="ClockTower"/> または、それが存在しない場合は
        /// <see cref="World"/> の時計塔  <see cref="ClockTower"/> を返します。
        /// <see cref="World"/> クラスは必ずデフォルトの時計塔を1つ保持しています。
        /// </remarks>
        protected internal ClockTower Time {
            get {
                return GetComponent<ClockTower> () ?? World.ClockTower;
            }
        }

        /// <summary>
        /// 標準のログ記録機能
        /// </summary>
        protected internal Logger Logger {
            get {
                return GetComponent<Logger> () ?? World.Logger;
            }
        }

        /// <summary>
        /// 標準のアニメーション コントローラー
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされたローカルのアニメーション コントローラー <see cref="AnimationController"/> または、それが存在しない場合は
        /// <see cref="World"/> のアニメーション コントローラー  <see cref="AnimationController"/> を返します。
        /// <see cref="World"/> クラスは必ずデフォルトのアニメーション コントローラーを1つ保持しています。
        /// 
        /// </remarks>
        protected internal AnimationController Animation {
            get {
                return GetComponent<AnimationController> () ?? World.AnimationController;
            }
        }

        /// <summary>
        /// 標準のサウンド プレイヤー
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされたローカルのサウンド プレイヤー <see cref="SoundPlayer"/> または、それが存在しない場合は
        /// <see cref="World"/> のサウンド プレイヤー  <see cref="SoundPlayer"/> を返します。
        /// <see cref="World"/> クラスは必ずデフォルトのサウンド プレイヤーを1つ保持しています。
        /// </remarks>
        protected internal SoundPlayer Sound {
            get {
                return GetComponent<SoundPlayer> () ?? World.SoundPlayer;
            }
        }

        /// <summary>
        /// 標準の郵便局（メッセージ通信）
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされたローカルの郵便局 <see cref="PostOffice"/> または、それが存在しない場合は
        /// <see cref="World"/> の郵便局  <see cref="PostOffice"/> を返します。
        /// <see cref="World"/> オブジェクトは必ずデフォルトの郵便局を1つ保持しています。
        /// </remarks>
        protected internal PostOffice PostOffice {
            get { return GetComponent<PostOffice> () ?? World.PostOffice; }
        }

        /// <summary>
        /// 標準のコリジョン解析機
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされたローカルのコリジョン解析機 <see cref="CollisionAnalyzer"/> 、またはそれが存在しない場合は
        /// <see cref="World"/> のコリジョン解析機  <see cref="CollisionAnalyzer"/> を返します。
        /// <see cref="World"/> オブジェクトは必ずデフォルトのコリジョン解析機を1つ保持しています。
        /// </remarks>
        protected internal CollisionAnalyzer CollisionAnalyzer {
            get { return GetComponent<CollisionAnalyzer> () ?? World.CollisionAnalyzer; }
        }

        /// <summary>
        /// 標準の物理シミュレーター
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされたローカルの物理シミュレーター <see cref="PhysicsSimulator"/> 、またはそれが存在しない場合は
        /// <see cref="World"/> の物理シミュレーター  <see cref="PhysicsSimulator"/> を返します。
        /// <see cref="World"/> オブジェクトは必ずデフォルトの物理シミュレーターを1つ保持しています。
        /// </remarks>
        protected internal PhysicsSimulator PhysicsSimulator {
            get { return GetComponent<PhysicsSimulator> () ?? World.PhysicsSimulator; }
        }

        protected internal GUI GUI{
            get{return GetComponent<GUI>() ?? World.GUI;}
    }

        /// <summary>
        /// このコンポーネントがアタッチ済みかどうかを確認するプロパティ
        /// </summary>
        public bool IsAttached {
            get { return node != null; }
        }

        /// <summary>
        /// このコンポーネントがアタッチされたノードがワールドに
        /// </summary>
        public bool IsUnderWorld {
            get { return (node != null) && (node.Root is World); }
        }

        #endregion


        #region Method
        /// <summary>
        /// コンポーネントの検索
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされたコンポーネントの中から、指定の型 <typeparamref name="T"/> のコンポーネントを取得します。
        /// 同型のコンポーネントが複数あった場合、どれが返るかは未定義です。
        /// </remarks>
        /// <typeparam name="T">コンポーネント型</typeparam>
        /// <returns></returns>
        protected T GetComponent<T> () where T : Component {
            if (node == null) {
                return null;
            }

            return node.GetComponent<T> ();
        }

        /// <summary>
        /// コンポーネントの検索
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされたコンポーネントの中からインデックス <paramref name="index"/> 番目の指定の型 <typeparamref name="T"/> のコンポーネントを取得します。
        /// </remarks>
        /// <typeparam name="T">コンポーネント型</typeparam>
        ///  <param name="index">インデックス番号</param>
        /// <returns></returns>
        protected T GetComponent<T> (int index) where T : Component {
            if (index < 0) {
                throw new ArgumentException ("Index is out of range");
            }
            if (node == null) {
                return null;
            }

            return node.GetComponent<T> (index);
        }

        /// <summary>
        /// ノードのユーザーデータの取得
        /// </summary>
        /// <remarks>
        /// 指定の名前のユーザーデータが存在しない場合、例外を発生します。
        /// </remarks>
        /// <typeparam name="T">データの型</typeparam>
        /// <param name="name">一意な名前</param>
        /// <returns></returns>
        protected T GetUserData<T> (string name) where T:class {
            if (node == null) {
                return null;
            }

            return node.UserData[name] as T;
        }

        /// <summary>
        /// ノードの検索
        /// </summary>
        /// <remarks>
        /// シーン全体から指定の名前のノードを検索します。
        /// 同一の名前を持ったノードが2つ以上存在する場合、どのノードが返るかは未定義です。
        /// 見つからない場合は <c>null</c> を返します。
        /// </remarks>
        /// <param name="name">検索したいノードの名前</param>
        /// <returns>ノード</returns>
        protected Node GetNode (string name) {
            return World.Find (name);
        }

        /// <summary>
        /// ノードの検索
        /// </summary>
        /// <remarks>
        /// シーン全体から指定の条件式 <paramref name="pred"/> を満たすノードを検索します。
        /// 条件式を満たすノードが2つ以上存在する場合、どのノードが返るかは未定義です。
        /// 見つからない場合は <c>null</c> を返します。
        /// </remarks>
        /// <param name="pred">条件式</param>
        /// <returns>ノード</returns>
        protected Node GetNode (Func<Node, bool> pred) {
            return World.Find (pred);
        }

        /// <summary>
        /// ノードの削除
        /// </summary>
        /// <remarks>
        /// 指定のノードを <paramref name="delayTime"/> 後にシーンから削除します。
        /// このメソッド内部で直ちに削除したい場合は -1 を指定します(非推奨)。
        /// このフレームを終了後に直ちに削除したい場合は 0 を指定します。
        /// 基本的にノードはこのメソッドを使って削除するのが一番安全です。
        /// 自分自身を削除する事も可能です。
        /// </remarks>
        /// <param name="node">削除したいノード</param>
        /// <param name="delayTime">遅延時間 (msec)</param>
        protected void Destroy (Node node, long delayTime) {
            if (node == null) {
                return;
            }
            if (delayTime < -1) {
                throw new ArgumentException ("DelayTime is invalid");
            }

            if (delayTime == -1) {
                node.Destroy (-1);
            }
            else {
                node.Destroy (World.ClockTower.CurrentTime + delayTime);
            }
        }

        /// <summary>
        /// ログの記録
        /// </summary>
        /// <remarks>
        /// <see cref="Logger"/> コンポーネントを使ってログを記録します。
        /// ログは <see cref="DebugTools.LogView"/> を使って閲覧可能です。
        /// </remarks>
        /// <seealso cref="Logger"/>
        /// <seealso cref="DebugTools.LogView"/>
        /// <param name="priority">優先度</param>
        /// <param name="message">メッセージ</param>
        protected void Log (int priority, string message) {
            Logger.Write (Node, priority, message);
        }

        /// <summary>
        /// 他のノードにメッセージを送信
        /// </summary>
        /// <remarks>
        /// 他のノードにメッセージを送信します。
        /// 送信アドレスはメールボックスに付けられたアドレス（文字列）です。
        /// "All" は予約後ですべてのメールボックスにメッセージが送信されます。
        /// </remarks>
        /// <param name="address">宛先アドレス</param>
        /// <param name="letter">メッセージ</param>
        protected void SendMessage (string address, object letter) {
            if (!IsAttached) {
                throw new InvalidOperationException ("Component is not attached");
            }
            if (!IsUnderWorld) {
                throw new InvalidOperationException ("Component is not under World");
            }
            PostOffice.Post (Node, address, letter);
        }



        /// <summary>
        /// アタッチ処理後のエントリーポイント
        /// </summary>
        /// <remarks>
        /// このメソッドの中で <see cref="Node"/> が <c>null</c> でない事は保証されています。
        /// </remarks>
        public virtual void OnAttached () {
        }

        /// <summary>
        /// デタッチ処理前のエントリーポイント
        /// </summary>
        /// <remarks>
        /// このメソッドの中で <see cref="Node"/> が <c>null</c> でない事は保証されています。
        /// </remarks>
        public virtual void OnDetached () {

        }

        /// <summary>
        /// ディスパッチ処理のエントリーポイント
        /// </summary>
        /// <remarks>
        /// コンポーネントで独自にイベントを発行したい場合はこれをオーバーライドします。
        /// 通常ユーザーがこれを使用する事はありません。
        /// </remarks>
        public virtual void OnDispatch () {
        }

        /// <summary>
        /// コンポーネントの最終処理のエントリーポイント
        /// </summary>
        /// <remarks>
        /// コンポーネントの最終処理のエントリー ポイントです。
        /// 主にコンポーネントのマネージド リソースの解放を目的としています。
        /// （と言っても普通は特にやることはなく、自動でGCによって回収されますが）
        /// アンマネージド リソースの解放はここではなく IDisposable インターフェースを実装して Dispose() に記述して下さい。
        /// この仮想関数はデタッチ後に呼ばれます。従ってノードの情報やシーン情報を利用する事はできません。
        /// </remarks>
        public virtual void OnFinalize () {
        }

        /// <summary>
        /// 更新処理前の初期化エントリーポイント
        /// </summary>
        /// <remarks>
        /// 通常の <see cref="OnUpdate"/> が呼ばれるタイミングで一度だけ呼ばれます。
        /// 1回だけ同一フレームで <see cref="OnUpdateInit"/> と <see cref="OnUpdate"/> の両方が呼ばれます。
        /// </remarks>
        /// <param name="msec"></param>
        public virtual void OnUpdateInit (long msec) {
        }

        /// <summary>
        /// 更新処理のエントリーポイント
        /// </summary>
        /// <remarks>
        /// 更新処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// 引数の <paramref name="msec"/> はゲームが起動してからの経過時間をミリ秒単位であらわします。
        /// </remarks>
        /// <param name="msec">ゲームが起動してからの起動時間 (msec)</param>
        public virtual void OnUpdate (long msec) {
        }

        /// <summary>
        /// コリジョン解析機の初期化処理のエントリーポイント
        /// </summary>
        /// <remarks>
        /// コリジョン解析機が更新処理を行うタイミングで一度だけ呼ばれる仮想関数のエントリーポイント。
        /// 通常ユーザーがこの仮想関数をオーバーライドする必要はありません。
        /// </remarks>
        public virtual void OnCollisionUpdateInit (long msec) {
        }

        /// <summary>
        /// コリジョン解析機の更新処理のエントリーポイント
        /// </summary>
        /// <remarks>
        /// コリジョン解析機が更新処理を行う仮想関数のエントリーポイント。
        /// 通常ユーザーがこの仮想関数をオーバーライドする必要はありません。
        /// </remarks>
        public virtual void OnCollisionUpdate (long msec) {
        }



        /// <summary>
        /// 物理エンジンの初期化処理のエントリーポイント
        /// </summary>
        /// <remarks>
        /// 物理エンジンが更新処理を行うタイミングで一度だけ呼ばれる仮想関数のエントリーポイント。
        /// 通常ユーザーがこの仮想関数をオーバーライドする必要はありません。
        /// </remarks>
        public virtual void OnPhysicsUpdateInit (long msec) {
        }

        /// <summary>
        /// 物理エンジンの更新処理のエントリーポイント
        /// </summary>
        /// <remarks>
        /// 物理エンジンが更新処理を行う仮想関数のエントリーポイント。
        /// 通常ユーザーがこの仮想関数をオーバーライドする必要はありません。
        /// </remarks>
        public virtual void OnPhysicsUpdate (long msec) {
        }


        /// <summary>
        /// 物理エンジンのコリジョン発生のエントリーポイント
        /// </summary>
        /// <remarks>
        /// 物理エンジンのコリジョン発生処理を行う仮想関数のエントリーポイント。
        /// </remarks>
        /// <param name="collidee">衝突の相手</param>
        public virtual void OnCollisionEnter (Node collidee) {
        }

        /// <summary>
        /// 物理エンジンのコリジョン消失のエントリーポイント
        /// </summary>
        /// <remarks>
        /// 物理エンジンのコリジョン消失処理を行う仮想関数のエントリーポイント。
        /// </remarks>
        /// <param name="collidee">衝突の相手</param>
        public virtual void OnCollisionExit (Node collidee) {
        }

        /// <summary>
        /// ライン イベントのエントリーポイント
        /// </summary>
        /// <remarks>
        /// イベントが設定されたラインを再生する時呼び出される仮想関数のエントリーポイント。
        /// このイベントは <see cref="LineReader"/> と同じノードにアタッチされたコンポーネントでのみ有効です。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// 第1引数の <paramref name="sender"/> はイベントの元になったラインです。
        /// 第2引数の <paramref name="args"/> はラインに設定されていた引数オブジェクトです。
        /// 実際の型はラインに記述されていた型です。
        /// </remarks>
        /// <param name="sender">イベントが設定されていたライン</param>
        /// <param name="args">任意のオブジェクト</param>
        public virtual void OnLineEvent (Line sender, object args) {
        }

        /// <summary>
        /// キーボード処理
        /// </summary>
        /// <remarks>
        /// キーボード処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// </remarks>
        public virtual void OnKeyPressed (KeyCode key) {
        }


        /// <summary>
        /// キーボード処理
        /// </summary>
        /// <remarks>
        /// キーボード処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// </remarks>
        public virtual void OnKeyReleased (KeyCode key) {
        }


        /// <summary>
        /// マウスボタン処理
        /// </summary>
        /// <remarks>
        /// マウス ボタンのクリック イベント処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// マウスの位置 (X,Y) はワールド座標系です。
        /// </remarks>
        /// <param name="button">マウスボタン</param>
        /// <param name="x">マウスのX座標（ワールド座標系）</param>
        /// <param name="y">マウスのY座標（ワールド座標系）</param>
        public virtual void OnMouseButtonPressed (MouseButton button, float x, float y) {
        }


        /// <summary>
        /// マウスボタン処理
        /// </summary>
        /// <remarks>
        /// マウス ボタンのリリース イベント処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// マウス位置 (X,Y) はワールド座標系です。
        /// </remarks>
        /// <param name="button">マウスボタン</param>
        /// <param name="x">マウスのX座標（ワールド座標系）</param>
        /// <param name="y">マウスのY座標（ワールド座標系）</param>
        public virtual void OnMouseButtonReleased (MouseButton button, float x, float y) {
        }

        /// <summary>
        /// マウスボタン処理
        /// </summary>
        /// <remarks>
        /// マウス ボタンのフォーカス イン イベント処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// マウス位置(X,Y)はワールド座標系です。
        /// </remarks>
        /// <param name="x">マウスのX座標（ワールド座標系）</param>
        /// <param name="y">マウスのY座標（ワールド座標系）</param>
        public virtual void OnMouseFocusIn (float x, float y) {
        }

        /// <summary>
        /// マウスボタン処理
        /// </summary>
        /// <remarks>
        /// マウス ボタンのフォーカス アウト イベント処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// マウス位置(X,Y)はワールド座標系です。
        /// </remarks>
        /// <param name="x">マウスのX座標（ワールド座標系）</param>
        /// <param name="y">マウスのY座標（ワールド座標系）</param>
        public virtual void OnMouseFocusOut (float x, float y) {
        }

        /// <summary>
        /// アニメーション処理
        /// </summary>
        /// <remarks>
        /// アニメーション処理を行う仮想関数のエントリーポイント。
        /// 通常はユーザーがこの仮想関数をオーバーライドする必要はありません。
        /// この仮想関数の適切な実装を提供するのはエンジン側の責任です。
        /// </remarks>
        /// <param name="msec">ワールド時刻 (msec)</param>
        /// <param name="dtime">デルタ タイム (msec)</param>
        public virtual void OnAnimate (long msec, long dtime) {

        }


        /// <summary>
        /// メール ボックスのエントリー ポイント
        /// </summary>
        /// <remarks>
        /// メール ボックスにメールを受信した時に呼び出される標準のメールボックス アクションです。
        /// ユーザーがこのアクションではなく独自のアクションを登録した場合はこのアクションは呼び出されません。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// </remarks>
        /// <param name="from">送信元ノード</param>
        /// <param name="address">送信先アドレス（文字列）</param>
        /// <param name="letter">通信メッセージ（任意）</param>
        public virtual void OnMailBox (Node from, string address, object letter) {
        }

        /// <summary>
        /// 描画前処理
        /// </summary>
        /// <remarks>
        /// 描画の前に処理を行う仮想関数のエントリーポイント。
        /// 描画される前に追加の処理を行いたい時に、この仮想関数をオーバーライドします。
        /// この前処理は <see cref="DD.Node.Visible"/> の値に関わらず常に呼び出されます。
        /// </remarks>
        /// <param name="window">ウィンドウ（SFML.Graphics.RenderWindow）</param>
        public virtual void OnPreDraw (object window) {
        }

        /// <summary>
        /// 描画処理
        /// </summary>
        /// <remarks>
        /// 描画処理を行う仮想関数のエントリーポイント。
        /// ユーザー定義のコンポーネントで描画時に追加の処理を行いたい時に、この仮想関数をオーバーライドします。
        /// </remarks>
        /// <param name="window">ウィンドウ（SFML.Graphics.RenderWindow）</param>
        public virtual void OnDraw (object window) {
        }

        #endregion

    }
}
