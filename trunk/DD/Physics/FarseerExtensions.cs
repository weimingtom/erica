using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Common;
using FarseerPhysics.Collision;

namespace DD.Physics {
    /// <summary>
    /// Farseer拡張 クラス
    /// </summary>
    /// <remarks>
    /// <note>
    /// あまり使ってないし消すか・・・
    /// </note>
    /// </remarks>
    public static class FarseerExtensions {
        /// <summary>
        /// タイプの変更
        /// </summary>
        /// <param name="type">DDのタイプ</param>
        /// <returns>Farseerのタイプ</returns>
        public static BodyType ToFarseer (this DD.Physics.ColliderType type) {
            switch (type) {
                case DD.Physics.ColliderType.Dynamic: return FarseerPhysics.Dynamics.BodyType.Dynamic;
                case DD.Physics.ColliderType.Static: return FarseerPhysics.Dynamics.BodyType.Static;
                case DD.Physics.ColliderType.Kinematic: return FarseerPhysics.Dynamics.BodyType.Kinematic;
                default: throw new NotImplementedException ("Sorry");
            }
        }

        /// <summary>
        /// タイプの変更
        /// </summary>
        /// <param name="type">Farseerのタイプ</param>
        /// <returns>DDのタイプ</returns>
        public static DD.Physics.ColliderType ToDD (this FarseerPhysics.Dynamics.BodyType type) {
            switch (type) {
                case FarseerPhysics.Dynamics.BodyType.Dynamic: return DD.Physics.ColliderType.Dynamic;
                case FarseerPhysics.Dynamics.BodyType.Static: return DD.Physics.ColliderType.Static;
                case FarseerPhysics.Dynamics.BodyType.Kinematic: return DD.Physics.ColliderType.Kinematic;
                default: throw new NotImplementedException ("Sorry");
            }
        }

    }
}
