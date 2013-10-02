using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// BUlletPhysicsとDDでは行列のかける順番が逆
// DD : M = ABC (C,B,Aの順）
// Bt : M = ABC (A,B,Cの順）
// DDの行列を転置するとBulletPhysicsの行列になる
// Bulletの平行移動成分は (M41,M42,M43)

namespace DD {
    /// <summary>
    /// BulletPhysics に拡張メソッドを追加するクラス
    /// </summary>
    public static class BulletPhysicsExtensions {

        /// <summary>
        /// DDの行列をBulletPhysicsの行列に変更
        /// </summary>
        /// <param name="m">DDの行列</param>
        /// <returns>BulletPhysicsの行列</returns>
        public static BulletSharp.Matrix ToBullet (this Matrix4x4 m) {
            var mat = new BulletSharp.Matrix ();
            mat.M11 = m[0];
            mat.M12 = m[4];
            mat.M13 = m[8];
            mat.M14 = m[12];
            mat.M21 = m[1];
            mat.M22 = m[5];
            mat.M23 = m[9];
            mat.M24 = m[13];
            mat.M31 = m[2];
            mat.M32 = m[6];
            mat.M33 = m[10];
            mat.M34 = m[14];
            mat.M41 = m[3];
            mat.M42 = m[7];
            mat.M43 = m[11];
            mat.M44 = m[15];
            return mat;
        }

        /// <summary>
        /// DDのベクトルをBulletPhysicsのベクトルに変更
        /// </summary>
        /// <param name="v">DDのベクトル</param>
        /// <returns>BulletPhysicsの行列</returns>
        public static BulletSharp.Vector3 ToBullet (this Vector3 v) {
            return new BulletSharp.Vector3 (v.X, v.Y, v.Z);
        }

        /// <summary>
        /// DDのクォータニオンをBulletPhysicsのクォータニオンに変更
        /// </summary>
        /// <remarks>
        /// クォータニオンに関しては一切変更無しで同一。
        /// </remarks>
        /// <param name="q">DDのクォータニオン</param>
        /// <returns>BulletPhysicsのクォータニオン</returns>
        public static BulletSharp.Quaternion ToBullet (this Quaternion q) {
            return new BulletSharp.Quaternion (q.X, q.Y, q.Z, q.W);
        }

        /// <summary>
        /// BulletPhysicsの行列をDDの行列に変更
        /// </summary>
        /// <param name="m">BulletPhysicsの行列</param>
        /// <returns>DDの行列</returns>
        public static Matrix4x4 ToDD (this BulletSharp.Matrix m) {
            var mat = new Matrix4x4 (m.M11, m.M21, m.M31, m.M41,
                                     m.M12, m.M22, m.M32, m.M42,
                                     m.M13, m.M23, m.M33, m.M43,
                                     m.M14, m.M24, m.M34, m.M44);
            return mat;
        }

        /// <summary>
        /// BulletPhysicsのベクトルをDDのベクトルに変更
        /// </summary>
        /// <param name="v">BulletPhysicsのベクトル</param>
        /// <returns>DDのベクトル</returns>
        public static Vector3 ToDD (this BulletSharp.Vector3 v) {
            return new Vector3 (v.X, v.Y, v.Z);
        }

        /// <summary>
        /// BulletPhysicsのクォータニオンにDDのクォータニオンに変更
        /// </summary>
        /// <remarks>
        /// クォータニオンに関しては一切変更無しで同一。
        /// </remarks>
        /// <param name="q">BulletPhysicsのクォータニオン</param>
        /// <returns>DDのクォータニオン</returns>
        public static Quaternion ToDD (this BulletSharp.Quaternion q) {
            return Quaternion.Set (q.X, q.Y, q.Z, q.W, false);
        }
    }
}
