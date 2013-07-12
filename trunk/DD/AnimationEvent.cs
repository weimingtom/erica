using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// アニメーション イベント ハンドラー
    /// </summary>
    /// <param name="sender">イベントが設定されていたクリップ</param>
    /// <param name="args">イベント引数</param>
        public delegate void AnimationEventHandler (AnimationClip sender, EventArgs args);

    /// <summary>
    /// アニメーション イベント構造体
    /// </summary>
    /// <remarks>
    /// アニメーション クリップにはある特定の時刻が再生された時に指定のハンドラー関数を呼び出す事が可能です。
    /// この機能をアニメーション イベントと呼びます。
    /// </remarks>
        public struct AnimationEvent : IComparable<AnimationEvent> {
            /// <summary>
            /// コンストラクター
            /// </summary>
            /// <param name="pos">ローカル ポジション (msec)</param>
            /// <param name="handler">ハンドラー関数</param>
            /// <param name="args">引数</param>
            public AnimationEvent (int pos, AnimationEventHandler handler, EventArgs args)
                : this () {
                this.Position = pos;
                this.Handler = handler;
                this.Args = args;
            }
            /// <summary>
            /// ローカル ポジション
            /// </summary>
            public int Position { get; private set; }

            /// <summary>
            /// ハンドラー関数
            /// </summary>
            public AnimationEventHandler Handler { get; private set; }

            /// <summary>
            /// 引数
            /// </summary>
            public EventArgs Args { get; private set; }

            /// <inheritdoc/>
            int IComparable<AnimationEvent>.CompareTo (AnimationEvent other) {
                return this.Position - other.Position;
            }
        }
}
