using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.DebugTools {
    public partial class DatabaseView : UserControl {
        World wld;

        public DatabaseView () {
            InitializeComponent ();
        }

        public World World {
            get { return wld; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("World is null");
                }
                this.wld = value;

                CreateDropDown ();
                CreateTabPages ();
            }
        }

        void CreateDropDown () {
            var dbms = wld.DataBaseManager;
            foreach (var db in dbms.DataBases) {
                toolStripButton1.DropDown.Items.Add (db.Key);
            }
        }

        /// <summary>
        /// テーブル1つに対してタブを1つ作成
        /// </summary>
        /// <remarks>
        /// <note>（*1）この空ループはDBからオンメモリキャッシュへデータをロードするため。
        /// Localプロパティは自動ではデータを更新しないので絶対に消さないこと！
        /// </note>
        /// </remarks>
        void CreateTabPages () {
            tabControl1.Controls.Clear ();
            
            var dbms = wld.DataBaseManager;

            foreach (var db in dbms.DataBases) {
                foreach (var tbl in db.Value.GetTables ()) {
                    var page = new TabPage (tbl.GetTableName());
                   
                    var gridView = new DataGridView ();
                    gridView.Dock = DockStyle.Fill;

                    // (*1) DBからデータをロード
                    foreach (var entry in tbl) {}

                    gridView.DataSource = tbl.Local;   // キャッシュの更新必要

                    page.Controls.Add (gridView);
                    tabControl1.Controls.Add (page);
                }
            }

        }

        private void toolStripButton2_Click (object sender, EventArgs e) {
            wld.DataBaseManager.SaveAllChanges();
        }

    }
}
