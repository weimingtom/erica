using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;
using BulletVector3 = BulletSharp.Vector3;

// メモ：
// 物理演算ワールドへの登録/削除は
// PhysicsSimulatorクラスでやるので個別にやらないように


namespace DD.Physics {
    /// <summary>
    /// 剛体物理 コンポーネント
    /// </summary>
    /// <remarks>
    /// 剛体物理コンポーネントはノードに物理演算の対象となる剛体を追加します。
    /// 剛体は主に3つの要素からなります。
    /// <list type="bullet">
    ///   <item>Shape : 剛体の形状</item>
    ///   <item>Material : 摩擦係数などの物質特性</item>
    ///   <item>Massなど : 質量ほか</item>
    /// </list>
    /// これらの3要素を適切に制御することで物体の物理シミュレーションを行うことが可能です。
    /// 剛体はパラメーターによって3種類に分類することが可能です。
    /// 以下略
    /// </remarks>
    public class RigidBody : Component, System.IDisposable {

        #region Field
        CollisionShape shape;
        PhysicsMaterial material;
        int collideWith;
        int ignoreWith;
        Vector3 offset;
        float mass;
        bool useGravity;
        bool useResponse;
        Vector3 linearFactor;
        Vector3 angularFactor;
        BulletSharp.RigidBody rigidBody;
        #endregion


        #region Constructor
        /// <summary>
        /// 剛体物理オブジェクトを作成するコンストラクター
        /// </summary>
        public RigidBody () {
            this.shape = null;
            this.material = null;
            this.offset = new Vector3 (0, 0, 0);
            this.collideWith = -1;
            this.ignoreWith = 0;
            this.mass = 1;
            this.useGravity = true;
            this.useResponse = true;
            this.linearFactor = new Vector3 (1, 1, 1);
            this.angularFactor = new Vector3 (1, 1, 1);
            this.rigidBody = null;
        }
        #endregion

        #region Property
        /// <summary>
        /// ダイナミック体であることを示すフラグ
        /// </summary>
        public bool IsDynamic {
            get { return mass > 0; }
            set {
                if (rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }
                if (mass == 0) {
                    this.Mass = 1;
                }
                this.rigidBody.CollisionFlags = rigidBody.CollisionFlags & ~CollisionFlags.KinematicObject;
                this.rigidBody.ActivationState = ActivationState.ActiveTag;
            }
        }

        /// <summary>
        /// スタティック体であることを示すフラグ
        /// </summary>
        public bool IsStatic {
            get { return (mass == 0) && !rigidBody.IsKinematicObject; }
            set {
                if (rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }
                this.Mass = 0;
                this.rigidBody.CollisionFlags = rigidBody.CollisionFlags & ~CollisionFlags.KinematicObject;
                this.rigidBody.ActivationState = ActivationState.ActiveTag;
            }
        }

        /// <summary>
        /// キネマティック体であることを示すフラグ
        /// </summary>
        public bool IsKinematic {
            get { return (mass == 0) && rigidBody.IsKinematicObject; }
            set {
                if (this.rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }
                this.Mass = 0;
                this.rigidBody.CollisionFlags = rigidBody.CollisionFlags | CollisionFlags.KinematicObject;
                this.rigidBody.ActivationState = ActivationState.DisableDeactivation;
            }
        }

        /// <summary>
        /// 2Dモードへの移行
        /// </summary>
        /// <remarks>
        /// この剛体を2Dモードへ移行します。
        /// </remarks>
        public bool Use2D {
            set {
                SetLinearFactor (0, 1, 0);
                SetAngularFactor (0, 0, 1);
            }
        }

