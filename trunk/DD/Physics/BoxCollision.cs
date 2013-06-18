using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace DD.Physics {
    /// <summary>
    /// ボックス コリジョン形状 クラス
    /// </summary>
    /// <remarks>
    /// ローカル座標の原点に箱の中心が来るように箱形のコリジョン形状を定義します。
    /// </remarks>
    public class BoxCollision : CollisionShape {
        #region Field
        float width;
        float height;
        float depth;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// 指定のサイズの箱形を原点を中心に作成します。
        /// 引数の<paramref name="width"/>, <paramref name="height"/>, <paramref name="depth"/> は
        /// いずれも端から端までの長さをピクセル数で指定します。
        /// </remarks>
        /// <param name="width">幅（ピクセル数）</param>
        /// <param name="height">高さ（ピクセル数）</param>
        /// <param name="depth">奥行き（ピクセル数）</param>
        public BoxCollision (float width, int height, int depth) {
            if (width < 0 || height < 0 || depth < 0) {
                throw new ArgumentException ("Size is invalid");
            }
            this.width = width;
            this.height = height;
            this.depth = depth;
        }
        #endregion

        #region Property
        /// <summary>
        /// 幅（ピクセル数）
        /// </summary>
        public float Width {
            get { return width; }
        }

        /// <summary>
        /// 高さ（ピクセル数）
        /// </summary>
        public float Height {
            get { return height; }
        }

        /// <summary>
        /// 奥行き（ピクセル数）
        /// </summary>
        public float Depth {
            get { return depth; }
        }
        #endregion

        #region Method
        /// <inheritdoc/>
        internal override Body CreateBody () {
            var phy = Physics2D.GetInstance ();
            var wld = (FarseerPhysics.Dynamics.World)phy.GetWorld ();

            return BodyFactory.CreateRectangle(wld, width/phy.PPM, height/phy.PPM, 1000);
        }
        #endregion

    }
}
