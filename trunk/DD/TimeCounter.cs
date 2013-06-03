using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace DD {
    /// <summary>
    /// カウンター クラス
    /// </summary>
    /// <remarks>
    /// 一定期間(<see cref="Interval"/>)毎に自動で +1 されるカウンターです。
    /// カウントアップする度にイベント <see cref="Elapsed"/> を呼び出します。
    /// カウンタが既定回数(<see cref="MaxCount"/>) に達すると停止します。
    /// インターバル <see cref="Interval"/> = 0 を指定すると、イベントは呼び出されず、
    /// いきなりカウンタが最大回数になります。
    /// なおイベント処理は非同期です。
    /// タイマーをストップした後でイベントが処理される可能性が十分にあります。
    /// 使用後に Close() または Dispose() の呼び出しが必要です。
    /// </remarks>
    public class TimeCounter : IDisposable {
        #region Field
        Timer timer;
        int count;
        int maxCount;
        int interval;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// (*1) インターバル = 0 は例外が発生するので適当に値を突っ込んでいる。
        ///      この状態では起動する事がないので大丈夫
        /// <param name="maxCount">最大カウント</param>
        /// <param name="interval">インターバル (msec)</param>
        public TimeCounter (int maxCount, int interval) 
        {
            this.count = 0;
            this.maxCount = maxCount;
            this.interval = interval;
            this.timer = new Timer ();
            timer.Interval = (interval > 0) ? interval : Int32.MaxValue;  // (*1)
            timer.Elapsed += new ElapsedEventHandler(OnElapsed);
        }

        #endregion

        #region Event
        /// <summary>
        /// カウントアップ イベント
        /// </summary>
        /// <remarks>
        /// <see cref="Interval"/> ミリ秒毎に呼び出されるイベントです。
        /// 最大 <see cref="MaxCount"/> 回呼び出されます。
        /// イベントは非同期でメインスレッドとは別のスレッドから呼び出されます。
        /// </remarks>
        public event Action Elapsed;
        #endregion

        #region Property
        /// <summary>
        /// インターバル (msec)
        /// </summary>
        /// <remarks>
        /// このインターバル毎にカウンタが +1 され、イベントが発生します。
        /// </remarks>
        public int Interval {
            get { return interval; }
        }

        /// <summary>
        /// カウント数
        /// </summary>
        public int Count {
            get { return count; }
        }

        /// <summary>
        /// 最大カウント
        /// </summary>
        /// <remarks>
        /// カウントがこの数に達するとタイマーが停止します。
        /// </remarks>
        public int MaxCount {
            get { return maxCount; }
        }

        /// <summary>
        /// 動作フラグ
        /// </summary>
        /// <remarks>
        /// 動作（カウントアップ）中は true が返ります。
        /// タイマーは非同期動作なので、このフラグが false を返した後にイベントハンドラーが呼び出される可能性があります。
        /// </remarks>
        public bool IsRunning {
            get { return timer.Enabled; }
        }


        #endregion


        #region Method
        /// <summary>
        /// タイマーを開始
        /// </summary>
        public void Start () {
            if (interval == 0) {
                  this.count = maxCount;
                  return;
            }

            this.timer.Start ();
        }
        
        /// <summary>
        /// タイマーを停止
        /// </summary>
        public void Stop () {
            this.timer.Stop ();
        }

        /// <summary>
        /// タイマーの削除
        /// </summary>
        public void Close () {
            this.timer.Close ();
        }

        /// <summary>
        /// インターバル ハンドラー
        /// </summary>
        /// <param name="sender">.Netのタイマー</param>
        /// <param name="args">引数</param>
        private void OnElapsed (Object sender, ElapsedEventArgs args) {
            count += 1;
            if (count <= maxCount) {
                if (Elapsed != null) {
                    Elapsed ();
                }
            }
            if (count == maxCount) {
                this.timer.Stop ();
            }
        }

        #endregion



        /// <inheritdoc/>
        public void Dispose () {
            timer.Close ();
            timer = null;
        }
    }
}
