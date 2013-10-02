using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {

        /// <summary>
        /// レイキャストの結果を受け取る構造体
        /// </summary>
        /// <remarks>
        /// レイキャストの結果を戻り値で受け取るための構造体です。
        /// 必ず戻り値として出現され、ユーザーが自分でインスタンス化することはありません。
        /// ヒットフラグが <c>false</c> の時、それ以外のプロパティは未定義です。
        /// </remarks>
        public struct RaycastResult {
            internal RaycastResult (float frac, float dist, Node node, Vector3 point, Vector3 normal) : this() {
                this.Hit = true;
                this.Fraction = frac;
                this.Distance = dist;
                this.Node = node;
                this.Point = point;
                this.Normal = normal;
            }

            /// <summary>
            /// ヒット フラグ
            /// </summary>
            /// <remarks>
            /// 何らかのコリジョン形状にヒットした場合 <c>true</c> が返ります。
            /// </remarks>
            public bool Hit { get; private set; }

            /// <summary>
            /// ヒット地点までの割合
            /// </summary>
            /// <remarks>
            /// レイキャストの開始地点から終了地点までの距離を1として、
            /// 開始地点からヒット地点までの距離を割合で表します [0,1]。
            /// </remarks>
            public float Fraction { get; private set; }

            /// <summary>
            /// ヒット地点までの距離
            /// </summary>
            /// <remarks>
            /// レイキャストの開始地点からヒット地点までの距離。
            /// </remarks>
            public float Distance { get; private set; }

            /// <summary>
            /// ヒットしたノード
            /// </summary>
            /// <remarks>
            /// ヒットしたコリジョン形状を持つノード。
            /// </remarks>
            public Node Node {get; private set; }

            /// <summary>
            /// ヒット地点（ワールド座標）
            /// </summary>
            public Vector3 Point { get; private set; }

            /// <summary>
            /// ヒット地点の法線（ワールド座標）
            /// </summary>
            /// <remarks>
            /// 必ず正規化されています。
            /// </remarks>
            public Vector3 Normal { get; private set; }
        }

}
