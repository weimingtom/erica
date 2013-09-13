using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

using BulletVector3 = BulletSharp.Vector3;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestRigidBody {
        [TestMethod]
        public void Test_New () {
            var rb = new RigidBody ();

            Assert.AreEqual (null, rb.Shape);
            Assert.AreEqual (null, rb.Material);
            Assert.AreEqual (null, rb.Data);
            Assert.AreEqual (new Vector3 (0, 0, 0), rb.Offset);
            Assert.AreEqual (-1, rb.CollideWith);
            Assert.AreEqual (0, rb.IgnoreWith);
            Assert.AreEqual (1, rb.Mass);
            Assert.AreEqual (true, rb.UseGravity);
            Assert.AreEqual (true, rb.UseResponse);
            Assert.AreEqual (new Vector3 (1, 1, 1), rb.LinearFactor);
            Assert.AreEqual (new Vector3 (1, 1, 1), rb.AngularFactor);

            Assert.AreEqual (true, rb.IsDynamic);
        }

        [TestMethod]
        public void Test_SetShape () {
            var rb = new RigidBody ();

            rb.Mass = 10;
            rb.LinearFactor = new Vector3 (0.1f, 0.2f, 0.3f);
            rb.AngularFactor = new Vector3 (0.4f, 0.5f, 0.6f);
            rb.Material = new PhysicsMaterial ();
            rb.Material.Restitution = 0.1f;
            rb.Material.LinearDamping = 0.2f;
            rb.Material.AngularDamping = 0.3f;
            rb.Material.Friction = 0.4f;
            rb.Material.RollingFriction = 0.5f;

            // Shapeのセット前に設定した値もきちんと反映されていなければならない
            var shape = new BoxShape (1, 2, 3);
            rb.Shape = shape;

            Assert.AreEqual (shape, rb.Shape);
            Assert.IsNotNull (rb.Data);

            // DDのメンバー変数ではなく
            // Bullet側に反映されていることを確認する
            Assert.AreEqual (0.1f, rb.Data.InvMass, 0.01f);
            Assert.AreEqual (new BulletVector3(0.1f, 0.2f, 0.3f), rb.Data.LinearFactor);
            Assert.AreEqual (new BulletVector3 (0.4f, 0.5f, 0.6f), rb.Data.AngularFactor);
            Assert.AreEqual (0.1f, rb.Data.Restitution);
            Assert.AreEqual (0.2f, rb.Data.LinearDamping);
            Assert.AreEqual (0.3f, rb.Data.AngularDamping);
            Assert.AreEqual (0.4f, rb.Data.Friction);
            Assert.AreEqual (0.5f, rb.Data.RollingFriction);
        }

        [TestMethod]
        public void Test_SetMaterial () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);
      
            // Shapeのセット後に設定した値もきちんと反映されていなければならない
            rb.Mass = 10;
            rb.LinearFactor = new Vector3 (0.1f, 0.2f, 0.3f);
            rb.AngularFactor = new Vector3 (0.4f, 0.5f, 0.6f);

            // Materialのセット後に設定した値が「反映されないのは仕様」
            var mat = new PhysicsMaterial();
            mat.Restitution = 0.1f;
            mat.LinearDamping = 0.2f;
            mat.AngularDamping = 0.3f;
            mat.Friction = 0.4f;
            mat.RollingFriction = 0.5f;
            rb.Material = mat;

            Assert.AreEqual (mat, rb.Material);

            // DDのメンバー変数ではなく
            // Bullet側に反映されていることを確認する
            Assert.AreEqual (0.1f, rb.Data.InvMass, 0.01f);
            Assert.AreEqual (new BulletVector3 (0.1f, 0.2f, 0.3f), rb.Data.LinearFactor);
            Assert.AreEqual (new BulletVector3 (0.4f, 0.5f, 0.6f), rb.Data.AngularFactor);
            Assert.AreEqual (0.1f, rb.Data.Restitution);
            Assert.AreEqual (0.2f, rb.Data.LinearDamping);
            Assert.AreEqual (0.3f, rb.Data.AngularDamping);
            Assert.AreEqual (0.4f, rb.Data.Friction);
            Assert.AreEqual (0.5f, rb.Data.RollingFriction);
        }

        [TestMethod]
        public void Test_UseGravity () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);

            rb.UseGravity = true;
            Assert.AreEqual (true, rb.UseGravity);

            rb.UseGravity = false;
            Assert.AreEqual (false, rb.UseGravity);
        }

        [TestMethod]
        public void Test_UseResponse () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);

            rb.UseResponse = true;
            Assert.AreEqual (true, rb.UseResponse);

            rb.UseResponse = false;
            Assert.AreEqual (false, rb.UseResponse);
        }

        [TestMethod]
        public void Test_IsDynamic_IsStatic_IsKinematic () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);

            rb.IsStatic = true;
            Assert.AreEqual (true, rb.IsStatic);
            Assert.AreEqual (0, rb.Mass);

            rb.IsKinematic = true;
            Assert.AreEqual (true, rb.IsKinematic);
            Assert.AreEqual (0, rb.Mass);

            rb.IsDynamic = true;
            Assert.AreEqual (true, rb.IsDynamic);
            Assert.AreEqual (1, rb.Mass);
        }


        [TestMethod]
        public void Test_SetOffset () {
            var rigid = new RigidBody ();

            rigid.SetOffset (1, 2, 3);
            Assert.AreEqual (new Vector3 (1, 2, 3), rigid.Offset);

            rigid.Offset = new Vector3 (4, 5, 6);
            Assert.AreEqual (new Vector3 (4, 5, 6), rigid.Offset);
        }

        [TestMethod]
        public void Test_SetMass () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);
            rb.Mass = 2;

            Assert.AreEqual (2, rb.Mass);
        }

        [TestMethod]
        public void Test_SetCollideWith () {
            var rigid = new RigidBody ();

            rigid.CollideWith = 0x12345678;

            Assert.AreEqual (0x12345678, rigid.CollideWith);
        }

        [TestMethod]
        public void Test_SetIgnoreWith () {
            var rigid = new RigidBody ();
            rigid.IgnoreWith = 0x12345678;

            Assert.AreEqual (0x12345678, rigid.IgnoreWith);
        }

        [TestMethod]
        public void Test_SetLinearFactor () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);

            rb.LinearFactor = new Vector3 (0.1f, 0.2f, 0.3f);
            Assert.AreEqual (new Vector3 (0.1f, 0.2f, 0.3f), rb.LinearFactor);

            rb.SetLinearFactor (0.4f, 0.5f, 0.6f);
            Assert.AreEqual (new Vector3 (0.4f, 0.5f, 0.6f), rb.LinearFactor);

        }

        [TestMethod]
        public void Test_SetAngularFactor () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);

            rb.AngularFactor = new Vector3 (0.1f, 0.2f, 0.3f);
            Assert.AreEqual (new Vector3 (0.1f, 0.2f, 0.3f), rb.AngularFactor);

            rb.SetAngularFactor (0.4f, 0.5f, 0.6f);
            Assert.AreEqual (new Vector3 (0.4f, 0.5f, 0.6f), rb.AngularFactor);

        }

        [TestMethod]
        public void Test_ApplyForce () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);

            // この段階ではエラーが出なければそれでいい
            rb.ApplyForce (1, 0, 0);
            rb.ApplyForce (new Vector3 (1, 0, 0), new Vector3 (1, 0, 0));
        }

        [TestMethod]
        public void Test_ApplyImpulse () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);

            // この段階ではエラーが出なければそれでいい
            rb.ApplyImpulse (1, 0, 0);
            rb.ApplyImpulse (new Vector3 (1, 0, 0), new Vector3 (1, 0, 0));
        }

        [TestMethod]
        public void Test_ApplyTorque () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);

            // この段階ではエラーが出なければそれでいい
            rb.ApplyTorque (1, 0, 0);
            rb.ApplyTorque (new Vector3(1,0,0));
        }

        [TestMethod]
        public void Test_ApplyTorqueImpusle () {
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);

            // この段階ではエラーが出なければそれでいい
            rb.ApplyTorqueImpulse (1, 0, 0);
            rb.ApplyTorqueImpulse (new Vector3(1, 0, 0));

        }

        [TestMethod]
        public void Test_PhysicsUpdateInit () {
            var node = new Node ("Node1");
            var rb = new RigidBody ();
            rb.Shape = new SphereShape (1);
            node.Attach (rb);

            var wld = new World ();
            wld.AddChild (node);

            // ここで PhysicsUpdateInit() が呼ばれる
            //   - 物理演算ワールドに登録
            wld.PhysicsUpdate (33);

            Assert.AreEqual (1, wld.PhysicsSimulator.RigidBodyCount);
            Assert.AreEqual (1, wld.PhysicsSimulator.RigidBodies.Count ());

            wld.Destroy ();
        }

    }
}
