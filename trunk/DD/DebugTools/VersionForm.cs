using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.DebugTools {
    /// <summary>
    /// DDのバージョン表示 フォーム
    /// </summary>
    public partial class VersionForm : Form {

        /// <summary>
        /// コンストラクター
        /// </summary>
        public VersionForm () {
            InitializeComponent ();
        }

        /// <summary>
        /// OKボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click (object sender, EventArgs e) {
            Dispose ();
            Close ();
        }
    }
}
