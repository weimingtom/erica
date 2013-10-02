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

            Assert.AreEqual (0, rb.ShapeCount);
            Assert.AreEqual (0, rb.Shapes.Count ());
            Assert.AreEqual (null, rb.Material);
            Assert.IsNotNull (rb.Data);
            Assert.AreEqual (new Vector3 (0, 0, 0), rb.Offset);
            Assert.AreEqual (-1, rb.CollideWith);
            Assert.AreEqual (0, rb.IgnoreWith);
            Assert.AreEqual (1, rb.Mass);
            Assert.AreEqual (true, rb.UseGravity);
            Assert.AreEqual (true, rb.UseContactResponse);
            Assert.AreEqual (new Vector3 (1, 1, 1), rb.LinearFactor);
            Assert.AreEqual (new Vector3 (1, 1, 1), rb.AngularFactor);

            Assert.AreEqual (true, rb.IsDynamic);
        }

        [TestMethod]
        public void Test_AddShape () {
            var rb = new RigidBody ();
            var shp = new BoxShape (1);
            var offset = new Vector3 (1, 2, 3);

            rb.AddShape (shp);

            Assert.AreEqual (1, rb.ShapeCount);
            Assert.AreEqual(shp, rb.GetShape(0));
        }

        [TestMethod]
        public void Test_RemoveShape () {
            var rb = new RigidBody ();
            var shp1 = new BoxShape (1);
            var shp2 = new BoxShape (1);
         
            rb.AddShape (shp1);
            rb.AddShape (shp2);
            rb.RemoveShape (0);

            Assert.AreEqual (1, rb.ShapeCount);
            Assert.AreEqual (shp2, rb.GetShape (0));
        }

        [TestMethod]
        public void Test_SetMaterial () {
            var rb = new RigidBody ();
            var mat = new PhysicsMaterial ();
            rb.Material = mat;

            Assert.AreEqual (mat, rb.Material);
        }

        [TestMethod]
        public void Test_UseGravity () {
            var rb = new RigidBody ();
            rb.AddShape (new SphereShape (1));

            rb.UseGravity = true;
            Assert.AreEqual (true, rb.UseGravity);

            rb.UseGravity = false;
            Assert.AreEqual (false, rb.UseGravity);
        }

        [TestMethod]
        public void Test_UseResponse () {
            var rb = new RigidBody ();
            rb.AddShape (new SphereShape (1));

            rb.UseContactResponse = true;
            Assert.AreEqual (true, rb.UseContactResponse);

            rb.UseContactResponse = false;
            Assert.AreEqual (false, rb.UseContactResponse);
        }

        [TestMethod]
        public void Test_IsDynamic_IsStatic_IsKinematic () {
            var rb = new RigidBody ();
            rb.AddShape (new SphereShape (1));

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
            rb.Mass = 2;

            Assert.AreEqual (2, rb.Mass);
        }

        [TestMethod]
        public void Test_SetCollideWith () {
            var rb = new RigidBody ();
            rb.CollideWith = 0x12345678;

            Assert.AreEqual (0x12345678, rb.CollideWith);
        }

        [TestMethod]
        public void Test_SetIgnoreWith () {
            var rb = new RigidBody ();
            rb.IgnoreWith = 0x12345678;

            Assert.AreEqual (0x12345678, rb.IgnoreWith);
        }

        [TestMethod]
        public void Test_SetLinearFactor () {
            var rb = new RigidBody ();
            rb.AddShape ( new SphereShape (1));

            rb.LinearFactor = new Vector3 (0.1f, 0.2f, 0.3f);
            Assert.AreEqual (new Vector3 (0.1f, 0.2f, 0.3f), rb.LinearFactor);

            rb.SetLinearFactor (0.4f, 0.5f, 0.6f);
            Assert.AreEqual (new Vector3 (0.4f, 0.5f, 0.6f), rb.LinearFactor);

        }

        [TestMethod]
        public void Test_SetAngularFactor () {
            var rb = new RigidBody ();
            rb.AddShape ( new SphereShape (1));

            rb.AngularFactor = new Vector3 (0.1f, 0.2f, 0.3f);
            Assert.AreEqual (new Vector3 (0.1f, 0.2f, 0.3f), rb.AngularFactor);

            rb.SetAngularFactor (0.4f, 0.5f, 0.6f);
            Assert.AreEqual (new Vector3 (0.4f, 0.5f, 0.6f), rb.AngularFactor);

        }

        [TestMethod]
        public void Test_ApplyForce () {
            var rb = new RigidBody ();
            rb.AddShape ( new SphereShape (1));

            // この段階ではエラーが出なければそれでいい
            rb.ApplyForce (1, 0, 0);
            rb.ApplyForce (new Vector3 (1, 0, 0), new Vector3 (1, 0, 0));
        }

        [TestMethod]
        public void Test_ApplyImpulse () {
            var rb = new RigidBody ();
            rb.AddShape (new SphereShape (1));

            // この段階ではエラーが出なければそれでいい
            rb.ApplyImpulse (1, 0, 0);
            rb.ApplyImpulse (new Vector3 (1, 0, 0), new Vector3 (1, 0, 0));
        }

        [TestMethod]
        public void Test_ApplyTorque () {
            var rb = new RigidBody ();
            rb.AddShape (new SphereShape (1));

            // この段階ではエラーが出なければそれでいい
            rb.ApplyTorque (1, 0, 0);
            rb.ApplyTorque (new Vector3(1,0,0));
        }

        [TestMethod]
        public void Test_ApplyTorqueImpusle () {
            var rb = new RigidBody ();
            rb.AddShape (new SphereShape (1));

            // この段階ではエラーが出なければそれでいい
            rb.ApplyTorqueImpulse (1, 0, 0);
            rb.ApplyTorqueImpulse (new Vector3(1, 0, 0));

        }

        [TestMethod]
        public void Test_PhysicsUpdateInit () {
            var node = new Node ("Node1");
            var rb = new RigidBody ();
            rb.AddShape ( new SphereShape (1));
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
