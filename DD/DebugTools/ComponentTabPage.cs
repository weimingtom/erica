using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace DD.DebugTools {
    public partial class ComponentTabPage : TabPage {
        Component cmp;

        public ComponentTabPage () {
            InitializeComponent ();
        }

        public Component Component {
            get { return cmp; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("Component is null");
                }

                this.cmp = value;
                this.Tag = cmp;
                this.Text = cmp.GetType ().Name;

                DisplayComponentMembers ();
            }
        }

        public void DisplayComponentMembers () {
            try {
                textBox1.Clear ();
                var propInfo = cmp.GetType ().GetProperties ();
                foreach (var prop in propInfo) {
                    if (prop.Name == "Item") {
                        // インデクサーは無視
                        continue;
                    }
                    var name = prop.Name;
                    var value = prop.GetValue (cmp, null);

                    textBox1.AppendText (string.Format ("{0} : {1}\r\n", name, value));
                }
            }
            catch (TargetParameterCountException e) {
                Console.Write ("インデクサー付きプロパティが呼ばれました");
            }
        }

        private void timer1_Tick (object sender, EventArgs e) {
            DisplayComponentMembers ();
        }



    }
}
