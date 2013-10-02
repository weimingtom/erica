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
       float halfHeight;
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
       /// <param name="halfHeight">高さ</param>
       public CapsuleShape (float radius, float halfHeight) {
           if (radius <= 0 || halfHeight <=0) {
               throw new ArgumentException ("Capsule size is invalid");
           }
           this.radius = radius;
           this.halfHeight = halfHeight;
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
       /// カプセルの高さの1/2
       /// </summary>
       /// <remarks>
       /// カプセルの高さは質量中心から球の中心までの距離です。
       /// </remarks>
       public float HalfHeight {
           get { return halfHeight; }
       }

       /// <summary>
       /// カプセルの高さ
       /// </summary>
       /// <remarks>
       /// カプセルの高さは球の中心から反対側の球の中心までの距離です。
       /// </remarks>
       public float Height {
           get { return halfHeight*2; }
       }

       /// <inheritdoc/>
       public override float ExSphereRadius {
           get { return halfHeight + radius; }
       }

       /// <inheritdoc/>
       public override float InSphereRadius {
           get { return radius; }
       }

       #endregion

       #region Method
       /// <inheritdoc/>
       public override BulletSharp.PairCachingGhostObject CreateGhostObject () {
           var col = new BulletSharp.PairCachingGhostObject ();
           col.CollisionShape = new BulletSharp.CapsuleShape (radius, halfHeight);
           col.CollisionFlags |= CollisionFlags.NoContactResponse;
           return col;
       }

       /// <inheritdoc/>
       public override BulletSharp.RigidBody CreateRigidBody (float mass) {
           var ppm = DD.Physics.PhysicsSimulator.PPM;
           
           var mstate = new DefaultMotionState ();
           var shape = new BulletSharp.CapsuleShape (radius/ppm, halfHeight/ppm);
           var info = new BulletSharp.RigidBodyConstructionInfo (mass, mstate, shape);

           return new BulletSharp.RigidBody (info);
       }

       /// <inheritdoc/>
       public override BulletSharp.CollisionShape CreateBulletShape () {
           var ppm = DD.Physics.PhysicsSimulator.PPM;
           
           return new BulletSharp.CapsuleShape (radius / ppm, halfHeight / ppm);
       }

       /// <inheritdoc/>
       public override string ToString () {
           return string.Format("Capsule : {0}, {1}", radius, halfHeight);
       }
       #endregion

   }
}
