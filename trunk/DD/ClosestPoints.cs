using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    public struct ClosestPoints {
        public ClosestPoints (Vector3 pointA, Vector3 pointB)
            : this () {
                this.PointA = pointA;
                this.PointB = pointB;
        }
        public Vector3 PointA {get; private set;}
        public Vector3 PointB { get; private set; }
        public Vector3 VectorAtoB { get { return PointB - PointA;  } }
    }
}
