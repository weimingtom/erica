using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;



namespace DD {

    /// <summary>
    /// ディレクターの状態
    /// </summary>
    public enum DirectorState {
        /// <summary>
        /// ディレクターは有効な状態
        /// </summary>
        Alive,

        /// <summary>
        /// ディレクターは無効な状態
        /// </summary>
        Dead
    }

    /// <summary>
    /// ディレクター クラス
    /// </summary>
    /// <remarks>
    /// スクリプトを管理する監督クラスです。
    /// スクリプトの追加、削除、更新、描画などを行います。
    /// 複数のスクリプトをスタック構造で管理し、上記の操作は一番上のスクリプトに対して行われます。
    /// この一番上のスクリプトをカレント スクリプトと呼びます。
    /// </remarks>
    /// <seealso cref="World"/>
    public class Director {

        #region Field
        DirectorState state;
        Stack<World> scripts;
        Stopwatch watch;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Director () {
            this.state = DirectorState.Alive;
            this.scripts = new Stack<World> ();
            this.watch = new Stopwatch ();

            watch.Start ();
        }
        #endregion


        #region Property
        /// <summary>
        /// アクティブ フラグ
        /// </summary>
        /// <remarks>
        /// このディレクターが正常に動作している時 true, そうでない時 false を返します。
        /// このフラグが false な時は <see cref="Exit"/> メソッドのみ呼び出し可能です。
        /// </remarks>
        public bool IsAlive {
            get { return state == DirectorState.Alive; }
        }

        /// <summary>
        /// 現在実行中のスクリプト
        /// </summary>
        /// <remarks>
        /// 現在実行中のスクリプトを取得します。
        /// </remarks>
        public World CurrentScript {
            get {return scripts.FirstOrDefault();}
        }

        /// <summary>
        /// シーンの個数
        /// </summary>
        /// <remarks>
        /// このディレクターにセットされたスクリプトの総数を取得します。
        /// </remarks>
        public int ScriptCount{
            get {return scripts.Count();}
        }

        /// <summary>
        /// スクリプトの列挙子
        /// </summary>
        /// <remarks>
        /// このディレクターにセットされたスクリプトをすべて列挙します。
        /// </remarks>
        public IEnumerable<World> Scripts {
            get { return scripts; }
        }
        #endregion

        #region Method

        /// <summary>
        /// ディレクター イベントの追加
        /// </summary>
        /// <param name="sender">イベントの送り手</param>
        /// <param name="args">イベント引数</param>
        public void AddEvent (object sender, EventArgs args) {

        }
        
        /// <summary>
        /// スクリプトの登録
        /// </summary>
        /// <remarks>
        /// このディレクターにスクリプトを追加します。
        /// 追加されたスクリプトは即座に <see cref="CurrentScript"/> になります。
        /// すでに追加されているスクリプトを再度登録するとエラーを発生します。
        /// </remarks>
        /// <param name="script">スクリプト</param>
        public void PushScript (World script){
            if (script.Director != null) {
                throw new InvalidOperationException ("Script has another director");
            }
            if (scripts.Contains (script)) {
                throw new InvalidOperationException ("Script is already registered");
            }

            script.SetDirector (this);
            this.scripts.Push (script);
        }

        /// <summary>
        /// シーンの削除
        /// </summary>
        /// <remarks>
        /// このディレクターからカレントのスクリプトを削除します。
        /// <see cref="CurrentScript"/> は次のスクリプトに移行します。
        /// </remarks>
        public void PopScript () {
            var script = this.scripts.Pop ();
            script.SetDirector (null);
        }

        /// <summary>
        /// スクリプトの更新
        /// </summary>
        /// <remarks>
        /// カレント スクリプトの更新処理を実行します。
        /// </remarks>
        public void Update () {
            if (IsAlive == false || CurrentScript == null) {
                return;
            }
            foreach (var node in CurrentScript.Downwards) {
                foreach (var comp in node.Components) {
                    comp.OnUpdate (watch.ElapsedMilliseconds);
                }
            }
        }

        /// <summary>
        /// ゲームのイベント発生
        /// </summary>
        /// <remarks>
        /// <see cref="CurrentScript"/> のイベント発生処理を実行します。
        /// </remarks>
        public void Raise () {
        }

        /// <summary>
        /// ゲームの終了
        /// </summary>
        /// <remarks>
        /// このディレクターを終了します。
        /// これ以降このディレクターを使用する事はできません。
        /// </remarks>
        public void Exit () {
            watch.Stop ();
            this.state = DirectorState.Dead;
        }
        #endregion


    }
}
