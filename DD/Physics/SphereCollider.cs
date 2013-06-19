using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace DD.Physics {
    /// <summary>
    /// スフィア コリジョン形状 クラス
    /// </summary>
    /// <remarks>
    /// ローカル座標の原点に球の中心が来るように球形のコリジョン形状を定義します。
    /// </remarks>
    public class SphereCollider : CollisionShape {
        #region Field
        float radius;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// 指定のサイズの箱形を原点を中心に作成します。
        /// 引数の <paramref name="radius"/> は半径（中心から外周まで）をピクセル数で指定します。
        /// </remarks>
        /// <param name="radius">半径（ピクセル数）</param>
        public SphereCollider (float radius) {
            if (radius < 0) {
                throw new ArgumentException ("Raidus is invalid");
            }
            this.radius = radius;
        }
        #endregion

        #region Property
        /// <summary>
        /// 球の半径
        /// </summary>
        /// <remarks>
        /// 球の半径をピクセル数で返します。
        /// </remarks>
        public float Radius {
            get { return radius; }
        }

        #endregion

        #region Method
        /// <inheritdoc/>
        internal override Shape CreateShape () {
            var phy = Physics2D.GetInstance ();
            return new CircleShape (radius/phy.PPM, 1000);
       }
        #endregion
    }
}
