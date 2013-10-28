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
    /// 配達記録 ビュー
    /// </summary>
    /// <remarks>
    /// メッセージ通信 <see cref="PostOffice"/> によるメール通信の記録を表示します。
    /// </remarks>
    public partial class DeliveryView : UserControl {

        #region Field
        World wld;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public DeliveryView () {
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
                    throw new ArgumentException ("World is null");
                }
                this.wld = value;
            }
        }
        #endregion

        /// <summary>
        /// タイマーによる配達記録の表示の更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick (object sender, EventArgs e) {
            if (wld != null) {
                textBox1.SuspendLayout ();

                var po = wld.PostOffice;
                foreach (var rec in po.DeliveryRecords) {
                    textBox1.AppendText (rec.ToString () + "\r\n");
                }
                po.ClearRecords ();

                textBox1.ResumeLayout ();
            }
        }

        /// <summary>
        /// ビューの終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton1_Click (object sender, EventArgs e) {
            var form = new NewMailForm ();
            form.ShowDialog ();

            form.Dispose ();
            form.Close ();
        }

    }
}
