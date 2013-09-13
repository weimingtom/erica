using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;

using BulletVector3 = BulletSharp.Vector3;

namespace DD.Physics {
    /// <summary>
    /// 物理シミューレーター コンポーネント
    /// </summary>
    /// <remarks>
    /// 物理シミュレーションを実行するクラスです。
    /// <see cref="World"/> クラスはデフォルトの <see cref="PhysicsSimulator"/> コンポーネントを持ち、
    /// 通常はこれを使用すれば十分です。
    /// </remarks>
    public class PhysicsSimulator : Component, System.IDisposable {
        #region Field
        DiscreteDynamicsWorld wld;
        Dispatcher dispatcher;
        ConstraintSolver solver;
        BroadphaseInterface broadphase;
        static float ppm = 64;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public PhysicsSimulator () {
            var cc = new DefaultCollisionConfiguration ();
            
            this.dispatcher = new CollisionDispatcher (cc);
            this.broadphase = new DbvtBroadphase ();
            this.solver = new SequentialImpulseConstraintSolver ();
            this.wld = new DiscreteDynamicsWorld (dispatcher, broadphase, solver, cc);
        
            this.wld.Gravity = new BulletSharp.Vector3 (0, -9.8f / ppm, 0);   // 重力は-Y方向
        }
        #endregion

        #region Property
        /// <summary>
        /// 物理演算ワールド
        /// </summary>
        /// <remarks>
        /// BulletSharpの物理演算用のワールドです。
        /// ユーザーは通常このプロパティを使用することはありません。
        /// </remarks>
        public BulletSharp.DiscreteDynamicsWorld PhysicsWorld {
            get { return wld; }
        }

        /// <summary>
        /// 1メートルあたりのピクセル数
        /// </summary>
        /// <remarks>
        /// BulletPhysics は精度良く演算するために動的物体のサイズが 0.2 ～ 5.0 [m]程度である必要があります。
        /// 従って画面サイズを800x600として 1ピクセル を 1m として計算すると多くの場合不正な結果が得られます。
        /// このプロパティは物理演算ワールドの1mがDDワールドの何単位に相当するかを決定します。
        /// 通常はDDワールドの1単位は1ピクセルとして画面に表示されます。
        /// デフォルトは 64 です。10ピクセル～300ピクセル程度の動的物体を精度良く計算可能です。
        /// <note>
        /// （注意）これ意外とクリティカルに効いてくるのできちんと 0.2 ～ 5.0 に収まるように PPM を調整すること。
        /// 通常はデフォルト値の64で十分だが、例えばシューティングゲームなどで 10 ピクセル以下の弾丸を使用する場合は、
        /// もう少し小さい値にしないと不正な結果が得られる。
        /// </note>
        /// </remarks>
        public static float PPM {
            get { return ppm; }
            set {
                if (ppm <= 0 || ppm > 1024) {
                    throw new ArgumentException ("PPM(Pixel Per Meter) is invalid.");
                }
                ppm = value;
            }
        }

        /// <summary>
        /// 物理演算ワールドに登録されている剛体の総数
        /// </summary>
        public int RigidBodyCount {
            get { return wld.CollisionObjectArray.Count (); }
        }

        /// <summary>
        /// 物理演算ワールドに登録されたすべての剛体を列挙する列挙子
        /// </summary>
        public IEnumerable<RigidBody> RigidBodies {
            get { return wld.CollisionObjectArray.Select (x => x.UserObject as RigidBody); }
        }

        /// <summary>
        /// 重力
        /// </summary>
        /// <remarks>
        /// 物理演算ワールド全体に設定された重力です。
        /// 重力の影響は剛体毎にON/OFFできます。
        /// </remarks>
        public Vector3 Gravity {
            get { return wld.Gravity.ToDD () * ppm;}
            set { 
                SetGravity (value.X, value.Y, value.Z);
            }
        }
        #endregion


        #region Method
        /// <summary>
        /// 重力の変更
        /// </summary>
        /// <param name="x">重力加速度のX成分（ワールド座標）</param>
        /// <param name="y">重力加速度のY成分（ワールド座標）</param>
        /// <param name="z">重力加速度のZ成分（ワールド座標）</param>
        public void SetGravity (float x, float y, float z) {
            this.wld.Gravity = new BulletSharp.Vector3 (x, y, z) / ppm;
        }

        /// <summary>
        /// 剛体を物理演算ワールドに登録
        /// </summary>
        /// <remarks>
        /// すでに登録済みの場合何もしません。
        /// </remarks>
        /// <param name="node">剛体をアタッチされたノード</param>
        /// <param name="globalTransform">変換行列</param>
        public void AddRigidBody (Node node) {
            if (node == null) {
                throw new ArgumentNullException ("Node is null");
            }
            if (!node.Is<RigidBody> ()) {
                throw new ArgumentException ("Node is not RigidBody");
            }
            if (IsRegistered(node)) {
                return;
            }
            var rb = node.GetComponent<RigidBody> ();
            if (rb.Shape == null) {
                throw new InvalidOperationException ("RigidBody shape is null");
            }

            rb.Data.WorldTransform = node.GlobalTransform.ToBullet() / ppm;
            wld.AddRigidBody (rb.Data);
        }

        /// <summary>
        /// 剛体を物理演算ワールドから削除
        /// </summary>
        /// <remarks>
        /// 登録されていなかった場合何もしません。
        /// </remarks>
        /// <param name="node">剛体をアタッチされたノード</param>
        public void RemoveRigidBody (Node node) {
            if (node == null) {
                throw new ArgumentNullException ("Node is null");
            }
            if (!node.Is<RigidBody> ()) {
                throw new ArgumentException ("Node is not RigidBody");
            }
            if (!IsRegistered (node)) {
                return;
            }
            var rb = node.GetComponent<RigidBody> ();
            if (rb.Shape == null) {
                throw new InvalidOperationException ("RigidBody shape is null");
            }

            wld.RemoveRigidBody (rb.Data);
        }

        /// <summary>
        ///剛体が物理演算ワールドに登録されているかどうかの問い合わせ
        /// </summary>
        /// <param name="node">剛体がアタッチされたノード</param>
        /// <returns></returns>
        public bool IsRegistered (Node node) {
            if (node == null) {
                return false;
            }
            if (!node.Is<RigidBody> ()) {
                throw new ArgumentException ("Node is not RigidBody");
            }
            var rb = node.GetComponent<RigidBody> ();
            return wld.CollisionObjectArray.Contains (rb.Data);
        }

        /// <summary>
        /// 物理シミュレーションの実行
        /// </summary>
        /// <remarks>
        /// 物理シミュレーションを実行します。
        /// BulletPhysicsは <paramref name="msec"/> の値にかかわらず 1/60 秒を1ステップとして計算します。
        /// それより短いステップ時間を指定した場合、実際にはシミュレーションを行わず単に補間した値を返します。
        /// それより長いステップ時間を指定した場合、複数回のシミュレーションが行われます。
        /// 通常はこれらを気にする必要はありません。
        /// </remarks>
        /// <param name="msec">ステップ時間 (msec)</param>
        public void Step (long msec) {

            // 物理シミュレーション
            wld.StepSimulation (msec / 1000f);
        }

        
        /// <inheritdoc/>
        void System.IDisposable.Dispose () {
            if (wld != null) {
                dispatcher.Dispose ();
                broadphase.Dispose ();
                solver.Dispose ();
                wld.Dispose ();
                this.dispatcher = null;
                this.broadphase = null;
                this.solver = null;
                this.wld = null;
            }
        }
        #endregion

    }
}
