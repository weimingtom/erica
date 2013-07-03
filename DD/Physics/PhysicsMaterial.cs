using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Physics {
    /// <summary>
    /// 物理特性 マテリアル
    /// </summary>
    /// <remarks>
    /// 物理特性を保存するマテリアル クラス。
    /// このマテリアルを <see cref="PhysicsBody"/> にセットすると、
    /// 物理オブジェクトとして計算の対象になります。
    /// </remarks>
    public class PhysicsMaterial : Material {
        #region Field
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public PhysicsMaterial () {
            this.Density = 1000;
            this.Friction = 0.4f;
            this.Restitution = 1.0f;
            this.LinearDamping = 0;
            this.AngulerDamping = 0;
        }
        #endregion

        #region Property
        /// <summary>
        /// 質量密度
        /// </summary>
        /// <remarks>
        /// 現在は 1000 (Kg/m^3) に固定です（=水）。
        /// </remarks>
        public float Density { get; set; }

        /// <summary>
        /// 摩擦係数
        /// </summary>
        /// <remarks>
        /// [0,1]です。
        /// </remarks>
        public float Friction { get; set; }

        /// <summary>
        /// 反発係数
        /// </summary>
        /// <remarks>
        /// [0,1]です。
        /// </remarks>
        public float Restitution { get; set; }

        /// <summary>
        /// 速度減衰定数
        /// </summary>
        /// <remarks>
        /// [0,1]です。
        /// </remarks>

        public float LinearDamping { get; set; }


        /// <summary>
        /// 角速度減衰定数
        /// </summary>
        /// <remarks>
        /// [0,1]です。
        /// </remarks>
        public float AngulerDamping { get; set; }
        
        #endregion

    }
}
