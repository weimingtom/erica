using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// サウンド再生 コンポーネント
    /// </summary>
    /// <remarks>
    /// サウンドを再生するためのコンポーネントです。
    /// サウンドクリップを直接再生する事もできるので、
    /// 現在のところあまり意味はない。
    /// </remarks>
    public class SoundPlayer : Component {
        #region Field
        List<SoundClip> clips;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public SoundPlayer () {
            this.clips = new List<SoundClip> ();
        }
        #endregion

        #region Propety
        /// <summary>
        /// クリップの総数
        /// </summary>
        public int ClipCount {
            get { return clips.Count (); }
        }

        /// <summary>
        /// すべてのクリップを列挙する列挙子
        /// </summary>
        public IEnumerable<SoundClip> Clips {
            get { return clips; }
        }

        /// <summary>
        /// すべてのクリップの名前を列挙する列挙子
        /// </summary>
        public IEnumerable<string> Names {
            get { return clips.Select (x => x.Name); }
        }

        /// <summary>
        /// クリップにアクセスするインデクサー
        /// </summary>
        /// <param name="name">クリップ名</param>
        /// <returns><see cref="SoundClip"/> オブジェクト</returns>
        public SoundClip this[string name] {
            get { return clips.Find(x => x.Name == name); }
        }
        #endregion

        #region Method
        /// <summary>
        /// クリップの追加
        /// </summary>
        /// <remarks>
        /// すでに登録済みのクリップは無視します。
        /// </remarks>
        /// <param name="clip">クリップ</param>
        /// <param name="playNow">今すぐ再生を始めるフラグ</param>
        public void AddClip (SoundClip clip, bool playNow = false) {
            if (clip == null) {
                throw new ArgumentNullException ("Clip is null");
            }
            if (clips.Contains (clip)) {
                return;
            }
            if (playNow) {
                clip.Play ();
            }
            this.clips.Add (clip);
        }

        /// <summary>
        /// クリップの削除
        /// </summary>
        /// <param name="clip">クリップ</param>
        /// <returns></returns>
        public bool RemoveClip (SoundClip clip) {
            return this.clips.Remove (clip);
        }

        /// <summary>
        /// クリップの取得
        /// </summary>
        /// <param name="index">クリップ番号</param>
        /// <returns><see cref="SoundClip"/> オブジェクト</returns>
        public SoundClip GetClip (int index) {
            if (index < 0 || index > ClipCount - 1) {
                throw new IndexOutOfRangeException ("Index if out of range");
            }
            return clips[index];
        }
        #endregion

    }
}
