using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using System.Diagnostics;
using Microsoft.Xna.Framework;

using XnaVector2 = Microsoft.Xna.Framework.Vector2;

namespace DD.Physics {
    /// <summary>
    /// コライダー コンポーネント
    /// </summary>
    /// <remarks>
    /// 衝突判定と物理エンジンによる挙動制御を行うコンポーネントです。
    /// ノードにこのコンポーネントをアタッチする事で、ノードは物理エンジンによってコントロールされるようになります。
    /// コライダーは制御方法の違いで (1) ダイナミック, (2) スタティック, (3) キネマティックの3種類があります。
    /// これとは別にトリガー モードと呼ばれる物理作用無しで衝突のみを判定する場合があります。
    /// </remarks>
    public class Collider : Component {

        #region Field
        Body body;
        CollisionShape shape;
        PhysicsMaterial mat;
        int hash;
        uint mask;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// デフォルトはスタティック オブジェクト。
        /// </remarks>
        public Collider () {
            var wld = Physics2D.GetInstance ().GetWorld () as FarseerPhysics.Dynamics.World;
            if (wld == null) {
                throw new InvalidOperationException ("Physics world is not created");
            }
            this.mask = 0xffffffff;
            this.shape = null;
            this.mat = null;
            this.hash = 0;

            this.body = new Body (wld, this);
 
        }
        #endregion

        #region Property
        /// <summary>
        /// コリジョン形状
        /// </summary>
        /// <remarks>
        /// このコライダーに設定されているコリジョン形状を取得するプロパティです。
        /// </remarks>
        public CollisionShape Shape {
            get { return shape; }
            set { SetShape (value); }
        }

        /// <summary>
        /// 物理素材
        /// </summary>
        /// <remarks>
        /// このコライダーに設定されている物理素材を所得するプロパティです。
        /// コライダーは形状と材質をセットすると物理オブジェクトとしてふるまいます。
        /// </remarks>
        public PhysicsMaterial Material {
            get { return mat; }
            set { SetMaterial (value); }
        }

        /// <summary>
        /// ボディ
        /// </summary>
        /// <remarks>
        /// 物理エンジンで使用するオブジェクト。
        /// Farseer.Dynamics.Bodyにキャストして使う。
        /// ユーザーはこのプロパティを使用しません。
        /// </remarks>
        public object Body {
            get { return body; }
        }

        /// <summary>
        /// コライダー タイプ
        /// </summary>
        /// <remarks>
        /// コライダーは振る舞いから(1) ダイナミック (2) スタティック (3) キネマティックの3種類があります。
        /// </remarks>
        public ColliderType Type {
            get {
                switch (body.BodyType) {
                    case BodyType.Dynamic: return ColliderType.Dynamic;
                    case BodyType.Static: return ColliderType.Static;
                    case BodyType.Kinematic: return ColliderType.Kinematic;
                    default: throw new NotImplementedException ("Sorry");
                }
            }
            set {
                switch (value) {
                    case ColliderType.Dynamic: this.body.BodyType = BodyType.Dynamic; break;
                    case ColliderType.Static: this.body.BodyType = BodyType.Static; break;
                    case ColliderType.Kinematic: this.body.BodyType = BodyType.Kinematic; break;
                    default: throw new NotImplementedException ("Sorry");
                }
            }
        }

        /// <summary>
        /// キネマティック タイプ
        /// </summary>
        /// <remarks>
        /// このコライダーがキネマティックかどうかを取得するプロパティです。
        /// </remarks>
        public bool IsKinematic {
            get { return (body.BodyType == BodyType.Kinematic); }
        }

        /// <summary>
        /// スタティック タイプ
        /// </summary>
        /// <remarks>
        /// このコライダーがスタティックかどうかを取得するプロパティです。
        /// </remarks>
        public bool IsStatic {
            get { return (body.BodyType == BodyType.Static); }
        }

        /// <summary>
        /// ダイナミック タイプ
        /// </summary>
        /// <remarks>
        /// このコライダーがダイナミックかどうかを取得するプロパティです。
        /// </remarks>
        public bool IsDynamic {
            get { return (body.BodyType == BodyType.Dynamic); }
        }

        /// <summary>
        /// 有効・無効の制御
        /// </summary>
        /// <remarks>
        /// このコライダー コンポーネントを（一時的に）有効化または無効化します。
        /// ボディ作成前は必ず <c>false</c> が返ります。
        /// </remarks>
        public bool IsEnabled {
            get { return body.Enabled; }
            set { this.body.Enabled = value; }
        }

