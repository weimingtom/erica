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
        Rectangle bbox;
        bool visible;
        bool clickable;
        sbyte drawPriority;
        uint groupID;
        int userID;
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
            this.name = name;
            this.bbox = new Rectangle ();
            this.visible = true;
            this.clickable = true;
             this.parent = null;
            this.children = new List<Node> ();
            this.components = new List<Component> ();
            this.drawPriority = 0;
            this.groupID = 0xffffffffu;
            this.userID = 0;
        }
        #endregion

        #region Property
        /// <summary>
        /// ノード名
        /// </summary>
        public string Name {
            get { return name; }
        }

        /// <summary>
        /// ユーザーID
        /// </summary>
        public int UserID {
            get { return userID; }
            set { this.userID = value; }
        }

        /// <summary>
        /// グループID
        /// </summary>
        public uint GroupID {
            get { return groupID; }
            set { this.groupID = value; }
        }

        /// <summary>
        /// バウンディング ボックス
        /// </summary>
        /// <remarks>
        /// このノードのバウンディング ボックス（ローカル座標系）を返します。
        /// バウンディング ボックスはノードのクリック領域として利用されます。
        /// </remarks>
        public Rectangle BoundingBox {
            get { return bbox; }
            set { SetBoundingBox (value.X, value.Y, value.Width, value.Height); }
        }



        /// <summary>
        /// 表示フラグ
        /// </summary>
        /// <remarks>
        /// このフラグが <c>true</c> の時は表示されます。
        /// </remarks>
        public bool Visible {
            get { return visible; }
            set { this.visible = value; }
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
            set { SetDrawPriority (value); }
        }

        /// <summary>
        /// クリック可能フラグ
        /// </summary>
        /// <remarks>
        /// このフラグが <c>true</c> の時は表示されます。
        /// </remarks>
        public bool Clickable {
            get { return clickable; }
            set { this.clickable = value; }
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
        public Matrix4x4 GlobalTransform {
            get { return Upwards.Aggregate (Matrix4x4.Identity, (t, node) => node.Transform * t); }
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
        /// グローバル座標系での位置X
        /// </summary>
        /// <remarks>
        /// このプロパティは利便性のために実装されています。
        /// <see cref="GlobalTransform"/> の平行移動成分のXと同じです。
        /// </remarks>
        public float GlobalX {
            get {
                Vector3 point;
                Quaternion rotation;
                Vector3 scale;
                GlobalTransform.Decompress (out point, out rotation, out scale);
                return point.X;
            }
        }

        /// <summary>
        /// グローバル座標系での位置Y
        /// </summary>
        /// <remarks>
        /// このプロパティは利便性のために実装されています。
        /// <see cref="GlobalTransform"/> の平行移動成分のYと同じです。
        /// </remarks>
        public float GlobalY {
            get {
                Vector3 point;
                Quaternion rotation;
                Vector3 scale;
                GlobalTransform.Decompress (out point, out rotation, out scale);
                return point.Y;
            }
        }

        /// <summary>
        /// グローバル座標系での位置Z
        /// </summary>
        /// <remarks>
        /// このプロパティは利便性のために実装されています。
        /// <see cref="GlobalTransform"/> の平行移動成分のYと同じです。
        /// </remarks>
        public float GlobalZ {
            get {
                Vector3 point;
                Quaternion rotation;
                Vector3 scale;
                GlobalTransform.Decompress (out point, out rotation, out scale);
                return point.Z;
            }
        }


        /*
        /// <summary>
        /// グローバル座標系での座標位置
        /// </summary>
        /// <remarks>
        /// このプロパティは <see cref="GlobalTranslation"/> と等価です。
        /// </remarks>
        public Vector3 GlobalPoint {
            get {
                return GlobalTranslation;
            }
        }

        /// <summary>
        /// グローバル座標系での平行移動要素
        /// </summary>
        public Vector3 GlobalTranslation {
            get {
                Vector3 T;
                Quaternion R;
                Vector3 S;
                GlobalTransform.Decompress (out T, out R, out S);
                return T;
            }
        }

        /// <summary>
        /// グローバル座標系での回転要素
        /// </summary>
        public Quaternion GlobalRotation {
            get {
                Vector3 T;
                Quaternion R;
                Vector3 S;
                GlobalTransform.Decompress (out T, out R, out S);
                return R;
            }
        }

        /// <summary>
        /// グローバル座標系でのスケール要素
        /// </summary>
        public Vector3 GlobalScale {
            get {
                Vector3 T;
                Quaternion R;
                Vector3 S;
                GlobalTransform.Decompress (out T, out R, out S);
                return S;
            }
        }
        */

        #endregion

        #region Method
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
            var G = Matrix4x4.CreateFromTranslation(tx,ty,tz);
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
            var G = Matrix4x4.CreateFromRotation(rot);
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
            var G = Matrix4x4.CreateFromScale(sx,sy,sz);
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
            comp.SetNode (this);
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
            comp.SetNode (null);
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
        /// 指定のユーザーIDのノードを下方向から検索します。
        /// 一致するIDのノードが2つ以上あった場合は、どれが返るかは未定義です。
        /// 見つからない場合は <c>null</c> が返ります。
        /// </remarks>
        /// <param name="userID">検索したいユーザーID</param>
        /// <returns></returns>
        public Node Find (int userID) {
            return (from node in Downwards
                    where node.userID == userID
                    select node).FirstOrDefault();
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
                    where pred(node) == true
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
            return (from comp in components
                    where comp is T
                    select (T)comp).FirstOrDefault ();
        }

        /// <summary>
        /// バウンディング ボックスの変更
        /// </summary>
        /// <param name="x">ボックスの左上のX座標</param>
        /// <param name="y">ボックスの左上のY座標</param>
        /// <param name="width">ボックスの幅</param>
        /// <param name="height">ボックスの高さ</param>
        public void SetBoundingBox (int x, int y, int width, int height) {
            if (width < 0 || height < 0) {
                throw new ArgumentException ("Width or Hegiht is invalid");
            }
            this.bbox = new Rectangle (x, y, width, height);
        }

        /// <summary>
        /// 表示優先度の変更
        /// </summary>
        /// 表示優先度を変更します。デフォルトは 0 で -127 が一番優先度が高く、128が一番低いです。
        /// <param name="priority">優先度[-127,128]</param>
        public void SetDrawPriority (sbyte priority) {
            this.drawPriority = priority;
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





        /// <inheritdoc/>
        public override string ToString () {
            return Name;
        }
        #endregion

    }
}
