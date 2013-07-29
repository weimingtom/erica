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
    public class RhombusCollisionShape : CollisionShape {

        #region Field
        float halfWidth;
        float halfHeight;
        #endregion

        #region Constructor
        public RhombusCollisionShape (float halfWidth, float halfHeight) : base(ShapeType.Polygon) {
            if (halfWidth < 0 || halfHeight < 0) {
                throw new ArgumentException ("Rhombus parameter is invalid");
            }
            this.halfWidth = halfWidth;
            this.halfHeight = halfHeight;
        }
        #endregion

        #region Property
        public float Width {
            get { return halfWidth * 2; }
        }

        public float Height {
            get { return halfHeight * 2; }
        }
        #endregion

        #region Method
        /// <inheritdoc/>
        internal override Shape CreateShapeBody (float ppm) {
            var hx = halfWidth / ppm;
            var hy = halfHeight / ppm;
            var center = new XnaVector2 (Offset.X, Offset.Y) / ppm;
            
            Vertices vertices = new Vertices (4);
            vertices.Add (new XnaVector2 (center.X, center.Y - hy));
            vertices.Add (new XnaVector2 (center.X + hx, center.Y));
            vertices.Add (new XnaVector2 (center.X, center.Y + hy));
            vertices.Add (new XnaVector2 (center.X - hx, center.Y));

            return new PolygonShape (vertices, 1000);
        }
        
        #endregion

    }
}
