using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.DebugTools {
    public partial class LogView : UserControl {
        int i = 0;
        World wld;

        public LogView () {
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

        private void timer1_Tick (object sender, EventArgs e) {
            if (wld != null) {
                textBox1.SuspendLayout ();
                
                foreach(var log in wld.Logger.Logs){
                    textBox1.AppendText(string.Format("{0} : {1} : {2}\r\n", log.Priority, log.Node, log.Message));
                }
                wld.Logger.Clear ();

                textBox1.ResumeLayout ();
            }
        }
    }
}
