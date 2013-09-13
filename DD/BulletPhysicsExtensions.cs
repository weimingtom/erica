using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// DDの行列を転置するとBulletPhysicsの行列になる


namespace DD {
    public static class BulletPhysicsExtensions {

        public static BulletSharp.Matrix ToBullet (this Matrix4x4 m){
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

        public static BulletSharp.Vector3 ToBullet (this Vector3 v) {
            return new BulletSharp.Vector3 (v.X, v.Y, v.Z);
        }

        public static BulletSharp.Quaternion ToBullet (this Quaternion q) {
            return new BulletSharp.Quaternion (q.X, q.Y, q.Z, q.W);
        }

        public static Matrix4x4 ToDD (this BulletSharp.Matrix m) {
            var mat = new Matrix4x4 (m.M11, m.M21, m.M31, m.M41,
                                     m.M12, m.M22, m.M32, m.M42,
                                     m.M13, m.M23, m.M33, m.M43,
                                     m.M14, m.M24, m.M34, m.M44);
            return mat;
        }

        public static Vector3 ToDD (this BulletSharp.Vector3 v) {
            return new Vector3 (v.X, v.Y, v.Z);
        }
        
        public static Quaternion ToDD (this BulletSharp.Quaternion q){
            return Quaternion.Set(q.X, q.Y, q.Z, q.W, false);
        }
    }
}
