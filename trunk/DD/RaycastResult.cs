using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {

        public struct RaycastResult {
            public RaycastResult (float frac, float dist, Node node, Vector3 point, Vector3 normal) : this() {
                this.Hit = true;
                this.Fraction = frac;
                this.Distance = dist;
                this.Node = node;
                this.Point = point;
                this.Normal = normal;
            }
            public bool Hit { get; private set; }
            public float Fraction { get; private set; }
            public float Distance { get; private set; }
            public Node Node {get; private set; }
            public Vector3 Point { get; private set; }
            public Vector3 Normal { get; private set; }
        }

}