        /// <summary>
        /// 剛体物理の形状
        /// </summary>
        /// <remarks>
        /// 剛体物理の多くのメソッド・プロパティは形状を定義しないと呼び出せません（当たり前）。
        /// </remarks>
        public CollisionShape Shape {
            get { return shape; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("Shape is null");
                }
                this.shape = value;
                this.rigidBody = value.CreateRigidBody (mass);
                this.rigidBody.UserObject = this;

                // Bullet側と同期
                this.rigidBody.LinearFactor = linearFactor.ToBullet ();
                this.rigidBody.AngularFactor = angularFactor.ToBullet ();

                this.rigidBody.CollisionFlags = this.rigidBody.CollisionFlags;

                // 係数のセット
                if (material != null) {
                    this.rigidBody.Restitution = material.Restitution;
                    this.rigidBody.Friction = material.Friction;
                    this.rigidBody.RollingFriction = material.RollingFriction;
                    this.rigidBody.SetDamping (material.LinearDamping, material.AngularDamping);
                }

            }
        }

        /// <summary>
        /// 剛体物理の特性
        /// </summary>
        public PhysicsMaterial Material {
            get { return material; }
            set {
                if (value == null) {
                    throw new ArgumentNullException ("Material is null");
                }
                this.material = value;

                // 係数のセット
                if (rigidBody != null) {
                    this.rigidBody.Restitution = material.Restitution;
                    this.rigidBody.Friction = material.Friction;
                    this.rigidBody.RollingFriction = material.RollingFriction;
                    this.rigidBody.SetDamping (material.LinearDamping, material.AngularDamping);
                }
            }
        }

        /// <summary>
        /// コリジョン対象ビットフラグ
        /// </summary>
        /// <remarks>
        /// この剛体が衝突する可能性のあるグループをビットフラグで表します。
        /// 1が立っているビットが衝突します。
        /// デフォルトは -1（すべてのビットが1）です。
        /// </remarks>
        public int CollideWith {
            get { return collideWith; }
            set { this.collideWith = value; }
        }

        /// <summary>
        /// 無視するコリジョン対象ビットフラグ
        /// </summary>
        /// <remarks>
        /// この剛体が衝突を無視するグループをビットフラグで表します。
        /// このフラグは <see cref="CollideWith"/> より優先するので、
        /// ここにビットが立っているグループは <see cref="CollideWith"/> の値にかかわらず無視されます。
        /// デフォルトは 0（すべてのビットが0） です。
        /// </remarks>
        public int IgnoreWith {
            get { return ignoreWith; }
            set { this.ignoreWith = value; }
        }

        /// <summary>
        /// オフセット
        /// </summary>
        /// <remarks>
        /// 質量中心を指定のオフセットになるように移動します。
        /// デフォルトではすべての形状で質量中心は(0,0,0)です。
        /// </remarks>
        public Vector3 Offset {
            get { return offset; }
            set { this.offset = value; }
        }

        /// <summary>
        /// 重力制御フラグ
        /// </summary>
        /// <remarks>
        /// 重力の影響を受けるか受けないかは剛体毎に変更可能で、
        /// このプロパティが <c>true</c> の時、ワールドに設定された重力が適用されます。
        /// デフォルトは <c>true</c> です。
        /// </remarks>
        public bool UseGravity {
            get { return useGravity; }
            set {
                if (this.rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }

                this.useGravity = value;
                if (value) {
                    this.rigidBody.Flags = rigidBody.Flags & ~RigidBodyFlags.DisableWorldGravity;
                }
                else {
                    this.rigidBody.Flags = rigidBody.Flags | RigidBodyFlags.DisableWorldGravity;
                }
            }
        }

        /// <summary>
        /// レスポンス制御フラグ
        /// </summary>
        /// <remarks>
        /// このフラグが <c>true</c> の時は物体を押し返します。
        /// デフォルトは <c>true</c> です。
        /// </remarks>
        public bool UseResponse {
            get { return useResponse; }
            set {
                if (this.rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }

                this.useResponse = value;
                if (value) {
                    this.rigidBody.CollisionFlags = rigidBody.CollisionFlags & ~CollisionFlags.NoContactResponse;
                }
                else {
                    this.rigidBody.CollisionFlags = rigidBody.CollisionFlags | CollisionFlags.NoContactResponse;
                }
            }
        }

