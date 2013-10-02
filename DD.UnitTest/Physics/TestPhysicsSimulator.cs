using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestPhysicsSimulator {

        [TestMethod]
        public void Test_New () {
            var phy = new PhysicsSimulator ();

            // 標準では PPM=64 だがユニットテストに限り PPM=1 で使用する。
           Assert.AreEqual (1, PhysicsSimulator.PPM);

            Assert.IsNotNull (phy.PhysicsWorld);
            Assert.AreEqual (new Vector3 (0, -9.8f, 0), phy.Gravity);
            Assert.AreEqual (0, phy.RigidBodyCount);
            Assert.AreEqual (0, phy.RigidBodies.Count ());
        }

        [TestMethod]
        public void Test_SetPPM () {
            var old = PhysicsSimulator.PPM;
            PhysicsSimulator.PPM  = 99;

            Assert.AreEqual (99, PhysicsSimulator.PPM);

            PhysicsSimulator.PPM = old;
        }


        [TestMethod]
        public void Test_SetGravity () {
            var wld = new World ();

            wld.PhysicsSimulator.Gravity = new Vector3 (1,2,3);
            Assert.AreEqual (new Vector3 (1, 2, 3), wld.PhysicsSimulator.Gravity);

            wld.PhysicsSimulator.SetGravity (4, 5, 6);
            Assert.AreEqual (new Vector3 (4,5,6), wld.PhysicsSimulator.Gravity);
        }

        [TestMethod]
        public void Test_AddRigidBody () {
            var node = new Node ();
            var rb = new RigidBody ();
            rb.AddShape(new SphereShape (1));
            rb.Material = new PhysicsMaterial ();
            node.Attach (rb);

            var phys = new PhysicsSimulator ();
            phys.AddRigidBody (node);

            Assert.AreEqual (1, phys.RigidBodyCount);
            Assert.AreEqual (1, phys.RigidBodies.Count ());
        }

        [TestMethod]
        public void Test_RemoveRigidBody () {
            var node = new Node ();
            var rb = new RigidBody ();
            rb.AddShape (new SphereShape (1))                ;
            rb.Material = new PhysicsMaterial ();
            node.Attach (rb);

            var phys = new PhysicsSimulator ();
            phys.AddRigidBody (node);
            phys.RemoveRigidBody (node);

            Assert.AreEqual (0, phys.RigidBodyCount);
            Assert.AreEqual (0, phys.RigidBodies.Count ());
        }

        [TestMethod]
        public void Test_IsRegistered () {
            var node = new Node ();
            var rb = new RigidBody ();
            rb.AddShape (new SphereShape (1));
            rb.Material = new PhysicsMaterial ();
            node.Attach (rb);

            var phys = new PhysicsSimulator ();
            phys.AddRigidBody (node);

            Assert.AreEqual (true, phys.IsRegistered(node));
        }

    }
}
