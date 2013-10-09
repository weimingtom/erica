using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// 時計塔コンポーネント
    /// </summary>
    /// <remarks>
    /// シーン中に1つ存在し現在時刻を管理します。
    /// ユーザーがこのコンポーネントをインスタンス化することはありません。
    /// </remarks>
    public class ClockTower : Component {

        #region Field
        long curTime;       // 現在時刻 (msec)
        long prevTime;      // 1つ前の時刻 (msec)
        int frameCount;     // ゲームを開始してからのフレーム数（カウント）
        float frameRate;    // フレーム レート
        long tickTime;     // FPS計測開始時刻
        int tickCount;     // FPS計測開始カウント
        #endregion


        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public ClockTower () {
            this.curTime = 0;
            this.prevTime = 0;
            this.frameCount = 0;
            this.frameRate = 0;
            this.tickTime = 0;
            this.tickCount = 0;
        }
        #endregion

        /// <summary>
        /// 現在時刻
        /// </summary>
        /// <remarks>
        /// 現在時刻とは最後に呼び出された <see cref="World.Update"/> の引数で渡された時刻の事です。
        /// </remarks>
        public long CurrentTime {
            get { return curTime; }
        }

        /// <summary>
        /// 1フレーム前の時刻
        /// </summary>
        /// <remarks>
        /// 現在時刻とは1つ前のフレームで呼び出された <see cref="World.Update"/> の引数で渡された時刻の事です。
        /// </remarks>
        public long PreviousTime {
            get { return prevTime; }
        }

        /// <summary>
        /// 現在時刻と1フレーム前の時刻の差
        /// </summary>
        /// <remarks>
        /// 1フレームの時間です。
        /// </remarks>
        public long DeltaTime {
            get { return curTime - prevTime; }
        }

        /// <summary>
        /// ゲームエンジンが起動してから処理したフレーム数の合計
        /// </summary>
        public int FrameCount {
            get { return frameCount; }
        }

        /// <summary>
        /// 1秒あたりのフレーム数
        /// </summary>
        /// <remarks>
        /// 実時間ではなく <see cref="World.Update"/> で渡された時間とフレーム数で計算されます。
        /// </remarks>
        public float FrameRate {
            get { return frameRate; }
        }

        /// <inheritdoc/>
        public override void OnUpdate (long msec) {
            this.prevTime = this.curTime;
            this.curTime = msec;
            this.frameCount += 1;

            if (msec - tickTime >= 1000) {
                this.frameRate = (frameCount - tickCount) / ((msec - tickTime)/1000f);
                this.tickTime = msec;
                this.tickCount = frameCount;
            }
        }

        /// <inheritdoc/>
        public override string ToString () {
            return string.Format ("Clock: {0}", curTime);
        }
    }
}
