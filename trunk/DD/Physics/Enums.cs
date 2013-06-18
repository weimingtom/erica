using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Physics {
    /// <summary>
    /// コリジョン タイプ
    /// </summary>
    /// <remarks>
    /// コリジョンタイプを3種類から選択します。
    /// </remarks>
        public enum ColliderType {

            /// <summary>
            /// ダイナミック
            /// </summary>
            /// <remarks>
            /// オブジェクトは物理エンジンによってすべて制御され動き回ります。
            /// ユーザーが明示的に動かす事も不可能ではないですが普通はしません。
            /// 他のダイナミック、スタティック、キネマティックな物体と衝突します。
            /// </remarks>
            Dynamic,

            /// <summary>
            /// スタティック
            /// </summary>
            /// <remarks>
            /// オブジェクトは力や衝突に影響されず常に静止しています。ユーザーが明示的に位置を変更する事が可能です。
            /// 他のダイナミック、スタティック、キネマティックな物体と衝突します（はじき飛ばします）。
            /// </remarks>
            Static,

            /// <summary>
            /// キネマティック
            /// </summary>
            /// <remarks>
            /// オブジェクトはスタティックと同様に力や衝突に影響されず静止していますが、
            /// ユーザーが速度を与える事でダイナミックと同じように動かす事が可能です。
            /// 他のダイナミック、スタティック、キネマティックな物体と衝突します（はじき飛ばします）。
            /// </remarks>
            Kinematic,
        }

}
