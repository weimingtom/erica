using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.DebugTools {
    /// <summary>
    /// シーン ビュー
    /// </summary>
    /// <remarks>
    /// シーンを表示するビューです。
    /// おそらくもっともお世話になるビューです。
    /// </remarks>
    public partial class SceneView : UserControl {
        #region Field
        World wld;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public SceneView () {
            InitializeComponent ();
            this.Dock = DockStyle.Fill;

        }
        #endregion
        /// <summary>
        /// DDワールド
        /// </summary>
        #region Property
        public World World {
            get { return wld; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("World is null");
                }
                this.wld = value;
                ConstructTreeView ();
            }
        }
        #endregion

        #region Field
        /// <summary>
        /// 左ペインのツリー ビューの表示
        /// </summary>
        void ConstructTreeView () {
            ConstructTreeNode (null, wld);
            treeView1.ExpandAll ();
            treeView1.SelectedNode = treeView1.Nodes[0];
        }

        /// <summary>
        /// 左ペインのつりーノードの表示
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="node"></param>
        void ConstructTreeNode (TreeNode treeNode, Node node) {
            if (treeNode == null) {
                treeNode = treeView1.Nodes.Add (node.UniqueID.ToString (), node.Name);
            }
            else {
                treeNode = treeNode.Nodes.Add (node.UniqueID.ToString (), node.Name);
            }
            treeNode.Tag = node;

            foreach (var child in node.Children) {
                ConstructTreeNode (treeNode, child);
            }

        }

        /// <summary>
        /// セレクトが移動した時の処理
        /// </summary>
        /// <remarks>
        /// 現在選択されているノードの情報を右ペインに表示します。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect (object sender, TreeViewEventArgs e) {
            var node = ((TreeView)sender).SelectedNode.Tag as Node;

            tabControl1.Controls.Clear ();
            foreach (var cmp in node.Components) {
                var tab = new ComponentTabPage ();
                tab.Component = cmp;
                tabControl1.TabPages.Add (tab);
            }

            treeView1.Select();
        }
        #endregion

    }
}