        /// <summary>
        /// コライダーの休止状態
        /// </summary>
        /// <remarks>
        /// この物理オブジェクトが休止状態かどうかを取得、設定します。
        /// このプロパティは他のプロパティと違い物理エンジンから直接値を取得します。
        /// ボディ作成前は必ず <c>false</c> が返ります。
        /// </remarks>
        public bool IsSleeping {
            get { return !body.Awake; }
            set { this.body.Awake = !value; }
        }

        /// <summary>
        /// 重力制御
        /// </summary>
        /// <remarks>
        /// このコライダーが重力の影響を受けるかどうかを取得、設定します。
        /// </remarks>
        public bool IsGravitational {
            get { return !body.IgnoreGravity; }
            set { this.body.IgnoreGravity = !value; }
        }

        /// <summary>
        /// 回転固定
        /// </summary>
        /// <remarks>
        /// このコライダーが回転を固定するかどうかを取得、設定します。
        /// ゲームにとって回転固定は必須です。
        /// </remarks>
        public bool IsFixedRotation {
            get { return body.FixedRotation; }
            set { this.body.FixedRotation = value; }
        }

        /// <summary>
        /// 弾丸モード
        /// </summary>
        /// <remarks>
        /// このコライダーを弾丸モードにします。
        /// 通常衝突判定はダイナミック体とダイナミック体の場合は始点と終点のみで行われますが、
        /// このプロパティを <c>true</c> にすると途中経路を時分割して連続的に行われます。
        /// 重い処理なので必要がない限り <c>true</c> にしないでください。
        /// </remarks>
        public bool IsBullet {
            get { return body.IsBullet; }
            set { this.body.IsBullet = value; }
        }

        /// <summary>
        /// トリガー モード
        /// </summary>
        /// <remarks>
        /// このコライダーをトリガー モードにします。
        /// トリガーモードの時は物体は力を受けず、他の物体と衝突することなく素通りします。
        /// ただし OnCollisionEnter() と OnCollisionExit() は呼び出されます。
        /// ゲームでは例えば回復ポイントなど物体がある空間に入った時に特定の処理を行う時に使用します。
        /// なおトリガーモードはスタティック、ダイナミック、キネマティックに関係なく設定可能です（普通はスタティックを使うが・・）。
        /// </remarks>
        public bool IsTrigger {
            get { return (body.FixtureList.Count == 0) ? false : body.FixtureList[0].IsSensor; }
            set { this.body.IsSensor = value; }
        }


        /// <summary>
        /// コリジョン マスク
        /// </summary>
        /// <remarks>
        /// コリジョン マスクです。
        /// </remarks>
        public uint CollisionMask {
            get { return mask; }
            set { this.mask = value; }
        }

        /// <summary>
        /// 重量
        /// </summary>
        /// <remarks>
        /// このコライダーの重量です。単位は [kg/m^3].
        /// 初期値は形状を水で満たした物と同じです。
        /// </remarks>
        public float Mass {
            get { return body.Mass; }
            set {
                if (value < 0) {
                    throw new ArgumentException ("Mass is invalid");
                }
                body.Mass = value;
                body.ResetMassData ();
            }
        }

        /// <summary>
        /// 質量中心の座標（ローカル座標）
        /// </summary>
        /// <remarks>
        /// 質量中心をローカル座標で取得します。
        /// 基本的には使用しません。
        /// </remarks>
        public Vector2 CenterOfMass {
            get {
                var center = body.LocalCenter;
                return new Vector2 (center.X, center.Y);
            }
        }

        /// <summary>
        /// 物体の速度
        /// </summary>
        /// <remarks>
        /// 物理エンジンによって計算された現在の速度。
        /// 角速度の変更はキネマティック体に限り可能です。
        /// それ以外のタイプでは無視されます。
        /// </remarks>
        public Vector2 LinearVelocity {
            get {
                return new Vector2 (body.LinearVelocity.X, body.LinearVelocity.Y);
            }
            set {
                if (!IsKinematic) {
                    return;
                }
                this.body.LinearVelocity = new XnaVector2 (value.X, value.Y);
            }
        }

        /// <summary>
        /// 物体の角速度
        /// </summary>
        /// <remarks>
        /// 物理エンジンによって計算された現在の角速度。
        /// 角速度の変更はキネマティック体の時に限り可能です。
        /// それ以外のタイプでは無視されます。
        /// </remarks>
        public float AngularVelocity {
            get { 
                return body.AngularVelocity / (float)Math.PI * 180; 
            }
            set {
                if (!IsKinematic) {
                    return;
                }
                this.body.AngularVelocity = value / 180f * (float)Math.PI; 
            }
        }

