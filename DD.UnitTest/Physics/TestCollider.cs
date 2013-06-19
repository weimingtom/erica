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
            var col = new Collider ();

            Assert.AreEqual (true, col.IsStatic);
            Assert.AreEqual (false, col.IsDynamic);
            Assert.AreEqual (false, col.IsKinematic);
            Assert.AreEqual (ColliderType.Static, col.Type);
            Assert.AreEqual (null, col.Shape);
            Assert.AreEqual (null, col.Material);
            Assert.AreEqual (true, col.IsEnabled);
            Assert.AreEqual (false, col.IsSleeping);
            Assert.AreEqual (false, col.IsBullet);
            Assert.AreEqual (true, col.IsGravitational);
            Assert.AreEqual (false, col.IsFixedRotation);
            Assert.AreEqual (false, col.IsTrigger);

            Assert.AreEqual (0xffffffffu, col.CollisionMask);
        }

        [TestMethod]
        public void Test_SetEnable () {
            var col = new Collider ();
            col.Shape = new SphereCollider (1.0f);
            col.Material = new PhysicsMaterial ();

            col.IsEnabled = false;
            Assert.AreEqual (false, col.IsEnabled);

            col.IsEnabled = true;
            Assert.AreEqual (true, col.IsEnabled);
        }

        [TestMethod]
        public void Test_SetGravitational () {
            var col = new Collider ();
            col.Shape = new SphereCollider (1.0f);
            col.Material = new PhysicsMaterial ();

            col.IsGravitational = false;
            Assert.AreEqual (false, col.IsGravitational);

            col.IsGravitational = true;
            Assert.AreEqual (true, col.IsGravitational);
        }

        [TestMethod]
        public void Test_SetFixRotation () {
            var col = new Collider ();
            col.Shape = new SphereCollider (1.0f);
            col.Material = new PhysicsMaterial ();

            col.IsFixedRotation = false;
            Assert.AreEqual (false, col.IsFixedRotation);

            col.IsFixedRotation = true;
            Assert.AreEqual (true, col.IsFixedRotation);
        }

        [TestMethod]
        public void Test_SetType () {
            var col = new Collider ();
            col.Shape = new SphereCollider (1.0f);
            col.Material = new PhysicsMaterial ();

            col.Type = ColliderType.Kinematic;
            Assert.AreEqual (ColliderType.Kinematic, col.Type);

            col.Type = ColliderType.Dynamic;
            Assert.AreEqual (ColliderType.Dynamic, col.Type);

            col.Type = ColliderType.Static;
            Assert.AreEqual (ColliderType.Static, col.Type);
        }

        [TestMethod]
        public void Test_SetMask () {
            var col = new Collider ();
            col.Shape = new SphereCollider (1.0f);
            col.Material = new PhysicsMaterial ();

            col.CollisionMask = 0x12345678u;
            Assert.AreEqual (0x12345678u, col.CollisionMask);
        }

        [TestMethod]
        public void Test_SetBullet () {
            var col = new Collider ();
            col.Shape = new SphereCollider (1.0f);
            col.Material = new PhysicsMaterial ();

            col.IsBullet = false;
            Assert.AreEqual (false, col.IsBullet);

            col.IsBullet = true;
            Assert.AreEqual (true, col.IsBullet);
        }

        [TestMethod]
        public void Test_SetTrigger () {
            var col = new Collider ();
            col.Shape = new SphereCollider (1.0f);
            col.Material = new PhysicsMaterial ();

            col.IsTrigger = true;
            Assert.AreEqual (true, col.IsTrigger);

            col.IsTrigger = false;
            Assert.AreEqual (false, col.IsTrigger);
        }

        [TestMethod]
        public void Test_SetLinearVelocity () {
            var col = new Collider ();
            col.Shape = new SphereCollider (1.0f);
            col.Material = new PhysicsMaterial ();

            col.LinearVelocity = new Vector3 (1, 2, 3);
            Assert.AreEqual (new Vector3 (1, 2, 0), col.LinearVelocity);
        }

        [TestMethod]
        public void Test_SetAngularVelocity () {
            var col = new Collider ();
            col.Shape = new SphereCollider (1.0f);
            col.Material = new PhysicsMaterial ();

            col.AngularVelocity = 1;
            Assert.AreEqual (1, col.AngularVelocity, 0.0001f);
        }
    }
}
