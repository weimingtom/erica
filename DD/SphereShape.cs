using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;

namespace DD {
    /// <summary>
    /// 球状のコリジョン形状クラス
    /// </summary>
    /// <remarks>
    /// </remarks>
   public  class SphereShape : CollisionShape {
       #region Field
       float radius;
       #endregion

       #region Constructor
       /// <summary>
       /// コンストラクター
       /// </summary>
       /// <param name="radius">半径</param>
       public SphereShape (float radius) {
           if (radius <= 0) {
               throw new ArgumentException ("Raidus is invalid");
           }
           this.radius = radius;
       }
       #endregion

       #region Property
       /// <summary>
       /// 半径
       /// </summary>
       public float Radius {
           get { return radius; }
       }

       /// <summary>
       /// 直径
       /// </summary>
       public float Diameter {
           get { return radius*2; }
       }

       /// <inheritdoc/>
       public override float ExSphereRadius {
           get { return radius; }
       }

       /// <inheritdoc/>
       public override float InSphereRadius {
           get { return radius; }
       }

       #endregion
       
       
       
       #region Method
       /// <inheritdoc/>
       public override BulletSharp.PairCachingGhostObject CreateGhostObject () {
           var col = new PairCachingGhostObject ();
           col.CollisionShape = new BulletSharp.SphereShape(radius);
           col.CollisionFlags |= CollisionFlags.NoContactResponse;

           return col;

       }

       /// <inheritdoc/>
       public override BulletSharp.RigidBody CreateRigidBody (float mass) {
           var ppm = DD.Physics.PhysicsSimulator.PPM;
           
           var mstate = new DefaultMotionState ();
           var shape = new BulletSharp.SphereShape (radius / ppm);
           var info = new BulletSharp.RigidBodyConstructionInfo (mass, mstate, shape);

           return new BulletSharp.RigidBody (info);
       }

       /// <inheritdoc/>
       public override BulletSharp.CollisionShape CreateBulletShape () {
           var ppm = DD.Physics.PhysicsSimulator.PPM;
           
           return new BulletSharp.SphereShape (radius / ppm);
       }


       /// <inheritdoc/>
       public override string ToString () {
           return string.Format("Sphere : {0}", radius);
       }
       #endregion


   }
}
