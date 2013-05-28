using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace DD {
    /*
    /// <summary>
    /// アニメーション可能 インターフェース
    /// </summary>
    /// <remarks>
    /// このインターフェースを継承する事でそのオブジェクトのプロパティがアニメーション可能になります。
    /// <note>
    /// このインターフェースは空だが外部から拡張メソッドによってアニメーション用のメソッドが追加されている。
    /// いわゆる Mixin と呼ばれるテクニック。
    /// </note>
    /// </remarks>
    public interface IAnimatable {
    }

    /// <summary>
    /// <see cref="IAnimatable"/> クラスに拡張メソッドを追加するクラス
    /// </summary>
    public static class IAnimatableExtensions {
        readonly static ConditionalWeakTable<IAnimatable, List<AnimationClip>> state = new ConditionalWeakTable<IAnimatable, List<AnimationClip>> ();

        /// <summary>
        /// クリップの追加
        /// </summary>
        /// <remarks>
        /// オブジェクトにアニメーション クリップを追加します。
        /// </remarks>
        /// <param name="anim"><see cref="IAnimatable"/> オブジェクト</param>
        /// <param name="clip">追加したいクリップ</param>
        public static void AddClip (this IAnimatable anim, AnimationClip clip) {
            if (clip == null) {
                throw new ArgumentNullException ("Clip is null");
            }
            state.GetOrCreateValue (anim).Add (clip);
        }

        /// <summary>
        /// クリップの削除
        /// </summary>
        /// <param name="anim"><see cref="IAnimatable"/> オブジェクト</param>
        /// <param name="clip">削除したいクリップ</param>
        public static void RemoveClip (this IAnimatable anim, AnimationClip clip) {
            if (clip == null) {
                return;
            }
            state.GetOrCreateValue (anim).Remove (clip);
        }

        /// <summary>
        /// クリップの個数
        /// </summary>
        /// <param name="anim"><see cref="IAnimatable"/> オブジェクト</param>
        /// <returns></returns>
        public static int ClipCount (this IAnimatable anim) {
            return state.GetOrCreateValue (anim).Count ();
        }

        /// <summary>
        /// すべてのクリップを列挙する列挙子
        /// </summary>
        /// <param name="anim"><see cref="IAnimatable"/> オブジェクト</param>
        /// <returns></returns>
        public static IEnumerable<AnimationClip> Clips (this IAnimatable anim) {
            return state.GetOrCreateValue (anim);
        }

        /// <summary>
        /// クリップの取得
        /// </summary>
        /// <param name="anim"><see cref="IAnimatable"/> オブジェクト</param>
        /// <param name="index">クリップ番号</param>
        /// <returns></returns>
        public static AnimationClip GetClip (this IAnimatable anim, int index) {
            if (index < 0 || index > anim.ClipCount () - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return state.GetOrCreateValue (anim).ElementAt (index);
        }

        /// <summary>
        /// アニメーション処理のためのエントリーポイント
        /// </summary>
        /// <param name="anim"><see cref="IAnimatable"/> オブジェクト</param>
        /// <param name="msec">時刻(msec)</param>
        public static void Animate (this IAnimatable anim, long msec) {
            var clips = state.GetOrCreateValue (anim);
            foreach (var clip in clips) {
            }
        }
    }
     * */
}
