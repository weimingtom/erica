using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Yaml;
using System.Yaml.Serialization;

namespace DD.DebugTools {
    public partial class NewMailForm : Form {
        YamlSerializer yaml;
        World wld;

        public NewMailForm () {
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


        private void button1_Click (object sender, EventArgs e) {
            if (yaml == null) {
                yaml = new YamlSerializer ();
            }
            var addr = textBox1.Text;
            var letter = yaml.Deserialize (textBox2.Text);

            if (wld != null && letter != null) {
                wld.PostOffice.Post (wld, addr, letter[0]);
            }

            this.Dispose ();
            this.Close ();
        }
    }
}