        /// <summary>
        /// 剛体の質量
        /// </summary>
        /// <remarks>
        /// 0を指定すると static 体になります。
        /// </remarks>
        public float Mass {
            get { return mass; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("Mass is invalid");
                }
                this.mass = value;
                if (rigidBody != null) {
                    var inertia = rigidBody.CollisionShape.CalculateLocalInertia (value);
                    this.rigidBody.SetMassProps (value, inertia);
                    this.rigidBody.UpdateInertiaTensor ();
                }
            }
        }


        /// <summary>
        /// 線形ファクター
        /// </summary>
        /// <remarks>
        /// 物理計算の結果得られた速度はこのファクターで乗算した結果が適応されます。
        /// すなわちここで 0 を指定した座標軸は位置が固定されます。
        /// 剛体の移動を2次元に制限したい場合は、このメソッドを使って制限してもかまいませんが、
        /// すでに用意されている <see cref="Box2DShape"/>, <see cref="Sphere2DShape"/> を使用した方が簡単です。
        /// </remarks>
        public Vector3 LinearFactor {
            get {
                return linearFactor;
            }
            set {
                SetLinearFactor (value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// 回転ファクター
        /// </summary>
        /// <remarks>
        /// 物理計算の結果得られた回転速度はこのファクターで乗算した結果が適応されます。
        /// すなわちここで 0 を指定した回転軸は位置が固定されます。
        /// 剛体の移動を2次元に制限したい場合は、このメソッドを使って制限してもかまいませんが、
        /// すでに用意されている <see cref="Box2DShape"/>, <see cref="Sphere2DShape"/> を使用した方が簡単です。
        /// </remarks>
        public Vector3 AngularFactor {
            get { return angularFactor; }
            set {
                SetAngularFactor (value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// 剛体の速度
        /// </summary>
        /// <remarks>
        /// 物理計算によって得られた剛体の速度です。
        /// </remarks>
        public Vector3 Velocity {
            get {
                if (rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }
                return this.rigidBody.LinearVelocity.ToDD ();
            }
        }

        /// <summary>
        /// 剛体の回転速度
        /// </summary>
        /// <remarks>
        /// 物理計算によって得られた剛体の回転速度です。
        /// 単位は毎秒あたりに回転する度数 (in degree) です。
        /// </remarks>
        public Vector3 AngularVelocity {
            get {
                if (rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }
                return this.rigidBody.AngularVelocity.ToDD ();
            }
        }

        /// <summary>
        /// 剛体に付加された力の合計
        /// </summary>
        /// <remarks>
        /// 直前の物理シミュレーションステップでこの剛体に付加された力の合計です。
        /// </remarks>
        public Vector3 Force {
            get {
                if (rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }
                return this.rigidBody.TotalForce.ToDD ();
            }
        }

        /// <summary>
        /// 剛体に付加されたトルクの合計
        /// </summary>
        /// <remarks>
        /// 直前の物理シミュレーションステップでこの剛体に付加されたトルクの合計です。
        /// </remarks>
        public Vector3 Torque {
            get {
                if (rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }
                return this.rigidBody.TotalTorque.ToDD ();
            }
        }

        /// <summary>
        /// アクティブ フラグ
        /// </summary>
        /// <remarks>
        /// 現在物理シミュレーションの計算対象に含まれているかどうかのフラグです。
        /// 多くの物理シミュレーションでは計算付加の低減のため、静止状態の剛体をスリープ状態に移行し計算を省略します。
        /// 通常はこのフラグをいじる必要性はありまあ線。
        /// </remarks>
        public bool IsActive {
            get {
                if (rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }
                return this.rigidBody.IsActive;
            }
            set {
                if (rigidBody == null) {
                    throw new InvalidOperationException ("RigidBody is null");
                }
                this.rigidBody.Activate ();
            }
        }

        /// <summary>
        /// 内部仕様のデータ
        /// </summary>
        /// <remarks>
        /// 内部使用の BulletPhsyics の RigidBody クラスを返します。
        /// ユーザーはこのプロパティを使用してはいけません。
        /// </remarks>
        internal BulletSharp.RigidBody Data {
            get { return rigidBody; }
        }


        #endregion



        #region Method
        /// <summary>
        /// オフセットの変更
        /// </summary>
        /// <param name="x">オフセットのX座標（ローカル座標）</param>
        /// <param name="y">オフセットのY座標（ローカル座標）</param>
        /// <param name="z">オフセットのZ座標（ローカル座標）</param>
        public void SetOffset (float x, float y, float z) {
            this.offset = new Vector3 (x, y, z);

        }

        /// <summary>
        /// 剛体に力の付加
        /// </summary>
        /// <remarks>
        /// 質量中心に指定の力を加えます。
        /// </remarks>
        /// <param name="x">力ベクトルのX（ローカル座標）</param>
        /// <param name="y">力ベクトルのY（ローカル座標）</param>
        /// <param name="z">力ベクトルのZ（ローカル座標）</param>
        public void ApplyForce (float x, float y, float z) {
            if (rigidBody == null) {
                throw new InvalidOperationException ("RigidBody is null");
            }
            this.rigidBody.ApplyCentralForce (new BulletVector3 (x, y, z));
        }

        /// <summary>
        /// 剛体に力の付加
        /// </summary>
        /// <remarks>
        /// 指定の地点にに指定の力を加えます。
        /// </remarks>
        /// <param name="force">力ベクトルのX（ローカル座標）</param>
        /// <param name="pointOfAction">作用点（ローカル座標）</param>
        public void ApplyForce (Vector3 force, Vector3 pointOfAction) {
            if (rigidBody == null) {
                throw new InvalidOperationException ("RigidBody is null");
            }
            this.rigidBody.ApplyForce (force.ToBullet (), pointOfAction.ToBullet ());
        }

        /// <summary>
        /// 剛体に力積の付加
        /// </summary>
        /// <remarks>
        /// 質量中心に指定の力積（力x時間）を加えます。
        /// </remarks>
        /// <param name="x">力積ベクトルのX（ローカル座標）</param>
        /// <param name="y">力積ベクトルのY（ローカル座標）</param>
        /// <param name="z">力積ベクトルのZ（ローカル座標）</param>
        public void ApplyImpulse (float x, float y, float z) {
            if (rigidBody == null) {
                throw new InvalidOperationException ("RigidBody is null");
            }
            this.rigidBody.ApplyCentralImpulse (new BulletVector3 (x, y, z));
        }

        /// <summary>
        /// 剛体に力積の付加
        /// </summary>
        /// <remarks>
        /// 指定の地点にに指定の力積を加えます。
        /// </remarks>
        /// <param name="impulse">力積ベクトルのX（ローカル座標）</param>
        /// <param name="pointOfAction">作用点（ローカル座標）</param>
        public void ApplyImpulse (Vector3 impulse, Vector3 pointOfAction) {
            if (rigidBody == null) {
                throw new InvalidOperationException ("RigidBody is null");
            }
            this.rigidBody.ApplyImpulse (impulse.ToBullet (), pointOfAction.ToBullet ());
        }

        /// <summary>
        /// 剛体にトルクの付加
        /// </summary>
        /// <remarks>
        /// 質量中心に指定のトルクを加えます。
        /// </remarks>
        /// <param name="x">トルクのX（ローカル座標）</param>
        /// <param name="y">トルクのY（ローカル座標）</param>
        /// <param name="z">トルクのZ（ローカル座標）</param>
        public void ApplyTorque (float x, float y, float z) {
            if (rigidBody == null) {
                throw new InvalidOperationException ("RigidBody is null");
            }
            this.rigidBody.ApplyTorque (new BulletVector3 (x, y, z));

        }

        /// <summary>
        /// 剛体にトルクの付加
        /// </summary>
        /// <remarks>
        /// 指定の地点にに指定のトルクを加えます。
        /// </remarks>
        /// <param name="torque">トルク（ローカル座標）</param>
        public void ApplyTorque (Vector3 torque) {
            if (rigidBody == null) {
                throw new InvalidOperationException ("RigidBody is null");
            }
            this.rigidBody.ApplyTorque (torque.ToBullet ());
        }


        /// <summary>
        /// 剛体にトルク積の付加
        /// </summary>
        /// <remarks>
        /// 剛体に指定のトルク積（トルクx時間）を加えます。
        /// </remarks>
        /// <param name="torque">トルク積（ローカル座標）</param>
        public void ApplyTorqueImpulse (float x, float y, float z) {
            if (rigidBody == null) {
                throw new InvalidOperationException ("RigidBody is null");
            }
            this.rigidBody.ApplyTorqueImpulse (new BulletVector3 (x, y, z));

        }

        /// <summary>
        /// 剛体にトルク積の付加
        /// </summary>
        /// <remarks>
        /// 剛体に指定のトルク積（トルクx時間）を加えます。
        /// </remarks>
        /// <param name="torque">トルク積（ローカル座標）</param>
        public void ApplyTorqueImpulse (Vector3 torqueImpulse) {
            if (rigidBody == null) {
                throw new InvalidOperationException ("RigidBody is null");
            }
            this.rigidBody.ApplyTorqueImpulse (torqueImpulse.ToBullet ());

        }

        /// <summary>
        /// 線形ファクターの変更
        /// </summary>
        /// <param name="x">線形ファクターのx</param>
        /// <param name="y">線形ファクターのy</param>
        /// <param name="z">線形ファクターのz</param>
        public void SetLinearFactor (float x, float y, float z) {
            if (x < 0 || x > 1 || y < 0 || y > 1 || z < 0 || z > 1) {
                throw new ArgumentException ("LinearFactor is invalid");
            }
            this.linearFactor = new Vector3 (x, y, z);
            if (rigidBody != null) {
                this.rigidBody.LinearFactor = new BulletVector3 (x, y, z);
            }
        }

        /// <summary>
        /// 回転ファクターの変更
        /// </summary>
        /// <param name="x">X軸まわりの回転ファクター</param>
        /// <param name="y">Y軸まわりの回転ファクター</param>
        /// <param name="z">Z軸まわりの回転ファクター</param>
        public void SetAngularFactor (float x, float y, float z) {
            if (x < 0 || x > 1 || y < 0 || y > 1 || z < 0 || z > 1) {
                throw new ArgumentException ("AngularFactor is invalid");
            }
            this.angularFactor = new Vector3 (x, y, z);
            if (rigidBody != null) {
                this.rigidBody.AngularFactor = new BulletVector3 (x, y, z);
            }
        }

        /// <inheritdoc/>
        public override void OnPhysicsUpdateInit (long msec) {
            if (rigidBody != null) {
                var phys = World.PhysicsSimulator;
                if (!phys.IsRegistered (Node)) {
                    // 本当はオフセットが必要
                    phys.AddRigidBody (Node);
                }
            }
        }

        /// <inheritdoc/>
        public override void OnPhysicsUpdate (long msec) {
            // 物理演算ワールドの座標をDD側に反映
            // PPM補正を忘れずに
            // 本当はオフセットが必要
            var T = rigidBody.CenterOfMassPosition.ToDD() * PhysicsSimulator.PPM;
            var R = rigidBody.Orientation.ToDD ();
            var S = new Vector3(1,1,1);

            Node.GlobalTransform = Matrix4x4.Compress(T, R, S);
            // +Matrix.CreateFromTranslation(-body.Offset)
        }

        /// <inheritdoc/>
        void System.IDisposable.Dispose () {
            if (rigidBody != null) {
                // すでにデタッチされているので明示的に物理演算ワールドから削除する事ができない
                // これまでの所問題になっていないので多分これで大丈夫。
                rigidBody.Dispose ();
                this.rigidBody = null;
            }
        }

        #endregion


    }
}
