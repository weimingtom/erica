using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Audio;

namespace DD {
    /// <summary>
    /// サウンド クリップ クラス
    /// </summary>
    /// <remarks>
    /// サウンド クリップは音声、効果音、BGMなど”音の固まり”であり、サウンド処理の基本単位となります。
    /// セリフや効果音のような短いクリップはすべてのデータをメモリ上に展開する場合と、
    /// BGMのように一部をメモリ上に展開するだけのストリーミング再生があります。
    /// 
    /// </remarks>
    public class SoundClip : IDisposable {
        #region Field
        string name;
        SFML.Audio.Sound sound;
        SFML.Audio.Music music;
        #endregion


        #region Constructor
        /// <summary>
        /// オンメモリ又はストリーミングのサウンド クリップ オブジェクトを作成するコンストラクター
        /// </summary>
        /// <remarks>
        /// ストリーミングの再生の有無を指定して <see cref="SoundClip"/> オブジェクトを作成します。
        /// ストリーミング再生は大きすぎてすべてのデータをメモリ上に乗せられないサウンド
        /// （例えばBGM）を再生するのに適しています。
        /// ストリーミング再生の時に限りループを有効にします。
        /// </remarks>
        /// <param name="fileName">ファイル名</param>
        /// <param name="streaming">ストリーミング再生</param>
        public SoundClip (string fileName, bool streaming) {
            if (fileName == null || fileName == "") {
                throw new ArgumentNullException ("Name is null or empty");
            }
            this.name = fileName;
            if (streaming) {
                this.sound = null;
                this.music = new Music (fileName);
                this.Loop = true;
            }
            else {
                this.sound = new Sound (new SoundBuffer (fileName));
                this.music = null;
                this.Loop = false;
            }
            
        }

        /// <summary>
        /// オンメモリ サウンド クリップ オブジェクトを作成するコンストラクター
        /// </summary>
        /// <remarks>
        /// ストリーミング再生を無効にして <see cref="SoundClip"/> オブジェクトを作成します。
        /// データはすべてメモリ上に置かれます。これは効果音などの再生に適しています。
        /// </remarks>
        /// <param name="fileName">ファイル名</param>
        public SoundClip (string fileName) : this (fileName, false) {
        }
        #endregion

        #region Property
        /// <summary>
        /// 名前
        /// </summary>
        public string Name {
            get { return name; }
        }

        /// <summary>
        /// ストリーミング再生
        /// </summary>
        public bool Streaming {
            get { return (music != null) ? true : false; }
        }

        /// <summary>
        /// ループ再生
        /// </summary>
        public bool Loop {
            get { return (sound != null) ? sound.Loop : music.Loop; }
            set { SetLoop (value); }
        }

        /// <summary>
        /// ボリューム
        /// </summary>
        /// <remarks>
        /// ボリュームを[0,1]で指定します。
        /// 0が無音で1が最大です。デフォルトは1.0です。
        /// </remarks>
        /// SFMLはボリュームを[0,100]で表すのでその1/100.
        public float Volume {
            get { return (sound != null) ? sound.Volume / 100f : music.Volume / 100f; }
            set { SetVolume (value); }
        }

        /// <summary>
        /// 再生時間(msec)
        /// </summary>
        /// <remarks>
        /// このクリップをすべて再生した時の時間を msec で返します。
        /// <note>
        /// マニュアルによると SFML:SoundBuffer.Duration の戻り値は msec だが、
        /// 実測では usec で返ってくるのでここで 1000 で割っている。
        /// 将来的に1000倍違ってたらごめん。
        /// </note>
        /// </remarks>
        public int Duration {
            get { return (sound != null) ? (int)sound.SoundBuffer.Duration / 1000: (int)music.Duration.TotalMilliseconds; ; }
        }

        /// <summary>
        /// 再生中フラグ
        /// </summary>
        public bool IsPlaying {
            get{return (sound!=null)? (sound.Status == SoundStatus.Playing) : (music.Status == SoundStatus.Playing);   }
        }
        #endregion

        /// <summary>
        /// 再生
        /// </summary>
        /// <remarks>
        /// 現在再生中の場合、曲の最初に戻って再生を続けます。
        /// </remarks>
        public void Play () {
            if (sound != null) {
                sound.Play ();
            }
            else {
                music.Play ();
            }
        }


        /// <summary>
        /// 停止
        /// </summary>
        /// <remarks>
        /// すでに停止中だった場合何もしません。
        /// </remarks>
        public void Stop () {
            if (sound != null) {
                sound.Stop ();
            }
            else {
                music.Stop ();
            }
        }

        /// <summary>
        /// ボリュームの変更
        /// </summary>
        /// <remarks>
        /// ボリュームを[0,1]の範囲で変更します。
        /// 0が無音で1が最大です。デフォルトは1.0です。
        /// </remarks>
        /// <param name="volume">ボリューム</param>
        public void SetVolume (float volume) {
            if (volume < 0) {
                throw new ArgumentException ("Volume is invalid");
            }
            if (sound != null) {
                sound.Volume = volume * 100;   // SFMLでは[0,100]
            }
            else {
                music.Volume = volume * 100;
            }
        }

        /// <summary>
        /// ループ再生の変更
        /// </summary>
        /// <param name="enable">ループ</param>
        public void SetLoop (bool enable) {
            if (sound != null) {
                sound.Loop = enable;
            }
            else {
                music.Loop = enable;
            }
        }

        /// <inheritdoc/>
        public void Dispose () {
            if (sound != null) {
                sound.Dispose ();
                sound = null;
            }
            if (music != null) {
                music.Dispose ();
                music = null;
            }
        }
    }
}
