using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DD.DebugTools {
    /// <summary>
    /// ログ ビュー
    /// </summary>
    /// <remarks>
    /// ログを閲覧するコントロール。
    /// </remarks>
    public partial class LogView : UserControl {
        #region Field
        World wld;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public LogView () {
            InitializeComponent ();
        }
        #endregion

        #region Property
        /// <summary>
        /// DDワールド
        /// </summary>
        public World World {
            get { return wld; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("World is null");
                }
                this.wld = value; 
            }
        }
        #endregion

        /// <summary>
        /// タイマーによるログ表示の更新
        /// </summary>
        /// <remarks>
        /// 優先度によっては赤色で表示するとかすべきだが、めんどくさいのでやってない。
        /// </remarks>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
