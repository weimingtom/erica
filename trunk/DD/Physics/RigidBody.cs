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
    ///   <item>Massなど : 質量ほか制御変数</item>
    /// </list>
    /// これらの3要素を適切に制御することで物体の物理シミュレーションを行うことが可能です。
    /// 剛体はパラメーターによって3種類に分類することが可能です。
    /// 
    /// 剛体の種類
    /// 物理演算の完全なコントロール下に置かれるダイナミック体とスタティック体、それに
    /// ユーザーがコントロール可能で他の物体をはじき飛ばすことが可能なキネマティック体の3種類が存在します。
    /// キネマティック体は現在未実装です。
    /// 
    /// コリジョン形状
    /// 現在の所コリジョン形状は1つしか対応していません。
    /// 将来的には複数の形状で1つの複雑なコリジョン形状を表現できるようにするつもりです。
    /// 
    /// コリジョン オフセット
    /// 現在機能しません。従って質量中心は必ずノードの原点(0,0,0)になります。
    /// </remarks>
    public class RigidBody : Component, System.IDisposable {

        #region Field
        List<CollisionShape> shapes;
        List<Vector3> offsets;    // 個別オフセット
        PhysicsMaterial material;
        int collideWith;
        int ignoreWith;
        Vector3 offset;   // 全体オフセット
        float mass;
        bool useGravity;
        bool useContactResponse;
        bool useCCD;
        Vector3 linearFactor;
        Vector3 angularFactor;
        BulletSharp.RigidBody rigidBody;
        #endregion


        #region Constructor
        /// <summary>
        /// 剛体物理オブジェクトを作成するコンストラクター
        /// </summary>
        public RigidBody () {
            this.shapes = new List<CollisionShape> ();
            this.offsets = new List<Vector3> ();
            this.material = null;
            this.offset = new Vector3 (0, 0, 0);
            this.collideWith = -1;
            this.ignoreWith = 0;
            this.mass = 1;
            this.useGravity = true;
            this.useContactResponse = true;
            this.useCCD = true;
            this.linearFactor = new Vector3 (1, 1, 1);
            this.angularFactor = new Vector3 (1, 1, 1);
            this.rigidBody = new BulletSharp.RigidBody(new BulletSharp.RigidBodyConstructionInfo (mass,
                                                                                                  new DefaultMotionState (),
                                                                                                  new CompoundShape(true)));
    
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
                SetLinearFactor (1, 1, 0);
                SetAngularFactor (0, 0, 1);
            }
        }


        /// <summary>
        /// コリジョン形状の数
        /// </summary>
        /// <remarks>
        /// コリジョン形状は複数セット可能で複雑な形状を表現可能ですが、
        /// 現在の所1つしか対応していません。
        /// このメソッドは常に1を返します。
        /// </remarks>
        public int ShapeCount {
            get { return shapes.Count(); }
        }

        /// <summary>
        /// すべてのコリジョン形状を列挙する列挙子
        /// </summary>
        public IEnumerable<CollisionShape> Shapes {
            get { return shapes; }
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
        /// 全体オフセット
        /// </summary>
        /// <remarks>
        /// 質量中心が指定のオフセットになるように移動します。
        /// デフォルトではすべての形状で質量中心は(0,0,0)です。
        /// このプロパティは未実装です。
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
                this.useGravity = value;
            }
        }

        /// <summary>
        /// レスポンス制御フラグ
        /// </summary>
        /// <remarks>
        /// このフラグが <c>true</c> の時は物体を押し返します。
        /// デフォルトは <c>true</c> です。
        /// </remarks>
        public bool UseContactResponse {
            get { return useContactResponse; }
            set {
                this.useContactResponse = value;
            }
        }

        /// <summary>
        /// 連続コリジョン判定モード
        /// </summary>
        /// <remarks>
        /// 現在このプロパティは未実装です
        /// </remarks>
        public bool UseCCD {
            get { return useCCD; }
            set {
                this.useCCD = value;
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
            }
        }


        /// <summary>
        /// 線形ファクター
        /// </summary>
        /// <remarks>
        /// 物理計算の結果得られた速度はこのファクターで乗算した結果が適応されます。
        /// すなわちここで 0 を指定した座標軸は位置が固定されます。
        /// 剛体の移動を2次元に制限したい場合は、このメソッドを使って制限するより
        /// <see cref="Use2D"/> プロパティを使用した方が簡単です。
        /// </remarks>
        /// <seealso cref="Use2D"/>
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
        /// 物理計算の結果得られた回転速度に、このファクターを乗算した結果が適応されます。
        /// すなわち物理演算の結果を人為的に増強（または弱く）させます。
        /// ここで 0 を指定した回転は固定されます。
        /// 剛体の移動および回転を2次元に制限したい場合は、このメソッドを使って制限するより
        /// <see cref="Use2D"/> プロパティを使用する方が簡単です。
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
        /// <param name="x">トルク積のX成分</param>
        /// <param name="y">トルク積のY成分</param>
        /// <param name="z">トルク積のZ成分</param>
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
        /// <param name="torqueImpulse">トルク積（ローカル座標）</param>
        public void ApplyTorqueImpulse (Vector3 torqueImpulse) {
            if (rigidBody == null) {
                throw new InvalidOperationException ("RigidBody is null");
            }
            this.rigidBody.ApplyTorqueImpulse (torqueImpulse.ToBullet ());

        }

        /// <summary>
        /// 線形ファクターの変更
        /// </summary>
        /// <remarks>
        /// 物理演算の結果得られた速度に対して、最後にこのファクターを乗算して物理ワールドに適応します。
        /// [0,1]が有効です。
        /// </remarks>
        /// <param name="x">線形ファクターのx成分</param>
        /// <param name="y">線形ファクターのy成分</param>
        /// <param name="z">線形ファクターのz成分</param>
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
        /// <remarks>
        /// 物理演算の結果得られた速度に対して、最後にこのファクターを乗算して物理ワールドに適応します。
        /// [0,1]が有効です。
        /// </remarks>
        /// <param name="x">X軸まわりの回転ファクター</param>
        /// <param name="y">Y軸まわりの回転ファクター</param>
        /// <param name="z">Z軸まわりの回転ファクター</param>
        public void SetAngularFactor (float x, float y, float z) {
            if (x < 0 || x > 1 || y < 0 || y > 1 || z < 0 || z > 1) {
                throw new ArgumentException ("AngularFactor is invalid");
            }
            this.angularFactor = new Vector3 (x, y, z);
        }

        /// <summary>
        /// オフセットを指定して形状の追加
        /// </summary>
        /// <remarks>
        /// 現在このメソッドは未実装です。従ってコリジョン オフセットの機能は使用できません。
        /// </remarks>
        /// <param name="shape"></param>
        /// <param name="offset"></param>
        public void AddShape (CollisionShape shape, Vector3 offset) {
            throw new NotImplementedException ("Sorry");
            //this.shapes.Add (shape);
            //this.offsets.Add (offset);
        }

        /// <summary>
        /// コリジョン形状の追加
        /// </summary>
        /// <remarks>
        /// コリジョン形状は複数セット可能ですが、現在1つしか対応していません。
        /// </remarks>
        /// <param name="shape">コリジョン形状</param>
        public void AddShape (CollisionShape shape) {
            if (shape == null) {
                throw new ArgumentNullException ("Collision shape is null");
            }
            if (shapes.Count () >= 1) {
                throw new NotImplementedException ("Sorry, CollisionShapes must be single");
            }

            this.shapes.Add (shape);
            this.offsets.Add (new Vector3 (0, 0, 0));

            var shp = rigidBody.CollisionShape as CompoundShape;
            shp.AddChildShape (Matrix.Identity, shape.CreateBulletShape ());
        }

        /// <summary>
        /// コリジョン形状の取得
        /// </summary>
        /// <param name="index">形状インデックス</param>
        /// <returns></returns>
        public CollisionShape GetShape (int index) {
            if (index < 0 || index > ShapeCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            return shapes[index];
        }

        /// <summary>
        /// コリジョン形状の削除
        /// </summary>
        /// <param name="index">形状インデックス</param>
        public void RemoveShape (int index) {
            this.shapes.RemoveAt (index);
            this.offsets.RemoveAt (index);

            var compShape = rigidBody.CollisionShape as CompoundShape;
            compShape.RemoveChildShapeByIndex (index);
        }


        /// <summary>
        /// すべてのDDワールドのパラメーターを物理演算ワールドに反映
        /// </summary>
        /// <remarks>
        /// 物理演算に関するすべてのパラメーターを物理演算ワールドに反映します。
        /// 現在の所登録後にパラメーターの変更を自動で検出して必要に応じて物理演算ワールドに反映する仕組みは実装されていません。
        /// 従って明示的にこのメソッドを呼び出してパラメーターを反映する必要があります。
        /// （物理演算ワールドに登録する最初の1回だけは自動で呼び出されます）
        /// <note>
        /// 重力の無効化はAddRigidBody()より前に実行されないと効果がない。
        /// </note>
        /// </remarks>
        public void SyncWithPhysicsWorld () {

            // 座標の更新
            var T = Node.Position  / PhysicsSimulator.PPM;
            var R = Node.Orientation;
            var S = new Vector3 (1, 1, 1);
            rigidBody.WorldTransform = Matrix4x4.Compress (T, R, S).ToBullet ();

            
            // 重量の更新
            var inertia = rigidBody.CollisionShape.CalculateLocalInertia (mass);
            rigidBody.SetMassProps (mass, inertia);
            rigidBody.UpdateInertiaTensor ();
            
            // 剛体パラメーターの更新
            rigidBody.LinearFactor = linearFactor.ToBullet ();
            rigidBody.AngularFactor = angularFactor.ToBullet ();
            rigidBody.Flags = (useGravity)
                               ? (rigidBody.Flags & ~RigidBodyFlags.DisableWorldGravity) 
                               : (rigidBody.Flags |  RigidBodyFlags.DisableWorldGravity);
            rigidBody.CollisionFlags = (useContactResponse)
                               ? (rigidBody.CollisionFlags & ~CollisionFlags.NoContactResponse)
                               : (rigidBody.CollisionFlags | CollisionFlags.NoContactResponse);
            if (useCCD) {
                // 未実装
                //rigidBody.CcdMotionThreshold = 1;
                //rigidBody.CcdSweptSphereRadius = 0.9f * compShape.InSphereRadius;
            }


            // マテリアル パラメーター(null可)の更新
            if (material != null) {
                rigidBody.Restitution = material.Restitution;
                rigidBody.Friction = material.Friction;
                rigidBody.RollingFriction = material.RollingFriction;
                rigidBody.SetDamping (material.LinearDamping, material.AngularDamping);
            }

        }

        /// <inheritdoc/>
        public override void OnPhysicsUpdateInit (long msec) {
            //if (rigidBody != null) {
                var phys = World.PhysicsSimulator;
                if (!phys.IsRegistered (Node)) {
                    // （メモ）
                    // UseGravityの変更はAddRigidBody()より
                    // 前に行う必要がある
                    SyncWithPhysicsWorld ();
                    phys.AddRigidBody (Node);
                 }
            //}
        }


        /// <inheritdoc/>
        public override void OnPhysicsUpdate (long msec) {
            // 物理演算ワールドの座標をDD側に反映
            // PPM補正を忘れずに!
            var T = rigidBody.WorldTransform.Origin.ToDD () * PhysicsSimulator.PPM;
            var R = rigidBody.Orientation.ToDD ();
            var S = new Vector3(1,1,1);

            Node.GlobalTransform = Matrix4x4.Compress(T, R, S);
        }

        // OnDestroyed()で物理演算ワールドから取り除くべき

        /// <inheritdoc/>
        void System.IDisposable.Dispose () {
            if (rigidBody != null) {
                // （注意）すでにデタッチされているので明示的に物理演算ワールドから削除する事ができない
                // これまでの所問題になっていないので多分これで大丈夫。
                rigidBody.Dispose ();
                this.rigidBody = null;
            }
        }

        #endregion


    }
}
