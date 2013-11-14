using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DD;

namespace あかときっ_偽_ {
    public partial class Form1 : Form {
        World wld;
        long frame;

        public Form1 () {
            InitializeComponent ();

            this.wld = CreateWorld();
            this.frame = 0;
        }

        /// <summary>
        /// ワールドの作成
        /// </summary>
        /// <returns></returns>
        public World CreateWorld () {
            var g2d = Graphics2D.GetInstance ();
            g2d.CreateWindow (panel1.Handle);

            Resource.SetTextureDirectory ("Data/");
            Resource.SetAudioDirectory ("Data/");

            var wld = new World ();
            
            var node1 = MyCharacter.Create ("Maki");
            var node2 = MainBattleWindow.Create ();
            var node3 = MyButton.Create (new Vector3 (10, 10, 0), MyButton.Pose.標準待機);
            var node4 = MyButton.Create (new Vector3 (100, 10, 0), MyButton.Pose.被弾);
            var node5 = MyStatusController.Create (new Vector3 (10, 180, 0));
            var node7 = MyStatusViewer.Create(new Vector3 (10, 100,0), "Maki");

            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            wld.AddChild (node4);
            wld.AddChild (node5);
            wld.AddChild (node7);
            
            return wld;
        }


        private void characterToolStripMenuItem_Click (object sender, EventArgs e) {
            var db = new Data.AkatokiEntities ();
            foreach (var ch in db.Characters) {
                Console.WriteLine ("{0} : {1}", ch.ID, ch.Name);
            }
            var view = new DataGridView ();
            view.DataSource = db.Characters.Local;
            view.Show();
            view.Dock = DockStyle.Fill;

            var form = new Form ();
            form.Controls.Add (view);
            form.Show ();
        }

        private void toolStripButton1_Click (object sender, EventArgs e) {
      
        }

        /// <summary>
        /// メインループ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick (object sender, EventArgs e) {
            Console.WriteLine ("Frame = {0}", frame++);

            if (wld != null) {
                var g2d = Graphics2D.GetInstance ();
                var interval = timer1.Interval;
                var msec = frame * interval;

                g2d.Dispatch (wld);

                wld.Deliver ();
                wld.Animate (msec, interval);
                wld.CollisionUpdate ();
                wld.Update (msec);
                wld.Purge ();
                g2d.Draw (wld);
            }
        }

        private void 終了ToolStripMenuItem_Click (object sender, EventArgs e) {
            if (wld != null) {
                wld.Destroy ();
                wld = null;
            }
        }

        private void Form1_FormClosed (object sender, FormClosedEventArgs e) {
            if (wld != null) {
                wld.Destroy ();
                wld = null;
            }
        }
    }
}