        /// <summary>
        /// このコライダーと衝突中の物体を列挙する列挙子
        /// </summary>
        /// <remarks>
        /// このコライダーと現在衝突中の物体を列挙する列挙子
        /// </remarks>
        public IEnumerable<Collision> Collisions {
            get {
                var phy = Physics2D.GetInstance ();

                var list = new List<Collision> ();
                for (var edge = body.ContactList; edge != null; edge = edge.Next) {

                    FixedArray2<XnaVector2> points;
                    XnaVector2 normal;               // A --> B
                    edge.Contact.GetWorldManifold (out normal, out points);

                    var collidee = ((edge.Contact.FixtureA.UserData != this) ? edge.Contact.FixtureA.UserData : edge.Contact.FixtureB.UserData) as Collider;

                    // DDでは法線は衝突相手から自分を向く方と定義しているので
                    // Bが衝突相手だった場合は反転が必要
                    if (collidee == edge.Contact.FixtureB.UserData) {
                        normal = -normal;
                    }
                    normal.Normalize ();

                    var count = edge.Contact.Manifold.PointCount;
                    if (count == 0 && edge.Contact.IsTouching ()) {
                        // センサー（トリガー）モード
                        // count=0 かつ Manifold は未定義
                        list.Add (new Collision (collidee, new Vector3(), new Vector3()));
                    } 
                    else if (count == 1) {
                        var cp = new Vector3 (points[0].X * phy.PPM, points[0].Y * phy.PPM, 0);
                        var nrm = new Vector3 (normal.X, normal.Y, 0);
                        list.Add (new Collision (collidee, cp, nrm));
                    }
                    else if (count == 2) {
                        var cp = new Vector3 ((points[0].X + points[0].X) / 2.0f * phy.PPM, (points[1].Y + points[1].Y) / 2.0f * phy.PPM, 0);
                        var nrm = new Vector3 (normal.X, normal.Y, 0);
                        list.Add (new Collision (collidee, cp, nrm));
                    }
                }
                return list;
            }
        }

        #endregion

        /// <summary>
        /// コリジョン形状の変更
        /// </summary>
        /// <remarks>
        /// コリジョン形状を変更します。
        /// コリジョン形状に <c>null</c> に設定すると例外が発生します。
        /// </remarks>
        /// <param name="shape">コリジョン形状</param>
        public void SetShape (CollisionShape shape) {
            if (shape == null) {
                throw new ArgumentNullException ("Shape is null");
            }

            this.shape = shape;
            this.body.CreateFixture (shape.CreateShape (), this);

            // コリジョン イベントは Body ではなく Fixture にセットされるので、
            // 形状を定義した後にセットする必要がある。
            this.body.OnCollision += new OnCollisionEventHandler (CollisionEnterEventHandler);
            this.body.OnSeparation += new OnSeparationEventHandler (CollisionExitEventHandler);
        }

        /// <summary>
        /// 物理特性の変更
        /// </summary>
        /// <remarks>
        /// コリジョンの物理材質を変更します。
        /// 値は次回 <see cref="OnPhysicsUpdate"/> が呼ばれたタイミングで物理エンジンに反映されます。
        /// 物理特性に <c>null</c> に設定すると例外が発生します。
        /// </remarks>
        /// <param name="mat">物理特性</param>
        public void SetMaterial (PhysicsMaterial mat) {
            if (mat == null) {
                throw new ArgumentNullException ("Material is null");
            }

            this.mat = mat;
        }

        /// <summary>
        /// マテリアルの物理エンジンへの反映
        /// </summary>
        /// <remarks>
        /// マテリアルの値を物理エンジンに反映します。
        /// ハッシュ値をチェックし必要ない場合は何もしません。
        /// </remarks>
        public void UpdateMaterial () {
            if (mat == null || hash == mat.GetHashValue ()) {
                return;
            }

            this.body.Friction = mat.Friction;
            this.body.Restitution = mat.Restitution;
            this.body.LinearDamping = mat.LinearDamping;
            this.body.AngularDamping = mat.AngulerDamping;

            this.hash = mat.GetHashValue ();
        }


