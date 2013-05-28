using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// ポイント構造体
    /// </summary>
    /// <remarks>
    /// 現在使われていません。将来的に使います。たぶん
    /// </remarks>
    public struct Point {
        float x;
        float y;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point (float x, float y)
            : this () {
                this.x = x;
                this.y = y;
        }

        /// <summary>
        /// X座標
        /// </summary>
        public float X {
            get { return x; }
            set { this.x = value; }
        }

        /// <summary>
        /// Y座標
        /// </summary>
        public float Y {
            get { return y; }
            set { this.y = value; }
        }

        /// <summary>
        /// 要素数
        /// </summary>
public int Length {
            get { return 2; }
        }

        /// <summary>
        /// インデクサ-
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return x;
                    case 1: return y;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
            set {
                switch (index) {
                    case 0: this.x = value; break;
                    case 1: this.y = value; break;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString () {
            return "(" + x + ", " + y + ")";
        }
    }
}
