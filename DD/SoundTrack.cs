using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// サウンド トラック
    /// </summary>
    /// <remarks>
    /// BGMや効果音などの一連の音の集まりです。サウンド クリップ <see cref="SoundClip"/> に登録して使用します。
    /// </remarks>
    /// <see cref="SoundClip"/>
    /// <seealso cref="MusicTrack"/>
    /// <seealso cref="SoundEffectTrack"/>
    public abstract class SoundTrack : IDisposable {

        /// <summary>
        /// ファイル名
        /// </summary>
        /// <remarks>
        /// サウンドの元になったファイル名です。
        /// </remarks>
        public abstract string FileName { get; }

        /// <summary>
        /// 再生時間 (msec)
        /// </summary>
        public abstract int Duration { get; }

        /// <summary>
        /// 再生ボリューム
        /// </summary>
        /// <remarks>
        /// 再生ボリュームは [0,1] の範囲です。
        /// </remarks>
        public abstract float Volume { get; set; }

        /// <summary>
        /// 再生中フラグ
        /// </summary>
        public abstract bool IsPlaying {get;}

        /// <summary>
        /// 再生開始
        /// </summary>
        /// <remarks>
        /// すでに再生中のサウンドを再度開始した場合、
        /// <see cref="MusicTrack"/> はそのままの位置で再生を続け、
        /// <see cref="SoundEffectTrack"/> は再生位置を０に戻して再生を最初からやり直します。
        /// </remarks>
        public abstract void Play ();

        /// <summary>
        /// 再生停止
        /// </summary>
        public abstract void Stop ();

        /// <inheritdoc>
        public abstract void Dispose ();
    }
}
