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
        World wld;
        HashSet<KeyCode> keyBuffer;
        List<Node> prevHits;
        Vector2 mouse;
        int wheele;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        private Graphics2D () {
            this.win = null;
            this.wld = null;
            this.prevHits = new List<Node> ();
            this.wheele = 0;
            this.keyBuffer = new HashSet<KeyCode> ();
            this.mouse = new Vector2 ();
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
        /// ウィンドウの取得
        /// </summary>
        /// <remarks>
        /// デバイス固有のウィンドウ オブジェクトを取得します。
        /// SFMLの場合は RenderWindow にキャストしてください。
        /// 通常ユーザーがこのメソッドを使用する事はありません。
        /// </remarks>
        /// <returns>ウィンドウ</returns>
        public object GetWindow () {
            return win;
        }

        /// <summary>
        /// キーバッファーに保存されているキーをすべて取得
        /// </summary>
        /// <remarks>
        /// キーばーっファーに保存されている（現在押されている）キーをすべて取得します。
        /// キーにはマウスボタンも含まれます。
        /// <seealso cref="KeyCode"/>
        /// </remarks>
        /// <returns>キーコード</returns>
        public IEnumerable<KeyCode> GetKeys () {
            return keyBuffer;
        }

        /// <summary>
        /// マウス位置（ピクセル座標）の取得
        /// </summary>
        /// <remarks>
        /// マウスの位置をピクセル座標（左上が(0,0)）で取得します。
        /// </remarks>
        /// <returns></returns>
        public Vector2 GetMousePosition () {
            return mouse;
        }

        /// <summary>
        /// マウス ホイールの回転位置の取得
        /// </summary>
        /// <remarks>
        /// マウス ホイールの回転位置を取得します。
        /// デバイス作成時の回転位置を0として、そこから回転に応じてプラス マイナスされます。
        /// 回転の検出はそれほど精度が良くありません（ハードウェア的にそういう仕様）。
        /// </remarks>
        /// <returns></returns>
        public float GetMouseWheele () {
            return wheele;
        }

        /// <summary>
        /// シーンの描画
        /// </summary>
        /// <remarks>
        /// 指定のシーン <paramref name="world"/> を描画します。
        /// 描画されるノードは <see cref="Node.Visible"/> フラグと表示優先度 <see cref="Node.DrawPriority"/> によって制御されます。
        /// </remarks>
        /// <param name="world">シーン</param>
        /// <param name="finish">描画の終了</param>
        public void Draw (World world, bool finish = true) {
            if (world == null || !win.IsOpen ()) {
                return;
            }

            if (world.ActiveCamera == null) {
                win.Clear (SFML.Graphics.Color.Blue);
            }
            else {
                var cam = world.ActiveCamera.GetComponent<Camera> ();
                if (cam.Type != ProjectionType.Screen) {
                    throw new InvalidOperationException ("Camera type is invalid for 2D, type=" + cam.Type);
                }
                cam.SetupView (win);
                cam.SetupClear (win);
            }

            // 全ノードの描画プレ処理
            var preDraws = from node in world.Downwards
                           orderby node.DrawPriority descending
                           select node;
            foreach (var node in preDraws) {
                foreach (var comp in node.Components) {
                    comp.OnPreDraw (win);
                }
            }

            // 全ノードの描画
            var nodes = from node in world.Downwards
                        where node.Upwards.Aggregate (true, (x, y) => x & y.Visible) == true
                        orderby node.DrawPriority descending
                        select node;
            foreach (var node in nodes) {
                foreach (var comp in node.Components) {
                    comp.OnDraw (win);
                }
            }

            // 表示
            if (finish) {
                win.Display ();
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

            this.wld = script;

            // ホイールはリリースされない仕様なので
            // 自力で開放する
            this.keyBuffer.Remove (KeyCode.MouseWheeleDown);
            this.keyBuffer.Remove (KeyCode.MouseWheeleUp);

            win.MouseButtonPressed += OnMouseButtonPressedEventHandler;
            win.MouseButtonReleased += OnMouseButtonReleasedEventHandler;
            win.MouseMoved += OnMouseMovedEventHandler;
            win.MouseWheelMoved += OnMouseWheelMovedEventHandler;
            win.KeyPressed += OnKeyPressedEventHandler;
            win.KeyReleased += OnKeyReleasedEventHandler;

            win.DispatchEvents ();

            win.MouseButtonPressed -= OnMouseButtonPressedEventHandler;
            win.MouseButtonReleased -= OnMouseButtonReleasedEventHandler;
            win.MouseMoved -= OnMouseMovedEventHandler;
            win.MouseWheelMoved -= OnMouseWheelMovedEventHandler;
            win.KeyPressed -= OnKeyPressedEventHandler;
            win.KeyReleased -= OnKeyReleasedEventHandler;

            foreach (var node in script.Downwards) {
                foreach (var cmp in node.Components) {
                    cmp.OnDispatch ();
                }
            }

            this.wld = null;
        }



        /// <summary>
        /// ウィンドウの作成
        /// </summary>
        /// <remarks>
        /// 新規にウィンドウを作成します。メニューもステータスバーも持たないゲームウィンドウのみの場合はこちらを使用して下さい。
        /// 引数で指定するウインドウ サイズは描画領域のピクセル数です。
        /// 通常はこれにOSに依存する飾り枠が追加されて少し大きいウィンドウが作成されます。
        /// </remarks>
        /// <param name="width">ウィンドウ幅（ピクセル数）</param>
        /// <param name="height">ウィンドウ高さ（ピクセル数）</param>
        /// <param name="title">ウィンドウ タイトル名</param>
        public void CreateWindow (int width, int height, string title) {
            if (width <= 0 || height <= 0) {
                throw new ArgumentException ("Window size is invalid");
            }

            var dir = System.IO.Directory.GetCurrentDirectory ();

            try {
                // DLLはexeと同じディレクトリに存在
                this.win = new RenderWindow (new VideoMode ((uint)width, (uint)height), title ?? "");
            }
            catch (DllNotFoundException e) {
                // ないときは libs の下も探す
                if (System.IO.Directory.Exists ("libs")) {
                    System.IO.Directory.SetCurrentDirectory (dir + "/libs");
                    this.win = new RenderWindow (new VideoMode ((uint)width, (uint)height), title ?? "");
                    System.IO.Directory.SetCurrentDirectory (dir);
                }
                else {
                    // Give up
                    throw e;
                }
            }

            win.SetVisible (true);
            win.Closed += new EventHandler (OnClosedEventHandler);
        }

        /// <summary>
        /// 指定のウィンドウハンドラー利用してウィンドウを作成
        /// </summary>
        /// <remarks>
        /// Windowsフォームのハンドラーを利用してウィンドウを作成します。
        /// フォームアプリの1コントロールに対してゲームを描画する場合はこちらを使用して下さい。
        /// サイズは親のコントールに依存します。
        /// </remarks>
        /// <param name="handler">Windowsフォームのウィンドウハンドラー</param>
        public void CreateWindow (IntPtr handler) {
            var dir = System.IO.Directory.GetCurrentDirectory ();

            try {
                // DLLはexeと同じディレクトリに存在
                this.win = new RenderWindow (handler);
            }
            catch (DllNotFoundException e) {
                // ないときは libs の下も探す
                if (System.IO.Directory.Exists ("libs")) {
                    System.IO.Directory.SetCurrentDirectory (dir + "/libs");
                    this.win = new RenderWindow (handler);
                    System.IO.Directory.SetCurrentDirectory (dir);
                }
                else {
                    // Give up
                    throw e;
                }
            }

            win.SetVisible (true);
            win.Closed += new EventHandler (OnClosedEventHandler);
        }


        /// <summary>
        /// 最大フレーム レートの設定
        /// </summary>
        /// <remarks>
        /// フレーム レートを固定します。
        /// </remarks>
        /// <param name="max">フレーム レート</param>
        public void SetFrameRateLimit (int max) {
            win.SetFramerateLimit ((uint)max);
        }


        /// <summary>
        /// ウィンドウ タイトルの変更
        /// </summary>
        /// <param name="title">タイトル</param>
        public void SetWindowTitle (string title) {
            this.win.SetTitle (title ?? "");
        }

        /// <summary>
        /// ウィンドウ サイズの変更
        /// </summary>
        /// <param name="width">幅（ピクセル数）</param>
        /// <param name="height">高さ（ピクセル数）</param>
        public void SetWindowSize (int width, int height) {
            if (width <= 0 || height <= 0) {
                throw new ArgumentException ("Window size is invalid");
            }
            this.win.Size = new Vector2u ((uint)width, (uint)height);
        }

        /// <summary>
        /// ウィンドウの可視性の変更
        /// </summary>
        /// <remarks>
        /// ウィンドウはこのメソッドで可視にするまで表示されません。
        /// </remarks>
        /// <param name="visibility">可視性</param>
        public void SetWindowVisible (bool visibility) {
            this.win.SetVisible (visibility);
        }

        /// <summary>
        /// ウィンドウ クローズ処理
        /// </summary>
        /// <param name="sender">ウィンドウ (SFML:RenderWindow)</param>
        /// <param name="args">現在では null</param>
        static void OnClosedEventHandler (object sender, EventArgs args) {
            var win = sender as RenderWindow;
            win.Close ();
            if (g2d.OnClosed != null) {
                g2d.OnClosed.Invoke (g2d, null);
            }
        }

        /// <summary>
        /// キーを押した時の処理
        /// </summary>
        /// <param name="sender">ウィンドウ (SFML:RenderWindow)</param>
        /// <param name="ev">キー イベント引数</param>
        void OnKeyPressedEventHandler (object sender, KeyEventArgs ev) {

            var key = ev.Code.ToDD ();

            this.keyBuffer.Add (key);

            foreach (var node in wld.Downwards) {
                foreach (var comp in node.Components) {
                    comp.OnKeyPressed (key);
                }
            }
        }

        /// <summary>
        /// キーを離した時の処理
        /// </summary>
        /// <param name="sender">ウィンドウ (SFML:RenderWindow)</param>
        /// <param name="ev">キー イベント引数</param>
        void OnKeyReleasedEventHandler (object sender, KeyEventArgs ev) {
            var key = ev.Code.ToDD ();

            this.keyBuffer.Remove (key);

            foreach (var node in wld.Downwards) {
                foreach (var comp in node.Components) {
                    comp.OnKeyReleased (key);
                }
            }
        }

        /// <summary>
        /// マウス ボタン イベントを処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="clicked">マウス ボタン イベント引数</param>
        private void OnMouseButtonPressedEventHandler (object sender, MouseButtonEventArgs clicked) {

            var btn = clicked.Button.ToDD ();
            var key = clicked.Button.ToDD_KeyCode ();
            var pos = win.MapPixelToCoords (new Vector2i (clicked.X, clicked.Y)).ToDD ();

            this.keyBuffer.Add (key);
            this.mouse = pos;

            var start = new Vector3 (pos.X, pos.Y, 1000);
            var end = new Vector3 (pos.X, pos.Y, -1000);

            var nodes = wld.RayCast (start, end).Select (x => x.Node);
            foreach (var node in nodes) {
                foreach (var comp in node.Components) {
                    comp.OnMouseButtonPressed (btn, (int)pos.X, (int)pos.Y);
                }
            }
        }

        /// <summary>
        /// マウスボタンの解放を処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="released">マウス ボタン イベント引数</param>
        private void OnMouseButtonReleasedEventHandler (object sender, MouseButtonEventArgs released) {

            var btn = released.Button.ToDD ();
            var key = released.Button.ToDD_KeyCode ();
            var pos = win.MapPixelToCoords (new Vector2i (released.X, released.Y)).ToDD ();

            this.keyBuffer.Remove (key);
            this.mouse = pos;

            var start = new Vector3 (pos.X, pos.Y, 1000);
            var end = new Vector3 (pos.X, pos.Y, -1000);

            var nodes = wld.RayCast (start, end).Select (x => x.Node);
            foreach (var node in nodes) {
                foreach (var comp in node.Components) {
                    comp.OnMouseButtonReleased (btn, (int)pos.X, (int)pos.Y);
                }
            }
        }

        /// <summary>
        /// マウス移動を処理するハンドラー
        /// </summary>
        /// <remarks>
        /// 一番手前のノードのみ処理される。
        /// </remarks>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="moved">マウス移動引数</param>
        private void OnMouseMovedEventHandler (object sender, MouseMoveEventArgs moved) {

            var pos = win.MapPixelToCoords (new Vector2i (moved.X, moved.Y)).ToDD ();
            this.mouse = pos;

            var start = new Vector3 (pos, 1000);
            var end = new Vector3 (pos, -1000);
            var hits = wld.RayCast (start, end).Select (x => x.Node);

            var focusin = hits.Except (prevHits);
            var focusout = prevHits.Except (hits);

            foreach (var node in focusin) {
                foreach (var comp in node.Components) {
                    comp.OnMouseFocusIn ((int)pos.X, (int)pos.Y);
                }
            }
            foreach (var node in focusout) {
                foreach (var comp in node.Components) {
                    comp.OnMouseFocusOut ((int)pos.X, (int)pos.Y);
                }
            }

            this.prevHits = hits.ToList ();
        }

        /// <summary>
        /// マウス ホイールの回転を処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="e">マウス ホイール イベント</param>
        void OnMouseWheelMovedEventHandler (object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0) {
                this.keyBuffer.Add (KeyCode.MouseWheeleUp);
            }
            if (e.Delta < 0) {
                this.keyBuffer.Add (KeyCode.MouseWheeleDown);
            }
            this.wheele += e.Delta;
        }

        /// <inheritdoc/>
        public void Dispose () {
            if (win != null) {
                win.Dispose ();
            }
        }

        #endregion


    }
}
