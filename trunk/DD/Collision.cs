using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace DD {
    /// <summary>
    /// コリジョン形状 コンポーネント
    /// </summary>
    /// <remarks>
    /// コリジョン検出に使用する物体形状を定義する抽象クラスです。
    /// 形状には <see cref="BoxCollision"/>, <see cref="SphereCollision"/> があります。
    /// </remarks>
    public abstract class Collision : Component {

        #region Field
        ShapeType type;
        Vector3 offset;
        bool draw;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Collision (ShapeType type) {
            this.type = type;
            this.offset = new Vector3 (0, 0, 0);
            this.draw = false;
        }
        #endregion


        #region Property
        /// <summary>
        /// 形状タイプ
        /// </summary>
        public ShapeType Type {
            get { return type; }
        }

        /// <summary>
        /// 頂点の個数
        /// </summary>
        /// <remarks>
        /// このコリジョンの頂点数。
        /// 球に限りセッターで頂点数を変更可能です。
        /// </remarks>
        public abstract int VertexCount{
            get;
            set;
        }

        /// <summary>
        /// すべての頂点を列挙する列挙子（ローカル座標）
        /// </summary>
        public abstract IEnumerable<Vector3> Vertices {
            get;
        }

        /// <summary>
        /// ポリゴン形状
        /// </summary>
        /// <remarks>
        /// ポリゴン（BoxとRhombus）が相当します。
        /// </remarks>
        public bool IsPolygon {
            get { return type == ShapeType.Polygon; }
        }

        /// <summary>
        /// 円形状
        /// </summary>
        public bool IsCircle {
            get { return type == ShapeType.Sphere; }
        }

        /// <summary>
        /// コリジョン形状の描画フラグ
        /// </summary>
        /// <remarks>
        /// このプロパティを <c>true</c> に変更するとコリジョン形状を描画します。
        /// デバッグ以外の目的で使用しないで下さい。
        /// </remarks>
        public bool DrawEnabled {
            get { return draw; }
            set { this.draw = value; }
        }

        /// <summary>
        /// オフセット
        /// </summary>
        public Vector3 Offset {
            get { return offset; }
            set { SetOffset (value.X, value.Y, value.Z); }
        }
        #endregion

        #region Method
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
        /// コリジョン形状が点Pを含むかどうかの判定
        /// </summary>
        /// <param name="x">点のX座標（ローカル座標系）</param>
        /// <param name="y">点のY座標（ローカル座標系）</param>
        /// <returns></returns>
        public abstract bool Contain (float x, float y, float z);

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
        #endregion
    }
}
