using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DD {

    /// <summary>
    /// アニメーション トラック クラス
    /// </summary>
    /// <remarks>
    /// アニメーション トラックは1つのプロパティの時系列の値の変化を記録した物です。
    /// ターゲットのプロパティ名と元になるキーフレーム データとその補完方法から構成されます。
    /// キーフレーム データは時間とデータのN個の組み合わせです。
    /// すべてのキーフレームはセットされ時刻は昇順に格納されている必要があります。
    /// </remarks>
    public class AnimationTrack {
        #region Field
        string targetProperty;
        List<Keyframe> frames;
        int compCount;
        Type compType;
        InterpolationType interpType;
        #endregion

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="targetProperty">ターゲット プロパティ名</param>
        /// <param name="interp">補完方法</param>
        public AnimationTrack (string targetProperty,InterpolationType interp) {
            if (targetProperty == null || targetProperty == "") {
                throw new ArgumentNullException ("Target Property is null or empty");
            }

            this.targetProperty = targetProperty;
            this.frames = new List<Keyframe>();
            this.interpType = interp;
            this.compCount = 0;
            this.compType = null;
        }

        #region Property
        /// <summary>
        /// ターゲット プロパティ名
        /// </summary>
        public string TargetProperty {
            get { return targetProperty; }
        }

        /// <summary>
        /// キーフレームの総数
        /// </summary>
        public int KeyframeCount {
            get { return frames.Count(); }
        }

        /// <summary>
        /// コンポーネント数
        /// </summary>
        /// <remarks>
        /// コンポーネント数は <see cref="AddKeyframe"/> が初めて呼ばれた時に設定されます。
        /// それまでは0が返ります。
        /// </remarks>
        public int ComponentCount {
            get { return compCount; }
        }

        /// <summary>
        /// コンポーネント タイプ
        /// </summary>
        /// <remarks>
        /// コンポーネント タイプは <see cref="AddKeyframe"/> が初めて呼ばれた時に設定されます。
        /// それまでは <c>null</c> が返ります。
        /// </remarks>
        public Type ComponentType {
            get { return compType; }
        }

        /// <summary>
        /// すべてのキーフレームを列挙する列挙子
        /// </summary>
        public IEnumerable<Keyframe> Keyframes {
            get { return frames; }
        }

        #endregion

        #region Method
        /// <summary>
        /// キーフレームの設定
        /// </summary>
        /// <remarks>
        /// 指定のキーフレームに（時刻、値）を設定します。
        /// （dynamic宣言なので）値にはすべての型を指定可能ですが、プリミティブ型（Int32, Single, etc.）か、
        /// ComponentCount プロパティとインデクサ-（'[]'）を実装した構造体以外は例外を発生します。
        /// また2回目以降に初回呼び出しと異なる型、コンポーネント数の <paramref name="value"/> を指定する事はできません。
        /// 値は書き換え対象のプロパティの型と同一か、暗黙的に変換できる必要があります。
        /// もしそうでない場合 OnAnimate() の処理の途中でキャスト例外が発生します。
        /// </remarks>
        /// <param name="time">ローカル ポジション（msec）</param>
        /// <param name="value">値</param>
        public void AddKeyframe (int time, dynamic value) {
            if (time < 0) {
                throw new ArgumentException ("Time is invalid");
            }
            var type = (Type)value.GetType ();
            if (!type.IsValueType) {
                throw new ArgumentException ("Type is invalid, type=", type.Name);
            }
            if (!type.IsPrimitive && (type.GetProperty ("ComponentCount") == null || type.GetProperty ("Item") == null)) {
                throw new ArgumentException ("Value don't have ComponentCount property or Indexer[]");
            }
            if (compType != null && type != compType) {
                throw new ArgumentException ("Value type is different from previous one, this=" + compType.Name);
            }
            var count = type.IsPrimitive ? 1 : value.ComponentCount;
            if (compType != null && count != compCount) {
                throw new ArgumentException ("Value length is different from previous one, this=" + compCount);
            }
            if (interpType == InterpolationType.SLerp && type != typeof (Quaternion)) {
                throw new ArgumentException ("SLerp is only for Quaternion value");
            }

            this.compType = type;
            this.compCount = count;
            this.frames.Add (new Keyframe (time, value));
            this.frames = frames.OrderBy (x => x.Time).ToList();
        }

        /// <summary>
        /// キーフレームの取得
        /// </summary>
        /// <param name="index">キーフレーム番号</param>
        /// <returns></returns>
        public Keyframe GetKeyframe (int index) {
            if (index < 0 || index > KeyframeCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return frames[index];
        }

        /// <summary>
        /// キーフレームのサンプリング
        /// </summary>
        /// <remarks>
        /// キーフレームを指定の補完方法で補完して値を返します。
        /// 戻り値は動的（dynamic）として宣言されていて、
        /// 型は <see cref="AddKeyframe"/> でセットした物と同じになります。
        /// </remarks>
        /// <param name="time">時刻</param>
        /// <returns>補完済みの値</returns>
        public dynamic Sample (float time) {
            var left = frames.LastOrDefault (x => x.Time <= time);
            if (left.Equals (default (Keyframe))) {
                return frames[0].Value;
            }
            var right = frames.FirstOrDefault (x => x.Time > time);
            if (right.Equals (default (Keyframe))) {
                return frames[KeyframeCount - 1].Value;
            }

            var a = (time - left.Time) / (right.Time - left.Time);

            switch (interpType) {
                case InterpolationType.Step: return Step (a, left.Value, right.Value);
                case InterpolationType.Linear: return Linear (a, left.Value, right.Value);
                case InterpolationType.SLerp: return SLerp (a, left.Value, right.Value);
                default: throw new NotImplementedException ("Sorry");
            }

        }

        /// <summary>
        /// ステップ補完
        /// </summary>
        /// <param name="a">補完係数 [0,1]</param>
        /// <param name="left">プリミティブ型か構造体</param>
        /// <param name="right">プリミティブ型か構造体</param>
        /// <returns></returns>
        internal dynamic Step (float a, dynamic left, dynamic right) {
            return left;
        }

        /// <summary>
        /// 線形補完
        /// </summary>
        /// <remarks>
        /// <note>
        /// (*1)は float 型だが(*2)はユーザーが指定した任意の型（intあり得る）。
        /// 明示的なキャストが必要！！
        /// </note>
        /// </remarks>
        /// <param name="a">補完係数 [0,1]</param>
        /// <param name="left">プリミティブ型か構造体</param>
        /// <param name="right">プリミティブ型か構造体</param>
        /// <returns></returns>
        internal dynamic Linear (float a, dynamic left, dynamic right) {
            if (compType.IsPrimitive) {
                var value = (left * (1 - a) + right * a);    // (*1)
                return Convert.ChangeType(value, compType);  // (*2)
            }
            else if (compType.IsValueType) {
                var values = (dynamic)Activator.CreateInstance (compType);
                for (var i = 0; i < compCount; i++) {
                    var value = left[i] * (1-a) + right[i] * a;                    // (*1)
                    var indexerType = compType.GetProperty ("Item").PropertyType;
                    values[i] = Convert.ChangeType(value, indexerType);            // (*2)
                }
                return values;
            }
            else {
                throw new InvalidOperationException ("This never happen!");
            }

        }

        /// <summary>
        /// 球面線形補完
        /// </summary>
        /// <remarks>
        /// この補完形式は <see cref="InterpolationType.SLerp"/> 専用です。
        /// それ以外の時は例外を発生します。
        /// </remarks>
        /// <param name="a">補完係数 [0,1]</param>
        /// <param name="left">クォータニオン1</param>
        /// <param name="right">クォータニオン2</param>
        /// <returns></returns>
        internal dynamic SLerp (float a, dynamic left, dynamic right) {
                return Quaternion.Slerp(a, left, right);
        }
        #endregion

    }
}
