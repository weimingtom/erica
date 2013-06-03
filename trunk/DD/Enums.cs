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

    /// <summary>
    /// 補完方法
    /// </summary>
    public enum InterpolationType {
        /// <summary>
        /// ステップ補完
        /// </summary>
        Step,

        /// <summary>
        /// 線形補完
        /// </summary>
        Linear,

        /// <summary>
        /// 球面線形補完
        /// </summary>
        SLerp,
    }

    /// <summary>
    /// 繰り返しモード
    /// </summary>
    public enum WrapMode {
        /// <summary>
        /// 一回限り
        /// </summary>
        Once,

        /// <summary>
        /// 繰り返し再生
        /// </summary>
        Loop
    }

    /// <summary>
    /// 繰り返しモード
    /// </summary>
    public enum LoopMode {
        /// <summary>
        /// 一回限り
        /// </summary>
        Once,

        /// <summary>
        /// 繰り返し再生
        /// </summary>
        Loop
    }

    /// <summary>
    /// 文字スタイル
    /// </summary>
    [Flags]
    public enum CharacterStyle {
        /// <summary>
        /// 標準
        /// </summary>
        Regular = 0,

        /// <summary>
        /// 太字
        /// </summary>
        Bold,

        /// <summary>
        /// 斜体
        /// </summary>
        Italic,

        /// <summary>
        /// 下線付き
        /// </summary>
        Underlined,

        /// <summary>
        /// 影付き
        /// </summary>
        /// <note>
        /// 影付き文字の一般的な実装は（自分で）文字をずらしながら2回書く事です。
        /// これだけ実装方法が他と少し違う。
        /// </note>
        Shadow
    }

    /// <summary>
    /// フィード モード
    /// </summary>
    public enum FeedMode {
        /// <summary>
        /// 手動送り
        /// </summary>
        Manual,

        /// <summary>
        /// 自動送り
        /// </summary>
        Automatic
    }

}