        /// <summary>
        /// 力の付加
        /// </summary>
        /// <remarks>
        /// 物体の質量中心に指定の力を加えます。
        /// 単位は 1N(ニュートン) = 1 [kgm/S^2] です。
        /// このメソッドは物理エンジンと直接通信します。
        /// 質量中心に力を加えるので速度のみが変化し回転は変化しません。
        /// <note>
        /// ここに入れる値はかなり大きい値。最低100000ぐらい与えないと変化しない。
        /// </note>
        /// </remarks>
        /// <param name="x">力のX成分 (N)</param>
        /// <param name="y">力のY成分 (N)</param>
        /// <param name="z">力のZ成分 (N)</param>
        public void ApplyForce (float x, float y, float z) {
            if (body == null) {
                return;
            }

            this.body.ApplyForce (new XnaVector2 (x, y));
        }

        /// <summary>
        /// 回転（トルク）の付加
        /// </summary>
        /// <remarks>
        /// 物体に指定のトルクをを加えます。
        /// 単位は [N･m] です。 
        /// その結果回転角速度が変化します。
        /// 物体そのもの速度は変化しません。
        /// </remarks>
        /// <param name="torque">トルク (N･m)</param>
        public void ApplyTorque (float torque) {
            if (body == null) {
                return;
            }

            this.body.ApplyTorque (torque);
        }

        /// <summary>
        /// コリジョン発生イベント ハンドラー
        /// </summary>
        /// <param name="fixtureA">フィクスチャーA</param>
        /// <param name="fixtureB">フィクスチャーB</param>
        /// <param name="contact">(Farseerの)コンタクト情報</param>
        /// <returns></returns>
        private bool CollisionEnterEventHandler (Fixture fixtureA, Fixture fixtureB, Contact contact) {
            var phy = Physics2D.GetInstance ();

            var collidee = ((contact.FixtureA.UserData != this) ? contact.FixtureA.UserData : contact.FixtureB.UserData) as Collider;

            if (((this.CollisionMask & collidee.GroupID) == 0) || ((collidee.CollisionMask & this.GroupID) == 0)) {
                return false;
            }
            
            XnaVector2 normal;   // A --> B
            FixedArray2<XnaVector2> points;
            contact.GetWorldManifold (out normal, out points);

            // DDでは法線は衝突相手から自分を向く方と定義しているので
            // Bが衝突相手だった場合は反転が必要
            if (collidee == contact.FixtureB.UserData) {
                normal = -normal;
            }
            normal.Normalize ();

            var count = contact.Manifold.PointCount;
            if (count == 0 && contact.IsTouching ()) {
                // センサー（トリガー）モード
                // count=0 かつ Manifold は未定義
                var cp = new Collision (collidee, new Vector3 (0, 0, 0), new Vector3 (0, 0, 0));
                foreach (var comp in Node.Components) {
                    comp.OnCollisionEnter (cp);
                }
            }
            else if (count == 1) {
                var p = new Vector3 (points[0].X * phy.PPM, points[0].Y * phy.PPM, 0);
                var n = new Vector3 (normal.X, normal.Y, 0);
                var cp = new Collision (collidee, p, n);
                foreach (var comp in Node.Components) {
                    comp.OnCollisionEnter (cp);
                }
            }
            else if (count == 2) {
                var p = new Vector3 ((points[0].X + points[1].X) / 2.0f * phy.PPM, (points[0].Y + points[1].Y) / 2.0f * phy.PPM, 0);
                var n = new Vector3 (normal.X, normal.Y, 0);
                var cp = new Collision (collidee, p, n);
                foreach (var comp in Node.Components) {
                    comp.OnCollisionEnter (cp);
                }
            }
            return true;
        }

        /// <summary>
        /// コリジョン解消イベント ハンドラー
        /// </summary>
        /// <param name="fixtureA">フィクスチャーA</param>
        /// <param name="fixtureB">フィクスチャーB</param>
        private void CollisionExitEventHandler (Fixture fixtureA, Fixture fixtureB) {
            var collidee = ((fixtureA.UserData != this) ? fixtureA.UserData : fixtureB.UserData) as Collider;
            foreach (var comp in Node.Components) {
                comp.OnCollisionExit (collidee);
            }
        }

        /// <inheritdoc/>
        public override void OnPhysicsUpdate () {
            if (body == null) {
                return;
            }
            var phy = Physics2D.GetInstance ();

            UpdateMaterial ();

            var x = body.WorldCenter.X * phy.PPM;
            var y = body.WorldCenter.Y * phy.PPM;
            var z = 0f;
            var angle = (body.Rotation / (float)Math.PI * 180);

            Node.SetGlobalTranslation (x, y, z);
            Node.SetGlobalRotation (angle, 0, 0, 1);
        }
    }
}
