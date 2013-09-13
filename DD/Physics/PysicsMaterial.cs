using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Physics {
    public class PhysicsMaterial : Material {
        #region Field
        float restitution;      // 反発係数 [0,1]
        float linearDamping;    // 減衰係数 [0,1]  0:減衰無し 1:減衰100%
        float angularDamping;   // 転がり減衰係数 [0,1]
        float friction;         // 摩擦係数 [0,1]
        float rollingFriction;  // 転がり摩擦係数 [0,1]
        #endregion

        public PhysicsMaterial () {
            this.restitution = 1;
            this.linearDamping = 0;
            this.angularDamping = 0;
            this.friction = 0.5f;
            this.rollingFriction = 0f;
        }

        public float Restitution {
            get { return restitution; }
            set { this.restitution = value; }
        }

        public float LinearDamping {
            get { return linearDamping; }
            set { this.linearDamping = value; }
        }

        public float AngularDamping {
            get { return angularDamping; }
            set { this.angularDamping = value; }
        }

        public float Friction {
            get { return friction; }
            set { this.friction = value; }
        }

        public float RollingFriction {
            get { return rollingFriction; }
            set { this.rollingFriction = value; }
        }

    }
}
