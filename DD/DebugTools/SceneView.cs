using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.DebugTools {
    public partial class SceneView : UserControl {
        
        World wld;

        public SceneView () {
            InitializeComponent ();
            this.Dock = DockStyle.Fill;

        }

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

        void ConstructTreeView () {
            ConstructTreeNode (null, wld);
            treeView1.ExpandAll ();
            treeView1.SelectedNode = treeView1.Nodes[0];
        }

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

    }
}
