using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using SFML.Audio;
using SFML.Window;
using SFML.Graphics;

namespace DD {
    /// <summary>
    /// 2Dグラフィックデバイス クラス
    /// </summary>
    /// <remarks>
    /// 2Dグラフィックスを描画するためのウィンドウマネージャー他の処理です。
    /// ディレクターを描画する事ができます。
    /// </remarks>
    /// <seealso cref="Director"/>
    public class Graphics2D {
        #region Field
        static Graphics2D g2d;
        RenderWindow window;
        #endregion

        #region Constructor
        private Graphics2D () {
            this.window = null;
        }
        #endregion

        #region Event
        /// <summary>
        /// ウィンドウクローズ イベント
        /// </summary>
        /// <remarks>
        /// ウィンドウが閉じられる時に呼び出されるイベントです。
        /// </remarks>
        public event EventHandler OnClose;
        #endregion

        #region Method

        /// <summary>
        /// ウィンドウ クローズ処理
        /// </summary>
        /// <param name="sender">ウィンドウ(SFML:RenderWindow)</param>
        /// <param name="e">現在では null</param>
        static void OnClosed (object sender, EventArgs e) {
            var window = sender as RenderWindow;
            window.Close ();
            if (g2d.OnClose != null){
                g2d.OnClose.Invoke(g2d, null);
            }
        }

        /// <summary>
        /// <see cref="Graphics2D"/> クラスのインスタンスの取得
        /// </summary>
        /// <returns></returns>
        public static Graphics2D GetInstance () {
            if (g2d == null) {
                g2d = new Graphics2D ();
            }

            return g2d;
        }

        /// <summary>
        /// ディレクターの描画
        /// </summary>
        /// <remarks>
        /// ディレクターを描画します。
        /// </remarks>
        /// <param name="director">ディレクター</param>
        public void Draw (Script script) {
            if (script == null) {
                return;
            }
            if (!window.IsOpen ()) {
                return;
            }

            // Process events
            window.DispatchEvents ();

            // Clear screen
            window.Clear (Color.Blue);

            // 全ノードの描画
            foreach (var node in script.Downwards) {
                if (node.Visible) {
                    foreach (var comp in node.Components) {
                        comp.OnDraw (window);
                    }
                }
            }

            // Update the window
            window.Display ();
        }

        /// <summary>
        /// ウィンドウの作成
        /// </summary>
        /// <remarks>
        /// 指定するウインドウ サイズは描画に使用される内部領域のピクセル数です。
        /// 通常はこれに飾り枠（OSに依存）がついて少し大きいウィンドウが作成されます。
        /// </remarks>
        /// <param name="width">ウィンドウ幅（ピクセル数）</param>
        /// <param name="height">ウィンドウ高さ（ピクセル数）</param>
        /// <param name="title">ウィンドウ タイトル名</param>
        public void CreateWindow (int width, int height, string title) {
            
            // DLLはlibsの下にあるので・・・
            string dir = System.IO.Directory.GetCurrentDirectory ();
            System.IO.Directory.SetCurrentDirectory (dir + "/libs");
            
            this.window = new RenderWindow (new VideoMode ((uint)width, (uint)height), title);

            System.IO.Directory.SetCurrentDirectory (dir);

            window.SetVisible (true);
            window.Closed += new EventHandler (OnClosed);
        }


        /// <summary>
        /// ウィンドウ タイトルの変更
        /// </summary>
        /// <param name="title">タイトル</param>
        public void SetWindowTitle (string title) {
            window.SetTitle (title);
        }

        /// <summary>
        /// ウィンドウ サイズの変更
        /// </summary>
        /// <param name="width">幅（ピクセル数）</param>
        /// <param name="height">高さ（ピクセル数）</param>
        public void SetWindowSize (int width, int height) {
            window.Size = new Vector2u ((uint)width, (uint)height);
        }

        /// <summary>
        /// ウィンドウの可視性の変更
        /// </summary>
        /// <remarks>
        /// ウィンドウはこのメソッドで可視にするまで表示されません。
        /// </remarks>
        /// <param name="visibility">可視性</param>
        public void SetWindowVisible (bool visibility) {
                window.SetVisible (visibility);
        }
        #endregion

    }
}
