using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;

namespace DD {
    /// <summary>
    /// カプセル形状クラス
    /// </summary>
    /// <remarks>
    /// カプセル形状を定義します。カプセルとは上下に球を持った円柱です。
    /// 主に（人型の）キャラクターの形状を近似するのに使用します。
    /// </remarks>
   public class CapsuleShape : CollisionShape {
       #region Field
       float radius;
       float height;
       #endregion

       #region Constructor
       /// <summary>
       /// カプセル形状を作成するコンストラクター
       /// </summary>
       /// <remarks>
       /// カプセルは上下が球で終端された円柱です。主に（人型の）キャラクターのおおよその形を近似するのに使用します。
       /// 高さは上下の球の中心から中心までの距離です。
       /// </remarks>
       /// <param name="radius">半径</param>
       /// <param name="height">高さ</param>
       public CapsuleShape (float radius, float height) {
           if (radius <= 0 || height <=0) {
               throw new ArgumentException ("Capsule size is invalid");
           }
           this.radius = radius;
           this.height = height;
       }
       #endregion

       #region Property
       /// <summary>
       /// カプセルの球の半径
       /// </summary>
       public float Radius {
           get { return radius; }
       }

       /// <summary>
       /// カプセルの高さ
       /// </summary>
       /// <remarks>
       /// カプセルの高さは球の中心から中心までの距離です。
       /// </remarks>
       public float Height {
           get { return height; }
       }
       #endregion

       /// <inheritdoc/>
       public override BulletSharp.PairCachingGhostObject CreateGhostObject () {
           var col = new BulletSharp.PairCachingGhostObject ();
           col.CollisionShape = new BulletSharp.CapsuleShape (radius, height);
           col.CollisionFlags |= CollisionFlags.NoContactResponse;
           return col;
       }

       /// <inheritdoc/>
       public override BulletSharp.RigidBody CreateRigidBody (float mass) {
           var ppm = DD.Physics.PhysicsSimulator.PPM;
           
           var mstate = new DefaultMotionState ();
           var shape = new BulletSharp.CapsuleShape (radius/ppm, height/ppm);
           var info = new BulletSharp.RigidBodyConstructionInfo (mass, mstate, shape);

           return new BulletSharp.RigidBody (info);
       }

       /// <inheritdoc/>
       public override string ToString () {
           return string.Format("Capsule : {0}, {1}", radius, height);
       }
   }
}
