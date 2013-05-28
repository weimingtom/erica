using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// コンポーネント クラス
    /// </summary>
    /// <remarks>
    /// コンポーネントはノードにアタッチされて、ノードに様々なの機能を追加します。
    /// </remarks>
    public class Component {
        #region Field
        Node node;
        #endregion


        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Component () {
            this.node = null;
        }
        #endregion

        #region Property
        /// <summary>
        /// 所属ノード
        /// </summary>
        public Node Node {
            get { return node; }
        }


        #endregion

        #region Event
        #endregion


        #region Method
        /// <summary>
        /// 親ノードの変更
        /// </summary>
        /// <param name="node">ノード</param>
        internal void SetNode (Node node) {
            this.node = node;
        }

        /// <summary>
        /// アタッチ処理後のエントリーポイント
        /// </summary>
        /// <remarks>
        /// <see cref="Node"/> が <c>null</c> でない事が保証されます。
        /// </remarks>
        public virtual void OnAttached () {
           }

        /// <summary>
        /// デタッチ処理前のエントリーポイント
        /// </summary>
        /// <remarks>
        /// <see cref="Node"/> が <c>null</c> でない事が保証されます。
        /// </remarks>
        public virtual void OnDetached () {

        }

        /// <summary>
        /// ディスパッチ処理のエントリーポイント
        /// </summary>
        /// <remarks>
        /// コンポーネントで独自にイベントを発行したい場合はこれをオーバーライドします。
        /// 通常ユーザーがこれを使用する事はありません。
        /// </remarks>
        public virtual void OnDispatch () {
        }

        /// <summary>
        /// 更新処理のエントリーポイント
        /// </summary>
        /// <remarks>
        /// 更新処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// 引数の <paramref name="msec"/> はゲームが起動してからの経過時間をミリ秒単位であらわします。
        /// </remarks>
        /// <param name="msec">ゲームが起動してからの起動時間(msec)</param>
        public virtual void OnUpdate (long msec) {
        }

        /// <summary>
        /// ライン イベントのエントリーポイント
        /// </summary>
        /// <remarks>
        /// イベントが設定されたラインを再生する時呼び出される仮想関数のエントリーポイント。
        /// このイベントは <see cref="LineReader"/> と同じノードにアタッチされたコンポーネントでのみ有効です。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// 第1引数の <paramref name="sender"/> はイベントの元になったラインです。
        /// 第2引数の <paramref name="args"/> はラインに設定されていた引数オブジェクトです。
        /// 実際の型はラインに記述されていた型です。
        /// </remarks>
        /// <param name="sender">イベントが設定されていたライン</param>
        /// <param name="args">任意のオブジェクト</param>
        public virtual void OnLineEvent (Line sender, object args) {
        }

        /// <summary>
        /// キーボード処理
        /// </summary>
        /// <remarks>
        /// キーボード処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// </remarks>
        public virtual void OnKeyPressed () {
        }


        /// <summary>
        /// キーボード処理
        /// </summary>
        /// <remarks>
        /// キーボード処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// </remarks>
        public virtual void OnKeyReleased () {
        }


        /// <summary>
        /// マウスボタン処理
        /// </summary>
        /// <remarks>
        /// マウス ボタンのクリック イベント処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// 座標位置(X,Y)はノードのローカル座標系です。
        /// </remarks>
        /// <param name="button">マウスボタン</param>
        /// <param name="x">マウスのX座標（ノード座標系）</param>
        /// <param name="y">マウスのY座標（ノード座標系）</param>
        public virtual void OnMouseButtonPressed (MouseButton button, int x, int y) {
         }


        /// <summary>
        /// マウスボタン処理
        /// </summary>
        /// <remarks>
        /// マウス ボタンのリリース イベント処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// 座標位置(X,Y)はノードのローカル座標系です。
        /// </remarks>
        /// <param name="button">マウスボタン</param>
        /// <param name="x">マウスのX座標（ノード座標系）</param>
        /// <param name="y">マウスのY座標（ノード座標系）</param>
        public virtual void OnMouseButtonReleased (MouseButton button, int x, int y) {
        }

        /// <summary>
        /// マウスボタン処理
        /// </summary>
        /// <remarks>
        /// マウス ボタンのフォーカス イン イベント処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// 座標位置(X,Y)はノードのローカル座標系です。
        /// </remarks>
        /// <param name="button">マウス ボタン</param>
        /// <param name="x">マウスのX座標（ノード座標系）</param>
        /// <param name="y">マウスのY座標（ノード座標系）</param>
        public virtual void OnMouseFocusIn (MouseButton button, int x, int y) {
        }

        /// <summary>
        /// マウスボタン処理
        /// </summary>
        /// <remarks>
        /// マウス ボタンのフォーカス アウト イベント処理を行う仮想関数のエントリーポイント。
        /// 必要ならこの仮想関数をオーバーライドして独自の処理を実装してください。
        /// 座標位置(X,Y)はノードのローカル座標系です。
        /// </remarks>
        /// <param name="button">マウス ボタン</param>
        /// <param name="x">マウスのX座標（ノード座標系）</param>
        /// <param name="y">マウスのY座標（ノード座標系）</param>
        public virtual void OnMouseFocusOut (MouseButton button, int x, int y) {
        }

        /// <summary>
        /// アニメーション処理
        /// </summary>
        /// <remarks>
        /// アニメーション処理を行う仮想関数のエントリーポイント。
        /// 通常はユーザーがこの仮想関数をオーバーライドする必要はありません。
        /// この仮想関数の適切な実装を提供するのはエンジン側の責任です。
        /// </remarks>
        /// <param name="msec">ワールド時刻(msec)</param>
        public virtual void OnAnimate (long msec) {

        }
    
        /// <summary>
        /// 描画処理
        /// </summary>
        /// <remarks>
        /// 描画処理を行う仮想関数のエントリーポイント。
        /// 通常はユーザーがこの仮想関数をオーバーライドする必要はありません。
        /// この仮想関数の適切な実装を提供するのはエンジン側の責任です。
        /// </remarks>
        /// <param name="window">ウィンドウ（SFML.Graphics.RenderWindow）</param>
        public virtual void OnDraw (object window) {

        }

        #endregion

    }
}
