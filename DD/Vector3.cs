using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// 3次元ベクトル構造体
    /// </summary>
    /// <remarks>
    /// 3次元の座標(x,y,z)を表します。
    /// ただし現在の所 z は使用しません。
    /// <note>
    /// 将来的に3次元に拡張する予定があるため。
    /// </note>
    /// </remarks>
    public struct Vector3 : IEquatable<Vector3> {

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="x">位置X</param>
        /// <param name="y">位置Y</param>
        /// <param name="z">位置Z</param>
        public Vector3 (float x, float y, float z)
            : this () {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3(Vector2 v, float z) : this(){
            this.X = v.X;
            this.Y = v.Y;
            this.Z = z;
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
        /// Z座標
        /// </summary>
        public float Z { get; private set; }

        /// <summary>
        /// コンポーネント数
        /// </summary>
        public int ComponentCount {
            get { return 3; }
        }

        /// <summary>
        /// ベクトルの長さ
        /// </summary>
        public float Length {
            get { return (float)Math.Sqrt (X * X + Y * Y + Z * Z); }
        }

        /// <summary>
        /// ベクトルの長さの自乗
        /// </summary>
        public float Length2 {
            get { return (X * X + Y * Y + Z * Z); }
        }

        /// <summary>
        /// このベクトルを正規化したベクトルを作成します
        /// </summary>
        /// <returns>正規化済みのベクトル</returns>
        public Vector3 Normalize () {
            if (Length == 0) {
                 throw new ArithmeticException ("Divied by 0");
            }
            return new Vector3 (X / Length, Y / Length, Z / Length);
        }

        /// <summary>
        /// 要素にアクセスするインデクサー
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>座標位置</returns>
        public float this [int index] {
            get {
                switch (index) {
                    case 0: return X;
                    case 1: return Y;
                    case 2: return Z;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
            internal set {
                switch (index) {
                    case 0: this.X = value; break;
                    case 1: this.Y = value; break;
                    case 2: this.Z = value; break;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
        }

        /// <summary>
        /// ゼロ ベクトル
        /// </summary>
        public static Vector3 Zero {
            get { return new Vector3 (0, 0, 0); }
        }

        /// <summary>
        /// ベクトルとfloatのかけ算
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">floatの値</param>
        /// <returns></returns>
        public static Vector3 operator * (Vector3 v, float f) {
            return new Vector3 (v.X * f, v.Y * f, v.Z * f);
        }

        /// <summary>
        /// floatとベクトルのかけ算
        /// </summary>
        /// <param name="f">floatの値</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static Vector3 operator * (float f, Vector3 v) {
            return v * f;
        }

        /// <summary>
        /// ベクトルとfloatの割り算
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <param name="f">floatの値</param>
        /// <returns></returns>
        public static Vector3 operator / (Vector3 v, float f) {
            if (f == 0) {
                throw new ArgumentException ("Divided by 0");
            }
            return new Vector3 (v.X / f, v.Y / f, v.Z / f);
        }

        /// <summary>
        /// ベクトル同士の足し算
        /// </summary>
        /// <remarks>
        /// 2つのベクトルをコンポーネント要素毎に足して新しいベクトルを作成します
        /// </remarks>
        /// <param name="v1">ベクトル１</param>
        /// <param name="v2">ベクトル２</param>
        /// <returns></returns>
        public static Vector3 operator + (Vector3 v1, Vector3 v2) {
            return new Vector3 (v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        /// <summary>
        /// ベクトル同士の引き算
        /// </summary>
        /// <remarks>
        /// 2つのベクトルをコンポーネント要素毎に引いて新しいベクトルを作成します
        /// </remarks>
        /// <param name="v1">ベクトル１</param>
        /// <param name="v2">ベクトル２</param>
        /// <returns></returns>
        public static Vector3 operator - (Vector3 v1, Vector3 v2) {
            return new Vector3 (v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        /// <summary>
        /// ベクトルの反転
        /// </summary>
        /// <param name="v">ベクトル</param>
        /// <returns>反転したベクトル</returns>
        public static Vector3 operator - (Vector3 v) {
            return new Vector3 (-v.X, -v.Y, -v.Z);
        }

        /// <summary>
        /// 2つのベクトルの内積
        /// </summary>
        /// <param name="a">ベクトルA</param>
        /// <param name="b">ベクトルB</param>
        /// <returns>内積値</returns>
        public static float Dot (Vector3 a, Vector3 b) {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        /// <summary>
        /// 2つのベクトルの角度（度数 [0,180]）
        /// </summary>
        /// <remarks>
        /// 2つのベクトルの角度を度数 (in degree) で返します。
        /// 戻り値は必ず [0,180] の範囲内です。180度より大きな角度やマイナスの角度は返しません。
        /// また角度が計算できない場合 NAN を返します。
        /// ベクトルAをベクトルBへ向けるための回転（クォータニオン）は以下の式を使って求まります。
        /// <code>
        ///  var angle = Vector3.Angle (a, b);
        ///  var cross = Vector3.Cross (a, b);
        ///  var rot = new Quarnion (angle, cross.X, cross.Y, cross.Z);
        /// </code>
        /// <note>
        /// 角度が計算できない場合って NAN で良いの？
        /// </note>
        /// </remarks>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float Angle (Vector3 a, Vector3 b) {
            if (a.Length2 == 0 || b.Length2 == 0) {
                return Single.NaN;
            }
            var cos = MyMath.Clamp(Vector3.Dot (a, b) / (a.Length * b.Length), -1, 1);
            var angle = (float)(Math.Acos(cos) / Math.PI * 180.0);
            return angle;
        }

        /// <summary>
        /// 2つのベクトルの外積
        /// </summary>
        /// <param name="a">ベクトルA</param>
        /// <param name="b">ベクトルB</param>
        /// <returns>外積値</returns>
        public static Vector3 Cross (Vector3 a, Vector3 b) {
            return new Vector3 (a.Y * b.Z - b.Y * a.Z, a.Z * b.X - b.Z * a.X, a.X * b.Y - b.X * a.Y);
        }


        /// <inheritdoc/>
        public override string ToString () {
            return String.Format ("({0:F3},{1:F3},{2:F3})", X, Y, Z);
        }

        /// <inheritdoc/>
        public override bool Equals (object obj) {
            if (obj == null || this.GetType () != obj.GetType ()) {
                return false;
            }
            return this.Equals ((Vector3)obj);
        }

        /// <inheritdoc/>
        public bool Equals (Vector3 other) {
            return (Math.Abs (X - other.X) < GlobalSettings.Torrelance) &&
                   (Math.Abs (Y - other.Y) < GlobalSettings.Torrelance) &&
                   (Math.Abs (Z - other.Z) < GlobalSettings.Torrelance);
        }

        /// <inheritdoc/>
        public override int GetHashCode () {
            return X.GetHashCode () ^ Y.GetHashCode () ^ Z.GetHashCode ();
        }

        /// <inheritdoc/>
        public static bool operator == (Vector3 a, Vector3 b) {
            return (a.X == b.X) && (a.Y == b.Y) && (a.Z == b.Z);
        }

        /// <inheritdoc/>
        public static bool operator != (Vector3 a, Vector3 b) {
            return !(a == b);
        }
    }


}
