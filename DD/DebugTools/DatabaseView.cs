using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.Entity;

namespace DD.DebugTools {
    /// <summary>
    /// データベース ビュー
    /// </summary>
    /// <remarks>
    /// データベースを見るためのコントロール。
    /// <see cref="Resource"/> を使って開かれたデータベースだけが表示の対象です。
    /// </remarks>
    public partial class DatabaseView : UserControl {
        #region Field
        World wld;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public DatabaseView () {
            InitializeComponent ();
        }
        #endregion

        #region Property
        /// <summary>
        /// DDワールド
        /// </summary>
        /// <remarks>
        /// ただし未使用。代入したタイミングでデータベースが表示される。
        /// </remarks>
        public World World{
            get { return wld; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("World is null");
                }
                this.wld = value;

                CreateDropDownList ();
                CreateTabPages (null);
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// データベース選択ドロップダウンリストの表示
        /// </summary>
        void CreateDropDownList () {
            foreach (var db in Resource.Databases) {
                var item = new ToolStripMenuItem (db.Key);
                item.Click += delegate (object sender, EventArgs e) {
                    CreateTabPages (db.Key);
                };
                toolStripDropDownButton1.DropDownItems.Add (item);
            }
        }

        /// <summary>
        /// テーブル表示タブの作成
        /// </summary>
        /// <remarks>
        /// 選択されたデータベースの全ての表を、表1つに付き1タブで表示します。
        /// <note>
        /// (*1)この空ループはDBからオンメモリキャッシュへデータをロードするために必要。
        /// Localプロパティは自動ではデータを更新しないので絶対に消さないこと！
        /// </note>
        /// </remarks>
        void CreateTabPages (string dbName) {
            tabControl1.Controls.Clear ();

            if (dbName == null && Resource.Databases.Count() > 0) {
                dbName = Resource.Databases.First().Key;
            }

            var db = Resource.GetDatabase (dbName);
            if (db != null) {
                foreach (var table in db.GetDbSets ()) {
                    var grid = new DataGridView ();
                    grid.Dock = DockStyle.Fill;
                    foreach (var entry in table) {
                        // (*1)
                    }
                    grid.DataSource = table.Local;

                    var page = new TabPage (table.GetDbSetName ());
                    page.Controls.Add (grid);

                    tabControl1.Controls.Add (page);
                }
            }

        }
        #endregion



    }
}
