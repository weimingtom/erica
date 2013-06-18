﻿using System;
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
    /// <seealso cref="World"/>
    public class Graphics2D : IDisposable {
        #region Field
        static Graphics2D g2d;
        RenderWindow win;
        World workingScript;
        Node prevHit;
        #endregion

        #region Constructor
        private Graphics2D () {
            this.win = null;
            this.workingScript = null;
            this.prevHit = null;
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

        #region Propety
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
        /// 最大フレームレートの設定
        /// </summary>
        /// <param name="max"></param>
        public void SetFrameRateLimit (int max) {
            win.SetFramerateLimit ((uint)max);
        }


        /// <summary>
        /// シーンの描画
        /// </summary>
        /// <remarks>
        /// 指定のシーンを描画します。
        /// ノードは <see cref="Node.Visible"/> フラグと表示優先度 <see cref="Node.DrawPriority"/> によって制御されます。
        /// </remarks>
        /// <param name="world">シーン</param>
        public void Draw (World world) {
            if (world == null || !win.IsOpen()) {
                return;
            }

            // スクリーンのクリア
            win.Clear (SFML.Graphics.Color.Blue);

            // 全ノードの描画
            var nodes = from node in world.Downwards
                        where node.Visible == true
                        orderby node.DrawPriority descending
                        select node;
            foreach (var node in nodes) {
                foreach (var comp in node.Components) {
                    comp.OnDraw (win);
                }
            }

            // 表示
            win.Display ();
        }

        /// <summary>
        /// イベントを処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="clicked">マウス ボタン イベント引数</param>
        private void MouseButtonPressedHandler (object sender, MouseButtonEventArgs clicked) {
            foreach (var node in workingScript.Downwards.Reverse ()) {
                var x = (float)clicked.X;
                var y = (float)clicked.Y;
                var z = 0f;
                node.LocalTransform.Apply (ref x, ref y, ref z);

                if (node.BoundingBox.Contain (x, y)) {
                    foreach (var comp in node.Components) {
                        switch (clicked.Button) {
                            case SFML.Window.Mouse.Button.Left: comp.OnMouseButtonPressed (MouseButton.Left, (int)x, (int)y); break;
                            case SFML.Window.Mouse.Button.Right: comp.OnMouseButtonPressed (MouseButton.Right, (int)x, (int)y); break;
                            case SFML.Window.Mouse.Button.Middle: comp.OnMouseButtonPressed (MouseButton.Middle, (int)x, (int)y); break;
                            default: break;
                        }
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// マウスボタンの解放を処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="released">マウス ボタン イベント引数</param>
        private void MouseButtonReleasedHandler (object sender, MouseButtonEventArgs released) {
            foreach (var node in workingScript.Downwards.Reverse ()) {
                var x = (float)released.X;
                var y = (float)released.Y;
                var z = 0f;
                node.LocalTransform.Apply (ref x, ref y, ref z);
                if (node.BoundingBox.Contain (x, y)) {
                    foreach (var comp in node.Components) {
                        switch (released.Button) {
                            case SFML.Window.Mouse.Button.Left: comp.OnMouseButtonReleased (MouseButton.Left, (int)x, (int)y); break;
                            case SFML.Window.Mouse.Button.Right: comp.OnMouseButtonReleased (MouseButton.Right, (int)x, (int)y); break;
                            case SFML.Window.Mouse.Button.Middle: comp.OnMouseButtonReleased (MouseButton.Middle, (int)x, (int)y); break;
                            default: break;
                        }
                    }
                    continue;
                }

            }
        }

        /// <summary>
        /// マウス移動を処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="move">マウス移動引数</param>
        private void MouseMovedHandler (object sender, MouseMoveEventArgs move) {
            var hit = (Node)null;

            foreach (var node in workingScript.Downwards.Reverse ()) {
                var x = (float)move.X;
                var y = (float)move.Y;
                var z = 0f;
                node.LocalTransform.Apply (ref x, ref y, ref z);
                if (node.BoundingBox.Contain (x, y)) {
                    hit = node;
                    break;
                }
            }

            if (hit != prevHit) {
                if (prevHit != null) {
                    var x = (float)move.X;
                    var y = (float)move.Y;
                    var z = 0f;
                    prevHit.LocalTransform.Apply (ref x, ref y, ref z);
                    foreach (var comp in prevHit.Components) {
                        comp.OnMouseFocusOut (MouseButton.Left, (int)x, (int)y);
                    }
                }
                if (hit != null) {
                    var x = (float)move.X;
                    var y = (float)move.Y;
                    var z = 0f;
                    hit.LocalTransform.Apply (ref x, ref y, ref z);
                    foreach (var comp in hit.Components) {
                        comp.OnMouseFocusIn (MouseButton.Left, (int)x, (int)y);
                    }
                }
                this.prevHit = hit;
            }

        }

        /// <summary>
        /// 保留中のイベントのディスパッチ
        /// </summary>
        /// <remarks>
        /// デバイスで保留中のイベントを指定のスクリプト <paramref name="script"/> にディスパッチするとともに、
        /// ゲーム内部で発生するイベントをディスパッチします。
        /// <note>
        /// スクリプトが切り替わった時に古い方にマウスのフォーカス アウト イベントが飛ぶ可能性があるが抑制すべき？
        /// 現状では特に対策をしていない。
        /// </note>
        /// </remarks>
        /// <param name="script">スクリプト</param>
        public void Dispatch (World script) {
            if (script == null) {
                throw new ArgumentNullException ("Script is null");
            }

            this.workingScript = script;

            win.MouseButtonPressed += MouseButtonPressedHandler;
            win.MouseButtonReleased += MouseButtonReleasedHandler;
            win.MouseMoved += MouseMovedHandler;

            win.DispatchEvents ();

            win.MouseButtonPressed -= MouseButtonPressedHandler;
            win.MouseButtonReleased -= MouseButtonReleasedHandler;
            win.MouseMoved -= MouseMovedHandler;

            foreach (var node in script.Downwards) {
                foreach (var cmp in node.Components) {
                    cmp.OnDispatch ();
                }
            }

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
                // ないときはlibsの下も探す
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
