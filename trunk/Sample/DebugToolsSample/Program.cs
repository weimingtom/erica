using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using DD.DebugTools;

namespace DD.Sample.DebugToolsSample {
    public partial class DebugToolsSampleProgram : Form {

        public static void Main (string[] args) {
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);

            // デバッグツールの直接起動
            //var form = new DebugToolsForm ();
            //form.World = CreateWorld (); 

            var form = new DebugToolsSampleProgram ();
            form.World = CreateWorld ();

            Application.Run (form);
        }

        Graphics2D g2d;
        World wld;
        int count;

        public DebugToolsSampleProgram () {
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

        private void Program_Load (object sender, EventArgs e) {
            this.g2d = null;
            this.wld = CreateWorld ();
            this.count = 0;

            g2d = Graphics2D.GetInstance ();
            g2d.CreateWindow (panel1.Handle);
        }

        static World CreateWorld () {
            var wld = new World ("MyWorld");

            var node1 = MyCharacters.Create ();
            var node2 = MyBackground.Create ();
            var node3 = MyDisplayPanel.Create (new Vector3(0, 380, 0));
            var node4 = MyStatusPanel.Create (new Vector3 (100, 100, 0));
            var node5 = MyButtons.Create (new Vector3 (520, 300, 0));

            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (node5);
            
            return wld;
        }

        private void timer1_Tick (object sender, EventArgs e) {

            var interval = ((Timer)sender).Interval;
            var msec = count * interval;

            g2d.Dispatch (wld);
            wld.Animate (msec, interval);
            wld.CollisionUpdate ();
            wld.Deliver ();
            wld.Update (msec);
            wld.Purge ();
            g2d.Draw (wld);

            this.count += 1;
        }

        private void Program_FormClosing (object sender, FormClosingEventArgs e) {
            if (wld != null) {
                wld.Destroy ();
                wld = null;
            }
        }

        private void toolStripButton1_Click (object sender, EventArgs e) {
            var form = new DebugToolsForm ();
            form.World = wld;
            form.Show ();
        }

        private void デバッグツールの起動ToolStripMenuItem_Click (object sender, EventArgs e) {
            var form = new DebugToolsForm ();
            form.World = wld;
            form.Show ();
        }

    }
}
