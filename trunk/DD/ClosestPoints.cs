using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// 最近説距離情報 構造体
    /// </summary>
    /// <remarks>
    /// Distance メソッドを使って求める2物体の最近説距離情報を保存します。
    /// </remarks>
    public struct ClosestPoints {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="pointA">ポイントA</param>
        /// <param name="pointB">ポイントB</param>
        public ClosestPoints (Vector3 pointA, Vector3 pointB)
            : this () {
                this.PointA = pointA;
                this.PointB = pointB;
        }
        /// <summary>
        /// ポイントA
        /// </summary>
        public Vector3 PointA {get; private set;}

        /// <summary>
        /// ポイントB
        /// </summary>
        public Vector3 PointB { get; private set; }

        /// <summary>
        /// ポイントAからBへのベクトル
        /// </summary>
        public Vector3 VectorAtoB { get { return PointB - PointA;  } }
    }
}
