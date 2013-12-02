using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
    /// <summary>
    /// 汎用ボタン クラス
    /// </summary>
    /// <remarks>
    /// 選択肢のためのクリック可能なボタン クラスです。
    /// </remarks>
    public class MyButton : Component {

        /// <summary>
        /// ボタン スロット
        /// </summary>
        public enum Slot {
            /// <summary>
            /// 標準状態（未選択）
            /// </summary>
            Normal = 0,
            /// <summary>
            /// フォーカス状態（マウス オーバー）
            /// </summary>
            Focused = 1,
            /// <summary>
            /// クリック状態（マウス ダウン）
            /// </summary>
            Clicked = 2
        }

        #region Field
        int width;
        int height;
        #endregion

        #region Event
        /// <summary>
        /// フォーカス状態で呼ばれるイベント
        /// </summary>
        /// <remarks>
        /// マウスがボタン領域に進入すると1回だけ呼ばれます。
        /// </remarks>
        public event EventHandler Focused;

        /// <summary>
        /// クリック状態で呼ばれるイベント
        /// </summary>
        /// <remarks>
        /// マウスがボタンをクリック（押した直後）すると1回だけ呼ばれます。
        /// </remarks>
        public event EventHandler Clicked;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public MyButton () {
            this.width = 0;
            this.height = 0;
        }
        #endregion

        #region Property
        /// <summary>
        /// ボタン幅（ピクセル数）
        /// </summary>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// ボタン高さ（ピクセル数）
        /// </summary>
        public int Height {
            get { return height; }
        }

        /// <summary>
        /// ボタンに表示するテキスト
        /// </summary>
        public string Text {
            get {
                return GetComponent<Label> ().Text;
            }
            set {
                GetComponent<Label> ().Text = value;
            }
        }
        #endregion

        #region Method

        /// <summary>
        /// ボタン ノードの作成
        /// </summary>
        /// <remarks>
        /// デフォルトのボタンノードを作成します。
        /// </remarks>
        /// <param name="pos">位置</param>
        /// <param name="width">ボタンの幅（ピクセル数）</param>
        /// <param name="height">ボタンの高さ（ピクセル数）</param>
        /// <param name="buttonName">ノード名</param>
        /// <returns></returns>
        public static Node Create (Vector3 pos, int width, int height, string buttonName) {
            var cmp = new MyButton ();
            cmp.width = width;
            cmp.height = height;

            var spr = new Sprite (width, height);
            spr.AddTexture (Resource.GetDefaultTexture ());  // normal
            spr.AddTexture (Resource.GetDefaultTexture ());  // focused
            spr.AddTexture (Resource.GetDefaultTexture ());  // clicked

            var col = new CollisionObject ();
            col.Shape = new BoxShape (width / 2, height / 2, 10);
            col.SetOffset (width / 2, height / 2, 10);

            var label = new Label ();

            var node = new Node (buttonName);
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);
            node.Attach (label);

            node.Translation = pos;

            return node;
        }

        /// <summary>
        /// テクスチャーの設定
        /// </summary>
        /// <remarks>
        /// 指定のボタン状態（ボタン スロット）で使用するテクスチャーを設定します。
        /// ボタンスロットにノーマルを指定すると全ての状態のテクスチャーが指定の物に変更されます。
        /// （従って最初に設定すべきです）
        /// </remarks>
        /// <param name="slot">ボタン スロット</param>
        /// <param name="tex">テクスチャー</param>
        /// <returns></returns>
        public Texture AddTexture (Slot slot, Texture tex) {
            var spr = GetComponent<Sprite> ();
            if (slot == Slot.Normal) {
                spr.SetTexture (0, tex);
                spr.SetTexture (1, tex);
                spr.SetTexture (2, tex);
            }
            else {
                spr.SetTexture ((int)slot, tex);
            }

            return tex;
        }

        /// <inhetidoc/>
        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var spr = GetComponent<Sprite> ();
            spr.ActiveTexture = (int)Slot.Clicked;

            if (Clicked != null) {
                Clicked.Invoke (this, null);
            }
        }

        /// <inhetidoc/>
        public override void OnMouseButtonReleased (MouseButton button, float x, float y) {
            var spr = GetComponent<Sprite> ();
            spr.ActiveTexture = (int)Slot.Normal;
        }

        /// <inhetidoc/>
        public override void OnMouseFocusIn (float x, float y) {
            var spr = GetComponent<Sprite> ();
            spr.ActiveTexture = (int)Slot.Focused;
            
            if (Focused != null) {
                Focused.Invoke (this, null);
            }
        }

        /// <inhetidoc/>
        public override void OnMouseFocusOut (float x, float y) {
            var spr = GetComponent<Sprite> ();
            spr.ActiveTexture = (int)Slot.Normal;
        }
        #endregion

    }
}
