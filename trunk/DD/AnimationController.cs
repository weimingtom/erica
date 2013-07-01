using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// アニメーション コントローラー コンポーネント
    /// </summary>
    /// <remarks>
    /// アニメーション クリップを制御するコンポーネント。
    /// </remarks>
    public class AnimationController : Component {

        #region Field
        List<AnimationClip> clips;
        #endregion

        /// <summary>
        /// コンストラクター 
        /// </summary>
        public AnimationController () {
            this.clips = new List<AnimationClip> ();
        }

        /// <summary>
        /// クリップ個数
        /// </summary>
        public int ClipCount{
            get{return clips.Count();}
        }

        /// <summary>
        /// すべてのクリップを列挙する列挙子
        /// </summary>
        public IEnumerable<AnimationClip> Clips {
            get { return clips; }
        }

        /// <summary>
        /// クリップに名前でアクセスするアクセッサー
        /// </summary>
        /// <remarks>
        /// 指定の名前のクリップが見つからない場合は <c>null</c> が返ります。
        /// </remarks>
        /// <param name="name">クリップの名前</param>
        /// <returns></returns>
        public AnimationClip this[string name] {
            get { 
                return (from x in clips
                             where x.Name == name
                             select x).FirstOrDefault();
            }
        }

        /// <summary>
        /// クリップの追加
        /// </summary>
        /// <param name="clip">アニメーション クリップ</param>
        public void AddClip (AnimationClip clip) {
            clips.Add (clip);
        }

        /// <summary>
        /// クリップの削除
        /// </summary>
        /// <param name="clip">アニメーション クリップ</param>
        public void RemoveClip            (AnimationClip clip) {

            clips.Remove (clip);
        }

        /// <summary>
        /// クリップの取得
        /// </summary>
        /// <param name="index">クリップ番号</param>
        /// <returns></returns>
        public AnimationClip GetClip (int index) {
            if (index < 0 || index > ClipCount - 1) {
                 throw new IndexOutOfRangeException("Index is out of range");
            }
            return clips[index];
        }

        /// <inheritdoc/>
        public override void OnAnimate (long msec) {
            foreach (var clip in clips.Where(x=>x.IsPlaying)) {
                var pos = clip.GetPlaybackPosition (msec);
                foreach (var track in clip.Tracks) {
                    var target = track.Item1;
                    var source = track.Item2;
                    var propInfo = target.GetType ().GetProperty (source.TargetProperty);
                    if (propInfo != null) {
                        var value = source.Sample (pos);
                        propInfo.SetValue (target, value, null);
                    }
                }
            }
        }
    }
}
