using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// 4x4行列構造体
    /// </summary>
    /// <remarks>
    /// 一般的な4x4行列を表します。
    /// </remarks>
    public struct Matrix4x4 : IEquatable<Matrix4x4> {

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="m00"></param>
        /// <param name="m01"></param>
        /// <param name="m02"></param>
        /// <param name="m03"></param>
        /// <param name="m10"></param>
        /// <param name="m11"></param>
        /// <param name="m12"></param>
        /// <param name="m13"></param>
        /// <param name="m20"></param>
        /// <param name="m21"></param>
        /// <param name="m22"></param>
        /// <param name="m23"></param>
        /// <param name="m30"></param>
        /// <param name="m31"></param>
        /// <param name="m32"></param>
        /// <param name="m33"></param>
        public Matrix4x4 (float m00, float m01, float m02, float m03,
                          float m10, float m11, float m12, float m13,
                          float m20, float m21, float m22, float m23,
                          float m30, float m31, float m32, float m33)
            : this () {
            this.M00 = m00;
            this.M01 = m01;
            this.M02 = m02;
            this.M03 = m03;
            this.M10 = m10;
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M20 = m20;
            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M30 = m30;
            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="m">16個のfloatの配列</param>
        public Matrix4x4 (float[] m)
            : this () {
            if (m == null) {
                throw new ArgumentNullException ("M4x4 is null");
            }
            if (m.Length < 16) {
                throw new ArgumentException ("M4x4 is less than required");
            }
            this.M00 = m[0];
            this.M01 = m[1];
            this.M02 = m[2];
            this.M03 = m[3];
            this.M10 = m[4];
            this.M11 = m[5];
            this.M12 = m[6];
            this.M13 = m[7];
            this.M20 = m[8];
            this.M21 = m[9];
            this.M22 = m[10];
            this.M23 = m[11];
            this.M30 = m[12];
            this.M31 = m[13];
            this.M32 = m[14];
            this.M33 = m[15];
        }

        /// <summary>
        /// コピーコンストラクター
        /// </summary>
        /// <param name="m">コピー元の<see cref="Matrix4x4"/> オブジェクト</param>
        public Matrix4x4 (Matrix4x4 m)
            : this () {
            this.M00 = m.M00;
            this.M01 = m.M01;
            this.M02 = m.M02;
            this.M03 = m.M03;
            this.M10 = m.M10;
            this.M11 = m.M11;
            this.M12 = m.M12;
            this.M13 = m.M13;
            this.M20 = m.M20;
            this.M21 = m.M21;
            this.M22 = m.M22;
            this.M23 = m.M23;
            this.M30 = m.M30;
            this.M31 = m.M31;
            this.M32 = m.M32;
            this.M32 = m.M32;
        }
        #endregion


        #region Peoprty
        /// <summary>
        /// M00要素
        /// </summary>
        public float M00 { get; private set; }
        /// <summary>
        /// M01要素
        /// </summary>
        public float M01 { get; private set; }
        /// <summary>
        /// M02要素
        /// </summary>
        public float M02 { get; private set; }
        /// <summary>
        /// M03要素
        /// </summary>
        public float M03 { get; private set; }
        /// <summary>
        /// M10要素
        /// </summary>
        public float M10 { get; private set; }
        /// <summary>
        /// M11要素
        /// </summary>
        public float M11 { get; private set; }
        /// <summary>
        /// M12要素
        /// </summary>
        public float M12 { get; private set; }
        /// <summary>
        /// M13要素
        /// </summary>
        public float M13 { get; private set; }
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
        /// M23要素
        /// </summary>
        public float M23 { get; private set; }
        /// <summary>
        /// M30要素
        /// </summary>
        public float M30 { get; private set; }
        /// <summary>
        /// M31要素
        /// </summary>
        public float M31 { get; private set; }
        /// <summary>
        /// M32要素
        /// </summary>
        public float M32 { get; private set; }
        /// <summary>
        /// M33要素
        /// </summary>
        public float M33 { get; private set; }

        /// <summary>
        /// コンポーネント数
        /// </summary>
        public int ComponentCount {
            get { return 16; }
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
                    case 3: return M03;
                    case 4: return M10;
                    case 5: return M11;
                    case 6: return M12;
                    case 7: return M13;
                    case 8: return M20;
                    case 9: return M21;
                    case 10: return M22;
                    case 11: return M23;
                    case 12: return M30;
                    case 13: return M31;
                    case 14: return M32;
                    case 15: return M33;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
            private set {
                switch (index) {
                    case 0: this.M00 = value; break;
                    case 1: this.M01 = value; break;
                    case 2: this.M02 = value; break;
                    case 3: this.M03 = value; break;
                    case 4: this.M10 = value; break;
                    case 5: this.M11 = value; break;
                    case 6: this.M12 = value; break;
                    case 7: this.M13 = value; break;
                    case 8: this.M20 = value; break;
                    case 9: this.M21 = value; break;
                    case 10: this.M22 = value; break;
                    case 11: this.M23 = value; break;
                    case 12: this.M30 = value; break;
                    case 13: this.M31 = value; break;
                    case 14: this.M32 = value; break;
                    case 15: this.M32 = value; break;
                    default: throw new IndexOutOfRangeException ("Index is out of range");
                }
            }
        }


        /// <summary>
        /// 単位行列
        /// </summary>
        public static Matrix4x4 Identity {
            get {
                return new Matrix4x4 (1, 0, 0, 0,
                                      0, 1, 0, 0,
                                      0, 0, 1, 0,
                                      0, 0, 0, 1);
            }
        }

        /// <summary>
        /// 行列の判別式
        /// </summary>
        public float Determinant {
            get {
                return M00 * M11 * M22 * M33 + M00 * M12 * M23 * M31 + M00 * M13 * M21 * M32
                       + M01 * M10 * M23 * M32 + M01 * M12 * M20 * M33 + M01 * M13 * M22 * M30
                       + M02 * M10 * M21 * M33 + M02 * M11 * M23 * M30 + M02 * M13 * M20 * M31
                       + M03 * M10 * M22 * M31 + M03 * M11 * M20 * M32 + M03 * M12 * M21 * M30
                       - M00 * M11 * M23 * M32 - M00 * M12 * M21 * M33 - M00 * M13 * M22 * M31
                       - M01 * M10 * M22 * M33 - M01 * M12 * M23 * M30 - M01 * M13 * M20 * M32
                       - M02 * M10 * M23 * M31 - M02 * M11 * M20 * M33 - M02 * M13 * M21 * M30
                       - M03 * M10 * M21 * M32 - M03 * M11 * M22 * M30 - M03 * M12 * M20 * M31;
            }
        }
        #endregion


        #region Method

        /// <summary>
        /// 逆行列の計算
        /// </summary>
        /// <remarks>
        ///この行列の逆行列を計算します。計算不可能（<see cref="Determinant"/> = 0）な時例外が発生します。
        /// </remarks>
        /// <returns></returns>
        public Matrix4x4 Inverse () {
            var det = Determinant;
            if (det == 0) {
                throw new ArithmeticException ("Can't invert this matrix");
            }
            var m = new Matrix4x4 ();
            m.M00 = 1 / det * (M11 * M22 * M33 + M12 * M23 * M31 + M13 * M21 * M32 - M11 * M23 * M32 - M12 * M21 * M33 - M13 * M22 * M31);
            m.M01 = 1 / det * (M01 * M23 * M32 + M02 * M21 * M33 + M03 * M22 * M31 - M01 * M22 * M33 - M02 * M23 * M31 - M03 * M21 * M32);
            m.M02 = 1 / det * (M01 * M12 * M33 + M02 * M13 * M31 + M03 * M11 * M32 - M01 * M13 * M32 - M02 * M11 * M33 - M03 * M12 * M31);
            m.M03 = 1 / det * (M01 * M13 * M22 + M02 * M11 * M23 + M03 * M12 * M21 - M01 * M12 * M23 - M02 * M13 * M21 - M03 * M11 * M22);
            m.M10 = 1 / det * (M10 * M23 * M32 + M12 * M20 * M33 + M13 * M22 * M30 - M10 * M22 * M33 - M12 * M23 * M30 - M13 * M20 * M32);
            m.M11 = 1 / det * (M00 * M22 * M33 + M02 * M23 * M30 + M03 * M20 * M32 - M00 * M23 * M32 - M02 * M20 * M33 - M03 * M22 * M30);
            m.M12 = 1 / det * (M00 * M13 * M32 + M02 * M10 * M33 + M03 * M12 * M30 - M00 * M12 * M33 - M02 * M13 * M30 - M03 * M10 * M32);
            m.M13 = 1 / det * (M00 * M12 * M23 + M02 * M13 * M20 + M03 * M10 * M22 - M00 * M13 * M22 - M02 * M10 * M23 - M03 * M12 * M20);
            m.M20 = 1 / det * (M10 * M21 * M33 + M11 * M23 * M30 + M13 * M20 * M31 - M10 * M23 * M31 - M11 * M20 * M33 - M13 * M21 * M30);
            m.M21 = 1 / det * (M00 * M23 * M31 + M01 * M20 * M33 + M03 * M21 * M30 - M00 * M21 * M33 - M01 * M23 * M30 - M03 * M20 * M30);
            m.M22 = 1 / det * (M00 * M11 * M33 + M01 * M13 * M30 + M03 * M10 * M31 - M00 * M13 * M31 - M01 * M10 * M33 - M03 * M11 * M30);
            m.M23 = 1 / det * (M00 * M13 * M21 + M01 * M10 * M23 + M03 * M11 * M20 - M00 * M11 * M23 - M01 * M13 * M20 - M03 * M10 * M21);
            m.M30 = 1 / det * (M10 * M22 * M31 + M11 * M20 * M32 + M12 * M21 * M30 - M10 * M21 * M32 - M11 * M22 * M30 - M12 * M20 * M31);
            m.M31 = 1 / det * (M00 * M21 * M32 + M01 * M22 * M30 + M02 * M20 * M31 - M00 * M22 * M31 - M01 * M20 * M32 - M02 * M21 * M30);
            m.M32 = 1 / det * (M00 * M12 * M31 + M01 * M10 * M32 + M02 * M11 * M30 - M00 * M11 * M32 - M01 * M12 * M30 - M02 * M10 * M31);
            m.M33 = 1 / det * (M00 * M11 * M22 + M01 * M12 * M20 + M02 * M10 * M21 - M00 * M12 * M21 - M01 * M10 * M22 - M02 * M11 * M20);

            return m;
        }

        /// <summary>
        /// 転地行列
        /// </summary>
        /// <returns></returns>
        public Matrix4x4 Transpose () {
            return new Matrix4x4 (M00, M10, M20, M30,
                                  M01, M11, M21, M31,
                                  M02, M12, M22, M32,
                                  M03, M13, M23, M33);
        }


        /// <summary>
        /// 行列のかけ算
        /// </summary>
        /// <param name="a">行列1</param>
        /// <param name="b">行列2</param>
        /// <returns></returns>
        public static Matrix4x4 operator * (Matrix4x4 a, Matrix4x4 b) {
            var m00 = a[0] * b[0] + a[1] * b[4] + a[2] * b[8] + a[3] * b[12];
            var m01 = a[0] * b[1] + a[1] * b[5] + a[2] * b[9] + a[3] * b[13];
            var m02 = a[0] * b[2] + a[1] * b[6] + a[2] * b[10] + a[3] * b[14];
            var m03 = a[0] * b[3] + a[1] * b[7] + a[2] * b[11] + a[3] * b[15];
            var m10 = a[4] * b[0] + a[5] * b[4] + a[6] * b[8] + a[7] * b[12];
            var m11 = a[4] * b[1] + a[5] * b[5] + a[6] * b[9] + a[7] * b[13];
            var m12 = a[4] * b[2] + a[5] * b[6] + a[6] * b[10] + a[7] * b[14];
            var m13 = a[4] * b[3] + a[5] * b[7] + a[6] * b[11] + a[7] * b[15];
            var m20 = a[8] * b[0] + a[9] * b[4] + a[10] * b[8] + a[11] * b[12];
            var m21 = a[8] * b[1] + a[9] * b[5] + a[10] * b[9] + a[11] * b[13];
            var m22 = a[8] * b[2] + a[9] * b[6] + a[10] * b[10] + a[11] * b[14];
            var m23 = a[8] * b[3] + a[9] * b[7] + a[10] * b[11] + a[11] * b[15];
            var m30 = a[12] * b[0] + a[13] * b[4] + a[14] * b[8] + a[15] * b[12];
            var m31 = a[12] * b[1] + a[13] * b[5] + a[14] * b[9] + a[15] * b[13];
            var m32 = a[12] * b[2] + a[13] * b[6] + a[14] * b[10] + a[15] * b[14];
            var m33 = a[12] * b[3] + a[13] * b[7] + a[14] * b[11] + a[15] * b[15];
            return new Matrix4x4 (m00, m01, m02, m03,
                                  m10, m11, m12, m13,
                                  m20, m21, m22, m23,
                                  m30, m31, m32, m33);
        }

        /// <summary>
        /// 行列からfloatの配列への変換
        /// </summary>
        /// <param name="mat">行列</param>
        /// <returns>16個のfloatの配列</returns>
        public static explicit operator float[] (Matrix4x4 mat) {
            return new float[16] { mat[0], mat[1], mat[2], mat[3], mat[4], mat[5], mat[6], mat[7], mat[8], mat[9], mat[10], mat[11], mat[12], mat[13], mat[14], mat[15] };
        }

        /// <summary>
        /// 平行移動をあらわす4x4の行列の作成
        /// </summary>
        /// <param name="tx">平行移動量X</param>
        /// <param name="ty">平行移動量Y</param>
        /// <param name="tz">平行移動量Z</param>
        /// <returns>4x4の行列</returns>
        public static Matrix4x4 CreateTranslation (float tx, float ty, float tz) {
            return new Matrix4x4 (1, 0, 0, tx,
                                  0, 1, 0, ty,
                                  0, 0, 1, tz,
                                  0, 0, 0, 1);
        }

        /// <summary>
        /// スケーリングをあらわす4x4の行列の作成
        /// </summary>
        /// <param name="sx">拡大縮小率X</param>
        /// <param name="sy">拡大縮小率Y</param>
        /// <param name="sz">拡大縮小率Z</param>
        /// <returns></returns>
        public static Matrix4x4 CreateScale (float sx, float sy, float sz) {
            return new Matrix4x4 (sx, 0, 0, 0,
                                  0, sy, 0, 0,
                                  0, 0, sz, 0,
                                  0, 0, 0, 1);
        }

        /// <summary>
        /// 回転をあらわす4x4の行列の作成
        /// </summary>
        /// <param name="angle">回転角度[0,360)</param>
        /// <param name="ax">回転軸X</param>
        /// <param name="ay">回転軸Y</param>
        /// <param name="az">回転軸Z</param>
        /// <returns></returns>
        public static Matrix4x4 CreateRotation (float angle, float ax, float ay, float az) {
            return CreateRotation (new Quaternion (angle, ax, ay, az));
        }

        /// <summary>
        /// 回転をあらわす4x4の行列の作成
        /// </summary>
        /// <param name="q">クォータニオン</param>
        /// <returns></returns>
        public static Matrix4x4 CreateRotation (Quaternion q) {
            return (Matrix4x4)q.Matrix3x3;
        }

        /// <summary>
        /// 4x4の行列をTRS成分に分解
        /// </summary>
        /// <param name="translation">平行移動成分</param>
        /// <param name="rotation">回転成分</param>
        /// <param name="scale">スケーリング成分</param>
        /// <returns>分解できれば true, そうでなければ false</returns>
        public bool Decompress (out Vector3 translation, out Quaternion rotation, out Vector3 scale) {
            var m = new Matrix4x4(this);
            var tx = m[3];
            var ty = m[7];
            var tz = m[11];
            var sx = (float)Math.Sqrt (m[0] * m[0] + m[4] * m[4] + m[8] * m[8] + m[12] * m[12]);
            var sy = (float)Math.Sqrt (m[1] * m[1] + m[5] * m[5] + m[9] * m[9] + m[13] * m[13]);
            var sz = (float)Math.Sqrt (m[2] * m[2] + m[6] * m[6] + m[10] * m[10] + m[14] * m[14]);
            if (sx == 0 || sy == 0 || sz == 0) {
                throw new ArithmeticException ("This matrix can't be divided to TRS");
            }

            translation = new Vector3 (tx, ty, tz);
            rotation = new Quaternion (new Matrix3x3 (m[0] / sx, m[1] / sy, m[2] / sz,
                                                      m[4] / sx, m[5] / sy, m[6] / sz,
                                                      m[8] / sx, m[9] / sy, m[10] / sz));
            scale = new Vector3 (sx, sy, sz);

            return true;
        }

        /// <summary>
        /// ベクトルの変換
        /// </summary>
        /// <remarks>
        /// この4x4行列を使って指定のベクトルを変換します。
        /// ベクトルは <c>W=1</c> が追加され、返還後に自動的にW除算されます。
        /// </remarks>
        /// <param name="x">ベクトルのX成分</param>
        /// <param name="y">ベクトルのY成分</param>
        /// <param name="z">ベクトルのZ成分</param>
        public void Apply (ref float x, ref float y, ref float z) {
            var v = new Vector3 (x, y, z);
            var m = this;
            x = m[0] * v[0] + m[1] * v[1] + m[2] * v[2] + m[3] * 1;
            y = m[4] * v[0] + m[5] * v[1] + m[6] * v[2] + m[7] * 1;
            z = m[8] * v[0] + m[9] * v[1] + m[10] * v[2] + m[11] * 1;
            var w = m[12] * v[0] + m[13] * v[1] + m[14] * v[2] + m[15] * 1;
            x /= w;
            y /= w;
            z /= w;
        }

        /// <inheritdoc/>
        public override bool Equals (object obj) {
            if (obj == null || this.GetType () != obj.GetType ()) {
                return false;
            }
            return this.Equals ((Matrix4x4)obj);
        }

        /// <inheritdoc/>
        public bool Equals (Matrix4x4 other) {
            return (Math.Abs (M00 - other.M00) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M01 - other.M01) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M02 - other.M02) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M03 - other.M03) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M10 - other.M10) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M11 - other.M11) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M12 - other.M12) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M13 - other.M13) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M20 - other.M20) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M21 - other.M21) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M22 - other.M22) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M23 - other.M23) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M30 - other.M30) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M31 - other.M31) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M32 - other.M32) < GlobalSettings.Torrelance) &&
                   (Math.Abs (M32 - other.M32) < GlobalSettings.Torrelance);
        }

        /// <inheritdoc/>
        public override int GetHashCode () {
            return M00.GetHashCode () ^ M01.GetHashCode () ^ M02.GetHashCode () ^ M03.GetHashCode () ^
                   M10.GetHashCode () ^ M11.GetHashCode () ^ M12.GetHashCode () ^ M13.GetHashCode () ^
                   M20.GetHashCode () ^ M21.GetHashCode () ^ M22.GetHashCode () ^ M23.GetHashCode () ^
                   M30.GetHashCode () ^ M31.GetHashCode () ^ M32.GetHashCode () ^ M33.GetHashCode ();
        }

        /// <inheritdoc/>
        public static bool operator == (Matrix4x4 a, Matrix4x4 b) {
            return (a.M00 == b.M00) && (a.M01 == b.M01) && (a.M02 == b.M02) && (a.M03 == b.M03) &&
                   (a.M10 == b.M10) && (a.M11 == b.M11) && (a.M12 == b.M12) && (a.M13 == b.M13) &&
                   (a.M20 == b.M20) && (a.M21 == b.M21) && (a.M22 == b.M22) && (a.M23 == b.M23) &&
                   (a.M30 == b.M30) && (a.M31 == b.M31) && (a.M32 == b.M32) && (a.M33 == b.M33);
        }

        /// <inheritdoc/>
        public static bool operator != (Matrix4x4 a, Matrix4x4 b) {
            return !(a == b);
        }

        /// <inheritdoc/>
        public override string ToString () {
            return String.Format ("({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15})",
                                  M00, M01, M02, M03, M10, M11, M12, M13, M20, M21, M22, M23, M30, M31, M32, M33);
        }

        #endregion

    }
}
