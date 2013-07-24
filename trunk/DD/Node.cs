using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="name">ノード名</param>
        public Node (string name)
            : base () {
            this.name = name ?? "";
            this.drawable = true;
            this.updatable = true;
            this.animatable = true;
            this.collidable = true;
            this.parent = null;
            this.children = new List<Node> ();
            this.components = new List<Component> ();
            this.drawPriority = 0;
            this.updatePriority = 0;
            this.groupID = 0xffffffffu;
            this.userData = new Dictionary<string, object> ();
            this.opacity = 1.0f;
            this.matrix = null;

        }
        #endregion

        #region Property
        /// <summary>
        /// ノード名
        /// </summary>
        public string Name {
            get { return name; }
            set { this.name = value ?? ""; }
        }

        /// <summary>
        /// グループID
        /// </summary>
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
        /// </remarks>
        public bool Animatable {
            get { return animatable; }
            set { this.animatable = value; }
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
            get { return components; }
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
        public Vector3 Point {
            get {
                Vector3 T;
                Quaternion R;
                Vector3 S;
                GlobalTransform.Decompress (out T, out R, out S);
                return T;
            }
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
        /// 指定の名前のノードを下方向から検索します。
        /// 一致する名前のノードが2つ以上あった場合は、どれが返るかは未定義です。
        /// 見つからない場合は <c>null</c> が返ります。
        /// </remarks>
        /// <param name="name">検索したいノード名</param>
        /// <returns></returns>
        public Node Find (string name) {
            return (from node in Downwards
                    where node.Name == (name ?? "")
                    select node).FirstOrDefault ();
        }

        /// <summary>
        /// ノードの検索
        /// </summary>
        /// <remarks>
        /// 指定の条件式 <paramref name="pred"/> を満たすノードを下方向から検索します。
        /// 条件に一致するノードが2つ以上あった場合は、どれが返るかは未定義です。
        /// 見つからない場合は <c>null</c> が返ります。
        /// </remarks>
        /// <param name="pred">条件式</param>
        /// <returns></returns>
        public Node Find (Func<Node, bool> pred) {
            return (from node in Downwards
                    where pred (node) == true
                    select node).FirstOrDefault ();
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

        // 微妙すぎるのでコメントアウト
        // 悩む
        /*
        public Node Instanciate<T> (Vector3 pos, Vector3 norm) where T : Component, new () {
            var node = new Node ();
            AddChild (node);
            node.Translation = pos;

            var comp = new T ();
            node.Attach (comp);

            comp.OnInit ();

            return node;
        }
        */



        /// <inheritdoc/>
        public override string ToString () {
            return Name;
        }
        #endregion

    }
}
