using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;

namespace DD {
    /// <summary>
    /// 箱形状 クラス
    /// </summary>
    /// <remarks>
    /// 縦、横、奥行きを持った箱状の形状を定義します。
    /// </remarks>
    public class BoxShape : CollisionShape {

        #region Field
        float halfWidth;
        float halfHeight;
        float halfDepth;
        #endregion

        #region Constructor
        /// <summary>
        /// ボックス形状を作成するコンストラクター
        /// </summary>
        /// <remarks>
        /// いずれか一辺の長さが 0 以下のボックスは作れません。
        /// 2Dのボックスを作成するには Box2DShape を使用して下さい。
        /// </remarks>
        /// <param name="halfWidth">横幅の1/2</param>
        /// <param name="halfHeight">縦幅の1/2</param>
        /// <param name="halfDepth">奥行きの1/2</param>
        public BoxShape (float halfWidth, float halfHeight, float halfDepth) {
            if (halfWidth <= 0 || halfHeight <= 0 || halfDepth <= 0) {
                throw new ArgumentException ("Box size is invalid");
            }
            this.halfWidth = halfWidth;
            this.halfHeight = halfHeight;
            this.halfDepth = halfDepth;
        }
        #endregion

        #region Property
        /// <summary>
        /// ボックスの横幅
        /// </summary>
        public float Width {
            get { return halfWidth * 2; }
        }

        /// <summary>
        /// ボックスの縦幅
        /// </summary>
        public float Height {
            get { return halfHeight * 2; }
        }

        /// <summary>
        /// ボックスの奥行き
        /// </summary>
        public float Depth {
            get { return halfDepth * 2; }
        }
        #endregion


        #region Method
        ///<inheritdoc/>
        public override BulletSharp.PairCachingGhostObject CreateGhostObject () {
            var col = new PairCachingGhostObject ();
            col.CollisionShape = new BulletSharp.BoxShape (halfWidth, halfHeight, halfDepth);
            col.CollisionFlags |= CollisionFlags.NoContactResponse;
            return col;
        }

        /// <inheritdoc/>
        public override BulletSharp.RigidBody CreateRigidBody (float mass) {
            
            var ppm = DD.Physics.PhysicsSimulator.PPM;

            var mstate = new DefaultMotionState ();
            var shape = new BulletSharp.BoxShape (halfWidth/ppm, halfHeight/ppm, halfDepth/ppm);
            var info = new BulletSharp.RigidBodyConstructionInfo (mass, mstate, shape);

            var body = new BulletSharp.RigidBody (info);

            return body;
        }



        /// <inheritdoc/>
        public override string ToString () {
            return String.Format ("Box: {0}, {1}, {2}", halfWidth, halfHeight, halfDepth);
        }
        #endregion


    }
}
