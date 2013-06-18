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
        CollisionShape shape;
        PhysicsMaterial mat;
        Body body;
        uint group;
        uint mask;
        ColliderType type;
        bool gravitational;
        bool fixedRotation;
        bool bullet;
        bool trigger;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public Collider (ColliderType type) {
            this.group = 0xffffffff;
            this.mask = 0xffffffff;
            this.shape = null;
            this.mat = null;
            this.type = type;
            this.gravitational = true;
            this.fixedRotation = false;
            this.bullet = false;
            this.trigger = false;
            this.body = null;
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
        /// Box2D専用というわけではないので object 型。
        /// Farseer.Dynamics.Bodyにキャストして使う。
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
            get { return type; }
            set { SetColliderType (value); }
        }

        /// <summary>
        /// キネマティック タイプ
        /// </summary>
        /// <remarks>
        /// このコライダーがキネマティックかどうかを取得するプロパティです。
        /// </remarks>
        public bool IsKinematic {
            get { return (type == ColliderType.Kinematic); }
        }

        /// <summary>
        /// スタティック タイプ
        /// </summary>
        /// <remarks>
        /// このコライダーがスタティックかどうかを取得するプロパティです。
        /// </remarks>
        public bool IsStatic {
            get { return (type == ColliderType.Static); }
        }

        /// <summary>
        /// ダイナミック タイプ
        /// </summary>
        /// <remarks>
        /// このコライダーがダイナミックかどうかを取得するプロパティです。
        /// </remarks>
        public bool IsDynamic {
            get { return (type == ColliderType.Dynamic); }
        }

        /// <summary>
        /// 有効・無効の制御
        /// </summary>
        /// <remarks>
        /// このコライダー コンポーネントを（一時的に）有効化または無効化します。
        /// ボディ作成前は必ず <c>false</c> が返ります。
        /// </remarks>
        public bool IsEnabled {
            get {
                if (body == null) {
                    return false;
                }
                return body.Enabled; 
            }
            set {
                if (body == null) {
                    return;
                }
                SetEnable (value);
            }
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
            get {
                if (body == null) {
                    return false;
                }
                return !body.Awake; 
            }
            set { SetSleep (value); }
        }

        /// <summary>
        /// 重力制御
        /// </summary>
        /// <remarks>
        /// このコライダーが重力の影響を受けるかどうかを取得、設定します。
        /// </remarks>
        public bool IsGravitational {
            get { return gravitational; }
            set { SetGravitional (value); }
        }

        /// <summary>
        /// 回転固定
        /// </summary>
        /// <remarks>
        /// このコライダーが回転を固定するかどうかを取得、設定します。
        /// ゲームにとって回転固定は必須です。
        /// </remarks>
        public bool IsFixedRotation {
            get { return fixedRotation; }
            set { SetFixedRotation (value); }
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
            get { return bullet; }
            set { SetBullet (value); }
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
            get { return trigger; }
            set { SetTrigger (value); }
        }

        /// <summary>
        /// コリジョン グループ
        /// </summary>
        /// <remarks>
        /// コリジョン グループです。現在実装されていません。
        /// </remarks>
        public uint CollisionGroup {
            get { return group; }
            set { SetCollisionGroup (value); }
        }

        /// <summary>
        /// コリジョン マスク
        /// </summary>
        /// <remarks>
        /// コリジョン マスクです。現在実装されていません。
        /// </remarks>
        public uint CollisionMask {
            get { return mask; }
            set { SetCollisionMask (value); }
        }

        /// <summary>
        /// 重量
        /// </summary>
        /// <remarks>
        /// このコライダーの重量です。単位は kg/m^3.
        /// 自動設定されるので変更できません。
        /// <note>
        /// なぜか FarseerPhysics では SetDensity が無いため後から密度を変更できない。
        /// 重量ベースでBody.Massを直接書き換えてしまっていいものかどうか悩み中。
        /// </note>
        /// </remarks>
        public float Mass {
            get { return body.Mass; }
        }

        /// <summary>
        /// 質量中心の座標（ローカル座標）
        /// </summary>
        /// <remarks>
        /// 質量中心をローカル座標で取得します。
        /// 基本的には使用しません。
        /// </remarks>
        public Vector3 CenterOfMass {
            get {
                var center = body.LocalCenter;
                return new Vector3 (center.X, center.Y, 0);
            }
        }

        /// <summary>
        /// 物体の速度
        /// </summary>
        /// <remarks>
        /// 物理エンジンによって計算された現在の速度。
        /// ボディ作成前は0ベクトルが返ります。セッターは無視されます。
        /// </remarks>
        public Vector3 LinearVelocity {
            get {
                if (body == null) {
                    return new Vector3();
                }
                return new Vector3 ( body.LinearVelocity.X,  body.LinearVelocity.Y, 0);
            }
            set {
                if (body == null) {
                    return;
                }
                SetLinearVelocity (value.X, value.Y, value.Z);
            }
        }

        /// <summary>
        /// 物体の角速度
        /// </summary>
        /// <remarks>
        /// 物理エンジンによって計算された現在の角速度。
        /// ボディ作成前は0が返ります。セッターは無視あれます。
        /// </remarks>
        public float AngularVelocity {
            get {
                if (body == null) {
                    return 0;
                }
                return body.AngularVelocity / (float)Math.PI * 180;
            }
            set {
                if (body == null) {
                    return;
                }
                SetAngularVelocity (value);
            }
        }

        /// <summary>
        /// このコライダーと衝突中の物体を列挙する列挙子
        /// </summary>
        /// <remarks>
        /// このコライダーと現在衝突中の物体を列挙する列挙子
        /// </remarks>
        public IEnumerable<ContactPoint> Collisions {
            get {
                var phy = Physics2D.GetInstance ();

                var list = new List<ContactPoint> ();
                for (var edge = body.ContactList; edge != null; edge = edge.Next) {

                    FixedArray2<Vector2> points;
                    Vector2 normal;               // A --> B
                    edge.Contact.GetWorldManifold (out normal, out points);

                    var collidee = ((edge.Contact.FixtureA.UserData != this) ? edge.Contact.FixtureA.UserData : edge.Contact.FixtureB.UserData) as Collider;
                    
                    // DDでは法線は衝突相手から自分を向く方と定義しているので
                    // Bが衝突相手だった場合は反転が必要
                    if (collidee == edge.Contact.FixtureB.UserData) {
                        normal = -normal;
                    }
                    normal.Normalize ();

                    var count = edge.Contact.Manifold.PointCount;
                    if (count == 1) {
                        var cp = new Vector3 (points[0].X * phy.PPM, points[0].Y * phy.PPM, 0);
                        var nrm = new Vector3 (normal.X, normal.Y, 0);
                        list.Add (new ContactPoint (collidee, cp, nrm));
                    }
                    else if(count == 2) {
                        var cp = new Vector3 ((points[0].X + points[0].X) / 2.0f * phy.PPM, (points[1].Y + points[1].Y) / 2.0f * phy.PPM, 0);
                        var nrm = new Vector3 (normal.X, normal.Y, 0);
                        list.Add (new ContactPoint (collidee, cp, nrm));
                    }
                    else if (edge.Contact.IsTouching ()) {
                        // センサー（トリガー）モード
                        // Manifoldは作られないのでcountも0
                        var cp = new Vector3 (0, 0, 0);
                        var nrm = new Vector3 (0, 0, 0);
                        list.Add (new ContactPoint (collidee, cp, nrm));
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
        /// コリジョン形状を変更します。このメソッドの後に <see cref="CreateBody"/> を呼び出してオブジェクトを再生産する必要があります。
        /// 自動では反映されません。
        /// コリジョン形状に <c>null</c> に設定すると例外が発生します。
        /// </remarks>
        /// <param name="shape">コリジョン形状</param>
        public void SetShape (CollisionShape shape) {
            if (shape == null) {
                throw new ArgumentNullException ("Shape is null");
            }
            this.shape = shape;
        }

        /// <summary>
        /// 物理特性の変更
        /// </summary>
        /// <remarks>
        /// コリジョンの物理材質を変更します。このメソッドの後に <see cref="CreateBody"/> を呼び出してオブジェクトを再生産する必要があります。
        /// 自動では反映されません。
        /// 物理特性に <c>null</c> に設定すると例外が発生します。
        /// </remarks>
        /// <param name="material">物理特性</param>
        public void SetMaterial (PhysicsMaterial material) {
            if (material == null) {
                throw new ArgumentNullException ("Material is null");
            }
            this.mat = material;
        }

        /// <summary>
        /// 物理オブジェクトの作成
        /// </summary>
        /// <remarks>
        /// ユーザーはコライダーの形状と物理特性をセットした後、このメソッドを呼び出して
        /// 物理オブジェクトを作成（設定を物理エンジンに反映）する必要があります。
        /// 自動的には反映されません。
        /// 設定済みのボディがある場合は削除します。
        /// </remarks>
        public void CreateBody () {
            if (shape == null) {
                throw new InvalidOperationException ("Shape is null");
            }
            if (mat == null) {
                throw new InvalidOperationException ("Material is null");
            }
            if (body != null) {
                body.Dispose ();
            }

            var phy = Physics2D.GetInstance ();
            if (phy.GetWorld () == null) {
                throw new InvalidOperationException ("Physics world is not created.");
            }

            this.body = shape.CreateBody ();
            foreach (var fix in body.FixtureList) {
                fix.UserData = this;
            }

            if (mat != null) {
                UpdateBody ();
            }
            body.OnCollision += new OnCollisionEventHandler (CollisionEnterEventHandler);
            body.OnSeparation += new OnSeparationEventHandler (CollisionExitEventHandler);
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
                        
            Vector2 normal;   // A --> B
            FixedArray2<Vector2> points;
            contact.GetWorldManifold (out normal, out points);

            var collidee = ((contact.FixtureA.UserData != this) ? contact.FixtureA.UserData : contact.FixtureB.UserData) as Collider;
            
            // DDでは法線は衝突相手から自分を向く方と定義しているので
            // Bが衝突相手だった場合は反転が必要
            if (collidee == contact.FixtureB.UserData) {
                normal = -normal;
            }
            normal.Normalize ();

            var count = contact.Manifold.PointCount;
            if (count == 1) {
                var p = new Vector3 (points[0].X * phy.PPM, points[0].Y * phy.PPM, 0);
                var n = new Vector3 (normal.X, normal.Y, 0);
                var cp = new ContactPoint (collidee, p, n);
                foreach (var comp in Node.Components) {
                    comp.OnCollisionEnter (cp);
                }
            }
            else if (count == 2) {
                var p = new Vector3 ((points[0].X + points[1].X) / 2.0f * phy.PPM, (points[0].Y + points[1].Y) / 2.0f * phy.PPM, 0);
                var n = new Vector3 (normal.X, normal.Y, 0);
                var cp = new ContactPoint (collidee, p, n);
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


        /// <summary>
        /// 物理オブジェクトの更新
        /// </summary>
        /// <remarks>
        /// 物理エンジンに設定を反映します。物理形状や物理特性を変更した後に呼び出します。
        /// 現状では自動的に更新しているので、ユーザーが自分でこれを呼ぶ必要はありません。
        /// （一人突っ込み：なら internal にしろよ）
        /// </remarks>
        public void UpdateBody () {
            if (shape == null) {
                throw new InvalidCastException ("Shape is null");
            }
            if (mat == null) {
                throw new InvalidCastException ("Material is null");
            }
            if (body == null) {
                throw new InvalidCastException ("Body is null");
            }

            body.BodyType = type.ToFarseer ();
            body.IsBullet = bullet;
            body.IsSensor = trigger;
            body.IgnoreGravity = !gravitational;
            body.FixedRotation = fixedRotation;
            body.LinearDamping = mat.LinearDamping;
            body.AngularDamping = mat.AngulerDamping;
            foreach (var fix in body.FixtureList) {
                fix.Restitution = mat.Restitution;
                fix.Friction = mat.Friction;
            }
        }

        /// <summary>
        /// コリジョン マスクの変更
        /// </summary>
        /// <remarks>
        /// 現状でコリジョン マスクは未実装です。
        /// </remarks>
        /// <param name="mask">マスク</param>
        public void SetCollisionMask (uint mask) {
            this.mask = mask;
        }

        /// <summary>
        /// コリジョン グループの変更
        /// </summary>
        /// <remarks>
        /// 現状でコリジョン グループは未実装です。
        /// </remarks>
        /// <param name="group">グループID</param>
        public void SetCollisionGroup (uint group) {
            this.group = group;
        }

        /// <summary>
        /// コリジョン タイプの変更
        /// </summary>
        /// <remarks>
        /// コリジョンタイプを変更します。
        /// (1)ダイナミック (2)スタティック (3) キネマティック
        /// </remarks>
        /// <param name="type">タイプ</param>
        public void SetColliderType (ColliderType type) {
            this.type = type;
        }

        /// <summary>
        /// 重力制御の変更
        /// </summary>
        /// <remarks>
        /// このコライダーが重力を受けるかどうかを変更します。
        /// </remarks>
        /// <param name="enable">有効・無効</param>
        public void SetGravitional (bool enable) {
            this.gravitational = enable;
        }

        /// <summary>
        /// 回転固定の変更
        /// </summary>
        /// <remarks>
        /// このコライダーが回転固定かどうかを変更します。
        /// </remarks>
        /// <param name="enable">有効・無効</param>
        public void SetFixedRotation (bool enable) {
            this.fixedRotation = enable;
        }

        /// <summary>
        /// 有効・無効の変更
        /// </summary>
        /// <remarks>
        /// このコライダーが物理エンジン制御されるかどうかを変更します。
        /// このメソッドは物理エンジンと直接通信します。
        /// <c>false</c> にするとこのコライダーは一時的に物理エンジンの制御から外れます。
        /// <note>
        /// 他の物体が無効化した物体に当たってきたらどうなるの？
        /// 判定自体もなくなってるの？
        /// 後で確認
        /// </note>
        /// </remarks>
        /// <param name="enable">有効・無効</param>
        public void SetEnable (bool enable) {
            if (body == null) {
                return;
            }
            this.body.Enabled = enable;
        }

        /// <summary>
        /// 弾丸モードの変更
        /// </summary>
        /// <remarks>
        /// 弾丸モードに変更します。
        /// 弾丸モードはダイナミック-ダイナミックの判定の精度が上がります。
        /// 重し処理なので必要があるまで <c>enable</c> にしないでください。
        /// </remarks>
        /// <param name="enalbe">有効・無効</param>
        public void SetBullet (bool enalbe) {
            this.bullet = enalbe;
        }

        /// <summary>
        /// トリガー モードの変更
        /// </summary>
        /// <remarks>
        /// トリガーモードの有効・無効を変更します。
        /// </remarks>
        /// <param name="enalbe">有効・無効</param>
        public void SetTrigger (bool enalbe) {
            this.trigger = enalbe;
        }

        /// <summary>
        /// 速度の変更
        /// </summary>
        /// <remarks>
        /// このコライダーの速度を変更します。
        /// このメソッドは物理エンジンと直接通信します。
        /// 通常速度は物理エンジンによって算出されるのでこのメソッドを使用してユーザーが直接変更しないでください。
        /// ただしキネマティック体はこのメソッドによって制御されます。
        /// </remarks>
        /// <param name="x">速度のX成分</param>
        /// <param name="y">速度のY成分</param>
        /// <param name="z">速度のZ成分</param>
        public void SetLinearVelocity (float x, float y, float z) {
            if (body == null) {
                return;
            }
            this.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2 (x, y);
        }

        /// <summary>
        /// 角速度の変更
        /// </summary>
        /// <remarks>
        /// このコライダーの角速度を変更します。
        /// このメソッドは物理エンジンと直接通信します。
        /// 通常角速度は物理エンジンによって算出されるのでこのメソッドを使用してユーザーが直接変更しないでください。
        /// ただしキネマティック体はこのメソッドによって制御されます。
        /// </remarks>
        /// <param name="angle">角速度 (in degree)</param>
        public void SetAngularVelocity (float angle) {
            if (body == null) {
                return;
            }
            this.body.AngularVelocity = angle / 180.0f * (float)Math.PI;
        }

        /// <summary>
        /// スリープ状態の変更
        /// </summary>
        /// <remarks>
        /// このコライダーを強制的にスリープ状態に移行させます。
        /// 通常物理エンジンが自動的にスリープ状態に移行させるので、このメソッドを使用して直接変更しないでください。
        /// </remarks>
        /// <param name="sleep">スリープ状態</param>
        public void SetSleep (bool sleep) {
            this.IsSleeping = sleep;
        }

        /// <summary>
        /// コリジョン形状のオフセット
        /// </summary>
        /// <remarks>
        /// コリジョン形状をローカル座標原点からオフセット分移動させます。
        /// 現在未実装です。
        /// </remarks>
        /// <param name="x">オフセットのX</param>
        /// <param name="y">オフセットのY</param>
        /// <param name="z">オフセットのZ</param>
        public void SetOffset (float x, float y, float z) {
            // ?
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

            this.body.ApplyForce (new Vector2(x,y));
        }

        /// <summary>
        /// 回転の付加
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

        /// <inheritdoc/>
        public override void OnDetached () {
            if (body != null) {
                body.Dispose ();
                body = null;
            }
        }

        /// <inheritdoc/>
        public override void OnPhysicsUpdate () {
            if (body == null) {
                return;
            }
            var phy = Physics2D.GetInstance ();

            var x = body.WorldCenter.X * phy.PPM;
            var y = body.WorldCenter.Y * phy.PPM;
            var z = 0f;
            var angle = (body.Rotation / (float)Math.PI * 180);

            Node.SetGlobalTranslation (x, y, z);
            Node.SetGlobalRotation (angle, 0, 0, 1);
        }
    }
}
