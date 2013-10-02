using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Physics {
    /// <summary>
    /// 物理マテリアル クラス
    /// </summary>
    /// <remarks>
    /// 物理演算で使用するパラメーターを定義するマテリアル クラスです。
    /// 
    /// </remarks>
    public class PhysicsMaterial : Material {
        #region Field
        float restitution;      // 反発係数 [0,1]
        float linearDamping;    // 減衰係数 [0,1]  0:減衰無し 1:減衰100%
        float angularDamping;   // 転がり減衰係数 [0,1]
        float friction;         // 摩擦係数 [0,1]
        float rollingFriction;  // 転がり摩擦係数 [0,1]
        #endregion

        /// <summary>
        /// コンストラクター
        /// </summary>
        public PhysicsMaterial () {
            this.restitution = 1;
            this.linearDamping = 0;
            this.angularDamping = 0;
            this.friction = 0.5f;
            this.rollingFriction = 0f;
        }

        /// <summary>
        /// 反発係数
        /// </summary>
        /// <remarks>
        /// 衝突したエネルギーの何パーセントを反射するかの係数です。
        /// 0以上の任意の値ですが典型的には [0,1] の値をとります。
        /// 初期値は 1 です。
        /// </remarks>
        public float Restitution {
            get { return restitution; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("Restituion is invalied");
                }
                this.restitution = value;
            }
        }

        /// <summary>
        /// 線形減衰係数
        /// </summary>
        /// <remarks>
        /// 物体が移動する時にかかる減衰量をあらわす係数です。
        /// 0以上の任意の値ですが典型的には [0,1] の値をとります。
        /// 0のとき減衰無しで1のとき100％減衰です。
        /// 初期値は 0 です。
        /// </remarks>
        public float LinearDamping {
            get { return linearDamping; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("LinearDamping is invalied");
                }
                this.linearDamping = value;
            }
        }

        /// <summary>
        /// 角度減衰係数
        /// </summary>
        /// <remarks>
        /// 物体の回転にかかる原水量をあらわす係数です。
        /// 0以上の任意の値ですが典型的には [0,1] の値をとります。
        /// 0のとき減衰無しで1のとき100％減衰です。
        /// 初期値は 0 です。
        /// </remarks>
        public float AngularDamping {
            get { return angularDamping; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("AngularDamping is invalied");
                }
                this.angularDamping = value;
            }
        }

        /// <summary>
        /// 摩擦係数
        /// </summary>
        /// <remarks>
        /// 物体の移動時に接触している物体からの受ける力をあらわす係数です。
        /// 0以上の任意の値ですが典型的には [0,1] の値をとります。
        /// 0のとき摩擦力無しで、1のとき摩擦力最大で100％減衰です。
        /// なおこの係数は静止摩擦力と動摩擦力の両方で共通で使用します。
        /// 従って両者を個別に設定することはできません。
        /// 初期値は 0.5 です。
        /// </remarks>
        public float Friction {
            get { return friction; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("Friction is invalied");
                }
                this.friction = value;
            }
        }

        /// <summary>
        /// 転がり摩擦係数
        /// </summary>
        /// <remarks>
        /// 物体の回転時に接触している物体から（回転を減らす方向に）受ける力をあらわす係数です。
        /// 0以上の任意の値ですが典型的には [0,1] の値をとります。
        /// 0のとき摩擦力無しで、1のとき摩擦力最大で100％減衰です。
        /// 初期値は 0 です。
        /// </remarks>
           public float RollingFriction {
            get { return rollingFriction; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("RollingFriction is invalied");
                }
                this.rollingFriction = value;
            }
        }

    }
}
