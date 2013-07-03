using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using XnaVector2 = Microsoft.Xna.Framework.Vector2;


namespace DD.Physics {
    /// <summary>
    /// スフィア コリジョン形状 コンポーネント
    /// </summary>
    /// <remarks>
    /// 球の中心がローカル座標の原点に一致する球形のコリジョン形状を定義します。
    /// 中心位置は後から変更可能です。
    /// </remarks>
    public class SphereCollisionShape : CollisionShape {
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
        public SphereCollisionShape (float radius): base(ShapeType.Sphere) {
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
        internal override Shape CreateShapeBody (float ppm) {
            if (ppm <= 0) {
                throw new ArgumentException ("PPM is invalid");
            }
            var sph = new CircleShape (radius / ppm, 1000);
            sph.Position = new XnaVector2 (Offset.X, Offset.Y) / ppm;
            return sph;
       }
        #endregion
    }
}
