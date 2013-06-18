using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestCollider {
        [TestMethod]
        public void Test_New () {
            var col = new Collider (ColliderType.Dynamic);

            Assert.AreEqual (true, col.IsDynamic);
            Assert.AreEqual (false, col.IsKinematic);
            Assert.AreEqual (false, col.IsStatic);
            Assert.AreEqual (ColliderType.Dynamic, col.Type);
            Assert.AreEqual (null, col.Shape);
            Assert.AreEqual (null, col.Material);
            Assert.AreEqual (false, col.IsEnabled);
            Assert.AreEqual (false, col.IsSleeping);
            Assert.AreEqual (false, col.IsBullet);
            Assert.AreEqual (true, col.IsGravitational);
            Assert.AreEqual (false, col.IsFixedRotation);
            Assert.AreEqual (false, col.IsTrigger);

            Assert.AreEqual (0xffffffffu, col.CollisionMask);
            Assert.AreEqual (0xffffffffu, col.CollisionGroup);
        }

        [TestMethod]
        public void Test_CreateBody () {
            var col = new Collider (ColliderType.Dynamic);
            var shape = new SphereCollision (1);
            var mat = new PhysicsMaterial ();
            col.Shape = shape;
            col.Material = mat;
            col.CreateBody ();

            Assert.IsNotNull (col.Body);
            Assert.AreEqual (shape, col.Shape);
            Assert.AreEqual (mat, col.Material);

            Assert.AreEqual (true, col.IsEnabled);
            Assert.AreEqual (false, col.IsSleeping);

            Assert.IsTrue (col.Mass > 0);
        }

        [TestMethod]
        public void Test_SetEnable () {
            var col = new Collider (ColliderType.Dynamic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.IsEnabled = false;
            Assert.AreEqual (false, col.IsEnabled);

            col.SetEnable (true);
            Assert.AreEqual (true, col.IsEnabled);
        }

        [TestMethod]
        public void Test_SetGravitational () {
            var col = new Collider (ColliderType.Dynamic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.IsGravitational = false;
            Assert.AreEqual (false, col.IsGravitational);

            col.SetGravitional (true);
            Assert.AreEqual (true, col.IsGravitational);
        }

        [TestMethod]
        public void Test_SetFixRotation () {
            var col = new Collider (ColliderType.Dynamic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.IsFixedRotation = false;
            Assert.AreEqual (false, col.IsFixedRotation);

            col.SetFixedRotation (true);
            Assert.AreEqual (true, col.IsFixedRotation);
        }

        [TestMethod]
        public void Test_SetType () {
            var col = new Collider (ColliderType.Dynamic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.Type = ColliderType.Kinematic;
            Assert.AreEqual (ColliderType.Kinematic, col.Type);

            col.SetColliderType (ColliderType.Static);
            Assert.AreEqual (ColliderType.Static, col.Type);
        }

        [TestMethod]
        public void Test_SetMask () {
            var col = new Collider (ColliderType.Dynamic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.CollisionMask = 0x12345678u;
            Assert.AreEqual (0x12345678u, col.CollisionMask);

            col.SetCollisionMask (0x87654321u);
            Assert.AreEqual (0x87654321u, col.CollisionMask);
        }

        [TestMethod]
        public void Test_SetGroup () {
            var col = new Collider (ColliderType.Dynamic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.CollisionGroup = 0x12345678u;
            Assert.AreEqual (0x12345678u, col.CollisionGroup);

            col.SetCollisionGroup (0x87654321u);
            Assert.AreEqual (0x87654321u, col.CollisionGroup);
        }

        [TestMethod]
        public void Test_SetBullet () {
            var col = new Collider (ColliderType.Dynamic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.IsBullet = true;
            Assert.AreEqual (true, col.IsBullet);

            col.SetBullet (false);
            Assert.AreEqual (false, col.IsBullet);
        }

        [TestMethod]
        public void Test_SetTrigger () {
            var col = new Collider (ColliderType.Dynamic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.IsTrigger = true;
            Assert.AreEqual (true, col.IsTrigger);

            col.SetTrigger (false);
            Assert.AreEqual (false, col.IsTrigger);
        }

        [TestMethod]
        public void Test_SetLinearVelocity () {
            var col = new Collider (ColliderType.Kinematic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.LinearVelocity = new Vector3 (1, 2, 3);
            Assert.AreEqual (new Vector3(1,2,0), col.LinearVelocity);

            col.SetLinearVelocity (1, 2, 3);
            Assert.AreEqual (new Vector3 (1, 2, 0), col.LinearVelocity);
        }

        [TestMethod]
        public void Test_SetAngularVelocity () {
            var col = new Collider (ColliderType.Kinematic);
            col.Shape = new SphereCollision (1.0f);
            col.Material = new PhysicsMaterial ();
            col.CreateBody ();

            col.AngularVelocity = 1;
            Assert.AreEqual (1, col.AngularVelocity, 0.0001f);

            col.SetAngularVelocity (2);
            Assert.AreEqual (2, col.AngularVelocity, 0.0001f);
        }
    }
}
