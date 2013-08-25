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
//using SFML.Graphics;
using SFML.Window;


namespace DD {
    /// <summary>
    /// スフィア コリジョン形状 コンポーネント
    /// </summary>
    /// <remarks>
    /// 球の中心がローカル座標の原点に一致する球形のコリジョン形状を定義します。
    /// 中心位置は後から変更可能です。
    /// </remarks>
    public class SphereCollision : Collision {
        #region Field
        float radius;
        int vertexCount;
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
        public SphereCollision (float radius): base(ShapeType.Sphere) {
            if (radius < 0) {
                throw new ArgumentException ("Raidus is invalid");
            }
            this.radius = radius;
            this.vertexCount = 8;
        }
        #endregion

        #region Property
        /// <summary>
        /// 球の半径
        /// </summary>
        /// <remarks>
        /// 球の半径です。
        /// </remarks>
        public float Radius {
            get { return radius; }
        }

        /// <summary>
        /// 球の直径
        /// </summary>
        /// <remarks>
        /// 球の直径です。
        /// </remarks>
        public float Diameter {
            get { return radius * 2; }
        }

        public override int VertexCount {
            get {
                return vertexCount;
            }
            set {
                if (value < 4) {
                    throw new ArgumentException ("VertexCount must be greater than 4");
                }
                this.vertexCount = value;
            }
        }

        public override IEnumerable<Vector3> Vertices {
            get {
                var vertices = new Vector3[vertexCount];
                for(var i=0; i < vertexCount; i++){
                    vertices[i] = Offset + new Vector3 (radius * (float)(Math.Cos ((i / (float)vertexCount) * Math.PI * 2)),
                                                        radius * (float)(Math.Sin ((i / (float)vertexCount) * Math.PI * 2)),
                                                        0);

                }
                return vertices;
            }
        }

        #endregion

        #region Method

        /// <inheritdoc/>
        public override bool Contain (float x, float y, float z) {
            var p = new Vector3 (x, y, z) - Offset;
            if (p.Length <= radius) {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        internal override Shape CreateShapeBody (float ppm) {
            if (ppm <= 0) {
                throw new ArgumentException ("PPM is invalid");
            }
            var sph = new CircleShape (radius / ppm, 1000);
            sph.Position = new XnaVector2 (Offset.X, Offset.Y) / ppm;
            return sph;
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
            spr.TextureRect = new SFML.Graphics.IntRect (0, 0, (int)radius*2, (int)radius*2);

            spr.Position = new Vector2f (T.X, T.Y);
            spr.Scale = new Vector2f (S.X, S.Y);
            spr.Rotation = angle;
            spr.Origin = new Vector2f (radius, radius);

            spr.Color = new Color (255, 255, 255, (byte)(127 * opacity)).ToSFML ();

            var win = window as SFML.Graphics.RenderWindow;
            win.Draw (spr);
        }
       #endregion
    }
}
