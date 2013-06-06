using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// 3x3行列構造体
    /// </summary>
    /// <remarks>
    /// 一般的な3x3行列を表す構造体です。
    /// </remarks>
    public struct Matrix3x3 : IEquatable<Matrix3x3> {

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="m00">M00成分</param>
        /// <param name="m01">M01成分</param>
        /// <param name="m02">M02成分</param>
        /// <param name="m10">M10成分</param>
        /// <param name="m11">M11成分</param>
        /// <param name="m12">M12成分</param>
        /// <param name="m20">M20成分</param>
        /// <param name="m21">M21成分</param>
        /// <param name="m22">M22成分</param>
        public Matrix3x3 (float m00, float m01, float m02,
                          float m10, float m11, float m12,
                          float m20, float m21, float m22)
            : this () {
            this.M00 = m00;
            this.M01 = m01;
            this.M02 = m02;
            this.M10 = m10;
            this.M11 = m11;
            this.M12 = m12;
            this.M20 = m20;
            this.M21 = m21;
            this.M22 = m22;
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="m">9個の要素を持つ float の配列</param>
        public Matrix3x3 (float[] m)
            : this () {
            if (m == null) {
                throw new ArgumentNullException ("M is null");
            }
            if (m.Length < 9) {
                throw new ArgumentException ("M is less than required");
            }
            this.M00 = m[0];
            this.M01 = m[1];
            this.M02 = m[2];
            this.M10 = m[3];
            this.M11 = m[4];
            this.M12 = m[5];
            this.M20 = m[6];
            this.M21 = m[7];
            this.M22 = m[8];
        }

        /// <summary>
        /// コピー コンストラクター
        /// </summary>
        /// <param name="m">コピー元の <see cref="Matrix3x3"/> オブジェクト</param>
        public Matrix3x3 (Matrix3x3 m)
            : this () {
            this.M00 = m.M00;
            this.M01 = m.M01;
            this.M02 = m.M02;
            this.M10 = m.M10;
            this.M11 = m.M11;
            this.M12 = m.M12;
            this.M20 = m.M20;
            this.M21 = m.M21;
            this.M22 = m.M22;
        }
        #endregion


        #region Property
        /// <summary>
        /// M00要素
        /// </summary>
        public float M00 { get; private set; }

        /// <summary>
        /// M01要素
        /// </summary>
        public float M01 { get; internal set; }

        /// <summary>
        /// M02要素
        /// </summary>
        public float M02 { get; private set; }

        /// <summary>
        /// M10要素
        /// </summary>
        public float M10 { get; internal set; }

        /// <summary>
        /// M11要素
        /// </summary>
        public float M11 { get; private set; }

        /// <summary>
        /// M12要素
        /// </summary>
        public float M12 { get; private set; }

        /// <summary>
        /// M20要素
        /// </summary>
        public float M20 { get; private set; }

        /// <summary>
        /// M21要素
        /// </summary>
        public float M21 { get; private set; }

        /// <summary>
        /// M22要素
        /// </summary>
        public float M22 { get; private set; }

        /// <summary>
        /// コンポーネント数
        /// </summary>
        public int ComponentCount {
            get { return 9; }
        }

        /// <summary>
        /// 要素にアクセスするインデクサー
        /// </summary>
        /// <param name="index">インデックス番号</param>
        /// <returns></returns>
        public float this[int index] {
            get {
                switch (index) {
                    case 0: return M00;
                    case 1: return M01;
                    case 2: return M02;
                    case 3: return M10;
                    case 4: return M11;
                    case 5: return M12;
                    case 6: return M20;
                    case 7: return M21;
                    case 8: return M22;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
            internal set {
                switch (index) {
                    case 0: this.M00 = value; break;
                    case 1: this.M01 = value; break;
                    case 2: this.M02 = value; break;
                    case 3: this.M10 = value; break;
                    case 4: this.M11 = value; break;
                    case 5: this.M12 = value; break;
                    case 6: this.M20 = value; break;
                    case 7: this.M21 = value; break;
                    case 8: this.M22 = value; break;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
        }


        /// <summary>
        /// 単位行列
        /// </summary>
        public static Matrix3x3 Identity {
            get {
                return new Matrix3x3 (1, 0, 0,
                                      0, 1, 0,
                                      0, 0, 1);
            }
        }

        /// <summary>
        /// 行列式
        /// </summary>
        public float Determinant {
            get {
                return M00 * M11 * M22 - M00 * M21 * M12 + M01 * M12 * M20 - M01 * M22 * M10 + M02 * M10 * M21 - M02 * M20 * M11;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 逆行列の計算
        /// </summary>
        /// <remarks>
        /// 逆行列が計算可能でない（<see cref="Determinant"/> == 0）時、例外が発生します。
        /// </remarks>
        /// <returns></returns>
        public Matrix3x3 Inverse () {
            var det = Determinant;
            if (det == 0) {
                throw new ArithmeticException ("Can't invert this matrix");
            }

            var m = new Matrix3x3 ();
            m.M00 = 1 / det * (M11 * M22 - M12 * M21);
            m.M01 = 1 / det * (M02 * M21 - M01 * M22);
            m.M02 = 1 / det * (M01 * M12 - M02 * M11);
            m.M10 = 1 / det * (M12 * M20 - M10 * M22);
            m.M11 = 1 / det * (M00 * M22 - M02 * M20);
            m.M12 = 1 / det * (M02 * M10 - M00 * M12);
            m.M20 = 1 / det * (M10 * M21 - M11 * M20);
            m.M21 = 1 / det * (M01 * M20 - M00 * M21);
            m.M22 = 1 / det * (M00 * M11 - M01 * M10);

            return m;
        }

        /// <summary>
        /// 転地行列の計算
        /// </summary>
        /// <returns></returns>
        public Matrix3x3 Transpose () {
            return new Matrix3x3 (M00, M10, M20,
                                  M01, M11, M21,
                                  M02, M12, M22);
        }


        /// <summary>
        /// 3x3行列から4x4行列への昇格
        /// </summary>
        /// <param name="m">3x3行列</param>
        /// <returns>4x4行列</returns>
        public static explicit operator Matrix4x4 (Matrix3x3 m) {
            return new Matrix4x4 (m[0], m[1], m[2], 0,
                                  m[3], m[4], m[5], 0,
                                  m[6], m[7], m[8], 0,
                                  0, 0, 0, 1);

        }

        /// <summary>
        /// 2つの行列のかけ算
        /// </summary>
        /// <param name="a">行列1</param>
        /// <param name="b">行列2</param>
        /// <returns></returns>
        public static Matrix3x3 operator * (Matrix3x3 a, Matrix3x3 b) {
            var m00 = a[0] * b[0] + a[1] * b[3] + a[2] * b[6];
            var m01 = a[0] * b[1] + a[1] * b[4] + a[2] * b[7];
            var m02 = a[0] * b[2] + a[1] * b[5] + a[2] * b[8];
            var m10 = a[3] * b[0] + a[4] * b[3] + a[5] * b[6];
            var m11 = a[3] * b[1] + a[4] * b[4] + a[5] * b[7];
            var m12 = a[3] * b[2] + a[4] * b[5] + a[5] * b[8];
            var m20 = a[6] * b[0] + a[7] * b[3] + a[8] * b[6];
            var m21 = a[6] * b[1] + a[7] * b[4] + a[8] * b[7];
            var m22 = a[6] * b[2] + a[7] * b[5] + a[8] * b[8];
            return new Matrix3x3 (m00, m01, m02,
                                  m10, m11, m12,
                                  m20, m21, m22);
        }

        /// <summary>
        /// 行列とベクトルのかけ算
        /// </summary>
        /// <remarks>
        /// 計算式は <c>V' = M*V</c> です。
        /// </remarks>
        /// <param name="m">行列</param>
        /// <param name="v">ベクトル</param>
        /// <returns></returns>
        public static Vector3 operator * (Matrix3x3 m, Vector3 v) {
            var x = m[0] * v[0] + m[1] * v[1] + m[2] * v[2];
            var y = m[3] * v[0] + m[4] * v[1] + m[5] * v[2];
            var z = m[6] * v[0] + m[7] * v[1] + m[8] * v[2];
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// floatの配列への変換
        /// </summary>
        /// <param name="mat">3x3行列</param>
        /// <returns></returns>
        public static explicit operator float[] (Matrix3x3 mat) {
            return new float[9] { mat[0], mat[1], mat[2], mat[3], mat[4], mat[5], mat[6], mat[7], mat[8] };
        }

        /// <inheritdoc/>
        public override bool Equals (object obj) {
            if (obj == null || this.GetType () != obj.GetType ()) {
                return false;
            }
            return this.Equals ((Matrix3x3)obj);
        }

        /// <inheritdoc/>
        public bool Equals (Matrix3x3 other) {
            return (Math.Abs (M00 - other.M00) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M01 - other.M01) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M02 - other.M02) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M10 - other.M10) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M11 - other.M11) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M12 - other.M12) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M20 - other.M20) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M21 - other.M21) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M22 - other.M22) < GlobalSettings.Torrelance);
        }

        /// <inheritdoc/>
        public override int GetHashCode () {
            return M00.GetHashCode () ^ M01.GetHashCode () ^ M02.GetHashCode () ^
                   M10.GetHashCode () ^ M11.GetHashCode () ^ M12.GetHashCode () ^
                   M20.GetHashCode () ^ M21.GetHashCode () ^ M22.GetHashCode ();
        }

        /// <inheritdoc/>
        public static bool operator == (Matrix3x3 a, Matrix3x3 b) {
            return (a.M00 == b.M00) && (a.M01 == b.M01) && (a.M02 == b.M02) &&
                   (a.M10 == b.M10) && (a.M11 == b.M11) && (a.M12 == b.M12) &&
                   (a.M20 == b.M20) && (a.M21 == b.M21) && (a.M22 == b.M22);
        }

        /// <inheritdoc/>
        public static bool operator != (Matrix3x3 a, Matrix3x3 b) {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override string ToString () {
            return String.Format ("({0},{1},{2},{3},{4},{5},{6},{7},{8})",
                                  M00, M01, M02, M10, M11, M12, M20, M21, M22);
        }
        #endregion


    }
}
