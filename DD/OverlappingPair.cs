using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// コンタクト ペア
    /// </summary>
    /// <remarks>
    /// 現在重なっている2つのノードを保持します。
    /// OverlapedPairにすべき？
    /// <note>
    /// この構造体は NodeA と NodeB を区別しないため演算子の == と Equals() を独自の関数でオーバーライドしている。
    /// </note>
    /// </remarks>
    public struct OverlappingPair : IEquatable<OverlappingPair> {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="a">ノードA</param>
        /// <param name="b">ノードB</param>
        public OverlappingPair (Node a, Node b) : this() {
            this.NodeA = a;
            this.NodeB = b;
        }

        /// <summary>
        /// ノードA
        /// </summary>
        public Node NodeA { get; private set; }

        /// <summary>
        /// ノードB
        /// </summary>
        public Node NodeB { get; private set; }


        /// <inheritdoc/>
        public override bool Equals (Object obj) {
            if (obj == null || obj.GetType () != this.GetType ()) {
                return false;
            }
            return this.Equals((OverlappingPair)obj);
        }

        /// <inheritdoc/>
        public bool Equals (OverlappingPair other) {
            // If run-time types are not exactly the same, return false.
            if (this.GetType () != other.GetType ())
                return false;

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            return (this as IEquatable<OverlappingPair>).Equals(other);
        }

        /// <inheritdoc/>
        bool IEquatable<OverlappingPair>.Equals (OverlappingPair other) {
            return (NodeA == other.NodeA && NodeB == other.NodeB) || (NodeA == other.NodeB && NodeB == other.NodeA);
        }


        /// <inheritdoc/>
        public static bool operator == (OverlappingPair lhs, OverlappingPair rhs) {
            // Equals handles case of null on right side.
            return lhs.Equals (rhs);
        }

        /// <inheritdoc/>
        public static bool operator != (OverlappingPair lhs, OverlappingPair rhs) {
            return !(lhs == rhs);
        }

        /// <inheritdoc/>
        public override int GetHashCode () {
            return NodeA.GetHashCode () ^ NodeB.GetHashCode ();
        }

        /// <inheritdoc/>
        public override string ToString () {
            return string.Format("Collide: {0} - {1}", NodeA, NodeB);
        }
    }
}
