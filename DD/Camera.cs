using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;
using SFML.Window;
using SFML.Graphics;


namespace DD {
   
    /// <summary>
    /// カメラ コンポーネント
    /// </summary>
    /// <remarks>
    /// カメラはシーンを描画します。
    /// シーン中には複数のカメラをセット可能で、その中から1つを選んで描画します。
    /// カメラは現在2Dの描画するための「スクリーン投影」方式のみ実装されています。
    /// 3Dで使用される「透視投影」と「平行投影」は未実装です。
    /// </remarks>
    public class Camera : Component {
        #region Field
        ProjectionType type;
        bool clear;
        Color clearColor;
        Rectangle screen;
        Rectangle viewport;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Camera () {
            this.type = ProjectionType.Undefined;
            this.viewport = new Rectangle (0, 0, 1, 1);
            this.screen = new Rectangle (0, 0, 0, 0);
            this.clear = true;
            this.clearColor = Color.Blue;
        }
        #endregion

        #region Property
        /// <summary>
        /// 投影タイプ
        /// </summary>
        /// <remarks>
        /// 現在2D描画用の「スクリーン投影」のみ実装されています。
        /// </remarks>
        public ProjectionType Type {
            get {return type;}
        }

        /// <summary>
        /// 画面クリアの有効無効フラグ
        /// </summary>
        /// <remarks>
        /// trueの時画面を <see cref="ClearColor"/> で消去してから描画を始めます。
        /// </remarks>
        public bool ClearEnabled {
            get { return clear; }
            set { this.clear = value; }
        }

        /// <summary>
        /// 画面のクリア色
        /// </summary>
        /// <remarks>
        /// <see cref="ClearEnabled"/> フラグが <c>true</c> の時に、描画前にこの色で塗りつぶされます。
        /// </remarks>
        public Color ClearColor {
            get { return clearColor; }
            set { this.clearColor = value; }
        }

        /// <summary>
        /// ビューポートの設定
        /// </summary>
        /// <remarks>
        /// ウィンドウの描画領域（ビューポート）をあらわすプロパティ。
        /// ビューポートはウィンドウサイズで正規化され [0,1] の範囲で指定します。
        /// </remarks>
        public Rectangle Viewport {
            get { return viewport; }
            set { this.viewport = value; }
        }

        /// <summary>
        /// 描画スクリーン領域（ローカル座標）
        /// </summary>
        /// <remarks>
        /// このカメラの仮想描画領域（ローカル座標）を設定します。
        /// カメラがセットされたノードのワールド座標を中心にこの描画領域が描画され、
        /// ウィンドウ（のビューポートに）一致するように引き延ばされて表示されます。
        /// </remarks>
        public Rectangle Screen {
            get { return screen; }
            set { SetScreen(value.X, value.Y, value.Width, value.Height); }
        }

        #endregion

        #region Method
        /// <summary>
        /// カメラを透視投影に変更
        /// </summary>
        /// <remarks>
        /// 未実装です。
        /// </remarks>
        /// <param name="fovy"></param>
        /// <param name="aspectRatio"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        public void SetPerspective (float fovy, float aspectRatio, float near, float far) {
            throw new NotImplementedException ("Sorry");
        }

        /// <summary>
        /// カメラの平行投影に変更
        /// </summary>
        /// <remarks>
        /// 未実装です。
        /// </remarks>
        /// <param name="fovy"></param>
        /// <param name="aspectRatio"></param>
        /// <param name="near"></param>
        /// <param name="far"></param>
        public void SetParallel (float fovy, float aspectRatio, float near, float far) {
            throw new NotImplementedException ("Sorry");
        }

        /// <summary>
        /// カメラの2Dスクリーン投影に変更
        /// </summary>
        /// <remarks>
        /// カメラの投影方式を2D用のスクリーン投影に変更します。
        /// ワールド座標の(x,y)-(x+<paramref name="width"/>,y+<paramref name="height"/>)が描画され、
        /// ウィンドウ（のビューポート）に一致するように伸張されて表示されます。
        /// (x,y)はこのカメラ ノードのグローバル座標位置です。
        /// <note>
        /// カメラ ノードのスケール係数によって描画領域（スクリーン）の大きさは変更されません（平行移動と回転は有効）。
        /// 現実世界と同様に大きいカメラで撮ろうが小さいカメラで撮ろうとレンズが同じなら同じ写真になります。
        /// </note>
        /// </remarks>
        /// <param name="x">スクリーンのオフセットX（ローカル座標）</param>
        /// <param name="y">スクリーンのオフセットY（ローカル座標）</param>
        /// <param name="width">スクリーン幅</param>
        /// <param name="height">スクリーン高さ</param>
        public void SetScreen (float x, float y, float width, float height) {
            if (width < 0 || height < 0) {
                throw new ArgumentException ("Width or Height is inalid");
            }

            this.type = ProjectionType.Screen;
            this.screen = new Rectangle (x, y, width, height);
        }

        /// <summary>
        /// ビューポートの設定
        /// </summary>
        /// <remarks>
        /// ビューポートはウィンドウの相対値 [0,1] で指定します。
        /// ウィンドウをはみ出す領域は指定できません。
        /// </remarks>
        /// <param name="x">ビューポートのX [0,1]</param>
        /// <param name="y">ビューポートのY [0,1]</param>
        /// <param name="width">ビューポートの幅 [0,1]</param>
        /// <param name="height">ビューポートの高さ [0,1]</param>
        public void SetViewport (float x, float y, float width, float height) {
            if (x < 0 || x > 1 || y < 0 || y > 1 || width < 0 || width > 1 || height < 0 || height > 1) {
                throw new ArgumentException ("Viewport is inalid");
            }
            if (x + width > 1 || y + height > 1) {
                throw new ArgumentException ("Viewport is inalid");
            }

            this.viewport = new Rectangle (x, y, width, height);
        }

        internal void SetupClear (object window) {
            var win = window as RenderWindow;
            
            if (clear) {
                win.Clear (clearColor.ToSFML ());
            }
        }

        /// <summary>
        /// カメラのセットアップ
        /// </summary>
        /// <remarks>
        /// カメラだけは一番はじめに処理しないといけないので OnDraw() には書けない。
        /// </remarks>
        /// <param name="window">ウィンドウ</param>
        internal void SetupView (object window) {
            if (!IsAttached) {
                throw new InvalidOperationException ("Camera must be attached to node");
            }
            var win = window as RenderWindow;
            
            Vector3 T;
            Quaternion R;
            Vector3 S;
            Node.GlobalTransform.Decompress(out T, out R, out S);

            // クォータニオンの性質上(0,0,1)軸まわりの回転か、
            // (0,0,-1)軸まわりの回転のどちらかが返ってくる（両者は等価）。
            // SFMLは(0,0,1)軸まわりの回転角で指定するので(0,0,-1)軸の時は反転が必要。
            var angle = R.Angle;
            if (R.Axis.Z < 0) {
                angle *= -1;
            }

            // スクリーン＆ビューポートの設定
            var view = win.GetView();
            var x = T.X + screen.X;
            var y = T.Y + screen.Y;
            var width = screen.Width;
            var height = screen.Height;
            view.Reset (new FloatRect(x, y, width, height));
            view.Viewport = new FloatRect(viewport.X, viewport.Y, viewport.Width, viewport.Height);
            view.Rotation = angle;
            //view.Zoom (T.Z);

            win.SetView(view);
        }

        #endregion

    }
}
