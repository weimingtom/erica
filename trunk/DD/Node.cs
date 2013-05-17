using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// ノード クラス
    /// </summary>
    /// <remarks>
    /// スクリプトを構成するノード クラス。
    /// ノードは <see cref="Script"/> を頂点とする木構造のグラフを構成します。
    /// </remarks>
    public class Node {

        #region Field
        string name;
        Node parent;
        List<Node> children;
        List<Component> components;
        int x;
        int y;
        bool visible;
        bool clickable;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Node () : this("") {
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="name">ノード名</param>
        public Node (string name) {
            this.name = name;
            this.x = 0;
            this.y = 0;
            this.visible = true;
            this.clickable = true;
            this.parent = null;
            this.children = new List<Node> ();
            this.components = new List<Component> ();
            
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
        /// ローカル座標位置X
        /// </summary>
        public int X {
            get { return x; }
            set { this.x = value; }
        }

        /// <summary>
        /// ローカル座標位置Y
        /// </summary>
        public int Y {
            get { return y; }
            set { this.y = value; }
        }

        /// <summary>
        /// グローバル座標位置X
        /// </summary>
        public int GlobalX {
            get { return Upwards.Aggregate (0, (x, node) => x + node.x); }
        }

        /// <summary>
        /// グローバル座標位置Y
        /// </summary>
        public int GlobalY {
            get { return Upwards.Aggregate (0, (y, node) => y + node.y); }
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
        /// 全ての子ノードを列挙する列挙子
        /// </summary>
        /// <remarks>
        /// 自分自身と自分から再帰的に辿れるすべての子ノードを列挙します。
        /// 順番は先頭が自分で幅優先探索で子ノードが続きます。
        /// <note>
        /// 幅優先で実装してあるがこの仕様は有益か？
        /// </note>
        /// </remarks>
        public IEnumerable<Node> Downwards {
            get {
                var downs = new List<Node> () {this};
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
        /// すべての親ノードを列挙する列挙子
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
        /// コンポーネントを列挙する列挙子
        /// </summary>
        public IEnumerable<Component> Components {
            get { return components; }
        }
        #endregion

        #region Method
        /// <summary>
        /// コンポーネントのアタッチ
        /// </summary>
        /// <remarks>
        /// コンポーネントをノードに追加します。
        /// すでに他のノードで登録されているコンポーネントは登録できません。
        /// </remarks>
        /// <param name="comp">コンポーネント</param>
        public void Attach (Component comp) {
            if (comp == null) {
                throw new ArgumentNullException ("Component is null");
            }
            comp.SetNode(this);
            this.components.Add (comp);
        }

        /// <summary>
        /// コンポーネントのデタッチ
        /// </summary>
        /// <remarks>
        /// ノードからコンポーネントを削除します。
        /// </remarks>
        /// <param name="comp">コンポーネント</param>
        public void Detach (Component comp) {
            if (comp == null) {
                return;
            }
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
            if (node.Parent != null) {
                throw new ArgumentException ("Node has parent already");
            }
            node.parent = this;
            this.children.Add (node);
        }

        /// <summary>
        /// 子ノードの削除
        /// </summary>
        /// <param name="child">ノード</param>
        public void RemoveChild (Node child) {
            if (child == null) {
                throw new ArgumentNullException ("Node is null");
            }
            if (child.Parent != this) {
                throw new ArgumentNullException ("Node is not child of this");
            }
            child.parent = null;
            this.children.Remove (child);

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
