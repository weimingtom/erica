using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Common;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using System.Diagnostics;

using XnaVector2 = Microsoft.Xna.Framework.Vector2;

namespace DD.Physics {
    /// <summary>
    /// 2Dの物理エンジン デバイス クラス
    /// </summary>
    /// <remarks>
    /// 2Dの物理演算を行う物理エンジン デバイスのクラスです。
    /// シングルトン化され常に同じインスタンスが返ります。
    /// 物理エンジンではDDDのシーン グラフとは別に独自に物理演算用のワールドを作成します。
    /// 長さの単位は m （メーター）です。基本的に物理エンジン上ではMKS単位系を使用します。
    /// つまりメーター、キログラム、秒で、従って重力加速度は 9.8m/秒 です。
    /// メーターとピクセル数の対応は1mあたりのピクセル数 <see cref="PPM"/>（ピクセル数/メーター）で
    /// 表しますが、ユーザーが直接物理エンジン ワールドに触る事はないのであまり気にする必要はありません。
    /// </remarks>
    public class Physics2D {


        #region Field
        static Physics2D p2d;
        FarseerPhysics.Dynamics.World wld;
        float ppm;
        long prev;
        #endregion

        #region ReadOnly
        readonly Vector2 gravity = new Vector2 (0, 9.8f);
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        private Physics2D () {
            this.wld = null;
            this.prev = 0;
        }

        /// <summary>
        /// <see cref="Physics2D"/> オブジェクトの取得
        /// </summary>
        /// <remarks>
        /// シングルトン化され常に同じインスタンスが返ります。
        /// 現在では2Dの物理エンジンとしてFarseerPhysics(Box2D)を使用しています。
        /// </remarks>
        /// <returns></returns>
        public static Physics2D GetInstance () {
            if (p2d == null) {
                p2d = new Physics2D ();
            }
            return p2d;
        }

        /// <summary>
        /// 物理世界の構築
        /// </summary>
        /// <remarks>
        /// （物理演算用の）世界を作成します。物理エンジンを使用するにはこの世界の構築が必要です。
        /// 引数は物理世界の1mが何ピクセルに相当するかを指定します。
        /// Box2Dでは慣用的に1mは32ピクセルです。 Box2D は 0.1m ～ 10m の範囲がもっとも精度良くシミュレーション可能です。
        /// 物体がこの範囲の外側に飛び出しても計算に問題はありませんが精度が悪くなります。
        /// </remarks>
        /// <param name="ppm">1mあたりのピクセル数(推奨32) [Pixel/Meter]</param>
        public void CreateWorld (float ppm) {
            if (ppm <= 0) {
                throw new ArgumentException ("Pixel/Meter is invalid");
            }
            if (wld != null) {
                this.wld.Clear ();
            }
            this.wld = new FarseerPhysics.Dynamics.World (new XnaVector2 (gravity.X, gravity.Y));
            this.ppm = ppm;
        }
        #endregion


        #region Propety
        /// <summary>
        /// 重力加速度
        /// </summary>
        /// <remarks>
        /// 重力加速度を取得・設定するプロパティです。
        /// 初期値では 9.8 (m/s^2) です（注意：プラスです）。
        /// </remarks>
        public Vector2 Gravity {
            get {
                if (wld == null) {
                    return gravity;
                }
                return new Vector2 (wld.Gravity.X, wld.Gravity.Y);
            }
            set { SetGravity (value.X, value.Y); }
        }

