using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;

namespace DD {
    
    /// <summary>
    /// コリジョン形状クラス
    /// </summary>
    /// <remarks>
    /// コリジョン検出に使われる形状を定義します。
    /// このクラスと派生クラスは形状を定義する他はほぼ何もしません。
    /// </remarks>
    public abstract class CollisionShape {
       /// <summary>
       /// この形状の Bullet Physics コリジョン オブジェクトの作成
       /// </summary>
       /// <remarks>
       /// Bullet Physics で使用される指定の形状のコリジョン オブジェクトを作成します。
       /// 正確に言うとゴーストオブジェクト。
       /// </remarks>
       /// <returns></returns>
       public abstract BulletSharp.PairCachingGhostObject CreateGhostObject ();

        /// <summary>
        /// この形状の Bullet Physics 剛体物理オブジェトの作成
        /// </summary>
        /// <remarks>
       /// Bullet Physics で使用される指定の形状の剛体物理オブジェクトを作成します。
       ///  </remarks>
       ///  <param name="mass">質量 (Kg)</param>
        /// <returns></returns>
       public abstract BulletSharp.RigidBody CreateRigidBody (float mass);

    }
}
