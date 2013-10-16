using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.DebugTools {
    public partial class VersionForm : Form {
        public VersionForm () {
            InitializeComponent ();
        }

        private void button1_Click (object sender, EventArgs e) {
            Dispose ();
            Close ();
        }
    }
}
