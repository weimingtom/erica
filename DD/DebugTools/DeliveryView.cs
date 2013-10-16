using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.DebugTools {
    public partial class DeliveryView : UserControl {
        int i = 0;
        World wld;

        public DeliveryView () {
            InitializeComponent ();
        }

        public World World {
            get { return wld; }
            set {
                if (value == null) {
                    throw new ArgumentException ("World is null");
                }
                this.wld = value;
            }
        }

        private void timer1_Tick (object sender, EventArgs e) {
            if (wld != null) {
                textBox1.SuspendLayout ();

                var po = wld.PostOffice;
                foreach (var rec in po.DeliveryRecords) {
                    textBox1.AppendText (rec.ToString () + "\r\n");
                }
                po.ClearRecords ();

                textBox1.ResumeLayout ();
            }
        }

        private void toolStripButton1_Click (object sender, EventArgs e) {
            var form = new NewMailForm ();
            form.ShowDialog ();

            form.Dispose ();
            form.Close ();
        }

    }
}
