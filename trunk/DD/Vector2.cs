using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
     /// <summary>
    /// 2次元ベクトル構造体
    /// </summary>
    /// <remarks>
    /// 2次元の座標(x,y)を表します。
    /// </remarks>
    public struct Vector2 : IEquatable<Vector2> {

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        public Vector2 (float x, float y)
            : this () {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// X座標
        /// </summary>
        public float X { get; private set; }

        /// <summary>
        /// Y座標
        /// </summary>
        public float Y { get; private set; }

        /// <summary>
        /// コンポーネント数
        /// </summary>
        public int ComponentCount {
            get { return 2; }
        }

        /// <summary>
        /// ベクトルの長さ
        /// </summary>
        public float Length {
            get { return (float)Math.Sqrt (X * X + Y * Y); }
        }

        /// <summary>
        /// ベクトルの長さの自乗
        /// </summary>
        public float Length2 {
            get { return (X * X + Y * Y); }
        }

        /// <summary>
        /// このベクトルを正規化したベクトルを作成します
        /// </summary>
        /// <returns>正規化済みのベクトル</returns>
        public Vector2 Normalize () {
            if (Length == 0) {
                throw new ArithmeticException ("Divied by 0");
            }
            return new Vector2 (X / Length, Y / Length);
        }

        /// <summary>
        /// 要素にアクセスするインデクサー
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>座標位置</returns>
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return X;
                    case 1: return Y;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
            internal set {
                switch (index) {
                    case 0: this.X = value; break;
                    case 1: this.Y = value; break;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
        }

        /// <summary>
        /// ベクトルとfloatのかけ算
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">floatの値</param>
        /// <returns></returns>
        public static Vector2 operator * (Vector2 v, float f) {
            return new Vector2 (v.X * f, v.Y * f);
        }

        /// <summary>
        /// floatとベクトルのかけ算
        /// </summary>
        /// <param name="f">floatの値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static Vector2 operator * (float f, Vector2 v) {
            return v * f;
        }

        /// <summary>
        /// ベクトルとfloatの割り算
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">floatの値</param>
        /// <returns></returns>
        public static Vector2 operator / (Vector2 v, float f) {
            if (f == 0) {
                throw new ArgumentException ("Divided by 0");
            }
            return new Vector2 (v.X / f, v.Y / f);
        }

        /// <summary>
        /// ベクトル同士の足し算
        /// </summary>
        /// <remarks>
        /// 2つのベクトルをコンポーネント要素毎に足して新しいベクトルを作成します
        /// </remarks>
        /// <param name="v1">ベクトル1</param>
        /// <param name="v2">ベクトル2</param>
        /// <returns></returns>
        public static Vector2 operator + (Vector2 v1, Vector2 v2) {
            return new Vector2 (v1.X + v2.X, v1.Y + v2.Y);
        }

        /// <summary>
        /// ベクトル同士の引き算
        /// </summary>
        /// <remarks>
        /// 2つのベクトルをコンポーネント要素毎に引いて新しいベクトルを作成します
        /// </remarks>
        /// <param name="v1">ベクトル1</param>
        /// <param name="v2">ベクトル2</param>
        /// <returns></returns>
        public static Vector2 operator - (Vector2 v1, Vector2 v2) {
            return new Vector2 (v1.X - v2.X, v1.Y - v2.Y);
        }

        /// <summary>
        /// ベクトルの反転
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <returns>反転したベクトル</returns>
        public static Vector2 operator - (Vector2 v) {
            return new Vector2 (-v.X, -v.Y);
        }

        /// <summary>
        /// 2つのベクトルの内積
        /// </summary>
        /// <param name="a">ベクトルA</param>
        /// <param name="b">ベクトルB</param>
        /// <returns>内積値</returns>
        public static float Dot (Vector2 a, Vector2 b) {
            return a.X * b.X + a.Y * b.Y;
        }

        /// <inheritdoc/>
        public override string ToString () {
            return String.Format ("({0},{1})", X, Y);
        }

        /// <inheritdoc/>
        public override bool Equals (object obj) {
            if (obj == null || this.GetType () != obj.GetType ()) {
                return false;
            }
            return this.Equals ((Vector2)obj);
        }

        /// <inheritdoc/>
        public bool Equals (Vector2 other) {
            return (Math.Abs (X - other.X) < GlobalSettings.Torrelance) &&
                   (Math.Abs (Y - other.Y) < GlobalSettings.Torrelance);
        }

        /// <inheritdoc/>
        public override int GetHashCode () {
            return X.GetHashCode () ^ Y.GetHashCode ();
        }

        /// <inheritdoc/>
        public static bool operator == (Vector2 a, Vector2 b) {
            return (a.X == b.X) && (a.Y == b.Y);
        }

        /// <inheritdoc/>
        public static bool operator != (Vector2 a, Vector2 b) {
            return !(a == b);
        }
    }
}
