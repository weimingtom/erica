using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;

namespace DD {

    /// <summary>
    /// サウンド エフェクト　トラック
    /// </summary>
    /// <remarks>
    /// サウンド エフェクト トラックは効果音などの短い時間のサウンドを、遅延なしで再生するのに適したトラックです。
    /// このトラックは指定の音データを1回だけ再生します。
    /// </remarks>
    public class SoundEffectTrack : SoundTrack {

        #region Field
        string fileName;
        SFML.Audio.Sound data;
        #endregion

        #region Construcotr
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public SoundEffectTrack (string fileName) {
            if (fileName == null || fileName == "") {
                throw new ArgumentNullException ("FileName is null or empty");
            }
            this.fileName = fileName;
            this.data = new Sound (new SoundBuffer (fileName));
            this.data.Loop = false;
        }
        #endregion

        #region Property
        /// <inheritdoc/>
        public override string FileName { 
            get {return fileName;}
        }

        /// <inheritdoc/>
        public override int Duration { 
            get{return (int)data.SoundBuffer.Duration / 1000;}
        }

        /// <inheritdoc/>
        public override float Volume { 
            get {return data.Volume/100f;}
            set{
                if(value < 0 || value > 1) {
                    throw new ArgumentException("Volume is invalie");
                }
                data.Volume = value*100;
            }
        }

        /// <inheritdoc/>
        public override bool IsPlaying { 
            get {
                return data.Status == SoundStatus.Playing;
            }
        }
        #endregion

        #region Method
        /// <inheritdoc/>
        public override void Play () {
            if (IsPlaying) {
                data.Stop ();
            }
            data.Play();  
        }

        /// <inheritdoc/>
        public override void Stop () {
            data.Stop();
        }

        /// <inheritdoc/>
        public override void Dispose () {
            if(data != null){
                data.Dispose();
                this.data = null;
            }
        }

        /// <inheritdoc/>
        public override string ToString () {
            return fileName;
        }

        #endregion

    }


}
