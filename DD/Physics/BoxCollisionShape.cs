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
    /// ボックス コリジョン形状 クラス
    /// </summary>
    /// <remarks>
    /// ローカル座標の原点に箱の中心が来るように箱形のコリジョン形状を定義します。
    /// </remarks>
    public class BoxCollisionShape : CollisionShape {
        #region Field
        float halfWidth;
        float halfHeight;
        float halfDepth;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// 指定のサイズの箱形を原点を中心に作成します。
        /// 引数の<paramref name="halfWidth"/>, <paramref name="halfHeight"/>, <paramref name="halfDepth"/> は
        /// いずれも中心から端までの長さ（端から端までの1/2）をピクセル数で指定します。
        /// 
        /// </remarks>
        /// <param name="halfWidth">幅の半分（ピクセル数）</param>
        /// <param name="halfHeight">高さの半分（ピクセル数）</param>
        /// <param name="halfDepth">奥行きの半分（ピクセル数）</param>
        public BoxCollisionShape (float halfWidth, int halfHeight, int halfDepth) : base(ShapeType.Box) {
            if (halfWidth < 0 || halfHeight < 0 || halfDepth < 0) {
                throw new ArgumentException ("Size is invalid");
            }
            this.halfWidth = halfWidth;
            this.halfHeight = halfHeight;
            this.halfDepth = halfDepth;
        }
        #endregion

        #region Property
        /// <summary>
        /// 幅（ピクセル数）
        /// </summary>
        public float Width {
            get { return halfWidth*2; }
        }

        /// <summary>
        /// 高さ（ピクセル数）
        /// </summary>
        public float Height {
            get { return halfHeight * 2; }
        }

        /// <summary>
        /// 奥行き（ピクセル数）
        /// </summary>
        public float Depth {
            get { return halfDepth * 2; }
        }

        #endregion

        #region Method


        /// <inheritdoc/>
        internal override Shape CreateShapeBody (float ppm) {
            var hw = halfWidth / ppm;
            var hh = halfHeight/ ppm;
            var center = new XnaVector2 (Offset.X, Offset.Y) / ppm;
            return new PolygonShape (PolygonTools.CreateRectangle (hw, hh, center, 0), 1000);
        }
        #endregion

    }
}
