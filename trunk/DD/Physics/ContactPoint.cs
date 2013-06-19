using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Physics {

    /// <summary>
    /// コリジョン構造体
    /// </summary>
    /// <remarks>
    /// 衝突地点の情報をワールド座標系で保持します。
    /// </remarks>
    public struct Collision {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// メモ：法線はここで正規化した方が良い？
        /// 現状では何もしていない。
        /// </remarks>
        /// <param name="collidee">衝突した相手（<see cref="Collider"/> オブジェクト）</param>
        /// <param name="point">衝突地点（ローカル座標）</param>
        /// <param name="normal">法線（ローカル座標）</param>
        internal Collision (Collider collidee, Vector3 point, Vector3 normal)
            : this () {
            this.Normal = normal;
            this.Point = point;
            this.Collidee = collidee;
        }

        /// <summary>
        /// 衝突した相手
        /// </summary>
        /// <remarks>
        /// 衝突の原因になった相手コライダー <see cref="Collider"/> です。
        /// </remarks>
        public Collider Collidee { get; private set; }

        /// <summary>
        /// 衝突地点
        /// </summary>
        /// <remarks>
        /// 衝突地点（ワールド座標）を表すプロパティです。
        /// 衝突地点はコリジョン形状の外皮からわずかに進入した地点になりますが、
        /// 厳密には決められていません。
        /// </remarks>
        public Vector3 Point { get; private set; }

        /// <summary>
        /// 衝突地点の法線
        /// </summary>
        /// <remarks>
        /// 衝突地点の法線（ワールド座標）を表すプロパティです。
        /// 法線は正規化され必ず外から自分に向かう方向を向いています。
        /// </remarks>
        public Vector3 Normal { get; private set; }
    }
}
