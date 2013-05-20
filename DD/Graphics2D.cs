using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    /// 2Dグラフィックスを描画するためのウィンドウ マネージャーです。
    /// スクリプトを描画するとともに、キーボード・マウスなどのIOイベントを処理します。
    /// このクラスはシングルトン化されています。
    /// </remarks>
    /// <seealso cref="Script"/>
    public class Graphics2D : IDisposable {
        #region Field
        static Graphics2D g2d;
        RenderWindow win;
        Script workingScript;
        #endregion

        #region Constructor
        private Graphics2D () {
            this.win = null;
            this.workingScript = null;
        }
        #endregion

        #region Event
        /// <summary>
        /// ウィンドウクローズ イベント
        /// </summary>
        /// <remarks>
        /// ウィンドウが閉じられる時に呼び出されるイベントです。
        /// </remarks>
        public event EventHandler OnClosed;
        #endregion

        #region Method

        /// <summary>
        /// ウィンドウ クローズ処理
        /// </summary>
        /// <param name="sender">ウィンドウ(SFML:RenderWindow)</param>
        /// <param name="args">現在では null</param>
        static void OnClosedHandler (object sender, EventArgs args) {
            var win = sender as RenderWindow;
            win.Close ();
            if (g2d.OnClosed != null) {
                g2d.OnClosed.Invoke (g2d, null);
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
        /// スクリプトの描画
        /// </summary>
        /// <remarks>
        /// 指定のスクリプトを描画します。
        /// </remarks>
        /// <param name="script">スクリプト</param>
        public void Draw (Script script) {
            if (script == null) {
                return;
            }
            if (!win.IsOpen ()) {
                return;
            }

            // Clear screen
            win.Clear (Color.Blue);

            // 全ノードの描画
            foreach (var node in script.Downwards) {
                if (node.Visible) {
                    foreach (var comp in node.Components) {
                        comp.OnDraw (win);
                    }
                }
            }

            // Update the window
            win.Display ();
        }

        /// <summary>
        /// イベントを処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="clicked">マウス ボタン イベント引数</param>
        private void MouseButtonPressedHandler (object sender, MouseButtonEventArgs clicked) {
            foreach (var node in workingScript.Downwards.Reverse ()) {
                var x = (int)clicked.X;
                var y = (int)clicked.Y;
                node.TransformToLocal (ref x, ref y);
                if (node.BoundingBox.Contain (x, y)) {
                    foreach (var comp in node.Components) {
                        switch (clicked.Button) {
                            case SFML.Window.Mouse.Button.Left: comp.OnMouseButtonPressed (MouseButton.Left, x, y); break;
                            case SFML.Window.Mouse.Button.Right: comp.OnMouseButtonPressed (MouseButton.Right, x, y); break;
                            case SFML.Window.Mouse.Button.Middle: comp.OnMouseButtonPressed (MouseButton.Middle, x, y); break;
                            default: break;
                        }
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// イベントを処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="released">マウス ボタン イベント引数</param>
        private void MouseButtonReleasedHandler (object sender, MouseButtonEventArgs released) {
            foreach (var node in workingScript.Downwards.Reverse ()) {
                var x = released.X;
                var y = released.Y;
                node.TransformToLocal (ref x, ref y);
                if (node.BoundingBox.Contain (x, y)) {
                    foreach (var comp in node.Components) {
                        switch (released.Button) {
                            case SFML.Window.Mouse.Button.Left: comp.OnMouseButtonReleased (MouseButton.Left, x, y); break;
                            case SFML.Window.Mouse.Button.Right: comp.OnMouseButtonReleased (MouseButton.Right, x, y); break;
                            case SFML.Window.Mouse.Button.Middle: comp.OnMouseButtonReleased (MouseButton.Middle, x, y); break;
                            default: break;
                        }
                    }
                    continue;
                }

            }
        }

        /// <summary>
        /// 保留中のイベントのディスパッチ
        /// </summary>
        /// <remarks>
        /// デバイスで保留中のイベントを指定のスクリプト <paramref name="script"/> にディスパッチします。
        /// </remarks>
        /// <param name="script">スクリプト</param>
        public void Dispatch (Script script) {
            if (script == null) {
                throw new ArgumentNullException ("Script is null");
            }

            this.workingScript = script;

            win.MouseButtonPressed += MouseButtonPressedHandler;
            win.MouseButtonReleased += MouseButtonReleasedHandler;

            win.DispatchEvents ();

            win.MouseButtonPressed -= MouseButtonPressedHandler;
            win.MouseButtonReleased -= MouseButtonReleasedHandler;
            
            this.workingScript = null;
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
            string dir = System.IO.Directory.GetCurrentDirectory ();

            try {
                // DLLはexeと同じディレクトリに存在
                this.win = new RenderWindow (new VideoMode ((uint)width, (uint)height), title);
            }
            catch (DllNotFoundException e) {
                // libsがあればそちらも探す
                if (System.IO.Directory.Exists ("libs")) {
                    System.IO.Directory.SetCurrentDirectory (dir + "/libs");
                    this.win = new RenderWindow (new VideoMode ((uint)width, (uint)height), title);
                    System.IO.Directory.SetCurrentDirectory (dir);
                }
                else {
                    // Give up
                    throw e;
                }
            }

            win.SetVisible (true);
            win.Closed += new EventHandler (OnClosedHandler);
        }


        /// <summary>
        /// ウィンドウ タイトルの変更
        /// </summary>
        /// <param name="title">タイトル</param>
        public void SetWindowTitle (string title) {
            win.SetTitle (title);
        }

        /// <summary>
        /// ウィンドウ サイズの変更
        /// </summary>
        /// <param name="width">幅（ピクセル数）</param>
        /// <param name="height">高さ（ピクセル数）</param>
        public void SetWindowSize (int width, int height) {
            win.Size = new Vector2u ((uint)width, (uint)height);
        }

        /// <summary>
        /// ウィンドウの可視性の変更
        /// </summary>
        /// <remarks>
        /// ウィンドウはこのメソッドで可視にするまで表示されません。
        /// </remarks>
        /// <param name="visibility">可視性</param>
        public void SetWindowVisible (bool visibility) {
            win.SetVisible (visibility);
        }
        #endregion


        /// <inheritdoc/>
        public void Dispose () {
            if (win != null) {
                win.Dispose ();
            }
        }
    }
}
