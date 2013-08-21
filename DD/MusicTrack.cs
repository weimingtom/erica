using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;

namespace DD {
    /// <summary>
    /// ミュージック　トラック
    /// </summary>
    /// <remarks>
    /// ミュージック トラックはBGMなどの長い音データをストリーミング再生するのに適したトラックです。
    /// 音データはファイルから一部のみメモリ上に展開され再生されます。
    /// このトラックは自動でループ再生されます。
    /// </remarks>
    public class MusicTrack : SoundTrack {

        #region Field
        SFML.Audio.Music data;
        string fileName;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public MusicTrack (string fileName) {
            if (fileName == null || fileName == "") {
                throw new ArgumentNullException ("FileName is null or empty");
            }
            this.fileName = fileName;
            this.data = new Music (fileName);
            this.data.Loop = true;
        }
        #endregion

        #region Property

        /// <inheritdoc>
        public override string FileName {
            get { return fileName; }
        }

        /// <inheritdoc>
        public override int Duration { 
            get { return (int)data.Duration.TotalMilliseconds; } 
        }

        /// <inheritdoc>
        public override float Volume {
            get {
                return data.Volume / 100f;
            }
            set {
                if (value < 0 || value > 1) {
                    throw new ArgumentException ("Volumen is invalid");
                }
                this.data.Volume = value * 100;
            }
        }

        /// <inheritdoc>
        public override bool IsPlaying {
            get { return data.Status == SoundStatus.Playing; }
        }
        #endregion

        #region Method
        /// <inheritdoc>
        public override void Play () {
            if (IsPlaying) {
                return;
            }
            data.Play();
        }

        /// <inheritdoc>
        public override void Stop () {
            data.Stop ();
        }

        /// <inheritdoc>
        public override void Dispose () {
            if (data != null) {
                data.Dispose ();
                this.data = null;
            }
        }

        /// <inheritdoc>
        public override string ToString () {
            return fileName;
        }

        #endregion

    }
}
