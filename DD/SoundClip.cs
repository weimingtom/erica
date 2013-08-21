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
    /// サウンド クリップは音声、効果音、BGMなど”音の固まり”の集合であり、サウンド処理の基本単位となります。
    /// </remarks>
    public class SoundClip {
        #region Field
        string name;
        List<SoundTrack> tracks;
        float volume;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="name">クリップ名</param>
        public SoundClip (string name = null) {
            this.name = name ?? "";
            this.tracks = new List<SoundTrack> ();
            this.volume = 1;
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
        /// トラック数
        /// </summary>
        public int TrackCount {
            get { return tracks.Count (); }
        }

        /// <summary>
        /// すべてのトラックを列挙する列挙子
        /// </summary>
        public IEnumerable<SoundTrack> Tracks {
            get { return tracks; }
        }

        /// <summary>
        /// 指定のインデックスのトラックを取得するインデクサー
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns></returns>
        public SoundTrack this [int index] {
            get {
                if (index < 0 || index > TrackCount - 1) {
                    throw new IndexOutOfRangeException ("Index is out of range");
                }
                return tracks[index];
            }
        }

        /// <summary>
        /// クリップの再生時間 (msec)
        /// </summary>
        /// <remarks>
        /// このクリップを再生した時にかかる時間を msec で返します。
        /// </remarks>
        public int Duration {
            get {
                if (tracks.Count () == 0) {
                    return 0;
                }
                return tracks.Max (x => x.Duration);
            }
        }

        /// <summary>
        /// ボリューム
        /// </summary>
        /// <remarks>
        /// ボリュームを [0,1] で指定します。
        /// 0が無音で1が最大です。デフォルトは 1 です。
        /// </remarks>
        public float Volume {
            get {
                return volume;
            }
            set {
                if (value < 0 || value > 1) {
                    throw new ArgumentException ("Volume is invalid");
                }
                this.volume = value;
                foreach (var track in tracks) {
                    track.Volume = value;
                }
            }
        }


        /// <summary>
        /// 再生中フラグ
        /// </summary>
        public  bool IsPlaying {
            get {
                return tracks.Aggregate (false, (x, track) => x | track.IsPlaying);
            }
        }
        #endregion

        #region Field
        /// <summary>
        /// トラックの追加
        /// </summary>
        /// <remarks>
        /// このサウンド　クリップにトラックを追加します。
        /// すでに登録されているトラックを再度追加すると例外が発生します。
        /// </remarks>
        /// <param name="track">トラック</param>
        public void AddTrack (SoundTrack track) {
            if (tracks.Contains (track)) {
                throw new InvalidOperationException ("Track is already registered");
            }
            this.tracks.Add (track);
        }

        /// <summary>
        /// トラックの削除
        /// </summary>
        /// <param name="index">インデックス</param>
        public void RemoveTrack (int index) {
            if (index < 0 || index > TrackCount-1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            this.tracks.RemoveAt (index);
        }

        /// <summary>
        /// トラックの取得
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns></returns>
        public SoundTrack GetTrack (int index) {
            if (index < 0 || index > TrackCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return tracks[index];
        }



        /// <summary>
        /// 再生開始
        /// </summary>
        /// <remarks>
        /// すべてのトラックを再生開始します。
        /// すでに再生中のトラックを再度開始した場合の動作はトラックの種類により異なります。
        /// </remarks>
        public  void Play () {
            foreach (var track in tracks) {
                track.Play ();
            }
        }


        /// <summary>
        /// 再生停止
        /// </summary>
        /// <remarks>
        /// すでに停止中だった場合何もしません。
        /// </remarks>
        public  void Stop () {
            foreach (var track in tracks) {
                track.Stop ();
            }
        }


        /// <inheritdoc/>
        public void Dispose () {
            foreach (var track in tracks) {
                track.Dispose ();
            }
        }
        #endregion

    }
}
