using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Physics {
    public struct Ray {
        public Ray (Vector3 pointA, Vector3 pointB, float f)
            : this () {
            this.PointA = pointA;
            this.PointB = pointB;
            this.Fraction = f;
        }
        public Vector3 PointA { get; private set; }
        public Vector3 PointB { get; private set; }
        public float Fraction { get; private set; }

        public Vector3 Origin { get { return PointA; } }
        public Vector3 Direction { get { return (PointB - PointA).Normalize (); } }
        public float UnitLength { get { return (PointB - PointA).Length; } }
        public float Length { get { return UnitLength * Fraction; } }
    }

    public struct RayIntersection {
        public RayIntersection (bool hit, Node node, Vector3 normal, float f, Ray ray)
            : this () {
            this.Hit = hit;
            this.Node = node;
            this.Normal = normal;
            this.Fraction = f;
            this.Ray = ray;
        }
        public bool Hit { get; private set; }
        public Node Node { get; private set; }
        public Vector3 Normal { get; private set; }
        public float Fraction { get; private set; }
        public Ray Ray { get; private set; }

        public float Distance { get { return Fraction * Ray.UnitLength; } }
    }



}
