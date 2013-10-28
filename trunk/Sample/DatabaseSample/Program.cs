using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.Sample.DatabaseSample {
    /// <summary>
    /// メイン フォーム
    /// </summary>
    public partial class Program : Form {

        int frame;
        World wld;


        public Program () {
            InitializeComponent ();
        }

        public static void Main (string[] args) {
            Application.EnableVisualStyles ();
            Application.SetCompatibleTextRenderingDefault (false);
            Application.Run (new Program());
        }

        public static World CreateWorld () {
            Resource.SetTextureDirectory ("DatabaseSample/Textures/");

            // ----------------------------------------
            var wld = new World ();
            
            var node1 = MyCharacterHolder.Create ();
            var node2 = MyCharacterViewer.Create (new Vector3 (0, 0, 0));
            var node3 = MyCharacterSelector.Create (new Vector3 (0, 0, 0));

            // ----------------------------------------

            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);

            return wld;
        }

        private void 終了ToolStripMenuItem_Click (object sender, EventArgs e) {
            Dispose ();
            Close ();
        }

        private void Program_Load (object sender, EventArgs e) {
            var g2d = Graphics2D.GetInstance ();
            g2d.CreateWindow (panel1.Handle);

            Console.WriteLine ("Start of Game");

            this.wld = CreateWorld ();

            var view = new DebugTools.DatabaseView ();
            view.Dock = DockStyle.Fill;
            
            var form = new Form ();
            form.Controls.Add (view);
            form.Show ();
        }

        private void timer1_Tick (object sender, EventArgs e) {

            if (wld != null) {
                this.frame += 1;
              
                var g2d = Graphics2D.GetInstance ();
                var msec = frame * 16;

                g2d.Dispatch (wld);
                wld.Animate (msec, 33);
                wld.Deliver ();
                wld.CollisionUpdate ();
                wld.Update (msec);
                wld.Purge ();
                g2d.Draw (wld);
            }
        }

        private void Program_FormClosing (object sender, FormClosingEventArgs e) {
            Console.WriteLine ("End of Game");
            if (wld != null) {
                wld.Destroy ();
            }
        }
    }
}
