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
using SFML.Window;

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
        public BoxCollisionShape (float halfWidth, float halfHeight, float halfDepth) : base(ShapeType.Polygon) {
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

        public override void OnDraw (object window) {
            if (!DrawEnabled) {
                return;
            }

            Vector3 T;
            Quaternion R;
            Vector3 S;
            Node.GlobalTransform.Decompress (out T, out R, out S);

            // クォータニオンは指定したのと等価な軸が反対で回転角度[0,180]の回転で返ってくる事がある
            // ここで回転軸(0,0,-1)のものを(0,0,1)に変換する必要がある
            var angle = R.Angle;
            var axis = R.Axis;
            var dot = Vector3.Dot (axis, new Vector3 (0, 0, 1));
            if (dot < 0) {
                angle = 360 - angle;
                axis = -axis;
            }

            var opacity = Node.Upwards.Aggregate (1.0f, (x, node) => x * node.Opacity);

            var spr = new SFML.Graphics.Sprite ();
            spr.Texture = Resource.GetDefaultTexture ().Data;
            spr.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)Width, (int)Height);

            spr.Position = new Vector2f (T.X, T.Y);
            spr.Scale = new Vector2f (S.X, S.Y);
            spr.Rotation = angle;
            spr.Origin = new Vector2f (halfWidth, halfHeight);

            spr.Color = new Color (255, 255, 255, (byte)(127 * opacity)).ToSFML ();

            var win = window as SFML.Graphics.RenderWindow;
            win.Draw (spr);
        }

        #endregion

    }
}
