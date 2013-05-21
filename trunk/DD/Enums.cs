using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {

    /// <summary>
    /// マウス ボタン
    /// </summary>
    public enum MouseButton {
        /// <summary>
        /// 左マウスボタン
        /// </summary>
        Left,

        /// <summary>
        /// 右マウスボタン
        /// </summary>
        Right,

        /// <summary>
        /// 中マウスボタン
        /// </summary>
        Middle,
    }

    /// <summary>
    /// ボタン状態
    /// </summary>
    public enum ButtonState {
        /// <summary>
        /// 標準時
        /// </summary>
       Normal,

        /// <summary>
        /// フォーカスを得ている時
        /// </summary>
        Focused,

        /// <summary>
        /// 押されている時
        /// </summary>
        Pressed,

        /// <summary>
        /// 押された状態でフォーカスを得ている時
        /// </summary>
        PressedFocused,

    }

}
