using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision;
using Microsoft.Xna.Framework;

using XnaVector2 = Microsoft.Xna.Framework.Vector2;
using FarseerPhysics.Common;

// メモ
// FarseerPhysicsで使用しているのはコリジョン検出だけ
// 具体的に言うと距離とレイキャストの2つ
// 空間構造は使用していない
// しょせん2Dのライブラリなので3Dでは使えない・・・


namespace DD {

    /// <summary>
    /// ノード クラス
    /// </summary>
    /// <remarks>
    /// シーン グラフを構成するノード クラス。
    /// ノードは <see cref="Root"/> を頂点とする木構造のグラフを構成します。
    /// </remarks>
    public class Node : Transformable {

        #region Field
        string name;
        Node parent;
        List<Node> children;
        List<Component> components;
        uint groupID;
        Dictionary<string, object> userData;
        float opacity;
        bool drawable;
        bool updatable;
        bool animatable;
        bool deliverable;
        bool collidable;
        sbyte drawPriority;
        sbyte updatePriority;
        Matrix4x4? matrix;      // = Cache of GlobalTransform
        #endregion



        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Node ()
            : this ("") {
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// ノード名が null または "" でないノードはデフォルトでノード名と同名のメールボックスを1つ保有します。
        /// </remarks>
        /// <param name="name">ノード名</param>
        public Node (string name)
            : base () {
            this.name = name ?? "";
            this.drawable = true;
            this.updatable = true;
            this.animatable = true;
            this.deliverable = true;
            this.collidable = true;
            this.parent = null;
            this.children = new List<Node> ();
            this.components = new List<Component> ();
            this.drawPriority = 0;
            this.updatePriority = 0;
            this.groupID = 0x0000ffffu;
            this.userData = new Dictionary<string, object> ();
            this.opacity = 1.0f;
            this.matrix = null;
        }
        #endregion


        #region Property
        /// <summary>
        /// ノード名
        /// </summary>
        /// <remarks>
        /// ユーザーがノードの識別に使用する名前です。重複する名前のノードがあってもかまいません。
        /// </remarks>
        public string Name {
            get { return name; }
            //set { this.name = value ?? ""; }
        }

        /// <summary>
        /// ユニークID
        /// </summary>
        /// <remarks>
        /// ノードはシステムによって必ずユニーク（一意）な事が保障されているIDを付加されます。
        /// </remarks>
        public int UniqueID {
            get { return GetHashCode (); }

        }

        /// <summary>
        /// グループID
        /// </summary>
        /// <remarks>
        /// デフォルト値は 0x0000ffff です。
        /// レンダリングや衝突判定で使用されます。
        /// </remarks>
        public uint GroupID {
            get { return groupID; }
            set { this.groupID = value; }
        }

        /// <summary>
        /// ユーザー データ
        /// </summary>
        /// <remarks>
        /// ユーザーはノードにユーザー データとして任意の値をキー、バリュー形式で追加する事が可能です。
        /// ユーザー データの管理はすべてユーザーの責任です。
        /// エンジン側では一切使用または管理しません。
        /// </remarks>
        public Dictionary<string, object> UserData {
            get { return userData; }
        }

        /// <summary>
        /// すべてのメール ボックスを列挙する列挙子
        /// </summary>
        public IEnumerable<MailBox> MailBoxs {
            get { return GetComponents<MailBox>(); }
        }


        /// <summary>
        /// 不透明度
        /// </summary>
        /// <remarks>
        /// ノードの不透明度
        /// </remarks>
        public float Opacity {
            get { return opacity; }
            set {
                if (value < 0 || value > 1) {
                    throw new ArgumentException ("Opacity is invalid");
                }
                this.opacity = value;
            }
        }

        /// <summary>
        /// 表示可能フラグ
        /// </summary>
        /// <remarks>
        /// このフラグが <c>true</c> のノードは表示されます。
        /// </remarks>
        public bool Drawable {
            get { return drawable; }
            set { this.drawable = value; }
        }

        /// <summary>
        /// 更新可能フラグ
        /// </summary>
        /// <remarks>
        /// このフラグが <c>true</c> のノードは更新されます。
        /// デフォルトは <c>true</c> です。
        /// </remarks>
        public bool Updatable {
            get { return updatable; }
            set { this.updatable = value; }
        }

        /// <summary>
        /// アニメート可能フラグ
        /// </summary>
        /// <remarks>
        /// このフラグが <c>true</c> のノードはアニメートされます。
        /// デフォルトは <c>true</c> です。
        /// </remarks>
        public bool Animatable {
            get { return animatable; }
            set { this.animatable = value; }
        }

        /// <summary>
        /// 配達可能フラグ
        /// </summary>
        /// <remarks>
        /// このフラグが <c>true</c> のノードはメールを受信します。
        /// デフォルトは <c>true</c> です。
        /// </remarks>
        public bool Deliverable {
            get { return deliverable; }
            set { this.deliverable = value; }
        }

        /// <summary>
        /// 衝突可能フラグ
        /// </summary>
        /// <remarks>
        /// このフラグが <c>true</c> のノードは衝突判定があります。
        /// 現在このプロパティは機能しません。
        /// </remarks>
        public bool Collidable {
            get { return collidable; }
            set { this.collidable = value; }
        }


        /// <summary>
        /// 表示優先度
        /// </summary>
        /// <remarks>
        /// このノードの表示優先度 (-127～128) を取得・設定するプロパティ。
        /// デフォルトは 0 で -127 が一番優先度が高く（一番手前に）表示されます。
        /// </remarks>
        public sbyte DrawPriority {
            get { return drawPriority; }
            set { this.drawPriority = value; }
        }

        /// <summary>
        /// 更新優先度
        /// </summary>
        /// <remarks>
        /// このノードの更新優先度 (-127～128) を取得・設定するプロパティ。
        /// デフォルトは 0 で -127 が一番優先度が高く最初に更新されます。
        /// </remarks>
        public sbyte UpdatePriority {
            get { return updatePriority; }
            set { this.updatePriority = value; }
        }

        /// <summary>
        /// 親ノード
        /// </summary>
        public Node Parent {
            get { return parent; }
        }

        /// <summary>
        /// 子ノードの個数
        /// </summary>
        public int ChildCount {
            get { return children.Count; }
        }

        /// <summary>
        /// コンポーネントの個数
        /// </summary>
        public int ComponentCount {
            get { return components.Count; }
        }

        /// <summary>
        /// 子ノードを列挙する列挙子
        /// </summary>
        public IEnumerable<Node> Children {
            get { return children; }
        }

        /// <summary>
        /// 自分自身と全ての子ノードを列挙する列挙子
        /// </summary>
        /// <remarks>
        /// 自分自身と自分から再帰的に辿れるすべての子ノードを列挙します。
        /// 順番は先頭が自分で幅優先探索で子ノードが続きます。
        /// <note>
        /// 幅優先で実装してあるがこの仕様は有益か？
        /// --> SceneDepth を実装して後からソートした方が実装が簡単になる。後で修正する
        /// </note>
        /// </remarks>
        public IEnumerable<Node> Downwards {
            get {
                var downs = new List<Node> () { this };
                var nodes = this.children;
                while (nodes.Count > 0) {
                    downs.AddRange (nodes);
                    var tmp = new List<Node> ();
                    foreach (var n in nodes) {
                        tmp.AddRange (n.children);
                    }
                    nodes = tmp;
                }
                return downs;
            }
        }

        /// <summary>
        /// 自分自身とすべての親ノードを列挙する列挙子
        /// </summary>
        /// <remarks>
        /// 自分自身と自分から辿れるすべての親ノードを列挙します。
        /// 順番は先頭が必ず自分です。
        /// </remarks>
        public IEnumerable<Node> Upwards {
            get {
                var ups = new List<Node> () { this };
                var node = this.parent;
                while (node != null) {
                    ups.Add (node);
                    node = node.parent;
                }
                return ups;
            }
        }

        /// <summary>
        /// ルート ノード
        /// </summary>
        /// <remarks>
        /// ルート ノードとツリーを上方向に null になるまでたどった時の一番上のノードの事を言います。
        /// 親ノードが <c>null</c> の場合は自分がルート ノードです。
        /// </remarks>
        public Node Root {
            get {
                return Upwards.LastOrDefault ();
            }
        }

        /// <summary>
        /// コンポーネントを列挙する列挙子
        /// </summary>
        public IEnumerable<Component> Components {
            get { return components.ToArray (); }
        }

        /// <summary>
        /// このノードのローカル座標系からグローバル座標系への複合変換行列
        /// </summary>
        /// <remarks>
        /// このプロパティは計算結果をキャッシュし、2回目以降は高速に動作します。
        /// </remarks>
        public Matrix4x4 GlobalTransform {
            get {
                if (matrix == null) {
                    this.matrix = Upwards.Aggregate (Matrix4x4.Identity, (t, node) => node.Transform * t);
                }
                return matrix.Value;
            }
        }

        /// <summary>
        /// グローバル座標系からこのノードのローカル座標系への複合変換行列
        /// </summary>
        public Matrix4x4 LocalTransform {
            get { return Upwards.Aggregate (Matrix4x4.Identity, (t, node) => t * node.Transform.Inverse ()); }
        }

        /// <summary>
        /// グローバル座標系からこのノードの親のローカル座標系への複合変換行列
        /// </summary>
        public Matrix4x4 ParentTransform {
            get { return Upwards.Skip (1).Aggregate (Matrix4x4.Identity, (t, node) => t * node.Transform.Inverse ()); }
        }

        /// <summary>
        /// 位置（ワールド座標系）
        /// </summary>
        /// <remarks>
        /// </remarks>
        public Vector3 Position {
            get {
                Vector3 T;
                Quaternion R;
                Vector3 S;
                GlobalTransform.Decompress (out T, out R, out S);
                return T;
            }
        }

        /// <summary>
        /// コリジョン形状
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされている最初のコリジョン コンポーネントを返します。
        /// コリジョンが何もアタッチされていない場合は <c>null</c> を返します。
        /// 複数のコリジョンがアタッチされていた場合、どれが返るかは未定義です。
        /// </remarks>
        public Collision Collision {
            get { return GetComponent<Collision> (); }
        }

        #endregion

        #region Method

        /// <inheritdoc/>
        public override void InvalidateTransformCache () {
            foreach (var node in Downwards) {
                node.matrix = null;
            }
            base.InvalidateTransformCache ();
        }

        /// <summary>
        /// コンポーネントの識別
        /// </summary>
        /// <remarks>
        /// このノードが指定のコンポーネント型 <typeref name="T"/> をアタッチされている時 <c>true</c> を返します。
        /// </remarks>
        /// <typeparam name="T">コンポーネント型</typeparam>
        /// <returns></returns>
        public bool Is<T> () where T : Component {
            return GetComponent<T> () != null;
        }

        /// <summary>
        /// 平行移動量の変更（グローバル座標）
        /// </summary>
        /// <remarks>
        /// それまでセットされていたこのノードの平行移動量を破棄し、新しい値に変更します。
        /// このメソッドはグローバル座標で指定する事を除き <see cref="Transformable.SetTranslation"/> と同じです。
        /// </remarks>
        /// <param name="tx">X方向の平行移動量</param>
        /// <param name="ty">Y方向の平行移動量</param>
        /// <param name="tz">Z方向の平行移動量</param>
        public void SetGlobalTranslation (float tx, float ty, float tz) {
            Vector3 T;
            Quaternion R;
            Vector3 S;
            var P = ParentTransform;
            var G = Matrix4x4.CreateFromTranslation (tx, ty, tz);
            (P * G).Decompress (out T, out R, out S);

            SetTranslation (T.X, T.Y, T.Z);
        }

        /// <summary>
        /// 回転成分の変更（グローバル座標）
        /// </summary>
        /// <remarks>
        /// このメソッドはグローバル座標で指定する事を除き <see cref="Transformable.SetRotation(Quaternion)"/> と同じです。
        /// </remarks>
        /// <param name="rot">回転クォータニオン</param>
        public void SetGlobalRotation (Quaternion rot) {
            Vector3 T;
            Quaternion R;
            Vector3 S;
            var P = ParentTransform;
            var G = Matrix4x4.CreateFromRotation (rot);
            (P * G).Decompress (out T, out R, out S);

            SetRotation (R);
        }

        /// <summary>
        /// 回転成分の変更（グローバル座標）
        /// </summary>
        /// <remarks>
        /// 回転角度は度数(degree)で指定します。値に制限はありません。0度以下や360度以上も可能です。
        /// 回転軸は正規化されている必要はありません。
        /// このメソッドはグローバル座標で指定する事を除き <see cref="Transformable.SetRotation(float,float,float,float)"/> と同じです。
        /// </remarks>
        /// <param name="angle">回転角度 [0,360)</param>
        /// <param name="ax">回転軸X</param>
        /// <param name="ay">回転軸Y</param>
        /// <param name="az">回転軸Z</param>
        public void SetGlobalRotation (float angle, float ax, float ay, float az) {
            SetGlobalRotation (new Quaternion (angle, ax, ay, az));
        }

        /// <summary>
        /// スケールの変更（グローバル座標）
        /// </summary>
        /// <remarks>
        /// このメソッドはグローバル座標で指定する事を除き <see cref="Transformable.SetScale(float,float,float)"/> と同じです。
        /// </remarks>
        /// <param name="sx">X方向の拡大率</param>
        /// <param name="sy">Y方向の拡大率</param>
        /// <param name="sz">Z方向の拡大率</param>
        public void SetGlobalScale (float sx, float sy, float sz) {
            Vector3 T;
            Quaternion R;
            Vector3 S;
            var P = ParentTransform;
            var G = Matrix4x4.CreateFromScale (sx, sy, sz);
            (P * G).Decompress (out T, out R, out S);

            SetScale (S.X, S.Y, S.Z);
        }


        /// <summary>
        /// コンポーネントのアタッチ
        /// </summary>
        /// <remarks>
        /// コンポーネントをノードに追加します。
        /// すでに他のノードで登録されているコンポーネントは登録できません。
        /// すでに登録済みのコンポーネントと同型のコンポーネントを登録可能です。
        /// これについては後日変更するかもしれません。
        /// このタイミングで <see cref="Component.OnAttached"/> が呼び出されます。
        /// </remarks>
        /// <param name="comp">コンポーネント</param>
        public void Attach (Component comp) {
            if (comp == null) {
                throw new ArgumentNullException ("Component is null");
            }
            if (comp.Node != null) {
                throw new ArgumentException ("Component has already attached to another node.");
            }
            this.components.Add (comp);
            comp.Node = this;
            comp.OnAttached ();
        }

        /// <summary>
        /// コンポーネントのデタッチ
        /// </summary>
        /// <remarks>
        /// ノードからコンポーネントを削除します。
        /// このタイミングで <see cref="Component.OnDetached"/> が呼び出されます。
        /// </remarks>
        /// <param name="comp">コンポーネント</param>
        public void Detach (Component comp) {
            if (comp == null) {
                return;
            }
            if (comp.Node != this) {
                return;
            }
            comp.OnDetached ();
            comp.Node = null;
            comp.IsUpdateInitCalled = false;
            this.components.Remove (comp);
        }

        /// <summary>
        /// 子ノードの追加
        /// </summary>
        /// <param name="node">ノード</param>
        public void AddChild (Node node) {
            if (node == null) {
                throw new ArgumentNullException ("Node is null");
            }
            if (node.parent != null) {
                throw new ArgumentException ("Node has parent already");
            }
            if (node == this) {
                throw new ArgumentException ("Add myself is invalid");
            }
            node.parent = this;
            this.children.Add (node);
        }

        /// <summary>
        /// 子ノードの削除
        /// </summary>
        /// <param name="node">ノード</param>
        public void RemoveChild (Node node) {
            if (node == null) {
                throw new ArgumentNullException ("Node is null");
            }
            if (node.parent != this) {
                throw new ArgumentNullException ("Node is not child of this");
            }
            node.parent = null;
            this.children.Remove (node);
        }

        /// <summary>
        /// ノードの検索
        /// </summary>
        /// <remarks>
        /// 指定の名前のノードを下方向から1つだけ検索します。
        /// 一致する名前のノードが2つ以上あった場合は、どれが返るかは未定義です。
        /// 見つからない場合は <c>null</c> が返ります。
        /// </remarks>
        /// <param name="name">ノード名</param>
        /// <returns></returns>
        public Node Find (string name) {
            return Finds (name).FirstOrDefault ();
        }

        /// <summary>
        /// ノードの検索
        /// </summary>
        /// <remarks>
        /// 指定の名前のノードを下方向からすべて検索します。
        /// 見つからない場合はサイズ 0 の列挙子が返ります。
        /// </remarks>
        /// <param name="name">ノード名</param>
        /// <returns></returns>
        public IEnumerable<Node> Finds (string name) {
            return from node in Downwards
                   where node.Name == (name ?? "")
                   select node;
        }

        /// <summary>
        /// ノードの検索
        /// </summary>
        /// <remarks>
        /// 指定の条件式 <paramref name="pred"/> を満たすノードを下方向から1つだけ検索します。
        /// 条件に一致するノードが2つ以上あった場合は、どれが返るかは未定義です。
        /// 見つからない場合は <c>null</c> が返ります。
        /// </remarks>
        /// <param name="pred">条件式</param>
        /// <returns></returns>
        public Node Find (Func<Node, bool> pred) {
            return Finds (pred).FirstOrDefault ();
        }

        /// <summary>
        /// ノードの検索
        /// </summary>
        /// <remarks>
        /// 指定の条件式 <paramref name="pred"/> を満たすノードを下方向からすべて検索します。
        /// 見つからない場合はサイズ0の列挙子が返ります。
        /// </remarks>
        /// <param name="pred">条件式</param>
        /// <returns></returns>
        public IEnumerable<Node> Finds (Func<Node, bool> pred) {
            return from node in Downwards
                   where pred (node) == true
                   select node;
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされているコンポーネントから指定の型 <typeparamref name="T"/> の物を返します。
        /// 一致する型のコンポーネントが2つ以上あった場合は、どれが返るかは未定義です。
        /// 見つからない場合は <c>null</c> が返ります。
        /// </remarks>
        /// <typeparam name="T">コンポーネント型</typeparam>
        /// <returns></returns>
        public T GetComponent<T> () where T : Component {
            if (this == null) {
                return null;
            }
            return (from comp in components
                    where comp is T
                    select (T)comp).FirstOrDefault ();
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされているコンポーネントからインデックス <paramref name="index"/> 番目の指定の型 <typeparamref name="T"/> の物を返します。
        /// 見つからない場合は <c>null</c> が返ります。
        /// </remarks>
        /// <typeparam name="T">コンポーネント型</typeparam>
        /// <param name="index">インデックス番号</param>
        /// <returns></returns>
        public T GetComponent<T> (int index) where T : Component {
            if (index < 0) {
                throw new ArgumentException ("Index is out of range");
            }
            if (this == null) {
                return null;
            }
            return (from comp in components
                    where comp is T
                    select (T)comp).Skip (index).FirstOrDefault ();
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <remarks>
        /// このノードにアタッチされているコンポーネントから指定の型 <typeparamref name="T"/> のものをすべて返します。
        /// 見つからない場合はサイズ0の列挙子が返ります。
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetComponents<T> () where T : Component {
            return  from cmp in components
                    where cmp is T
                    select (T)cmp;
        }

        /// <summary>
        /// 子ノードの取得
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>ノード</returns>
        public Node GetChild (int index) {
            if (index < 0 || index >= ChildCount) {
                throw new ArgumentException ("Index is out of children");
            }
            return children[index];
        }

        /// <summary>
        /// コンポーネントの取得
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>コンポーネント</returns>
        public Component GetComponent (int index) {
            if (index < 0 || index >= ComponentCount) {
                throw new ArgumentException ("Index is out of components");
            }
            return components[index];
        }

   



        /// <summary>
        /// 2つのノードのコリジョンの重複判定
        /// </summary>
        /// <remarks>
        /// ノードのどちらか一方、または両方が <c>null</c> またはコリジョン形状がアタッチされていない場合は <c>false</c> が返ります。
        /// </remarks>
        /// <param name="nodeA">ノードA</param>
        /// <param name="nodeB">ノードA</param>
        /// <returns>重複している時 <c>true</c>, そうでないとき <c>false</c></returns>
        public static bool Overlap (Node nodeA, Node nodeB) {
            return Distance (nodeA, nodeB) == 0;
        }

        /// <summary>
        /// 2つのノードのコリジョンの距離
        /// </summary>
        /// <remarks>
        /// 2つのノードのコリジョンを結ぶ最短距離を求めます。
        /// コリジョンが重複している時は一律 0 が返ります。負の値を返すことはありません。
        /// ノードのどちらか一方、または両方が <c>null</c> またはコリジョン形状がアタッチされていない場合は <c>NaN</c> が返ります。
        /// </remarks>
        /// <param name="nodeA">ノードA</param>
        /// <param name="nodeB">ノードB</param>
        /// <returns>0より大きな浮動小数値、または0、測定不能の時 <c>NaN</c>.</returns>
        public static float Distance (Node nodeA, Node nodeB) {
            if (nodeA == null || nodeB == null || !nodeA.Is<Collision> () || !nodeB.Is<Collision> ()) {
                return Single.NaN;
            }

            var colA = nodeA.GetComponent<Collision> ();
            var traA = nodeA.GlobalTransform;

            var colB = nodeB.GetComponent<Collision> ();
            var traB = nodeB.GlobalTransform;

            DistanceInput input = new DistanceInput ();
            DistanceProxy proxyA = new DistanceProxy ();
            DistanceProxy proxyB = new DistanceProxy ();
            proxyA.Set (colA.CreateShapeBody (1), 0);
            proxyB.Set (colB.CreateShapeBody (1), 0);

            input.ProxyA = proxyA;
            input.ProxyB = proxyB;
            input.UseRadii = true;

            Vector3 T;
            Matrix3x3 R;
            Vector3 S;

            nodeA.GlobalTransform.Decompress (out T, out R, out S);
            var posA = new XnaVector2 (T.X, T.Y);
            var rotA = new Mat22 (R[0], R[1], R[3], R[4]);
            input.TransformA = new Transform (ref posA, ref rotA);

            nodeB.GlobalTransform.Decompress (out T, out R, out S);
            var posB = new XnaVector2 (T.X, T.Y);
            var rotB = new Mat22 (R[0], R[1], R[3], R[4]);
            input.TransformB = new Transform (ref posB, ref rotB);

            DistanceOutput output;
            SimplexCache cache;
            FarseerPhysics.Collision.Distance.ComputeDistance (out output, out cache, input);

            return output.Distance;
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
        /// <returns>0より大きな浮動小数値、または0、測定不能の時 <c>NaN</c>.</returns>
        public static float RayCast (Node nodeA, Vector3 start, Vector3 end) {
            if (nodeA == null || !nodeA.Is<Collision> ()) {
                return Single.NaN;
            }

            Vector3 T;
            Matrix3x3 R;
            Vector3 S;

            nodeA.GlobalTransform.Decompress (out T, out R, out S);
            var posA = new XnaVector2 (T.X, T.Y);
            var rotA = new Mat22 (R[0], R[1], R[3], R[4]);
            var traA = new Transform (ref posA, ref rotA);

            var colA = nodeA.GetComponent<Collision> ();
            var shpA = colA.CreateShapeBody (1);

            RayCastInput input;
            input.Point1 = new XnaVector2 (start.X, start.Y);
            input.Point2 = new XnaVector2 (end.X, end.Y);
            input.MaxFraction = 1;

            RayCastOutput output;

            var hit = shpA.RayCast (out output, ref input, ref traA, 0);

            return (hit == false) ? 0 : output.Fraction * (end - start).Length;
        }

        /// <summary>
        /// ノードのスィープ判定
        /// </summary>
        /// <remarks>
        /// ノードBを移動ベクトル分だけ動かした時に、ノードAと交差するかどうかを判定します。
        /// 戻り値はノードとノードが接触するまでの移動量です。
        /// 接触しない場合は 0 が返ります。
        /// ノードのどちらか一方、または両方が <c>null</c> またはコリジョン形状がアタッチされていない場合は <c>NaN</c> が返ります。
        /// </remarks>
        /// <param name="nodeA">ノードA（固定）</param>
        /// <param name="nodeB">ノードB（移動）</param>
        /// <param name="v">移動ベクトル</param>
        /// <returns>0より大きな浮動小数値、または0、測定不能の時 <c>NaN</c>.</returns>
        public static float Sweep (Node nodeA, Node nodeB, Vector3 v) {
            if (nodeA == null || nodeB == null || !nodeA.Is<Collision> () || !nodeB.Is<Collision> ()) {
                return Single.NaN;
            }

            return (from vertex in nodeB.Collision.Vertices
                    let pos = nodeB.GlobalTransform.Apply (vertex)
                    let d = RayCast (nodeA, pos, pos + v)
                    where d > 0
                    orderby d
                    select d).FirstOrDefault ();
        }


        /// <summary>
        /// 点Pがこのノードに包含されるかどうかの判定
        /// </summary>
        /// <remarks>
        /// このノードがコリジョン形状を持ち、その中に点Pが含まれる場合、
        /// このメソッドは <c>true</c> を返します。
        /// </remarks>
        /// <param name="node">ノード</param>
        /// <param name="x">点の位置X（グローバル座標）</param>
        /// <param name="y">点の位置Y（グローバル座標）</param>
        /// <param name="z">点の位置Z（グローバル座標）</param>
        /// <returns></returns>
        public static bool Contain (Node node, float x, float y, float z) {

            var p = node.LocalTransform.Apply (x, y, z);

            foreach (var col in node.GetComponents<Collision> ()) {
                if (col.Contain (p.X, p.Y, p.Z)) {
                    return true;
                };
            }

            return false;
        }

        /// <inheritdoc/>
        public override string ToString () {
            return Name;
        }
        #endregion

    }
}
