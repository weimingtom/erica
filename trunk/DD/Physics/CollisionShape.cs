using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;

namespace DD.Physics {
    /// <summary>
    /// コリジョン形状 クラス
    /// </summary>
    /// <remarks>
    /// コリジョン形状を定義する抽象クラスです。
    /// 現在では <see cref="BoxCollider"/>, <see cref="SphereCollider"/> があります。
    /// </remarks>
    public abstract class CollisionShape {
        
        /// <summary>
        /// 物理ボディの作成
        /// </summary>
        /// <remarks>
        /// 物理エンジン側に演算で使用するボディを作成します。
        /// 密度は固定で 1000/m^3 （水）です。
        /// </remarks>
        /// <returns>Farseerシェイプ</returns>
        internal abstract Shape CreateShape ();
    }
}
