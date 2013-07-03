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
using DD.Physics;

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
        HashSet<KeyCode> keyBuffer;
        Vector2 mouse;
        int wheele;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        private Graphics2D () {
            this.win = null;
            this.workingScript = null;
            this.prevHit = null;
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
        /// スクリーン上の1点を指定してノードの検出
        /// </summary>
        /// <remarks>
        /// スクリーン上の1点（典型的にはマウスの位置）を指定して対象ノード <paramref name="node"/> 以下をピックします。
        /// ピック対象になるのは <see cref="CollisionShape"/> を保有するノードだけです。
        /// 結果はピックされたすべてのノードを <see cref="Node.DrawPriority"/> の順番でソートして返します。
        /// 1ノードもヒットしなかった場合カウント0がかえり<c>null</c> を返す事はありません。
        /// </remarks>
        /// <param name="node">ピック対象のノード（これ以下すべて）</param>
        /// <param name="x">ピックするスクリーン上の点のX（ピクセル座標）</param>
        /// <param name="y">ピックするスクリーン上の点のY（ピクセル座標）</param>
        /// <returns></returns>
        public static IEnumerable<Node> Pick (Node node, float x, float y) {
            if (node == null) {
                return new Node[0];
            }
            var point = new Vector2 (x, y);

            return from n in node.Downwards
                   let col = n.GetComponent<CollisionShape> ()
                   where (col != null) && Physics2D.Contain (col, col.Node.GlobalTransform, point)
                   orderby n.DrawPriority
                   select n;
        }

        /// <summary>
        /// シーンの描画
        /// </summary>
        /// <remarks>
        /// 指定のシーンを描画します。
        /// ノードは <see cref="Node.Visibility"/> フラグと表示優先度 <see cref="Node.DrawPriority"/> によって制御されます。
        /// </remarks>
        /// <param name="world">シーン</param>
        public void Draw (World world) {
            if (world == null || !win.IsOpen ()) {
                return;
            }

            // スクリーンのクリア
            win.Clear (SFML.Graphics.Color.Blue);

            // 全ノードの描画
            var nodes = from node in world.Downwards
                        where node.Upwards.Aggregate (true, (x, y) => x & y.Visibility) == true
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

            this.workingScript = null;
        }



        /// <summary>
        /// ウィンドウの作成
        /// </summary>
        /// <remarks>
        /// 指定するウインドウ サイズは描画に使用される内部領域のピクセル数です。
        /// 通常はこれに飾り枠が追加されて（OSに依存）少し大きいウィンドウが作成されます。
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
                // ないときは libs の下も探す
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
            win.Closed += new EventHandler (OnClosedEventHandler);
        }


        /// <summary>
        /// 最大フレームレートの設定
        /// </summary>
        /// <remarks>
        /// フレーム レートを固定します。
        /// </remarks>
        /// <param name="max"></param>
        public void SetFrameRateLimit (int max) {
            win.SetFramerateLimit ((uint)max);
        }


        /// <summary>
        /// ウィンドウ タイトルの変更
        /// </summary>
        /// <param name="title">タイトル</param>
        public void SetWindowTitle (string title) {
            this.win.SetTitle (title);
        }

        /// <summary>
        /// ウィンドウ サイズの変更
        /// </summary>
        /// <param name="width">幅（ピクセル数）</param>
        /// <param name="height">高さ（ピクセル数）</param>
        public void SetWindowSize (int width, int height) {
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
        /// <param name="e">キー イベント引数</param>
        void OnKeyPressedEventHandler (object sender, KeyEventArgs e) {
            this.keyBuffer.Add (e.Code.ToDD ());
        }

        /// <summary>
        /// キーを離した時の処理
        /// </summary>
        /// <param name="sender">ウィンドウ (SFML:RenderWindow)</param>
        /// <param name="e">キー イベント引数</param>
        void OnKeyReleasedEventHandler (object sender, KeyEventArgs e) {
            this.keyBuffer.Remove (e.Code.ToDD ());
        }

        /// <summary>
        /// マウス ボタン イベントを処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="clicked">マウス ボタン イベント引数</param>
        private void OnMouseButtonPressedEventHandler (object sender, MouseButtonEventArgs clicked) {

            this.keyBuffer.Add (clicked.Button.ToDD ());

            var x = clicked.X;
            var y = clicked.Y;

            var picked = Graphics2D.Pick (workingScript, clicked.X, clicked.Y);
            foreach (var node in picked) {
                foreach (var comp in node.Components) {
                    switch (clicked.Button) {
                        case SFML.Window.Mouse.Button.Left: comp.OnMouseButtonPressed (MouseButton.Left, (int)x, (int)y); break;
                        case SFML.Window.Mouse.Button.Right: comp.OnMouseButtonPressed (MouseButton.Right, (int)x, (int)y); break;
                        case SFML.Window.Mouse.Button.Middle: comp.OnMouseButtonPressed (MouseButton.Middle, (int)x, (int)y); break;
                        default: break;
                    }
                }
            }

        }

        /// <summary>
        /// マウスボタンの解放を処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="released">マウス ボタン イベント引数</param>
        private void OnMouseButtonReleasedEventHandler (object sender, MouseButtonEventArgs released) {

            this.keyBuffer.Remove (released.Button.ToDD ());

            var x = released.X;
            var y = released.Y;
            var picked = Graphics2D.Pick (workingScript, released.X, released.Y);
            foreach (var node in picked) {
                foreach (var comp in node.Components) {
                    switch (released.Button) {
                        case SFML.Window.Mouse.Button.Left: comp.OnMouseButtonReleased (MouseButton.Left, (int)x, (int)y); break;
                        case SFML.Window.Mouse.Button.Right: comp.OnMouseButtonReleased (MouseButton.Right, (int)x, (int)y); break;
                        case SFML.Window.Mouse.Button.Middle: comp.OnMouseButtonReleased (MouseButton.Middle, (int)x, (int)y); break;
                        default: break;
                    }
                }
            }


        }

        /// <summary>
        /// マウス移動を処理するハンドラー
        /// </summary>
        /// <param name="sender">ウィンドウ</param>
        /// <param name="move">マウス移動引数</param>
        private void OnMouseMovedEventHandler (object sender, MouseMoveEventArgs move) {

            this.mouse = new Vector2 (move.X, move.Y);

            var hit = Graphics2D.Pick (workingScript, move.X, move.Y).FirstOrDefault();

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
