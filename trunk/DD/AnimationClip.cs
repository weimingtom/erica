using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// アニメーション クリップ クラス
    /// </summary>
    /// <remarks>
    /// アニメーションクリップは“走る”、“歩く”などの一連の動作を表すアニメーションの基本単位です。
    /// 原則としてアニメーションに対する操作はこのクリップ単位で行います。
    /// </remarks>
    public class AnimationClip {
        #region Field
        string name;
        int duration;
        int localRefTime;
        float worldRefTime;
        float speed;
        WrapMode wrapMode;
        List<Tuple<object, AnimationTrack>> tracks;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// <paramref name="name"/> はこのクリップを識別しやすいようにユーザーが自由に決められる名前です。
        /// エンジン側では使用しません。
        /// </remarks>
        /// <param name="name">このクリップの名前</param>
        public AnimationClip (string name) {
            this.name = name ?? "";
            this.duration = 0;
            this.localRefTime = 0;
            this.worldRefTime = 0;
            this.speed = 1;
            this.tracks = new List<Tuple<object, AnimationTrack>> ();
            this.wrapMode = WrapMode.Loop;
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        public AnimationClip () : this("") {
        }
        #endregion

        #region Propety
        /// <summary>
        /// クリップの名前
        /// </summary>
        public string Name {
            get { return name; }
        }

        /// <summary>
        /// クリップの長さ（msec）
        /// </summary>
        public int Duration {
            get { return duration; }
            set { SetDuration (value); }
        }

        /// <summary>
        /// 繰り返しモード
        /// </summary>
        public WrapMode WrapMode {
            get { return wrapMode; }
            set { SetWrapMode (value); }
        }

        /// <summary>
        /// 再生速度
        /// </summary>
        /// <remarks>
        /// このプロパティを直接書き換えて再生中のアニメーションの速度を変更すると、
        /// アニメーションが不連続になります。
        /// それを避けたい場合は <see cref="SetSpeed"/> メソッドを使用して変更してください。
        /// </remarks>
        public float Speed {
            get { return speed; }
            set { SetSpeed (value, 0); }
        }

        /// <summary>
        /// トラック数
        /// </summary>
        public int TrackCount {
            get { return tracks.Count; }
        }

        /// <summary>
        /// すべてのトラックを列挙する列挙子
        /// </summary>
        public IEnumerable<Tuple<object, AnimationTrack>> Tracks {
            get { return tracks; }
        }
        #endregion

        #region Method
        /// <summary>
        /// トラックの追加
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="target">ターゲット オブジェクト</param>
        /// <param name="track">追加したいトラック</param>
        public void AddTrack (object target, AnimationTrack track) {
            if (target == null) {
                throw new ArgumentNullException ("Target is null");
            }
            if (track == null) {
                throw new ArgumentNullException ("Track is null");
            }
            this.tracks.Add (Tuple.Create(target, track));
        }

        /// <summary>
        /// トラックの削除
        /// </summary>
        /// <remarks>
        /// 登録されていなかったトラックの削除は安全に無視します。
        /// </remarks>
        /// <param name="track">削除したいトラック</param>
        /// <returns>削除したら <c>true</c>、そうでなければ <c>false</c>。</returns>
        public int RemoveTrack (AnimationTrack track) {
            return this.tracks.RemoveAll (x => x.Item2 == track);
        }

        /// <summary>
        /// トラックの取得
        /// </summary>
        /// <param name="index">トラック番号</param>
        /// <returns></returns>
        public Tuple<object, AnimationTrack> GetTrack (int index) {
            if (index < 0 || index > TrackCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return tracks[index];
        }

        /// <summary>
        /// 再生位置の変更
        /// </summary>
        /// <remarks>
        /// 指定のワールド時刻 <paramref name="worldTime"/> に指定のローカルポジション
        /// <paramref name="position"/> が再生されるようにクリップの再生位置を変更します。
        /// </remarks>
        /// <param name="position">ローカル ポジション</param>
        /// <param name="worldTime">ワールド時刻</param>
        public void SetPlaybackPoisition (int position, float worldTime) {
            this.localRefTime = position;
            this.worldRefTime = worldTime;
        }

        /// <summary>
        /// 再生速度の変更
        /// </summary>
        /// <remarks>
        /// 再速度を変更します。1が標準で2だと2倍の速度で再生されます。速度は0またはマイナスも可能です。
        /// 再生中のクリップが連続するように、通常はワールド時間 <paramref name="worldTime"/> に現在時刻を指定します。
        /// </remarks>
        /// <param name="speed">新しい再生速度</param>
        /// <param name="worldTime">現在のワールド時刻</param>
        public void SetSpeed (float speed, float worldTime) {
            this.localRefTime = GetPlaybackPosition (worldTime);
            this.worldRefTime = worldTime;
            this.speed = speed;
        }

        /// <summary>
        /// クリップの長さの変更
        /// </summary>
        /// <remarks>
        /// クリップの長さを変更します。長さは msec 単位で指定します。
        /// クリップの繰り返しはこのクリップ長を元に行われます。
        /// </remarks>
        /// <param name="duration">クリップの長さ</param>
        public void SetDuration (int duration) {
            if (duration <= 0) {
                throw new ArgumentException ("Duration is invalid");
            }
            this.duration = duration;
        }

        /// <summary>
        /// 繰り返しモードの変更
        /// </summary>
        /// <param name="mode">繰り返しモード</param>
        public void SetWrapMode (WrapMode mode) {
            this.wrapMode = mode;
        }

        /// <summary>
        /// 再生位置の取得
        /// </summary>
        /// <remarks>
        /// 指定のワールド時刻 <paramref name="worldTime"/> に再生されるローカル ポジション位置を取得します。
        /// 繰り返しモードが Loop の場合は折りたたまれて [0,<see cref="Duration"/>) の範囲の値が返ります。 
        /// Once の場合は特にそういう制限はありません。
        /// </remarks>
        /// <param name="worldTime">ワールド時刻</param>
        /// <returns></returns>
        public int GetPlaybackPosition (float worldTime) {
            if (duration == 0) {
                throw new InvalidOperationException ("Duraion is 0");
            }

            var pos = (worldTime - worldRefTime) * speed + localRefTime;
            if (wrapMode == WrapMode.Once) {
                return (int)pos;
            }
            else {
                while (pos < 0 || pos >= duration) {
                    if (pos < 0) {
                        pos += duration;
                    }
                    if (pos >= duration) {
                        pos -= duration;
                    }
                }
                return (int)pos;
            }
        }

        #endregion
    }
}
