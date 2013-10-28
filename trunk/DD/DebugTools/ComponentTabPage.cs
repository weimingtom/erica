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

    /// <summary>
    /// DDコンポーネントを表示するタブ コントロール
    /// </summary>
    public partial class ComponentTabPage : TabPage {
        Component cmp;

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public ComponentTabPage () {
            InitializeComponent ();
        }
        #endregion

        #region Property
        /// <summary>
        /// 表示対象のDDコンポーネント
        /// </summary>
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
        #endregion

        #region Method
        /// <summary>
        /// コンポーネントの全てのプロパティを表示
        /// </summary>
        /// <remarks>
        /// 単にプロパティに ToString() を付けて表示しているだけ。
        /// もう少し工夫の余地があるようなどうでも良いような。
        /// </remarks>
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
            catch (TargetParameterCountException ) {
                Console.Write ("インデクサー付きプロパティが呼ばれました");
            }
        }

        /// <summary>
        /// タイマーによる表示の更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick (object sender, EventArgs e) {
            DisplayComponentMembers ();
        }
        #endregion



    }
}
