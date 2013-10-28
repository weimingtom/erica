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
    /// 何もしない ビュー
    /// </summary>
    /// <remarks>
    /// 空のツールチップスとステータスバーだけ表示するビュー。
    /// </remarks>
    public partial class NullView : UserControl {
        /// <summary>
        /// コンストラクター
        /// </summary>
        public NullView () {
            InitializeComponent ();
        }
    }
}
