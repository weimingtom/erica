using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// ステート マシーン クラス
    /// </summary>
    /// <remarks>
    /// 標準的なステート マシーンの実装です。
    /// <typeref name="T"/> を型パラメーターとして受け取り、
    /// そのすべての要素をステートとして持つステート マシーンを作成します。
    /// </remarks>
    public class StateMachine<T> where T : struct, IConvertible {

        #region Inner
        /// <summary>
        /// ステート
        /// </summary>
        /// <remarks>
        /// ステートマシン用のステート
        /// </remarks>
        public class State {
            #region Field
            T enumValue;
            Dictionary<State, Func<bool>> transitions;
            #endregion

            #region Constructor
            /// <summary>
            /// コンストラクター
            /// </summary>
            /// <param name="enumValue">ステートをあらわすenum値</param>
            internal State (T enumValue) {
                this.enumValue = enumValue;
                this.transitions = new Dictionary<State, Func<bool>> ();
            }

            #endregion

            /// <summary>
            /// 入力イベント
            /// </summary>
            public event Action<T> Enter;

            /// <summary>
            /// 出力イベント
            /// </summary>
            public event Action<T> Exit;

            /// <summary>
            /// ステート名
            /// </summary>
            public string Name {
                get { return enumValue.ToString (); }
            }

            /// <summary>
            /// ステートの enum 値
            /// </summary>
            public T EnumValue {
                get { return enumValue; }
            }

            /// <summary>
            /// すべての遷移を列挙する列挙子
            /// </summary>
            public IEnumerable<KeyValuePair<State, Func<bool>>> Transitions {
                get { return transitions; }
            }

            /// <summary>
            /// 入力イベントの起動
            /// </summary>
            public void InvokeEnterEvent () {
                if (Enter != null) {
                    Enter.Invoke (enumValue);
                }
            }

            /// <summary>
            /// 出力イベントの起動
            /// </summary>
            public void InvokeExitEvent () {
                if (Exit != null) {
                    Exit.Invoke (enumValue);
                }
            }

            /// <summary>
            /// ステート遷移の登録
            /// </summary>
            /// <remarks>
            /// このステートから遷移するステートを登録します。
            /// 条件 <paramref name="pred"/> を満たす時 <paramref name="to"/> に遷移します。
            /// 自分自身への遷移も可。
            /// </remarks>
            /// <param name="to">遷移先ステート</param>
            /// <param name="pred">遷移条件</param>
            public void AddTransition (State to, Func<bool> pred) {
                if (this.transitions.ContainsKey (to)) {
                    throw new ArgumentException ("Transition state is already registered");
                }
                if (pred == null) {
                    throw new ArgumentNullException ("Predicate is null");
                }
                this.transitions.Add (to, pred);
            }

            /// <summary>
            /// ステートからenumへの明示的な型変換
            /// </summary>
            /// <param name="state"> 変換元の<see cref="State"/>オブジェクト</param>
            /// <returns>対応する <typeref name="T"/>型のenum値</returns>
            public static implicit operator T (State state) {
                return state.enumValue;
            }

        }
        #endregion

        #region Field
        readonly List<State> states;
        State currnet;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public StateMachine () {
            if (!typeof (T).IsEnum) {
                throw new ArgumentException ("T of StateMachine must be enum");
            }
            this.states = new List<State> ();
            foreach (T state in typeof (T).GetEnumValues ()) {
                this.states.Add (new State (state));
            }
            this.currnet = this[default (T)];
        }
        #endregion

        #region Property
        /// <summary>
        /// 現在のステート
        /// </summary>
        public State CurrentState {
            get { return currnet; }
            set { SetCurrentState (value); }
        }

        /// <summary>
        /// 現在のステート（enum値）
        /// </summary>
        public T Current {
            get { return currnet.EnumValue; }
            set { SetCurrent (value); }
        }

        /// <summary>
        /// enum値に対応するステートを取得するインデクサー
        /// </summary>
        /// <param name="enumValue">ステートの enum 値</param>
        /// <returns>ステート</returns>
        public State this[T enumValue] {
            get { return states.Find (x => x.EnumValue.Equals (enumValue)); }
        }

        /// <summary>
        /// ステート数
        /// </summary>
        public int StateCount {
            get { return states.Count (); }
        }

        /// <summary>
        /// 遷移数
        /// </summary>
        public int TransitionCount {
            get {
                return states.SelectMany (x => x.Transitions).Count ();
            }
        }

        /// <summary>
        /// すべてのステートを列挙する列挙子
        /// </summary>
        public IEnumerable<State> States {
            get { return states; }
        }

        /// <summary>
        /// すべての遷移を列挙する列挙子
        /// </summary>
        public IEnumerable<KeyValuePair<State, Func<bool>>> Transitions {
            get { return states.SelectMany (x => x.Transitions); }
        }


        #endregion



        #region Method
        /// <summary>
        /// ステートの遷移
        /// </summary>
        /// <remarks>
        /// 現在のステートから遷移条件を満たす次のステートに遷移します。
        /// 
        /// </remarks>
        public void Process () {
            foreach (var tran in CurrentState.Transitions) {
                var next = tran.Key;
                var pred = tran.Value;
                if (pred ()) {
                    if (next != currnet) {
                        next.InvokeEnterEvent ();
                        currnet.InvokeExitEvent ();
                        this.currnet = next;
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// ステートの登録
        /// </summary>
        /// <param name="state">ステート</param>
        /// <param name="enter">入力イベント</param>
        /// <param name="exit">出力イベント</param>
        public void AddEvent (State state, Action<T> enter, Action<T> exit) {
            if (state == null) {
                throw new ArgumentNullException ("State is null");
            }
            if (!states.Contains (state)) {
                throw new ArgumentException ("State doesn't exist in this Machine");
            }
            if (enter != null) {
                state.Enter += enter;
            }
            if (exit != null) {
                state.Exit += exit;
            }
        }

        /// <summary>
        /// 遷移の登録
        /// </summary>
        /// <param name="from">遷移元ステート</param>
        /// <param name="to">遷移先ステート</param>
        /// <param name="pred">遷移条件</param>
        public void AddTransition (State from, State to, Func<bool> pred) {
            if (from == null) {
                throw new ArgumentNullException ("From is null");
            }
            if (!states.Contains (from)) {
                throw new ArgumentException ("From doesn't exist in this Machine");
            }
            if (to == null) {
                throw new ArgumentNullException ("From is null");
            }
            if (!states.Contains (to)) {
                throw new ArgumentException ("From doesn't exist in this Machine");
            }
            if (pred == null) {
                throw new ArgumentNullException ("Predicate is null");
            }
            from.AddTransition (to, pred);
        }

        /// <summary>
        /// カレント ステートの変更
        /// </summary>
        /// <param name="state">ステート</param>
        public void SetCurrentState (State state) {
            if (state == null) {
                throw new ArgumentNullException ("State is null");
            }
            if (!states.Contains (state)) {
                throw new ArgumentException ("State don't exist in this machine");
            }

            this.currnet = state;
            //this.currnet = states.Find (x => x.EnumValue.Equals (state)); ;
        }

        /// <summary>
        /// カレント ステートの変更（enum値）
        /// </summary>
        /// <param name="state"></param>
        public void SetCurrent (T state){
            this.currnet = states.Find (x => x.EnumValue.Equals (state)); ;
        }

        /// <summary>
        /// ステートの取得
        /// </summary>
        /// <param name="index">ステート番号</param>
        /// <returns>ステート</returns>
        public State GetState (int index) {
            if (index < 0 || index > StateCount - 1) {
                throw new IndexOutOfRangeException ("Incex is out of range");
            }
            return states[index];
        }
        #endregion


    }
}