        /// <summary>
        /// 1mあたりのピクセル数
        /// </summary>
        /// <remarks>
        /// DDD上のピクセル座標をこの値で割った物が物理エンジン上のサイズです。
        /// 初期値は 32 です。特にこだわりがなければ変更する必要はありません。
        /// </remarks>
        public float PPM {
            get { return ppm; }
            set {
                if (ppm < 1) {
                    throw new ArgumentException ("MPP(Pixel Per Meter) is invalid");
                }
                this.ppm = value;
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 物理世界の取得
        /// </summary>
        /// <remarks>
        /// 物理演算で使用するワールドです。Farseer.Dynamics.Worldにキャストしてください。
        /// 通常ユーザーがこのメソッドを使用する事はありません。
        /// </remarks>
        /// <returns></returns>
        public object GetWorld () {
            return wld;
        }

        /// <summary>
        /// 重力加速度の変更
        /// </summary>
        /// <remarks>
        /// 単位はMKS単位系です。初期値は 9.8 [m/s^2] です。
        /// </remarks>
        /// <param name="x">X方向の重力加速度 [m/s^2]</param>
        /// <param name="y">Y方向の重力加速度 [m/s^2]</param>
        public void SetGravity (float x, float y) {
            if (wld != null) {
                this.wld.Gravity = new XnaVector2 (x, y);
            }
        }

        /// <summary>
        /// 2物体の衝突の検出
        /// </summary>
        /// <remarks>
        /// 2物体の衝突を検出します。
        /// 現在のところ衝突地点とその法線について、それっぽい値を返していますが明確な取り決めはありません。
        /// </remarks>
        /// <param name="shapeA">コリジョン形状A</param>
        /// <param name="matA">変換行列A</param>
        /// <param name="shapeB">コリジョン形状B</param>
        /// <param name="matB">変換行列B</param>
        /// <param name="col">結果を受け取るコリジョン構造体</param>
        /// <returns>衝突があれば<c>true</c>, そうでなければ <c>false</c>.</returns>
        public static bool Collide (CollisionShape shapeA, Matrix4x4? matA, CollisionShape shapeB, Matrix4x4? matB, out Collision col) {
            if (shapeA == null || shapeB == null) {
                throw new ArgumentNullException ("Shapes are null");
            }

            
            var mani = new Manifold();
            Vector3 T;
            Matrix3x3 R;
            Vector3 S;

            (matA ?? Matrix4x4.Identity).Decompress (out T, out R, out S);
            var shpA = shapeA.CreateShapeBody (1.0f);
            var posA = new XnaVector2 (T.X, T.Y);
            var rotA = new Mat22 (R[0], R[1], R[3], R[4]);
            var traA = new Transform (ref posA, ref rotA);
            
            (matB ?? Matrix4x4.Identity).Decompress (out T, out R, out S);
            var shpB = shapeB.CreateShapeBody (1.0f);
            var posB = new XnaVector2 (T.X, T.Y);
            var rotB = new Mat22 (R[0], R[1], R[3], R[4]);
            var traB = new Transform (ref posB, ref rotB);

            if (shapeA is BoxCollisionShape && shapeB is BoxCollisionShape) {
                FarseerPhysics.Collision.Collision.CollidePolygons (ref mani, (PolygonShape)shpA, ref traA, (PolygonShape)shpB, ref traB);
            }
            if (shapeA is BoxCollisionShape && shapeB is SphereCollisionShape) {
                FarseerPhysics.Collision.Collision.CollidePolygonAndCircle (ref mani, (PolygonShape)shpA, ref traA, (CircleShape)shpB, ref traB);
            }
            if (shapeA is SphereCollisionShape && shapeB is BoxCollisionShape) {
                MyMath.Swap (ref shpA, ref shpB);
                MyMath.Swap (ref traA, ref traB);
                FarseerPhysics.Collision.Collision.CollidePolygonAndCircle (ref mani, (PolygonShape)shpA, ref traA, (CircleShape)shpB, ref traB);
            }
            if (shapeA is SphereCollisionShape && shapeB is SphereCollisionShape) {
                FarseerPhysics.Collision.Collision.CollideCircles (ref mani, (CircleShape)shpA, ref traA, (CircleShape)shpB, ref traB);
            }

            XnaVector2 normal;
            FixedArray2<XnaVector2> points;

            switch (mani.PointCount) {
                case 0: {
                    col = new Collision (null, Vector3.Zero, Vector3.Zero);
                        return false;
                    }
                case 1: {
                        FarseerPhysics.Collision.Collision.GetWorldManifold (ref mani, ref traA, shpA.Radius, ref traB, shpB.Radius, out normal, out points);
                        var p = new Vector3 (points[0].X, points[0].Y, 0);
                        var n = new Vector3 (normal.X, normal.Y, 0);
                        col = new Collision (null, p, n);
                        return true;
                    }
                case 2: {
                        FarseerPhysics.Collision.Collision.GetWorldManifold (ref mani, ref traA, shpA.Radius, ref traB, shpB.Radius, out normal, out points);
                        var p = new Vector3 ((points[0].X + points[1].X) / 2.0f, (points[0].Y + points[1].Y) / 2.0f, 0);
                        var n = new Vector3 (normal.X, normal.Y, 0);
                        col = new Collision (null, p, n);
                        return true;
                    }
                default: throw new InvalidOperationException ("This never happen");
            }
        }

      /// <summary>
        /// 2物体の衝突の検出
        /// </summary>
        /// <remarks>
        /// 2物体の衝突を検出します。衝突地点の情報を受け取らない事を除き同名の関数と等価です。
        /// </remarks>
        /// <param name="shapeA">コリジョン形状A</param>
        /// <param name="matA">変換行列A</param>
        /// <param name="shapeB">コリジョン形状B</param>
        /// <param name="matB">変換行列B</param>
        /// <returns>衝突があれば<c>true</c>, そうでなければ <c>false</c>.</returns>
        public static bool Collide (CollisionShape shapeA, Matrix4x4? matA, CollisionShape shapeB, Matrix4x4? matB) {
            Collision col;
            return Collide(shapeA, matA, shapeB, matB, out col);
        }

        /// <summary>
        /// 2物体の最短距離の測定
        /// </summary>
        /// <remarks>
        /// 2つのコリジョン形状の最短距離を求めます。
        /// 2つの物体がオーバーラップしていた場合は一律 0 が帰り、負の値が変える事はありません。
        /// </remarks>
        /// <param name="shapeA">コリジョン形状A</param>
        /// <param name="matA">変換行列A</param>
        /// <param name="shapeB">コリジョン形状B</param>
        /// <param name="matB">変換行列B</param>
        /// <returns>距離</returns>
        public static float Distance (CollisionShape shapeA, Matrix4x4? matA, CollisionShape shapeB, Matrix4x4? matB) {

            DistanceOutput output;
            SimplexCache cache;
            DistanceInput input = new DistanceInput ();
            DistanceProxy proxyA = new DistanceProxy ();
            DistanceProxy proxyB = new DistanceProxy ();

            proxyA.Set (shapeA.CreateShapeBody (1), 0);
            proxyB.Set (shapeB.CreateShapeBody (1), 0);

            input.ProxyA = proxyA;
            input.ProxyB = proxyB;
            input.UseRadii = true;
            
            Vector3 T;
            Matrix3x3 M;
            Vector3 S;
            (matA ?? Matrix4x4.Identity).Decompress(out T, out M, out S);

            var posA = new XnaVector2 (T.X, T.Y);
            var rotA = new Mat22 (M[0], M[1], M[3], M[4]);
            input.TransformA = new Transform (ref posA, ref rotA);

            (matB ?? Matrix4x4.Identity).Decompress (out T, out M, out S);

            var posB = new XnaVector2 (T.X, T.Y);
            var rotB = new Mat22 (M[0], M[1], M[3], M[4]);
            input.TransformB = new Transform (ref posB, ref rotB);
            
            FarseerPhysics.Collision.Distance.ComputeDistance (out output, out cache, input);

            return output.Distance;
        }


        /// <summary>
        /// コリジョン形状と1点の検出
        /// </summary>
        /// <remarks>
        /// 日本語変・・・
        /// </remarks>
        /// <param name="shapeA">コリジョン形状</param>
        /// <param name="matA">変換行列</param>
        /// <param name="point">判定したい1点</param>
        /// <returns>コリジョン形状の中にあれば<c>true</c>, そうでなければ <c>false</c>. </returns>
        public static bool Contain (CollisionShape shapeA, Matrix4x4? matA, Vector2 point) {

            Vector3 T;
            Matrix3x3 R;
            Vector3 S;

            (matA ?? Matrix4x4.Identity).Decompress (out T, out R, out S);
            var shpA = shapeA.CreateShapeBody (1.0f);
            var posA = new XnaVector2 (T.X, T.Y);
            var rotA = new Mat22 (R[0], R[1], R[3], R[4]);
            var traA = new Transform (ref posA, ref rotA);

            var posB = new XnaVector2(point.X, point.Y);

            return shpA.TestPoint (ref traA, ref posB);
        }

        /// <summary>
        /// 物理エンジンを1ステップ進める
        /// </summary>
        /// <remarks>
        /// 物理エンジンを1ステップ進めます。オブジェクトの位置･回転は計算結果に従って変更されます。
        /// またコリジョン イベントが発生する場合があります。
        /// <seealso cref="Component.OnPhysicsUpdate"/>
        /// <seealso cref="Component.OnCollisionEnter"/>
        /// <seealso cref="Component.OnCollisionExit"/>
        /// </remarks>
        /// <param name="ddworld">DDのワールド</param>
        /// <param name="msec">ワールド時刻 (msec)</param>
        public void Step (DD.World ddworld, long msec) {
            if (prev == 0) {
                prev = msec;
            }

            var colliders = from node in ddworld.Downwards
                            from comp in node.Components
                            where comp is PhysicsBody
                            select (PhysicsBody)comp;

            foreach (var col in colliders) {

                Vector3 T;
                Quaternion R;
                Vector3 S;
                col.Node.GlobalTransform.Decompress (out T, out R, out S);

                // クォータニオンの性質上(0,0,1)軸まわりの回転か、
                // (0,0,-1)軸まわりの回転のどちらかが返ってくる（両者は等価）。
                // Box2Dは(0,0,1)軸まわりの回転角で指定するので(0,0,-1)軸の時は反転が必要。
                var angle = R.Angle / 180f * (float)Math.PI;
                if (R.Axis.Z < 0) {
                    angle *= -1;
                }

                var body = (Body)col.Body;
                body.Position = new XnaVector2 (T.X / PPM, T.Y / PPM);
                body.Rotation = angle;
            }


            float dt = (msec - prev) / 1000f;
            wld.Step (dt);


            foreach (var node in ddworld.Downwards) {
                foreach (var comp in node.Components) {
                    comp.OnPhysicsUpdate ();
                }
            }

            this.prev = msec;
        }
        #endregion
    }
}
