using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace DD.Physics {
    /// <summary>
    /// コリジョン形状 コンポーネント
    /// </summary>
    /// <remarks>
    /// コリジョン検出に使用する物体形状を定義する抽象クラスです。
    /// 形状には <see cref="BoxCollisionShape"/>, <see cref="SphereCollisionShape"/> があります。
    /// </remarks>
    public abstract class CollisionShape : Component {

        #region Field
        ShapeType type;
        Vector3 offset;
        #endregion

        /// <summary>
        /// コンストラクター
        /// </summary>
        public CollisionShape (ShapeType type) {
            this.type = type;
            this.offset = new Vector3 (0, 0, 0);
        }

        /// <summary>
        /// 形状タイプ
        /// </summary>
        public ShapeType Type {
            get { return type; }
        }

        public bool IsPolygon {
            get { return type == ShapeType.Polygon; }
        }

        public bool IsCircle {
            get { return type == ShapeType.Sphere; }
        }

        /// <summary>
        /// オフセット
        /// </summary>
        public Vector3 Offset {
            get { return offset; }
            set { SetOffset (value.X, value.Y, value.Z); }
        }

        /// <summary>
        /// オフセットの設定
        /// </summary>
        /// <param name="x">X方向のオフセット</param>
        /// <param name="y">Y方向のオフセット</param>
        /// <param name="z">Z方向のオフセット</param>
        public void SetOffset (float x, float y, float z) {
            this.offset = new Vector3 (x, y, z);
        }

        /// <summary>
        /// 物理ボディの作成
        /// </summary>
        /// <remarks>
        /// 物理エンジン側に演算で使用するボディを作成します。
        /// 引数の <paramref name="ppm"/> は物理エンジンの 1m が何ピクセルに相当するかを指定します。
        /// 密度は固定で [1000/m^3] （=水）です。
        /// </remarks>
        /// <param name="ppm">1mあたりのピクセル数 （Pixel Per Meter）</param>
        /// <returns>Farseerシェイプ</returns>
        internal abstract Shape CreateShapeBody (float ppm);
    }
}
