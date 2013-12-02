using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
    /// <summary>
    /// 複数の選択肢を提示するパネル クラス
    /// </summary>
    /// <remarks>
    /// 複数の選択肢（<see cref="MyButton"/>）を表示し、ユーザーがどれか1つを選択します。
    /// パネルは複数のパネルを連続して表示可能です。
    /// 基本的にこのクラスはユーザーが直接使用することを想定していません。
    /// </remarks>
    public class MyPanel : Component {

        #region Field
        List<MyButton> btns;
        string selected;
        MyPanel next;
        MyPanel prev;
        #endregion

        #region Event
        /// <summary>
        /// 選択完了イベント
        /// </summary>
        /// <remarks>
        /// 選択が完了すると呼び出されます。
        /// パネルが連続している場合は最後のパネルのみ呼び出されます。
        /// </remarks>
        public event EventHandler Completed;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public MyPanel () {
            this.btns = new List<MyButton> ();
            this.selected = null;
            this.next = null;
            this.prev = null;
        }
        #endregion


        #region Property
        /// <summary>
        /// 選択された選択肢（文字列）
        /// </summary>
        /// <remarks>
        /// ユーザーが選択した選択肢を表す文字列です。
        /// </remarks>
        public string Selected {
            get { return selected; }
            set { this.selected = value; }
        }

        /// <summary>
        /// ボタン数
        /// </summary>
        /// <remarks>
        /// ボタン数（選択肢の数）です。
        /// </remarks>
        public int ButtonCount {
            get { return btns.Count (); }
        }

        /// <summary>
        /// 全てのボタンを列挙する列挙子
        /// </summary>
        public IEnumerable<MyButton> Buttons {
            get { return btns; }
        }
        #endregion

        #region Method
        /// <summary>
        /// 次のパネルに進める
        /// </summary>
        /// <remarks>
        /// 現在表示中のパネルを非表示にして、（あるなら）次のパネルを表示します。
        /// 次のパネルがない場合は <see cref="Compoleted"/> イベントを呼び出し終了します。
        /// </remarks>
        public void Next () {
            Node.Visible = false;
            Node.Collidable = false;
            if (next == null) {
                if (Completed != null) {
                    Completed.Invoke (this, null);
                }
            }
            else {
                next.Node.Visible = true;
                next.Node.Collidable = true;
            }
        }

        /// <summary>
        /// 前のパネルに戻る
        /// </summary>
        /// <remarks>
        /// 現在表示中のパネルを非表示にして、（あるなら）前のパネルを表示します。
        /// 前のパネルがない場合は特に何もしません（対応するイベントはありません）。
        /// </remarks>
        public void Prev() {
            Node.Visible = false;
            Node.Collidable = false;
            if (prev == null) {
                // do nothing
            }
            else {
                prev.Node.Visible = true;
                prev.Node.Collidable = true;
            }
        }

        /// <summary>
        /// 次のパネルの設定
        /// </summary>
        /// <remarks>
        /// パネルの次のパネルを設定します。
        /// </remarks>
        /// <param name="panel">次のパネル</param>
        public void SetNext (MyPanel panel) {
            this.next = panel;
            if (panel != null) {
                panel.prev = this;
            }
        }

        /// <summary>
        /// パネル ノードの作成
        /// </summary>
        /// <param name="pos">位置</param>
        /// <returns></returns>
        public static Node Create (Vector3 pos) {
            var cmp = new MyPanel ();

            var node = new Node ("");
            node.Attach (cmp);

            node.Translation = pos;

            return node;
        }

        /// <summary>
        /// ボタンの追加
        /// </summary>
        /// <remarks>
        /// ボタンノードとは <see cref="MyButton"/> コンポーネントを持つノードです。
        /// ボタン削除メソッドはありません。
        /// </remarks>
        /// <param name="node">ボタン ノード</param>
        /// <returns></returns>
        public MyButton AddButton (Node node) {
            if (node == null) {
                throw new ArgumentNullException ("Node is null");
            }
            if (!node.Has<MyButton>()) {
                throw new ArgumentException ("Node does not have MyButton component");
            }

            Node.AddChild (node);
            node.Translation = new Vector3 (0, btns.Sum (x => x.Height), 0);

            var btn = node.GetComponent<MyButton> ();
            btns.Add (btn);

            return btn;
        }
        #endregion


    }
}
