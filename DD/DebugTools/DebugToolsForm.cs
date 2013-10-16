using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.DebugTools {
    public partial class DebugToolsForm : Form {
        World wld;

        public DebugToolsForm () {
            InitializeComponent ();

        }

        public World World {
            get { return wld; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("World is null");
                }
                this.wld = value;
            }
        }

        private void DebugToolsForm_Load (object sender, EventArgs e) {
            // デフォルトでシーンビューを開く
        }

        private void シーンToolStripMenuItem_Click (object sender, EventArgs e) {
            foreach (Control con in panel1.Controls) {
                con.Dispose ();
            }
            panel1.Controls.Clear ();

            var view = new SceneView ();
            view.World = wld;
            
            panel1.Controls.Add (view);
        }

        private void メッセージToolStripMenuItem_Click (object sender, EventArgs e) {
            foreach (Control con in panel1.Controls) {
                con.Dispose ();
            }
            panel1.Controls.Clear ();

            var view = new DeliveryView ();
            view.World = wld;

            panel1.Controls.Add (view);
        }

        private void ログToolStripMenuItem_Click (object sender, EventArgs e) {
            foreach(Control con in panel1.Controls){
                con.Dispose ();
            }
            panel1.Controls.Clear ();

            var view = new LogView ();
            view.World = wld;

            panel1.Controls.Add (view);
        }

        private void バージョンToolStripMenuItem_Click (object sender, EventArgs e) {
            var form = new VersionForm ();
            form.ShowDialog ();
        }

        private void 終了ToolStripMenuItem_Click (object sender, EventArgs e) {
            Dispose ();
            Close ();
        }

    }
}
