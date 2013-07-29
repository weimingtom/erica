using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// ボタン状態
    /// </summary>
    public enum ButtonState {
        /// <summary>
        /// 標準状態
        /// </summary>
        Normal,

        /// <summary>
        /// 標準状態（フォーカスあり）
        /// </summary>
        FocusedNormal,

        /// <summary>
        /// 押された状態
        /// </summary>
        Pressed,

        /// <summary>
        /// 押された状態（フォーカスあり）
        /// </summary>
        FocusedPressed
    }

    /// <summary>
    /// ボタン種別
    /// </summary>
    public enum ButtonType {
        /// <summary>
        /// プッシュ式
        /// </summary>
        Push,

        /// <summary>
        /// トグル式
        /// </summary>
        Toggle
    }

    /// <summary>
    /// カメラの投影方式
    /// </summary>
    public enum ProjectionType {

        /// <summary>
        /// 未定義
        /// </summary>
        Undefined,

        /// <summary>
        /// 平行投影
        /// </summary>
        Parallel,

        /// <summary>
        /// 透視投影
        /// </summary>
        Projection,

        /// <summary>
        /// 2Dスクリーン投影
        /// </summary>
        Screen
    }

    /// <summary>
    /// シェイプ タイプ
    /// </summary>
    public enum ShapeType {
        /// <summary>
        /// 未定義（使用しない）
        /// </summary>
        Undefined,

        /// <summary>
        /// 多角形
        /// </summary>
        /// <remarks>
        /// 多角形は方形（<see cref="BoxCollisionShape"/>）と菱形（<see cref="RhombusCollisionShape"/>）の両方が該当します。
        /// </remarks>
        Polygon,

        /// <summary>
        /// 球形
        /// </summary>
        Sphere,

    }




    /// <summary>
    /// キー コード
    /// </summary>
    /// <remarks>
    /// キー コードは一般的な109キーボードに加えてマウス ボタンも含まれます。
    /// </remarks>
    public enum KeyCode {
        /// <summary>
        /// 使用しません
        /// </summary>
        None,

        /// <summary>
        /// バックスペース 'Back Space'
        /// </summary>
        Backspace,

        /// <summary>
        /// 削除 'Delete'
        /// </summary>
        Delete,

        /// <summary>
        /// タブ 'Tab'
        /// </summary>
        Tab,

        /// <summary>
        /// クリア 'Clear'
        /// </summary>
        /// <remarks>
        /// Clearキーなんてあったっけ？
        /// </remarks>
        Clear,

        /// <summary>
        /// リターン 'Enter'
        /// </summary>
        Return,

        /// <summary>
        /// ポーズ 'Pause'
        /// </summary>
        Pause,

        /// <summary>
        /// エスケープ 'Escape'
        /// </summary>
        Escape,

        /// <summary>
        /// スペース 'Space'
        /// </summary>
        Space,

        /// <summary>
        /// ナンバーパッド '0'
        /// </summary>
        Keypad0,

        /// <summary>
        /// ナンバーパッド '1'
        /// </summary>
        Keypad1,

        /// <summary>
        /// ナンバーパッド '2'
        /// </summary>
        Keypad2,

        /// <summary>
        ///ナンバーパッド '3'
        /// </summary>
        Keypad3,

        /// <summary>
        /// ナンバーパッド '4'
        /// </summary>
        Keypad4,

        /// <summary>
        /// ナンバーパッド '5'
        /// </summary>
        Keypad5,

        /// <summary>
        /// ナンバーパッド '6'
        /// </summary>
        Keypad6,

        /// <summary>
        ///ナンバーパッド '7'
        /// </summary>
        Keypad7,

        /// <summary>
        /// ナンバーパッド '8'
        /// </summary>
        Keypad8,

        /// <summary>
        /// ナンバーパッド '9'
        /// </summary>
        Keypad9,

        /// <summary>
        ///ナンバーパッド '.'
        /// </summary>
        KeypadPeriod,

        /// <summary>
        /// ナンバーパッド '/'
        /// </summary>
        KeypadDivide,

        /// <summary>
        /// ナンバーパッド '*'
        /// </summary>
        KeypadMultiply,

        /// <summary>
        /// ナンバーパッド '-'
        /// </summary>
        KeypadMinus,

        /// <summary>
        ///ナンバーパッド '+'
        /// </summary>
        KeypadPlus,

        /// <summary>
        /// ナンバーパッド 'Enter'
        /// </summary>
        KeypadEnter,

        /// <summary>
        /// ナンバーパッド '='
        /// </summary>
        KeypadEquals,

        /// <summary>
        /// 上矢印 '↑'
        /// </summary>
        UpArrow,

        /// <summary>
        /// 下矢印 '↓'
        /// </summary>
        DownArrow,

        /// <summary>
        /// 右矢印 '→'
        /// </summary>
        RightArrow,

        /// <summary>
        /// 左矢印 '←'
        /// </summary>
        LeftArrow,
        
        /// <summary>
        /// 挿入 'Insert'
        /// </summary>
        Insert,

        /// <summary>
        /// ホーム 'Home'
        /// </summary>
        Home,

        /// <summary>
        /// 'End'
        /// </summary>
        End,

        /// <summary>
        /// ページ アップ 'PageUp'
        /// </summary>
        PageUp,

        /// <summary>
        /// ページ ダウン 'PageDown'
        /// </summary>
        PageDown,

        /// <summary>
        /// ファンクション キー 'F1'
        /// </summary>
        F1,

        /// <summary>
        /// ファンクション キー 'F2'
        /// </summary>
        F2,

        /// <summary>
        /// ファンクション キー 'F3'
        /// </summary>
        F3,

        /// <summary>
        /// ファンクション キー 'F4'
        /// </summary>
        F4,

        /// <summary>
        /// ファンクション キー 'F5'
        /// </summary>
        F5,

        /// <summary>
        /// ファンクション キー 'F6'
        /// </summary>
        F6,

        /// <summary>
        /// ファンクション キー 'F7'
        /// </summary>
        F7,

        /// <summary>
        /// ファンクション キー 'F8'
        /// </summary>
        F8,

        /// <summary>
        /// ファンクション キー 'F9'
        /// </summary>
        F9,

        /// <summary>
        /// ファンクション キー 'F10'
        /// </summary>
        F10,

        /// <summary>
        /// ファンクション キー 'F11'
        /// </summary>
        F11,

        /// <summary>
        /// ファンクション キー 'F12'
        /// </summary>
        F12,

        /// <summary>
        /// ファンクション キー 'F13'
        /// </summary>
        F13,

        /// <summary>
        /// ファンクション キー 'F14'
        /// </summary>
        F14,

        /// <summary>
        /// ファンクション キー 'F15'
        /// </summary>
        F15,
        
        /// <summary>
        /// アルファベット キー '0'
        /// </summary>
        Alpha0,

        /// <summary>
        /// アルファベット キー '1'
        /// </summary>
        Alpha1,

        /// <summary>
        /// アルファベット キー '2'
        /// </summary>
        Alpha2,

        /// <summary>
        /// アルファベット キー '3'
        /// </summary>
        Alpha3,

        /// <summary>
        /// アルファベット キー '4'
        /// </summary>
        Alpha4,

        /// <summary>
        /// アルファベット キー '5'
        /// </summary>
        Alpha5,

        /// <summary>
        /// アルファベット キー '6'
        /// </summary>
        Alpha6,

        /// <summary>
        /// アルファベット キー '7'
        /// </summary>
        Alpha7,

        /// <summary>
        /// アルファベット キー '8'
        /// </summary>
        Alpha8,

        /// <summary>
        /// アルファベット キー '9'
        /// </summary>
        Alpha9,

        /// <summary>
        /// 'Exclaim'
        /// </summary>
        Exclaim,

        /// <summary>
        /// 2重引用符 '"'
        /// </summary>
        DoubleQuote,

        /// <summary>
        /// 'Hash'
        /// </summary>
        Hash,

        /// <summary>
        /// ドル記号 '$'
        /// </summary>
        Dollar,

        /// <summary>
        /// アンド記号 '&amp;'
        /// </summary>
        Ampersand,

        /// <summary>
        /// 引用符 '''
        /// </summary>
        Quote,

        /// <summary>
        /// 左括弧 '['
        /// </summary>
        LeftParen,

        /// <summary>
        /// 右括弧 ']'
        /// </summary>
        RightParen,

        /// <summary>
        /// 星印 '*'
        /// </summary>
        Asterisk,

        /// <summary>
        /// プラス記号 '+'
        /// </summary>
        Plus,

        /// <summary>
        /// コンマ ','
        /// </summary>
        Comma,

        /// <summary>
        /// マイナス記号 '-'
        /// </summary>
        Minus,

        /// <summary>
        /// ピリオド '.'
        /// </summary>
        Period,

        /// <summary>
        /// 除算記号 '/'
        /// </summary>
        Slash,

        /// <summary>
        /// コロン ':'
        /// </summary>
        Colon,

        /// <summary>
        /// セミコロン ';'
        /// </summary>
        Semicolon,

        /// <summary>
        /// より小さい '&lt;'
        /// </summary>
        Less,

        /// <summary>
        /// 等号 '='
        /// </summary>
        Equals,

        /// <summary>
        /// より大きい '&gt;'
        /// </summary>
        Greater,

        /// <summary>
        /// 疑問符 '?'
        /// </summary>
        Question,

        /// <summary>
        /// アットマーク '@'
        /// </summary>
        At,

        /// <summary>
        /// 左角形括弧 '['
        /// </summary>
        LeftBracket,

        /// <summary>
        /// 逆斜線 '\'
        /// </summary>
        /// <remarks>
        /// 日本語キーボードでは円記号'\'になる事が多い
        /// </remarks>
        Backslash,

        /// <summary>
        /// 右角形括弧 ']'
        /// </summary>
        RightBracket,

        /// <summary>
        /// キャレット '^'
        /// </summary>
        Caret,

        /// <summary>
        /// 下線 '_'
        /// </summary>
        Underscore,

        /// <summary>
        /// 逆引用符 '`'
        /// </summary>
        BackQuote,

        /// <summary>
        /// 文字 'A'
        /// </summary>
        A,

        /// <summary>
        /// 文字 'B'
        /// </summary>
        B,

        /// <summary>
        /// 文字 'C'
        /// </summary>
        C,

        /// <summary>
        /// 文字 'D'
        /// </summary>
        D,

        /// <summary>
        /// 文字 'E'
        /// </summary>
        E,

        /// <summary>
        /// 文字 'F'
        /// </summary>
        F,

        /// <summary>
        /// 文字 'G'
        /// </summary>
        G,

        /// <summary>
        /// 文字 'H'
        /// </summary>
        H,

        /// <summary>
        /// 文字 'I'
        /// </summary>
        I,

        /// <summary>
        /// 文字 'J'
        /// </summary>
        J,

        /// <summary>
        /// 文字 'K'
        /// </summary>
        K,

        /// <summary>
        /// 文字 'L'
        /// </summary>
        L,

        /// <summary>
        /// 文字 'M'
        /// </summary>
        M,

        /// <summary>
        /// 文字 'N'
        /// </summary>
        N,

        /// <summary>
        /// 文字 'O'
        /// </summary>
        O,

        /// <summary>
        /// 文字 'P'
        /// </summary>
        P,

        /// <summary>
        /// 文字 'Q'
        /// </summary>
        Q,

        /// <summary>
        /// 文字 'R'
        /// </summary>
        R,

        /// <summary>
        /// 文字 'S'
        /// </summary>
        S,

        /// <summary>
        /// 文字 'T'
        /// </summary>
        T,

        /// <summary>
        /// 文字 'U'
        /// </summary>
        U,

        /// <summary>
        /// 文字 'V'
        /// </summary>
        V,

        /// <summary>
        /// 文字 'W'
        /// </summary>
        W,

        /// <summary>
        /// 文字 'X'
        /// </summary>
        X,

        /// <summary>
        /// 文字 'Y'
        /// </summary>
        Y,

        /// <summary>
        /// 文字 'Z'
        /// </summary>
        Z,

        /// <summary>
        /// ナンバー ロック 'Num Lock'
        /// </summary>
        Numlock,

        /// <summary>
        /// キャプス ロック 'Caps Lock'
        /// </summary>
        CapsLock,

        /// <summary>
        /// スクロール ロック 'Scroll Lock'
        /// </summary>
        ScrollLock,

        /// <summary>
        /// 右シフト 'Shift'
        /// </summary>
        RightShift,

        /// <summary>
        /// 左シフト 'Shift'
        /// </summary>
        LeftShift,

        /// <summary>
        /// 右コントロール 'Ctrl'
        /// </summary>
        RightControl,

        /// <summary>
        /// 左コントロール
        /// </summary>
        LeftControl,

        /// <summary>
        /// 右アルト 'Alt'
        /// </summary>
        RightAlt,

        /// <summary>
        /// 左アルト 'Alt'
        /// </summary>
        LeftAlt,

        /// <summary>
        /// 左コマンドキー 'Command'
        /// </summary>
        LeftCommand,

        /// <summary>
        /// 左アップルキー 'Apple'
        /// </summary>
        LeftApple,

        /// <summary>
        /// 左ウィンドウキー 'Window'
        /// </summary>
        LeftWindows,

        /// <summary>
        /// 右コマンドキー 'Command'
        /// </summary>
        RightCommand,

        /// <summary>
        /// 右アップルキー 'Apple'
        /// </summary>
        RightApple,

        /// <summary>
        /// 右ウィンドウキー 'Window'
        /// </summary>
        RightWindows,

        /// <summary>
        /// グラフ キー 'Alt'
        /// </summary>
        /// <remarks>
        /// Windowsでは右アルト キーと同じ？
        /// </remarks>
        AltGr,

        /// <summary>
        /// ヘルプ 'Help'
        /// </summary>
        Help,

        /// <summary>
        /// 印刷 'Print'
        /// </summary>
        Print,

        /// <summary>
        /// システムリクエスト 'SysReq'
        /// </summary>
        SysReq,

        /// <summary>
        /// ブレイク 'Break'
        /// </summary>
        Break,

        /// <summary>
        /// メニュー 'Menu'
        /// </summary>
        Menu,

        /// <summary>
        /// マウス ボタン 0
        /// </summary>
        /// <remarks>
        /// 通常は左ボタン
        /// </remarks>
        Mouse0,

        /// <summary>
        /// マウス ボタン 1
        /// </summary>
        /// <remarks>
        /// 通常は右ボタン
        /// </remarks>
        Mouse1,

        /// <summary>
        /// マウス ボタン 2
        /// </summary>
        /// <remarks>
        /// 通常は中ボタン
        /// </remarks>
        Mouse2,

        /// <summary>
        /// マウス ボタン 3
        /// </summary>
        Mouse3,

        /// <summary>
        /// マウス ボタン 4
        /// </summary>
        Mouse4,
        
        /// <summary>
        /// マウス ボタン 5
        /// </summary>
        Mouse5,

        /// <summary>
        /// マウス ボタン 6
        /// </summary>
        Mouse6,

        /// <summary>
        /// マウス ホイール上
        /// </summary>
        /// <remarks>
        /// マウスホイールを上方向に回転した時に1チック毎に発生します。
        /// </remarks>
        MouseWheeleUp,

        /// <summary>
        /// マウス ホイール上
        /// </summary>
        /// <remarks>
        /// マウスホイールを下方向に回転した時に1チック毎に発生します。
        /// </remarks>
        MouseWheeleDown,
        
        /// <summary>
        /// キーコードの総数
        /// </summary>
        KeyCodeCount,
    }


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

    /*
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
    */

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
