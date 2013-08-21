using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {


    /// <summary>
    /// アニメーション クリップ クラス
    /// </summary>
    /// <remarks>
    /// アニメーション クリップは“走る”、“歩く”などの一連の動作を表すアニメーションの基本単位です。
    /// 原則としてアニメーションに対する操作はこのクリップ単位で行います。
    /// 書き換え対象のオブジェクトは”弱い参照”によって参照されます。従ってここ以外の参照がすべて無くなるとGCの対象になります。
    /// 対象が削除済みのオブジェクトの場合はクリップは何もせず無視します（エラーにはなりません）。
    /// クリップ自体はそのまま残ります。
    /// <note>
    /// ターゲット オブジェクトが解放済みかどうか簡単に調べられるメソッドが必要なような、別にいらないような。
    /// 今でも GetTrack() で弱い参照込みで取得して調べられるが・・・
    /// </note>
    /// </remarks>
    public class AnimationClip {
        /// <summary>
        /// 再生状態
        /// </summary>
        public enum State {
            /// <summary>
            /// 再生中
            /// </summary>
            Playing,
            /// <summary>
            /// 停止中
            /// </summary>
            Stopped
        }

        #region Field
        State state;
        string name;
        int duration;
        int localRefTime;
        float worldRefTime;
        float speed;
        WrapMode wrapMode;
        List<Tuple<WeakReference, AnimationTrack>> tracks;
        List<AnimationEvent> events;
        #endregion

        /// <summary>
        /// イベント数
        /// </summary>
        /// <remarks>
        /// このクリップに設定されているイベント数
        /// </remarks>
        public int EventCount {
            get { return events.Count (); }
        }

        /// <summary>
        /// すべてのイベントを列強する列挙子
        /// </summary>
        public IEnumerable<AnimationEvent> Events {
            get { return events; }
        }

        /// <summary>
        /// イベントの取得
        /// </summary>
        /// <param name="index">イベント インデックス</param>
        /// <returns></returns>
        public AnimationEvent GetEvent (int index) {
            if (index < 0 || index > EventCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return events[index];
        }

        /// <summary>
        /// イベントの追加
        /// </summary>
        /// <param name="position">ローカルポジション (msec)</param>
        /// <param name="handler">イベントで呼び出される関数</param>
        /// <param name="args">関数に渡される引数</param>
        public void AddEvent (int position, AnimationEventHandler handler, EventArgs args) {
            if (position < 0 || position > duration) {
                throw new ArgumentException ("Position is invalid");
            }
            if (handler == null) {
                throw new ArgumentNullException ("Handler is null");
            }
            
            this.events.Add (new AnimationEvent (position, handler, args));
            events.Sort ();
        }

        /// <summary>
        /// イベントの削除
        /// </summary>
        /// <param name="index">インデックス</param>
        public void RemoveEvent (int index) {
            if (index < 0 || index > EventCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            this.events.RemoveAt (index);
        }

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// <paramref name="name"/> はこのクリップを識別しやすいようにユーザーが自由に決められる名前です。
        /// エンジン側では使用しません。
        /// </remarks>
        /// <param name="duration">クリップの長さ（msec）</param>
        /// <param name="name">このクリップの名前</param>
        public AnimationClip (int duration, string name) {
            this.name = name ?? "";
            this.duration = duration;
            this.localRefTime = 0;
            this.worldRefTime = 0;
            this.speed = 1;
            this.wrapMode = WrapMode.Loop;
            this.state = State.Stopped;
            this.tracks = new List<Tuple<WeakReference, AnimationTrack>> ();
            this.events = new List<AnimationEvent>();
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="duration">クリップの長さ（msec）</param>
        public AnimationClip (int duration)
            : this (duration, "") {
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
            set {
                if (value <= 0) {
                    throw new ArgumentException ("Duration is invalid");
                }
                this.duration = value;
            }
        }

        /// <summary>
        /// 繰り返しモード
        /// </summary>
        public WrapMode WrapMode {
            get { return wrapMode; }
            set { this.wrapMode = value; }
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
        /// 再生中フラグ
        /// </summary>
        public bool IsPlaying {
            get { return state == State.Playing; }
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
        public IEnumerable<Tuple<WeakReference, AnimationTrack>> Tracks {
            get { return tracks; }
        }
        #endregion

        #region Method

        /// <summary>
        /// 再生の開始
        /// </summary>
        /// <remarks>
        /// このクリップを再生します。すでに再生中のクリップを再度再生した場合、単にこれを無視します。
        /// </remarks>
        public  void Play () {
            this.state = State.Playing;
        }

        /// <summary>
        /// 再生の停止
        /// </summary>
        /// <remarks>
        /// このクリップの再生を停止します。すでに停止中のクリップを再度停止した場合、単にこれを無視します。
        /// </remarks>
        public  void Stop () {
            this.state = State.Stopped;
        }

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
            this.tracks.Add (Tuple.Create (new WeakReference (target), track));
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
        public Tuple<WeakReference, AnimationTrack> GetTrack (int index) {
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

        /// <inheritdoc/>
        public override string ToString () {
            return Name;
        }

        #endregion
    }
}
