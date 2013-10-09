using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestNodeDestroyer {
        class MyComponent : Component, IDisposable {
            public bool IsFinalized { get; private set; }
            public bool IsDisposed { get; private set; }
            public override void OnFinalize () {
                this.IsFinalized = true;   
            }
            void IDisposable.Dispose () {
                this.IsDisposed = true;
            }
        }
        [TestMethod]
        public void Test_New () {
            var ddn = new NodeDestroyer ();

            Assert.AreEqual (0, ddn.ReservationCount);
            Assert.AreEqual (0, ddn.Reserves.Count());
        }

        [TestMethod]
        public void Test_Reserve () {
            var ddn = new NodeDestroyer ();
            var node = new Node ("DeadSoul");
            ddn.Reserve (node, 100);


            Assert.AreEqual (1, ddn.ReservationCount);
            Assert.AreEqual (1, ddn.Reserves.Count ());
            Assert.AreEqual (node, ddn.GetReservation (0).Node);
            Assert.AreEqual (100, ddn.GetReservation (0).PurgeTime); 
        }

        [TestMethod]
        public void Test_OnUpdate () {
            var ddn = new NodeDestroyer ();
            var node = new Node ("DeadSoul");
            ddn.Reserve (node, 100);

            ddn.OnUpdate (33);

            Assert.AreEqual (100, ddn.GetReservation (0).PurgeTime);
            Assert.AreEqual (67, ddn.GetReservation (0).LifeTime); 
        }

        [TestMethod]
        public void Test_Purge () {
            var ddn = new NodeDestroyer ();
            ddn.Reserve (new Node ("DeadSoul"), 1);
            ddn.Reserve (new Node ("DeadSoul"), 2);
            ddn.Reserve (new Node ("DeadSoul"), 3);

            ddn.OnUpdate (0);
            ddn.Purge ();
            Assert.AreEqual (3, ddn.ReservationCount);

            ddn.OnUpdate (1);
            ddn.Purge ();
            Assert.AreEqual (2, ddn.ReservationCount);

            ddn.OnUpdate (2);
            ddn.Purge ();
            Assert.AreEqual (1, ddn.ReservationCount);

            ddn.OnUpdate (3);
            ddn.Purge ();
            Assert.AreEqual (0, ddn.ReservationCount);
        }


        [TestMethod]
        public void Test_Destroy_Immediate () {
            var node = new Node ("DeadSoul");

            var wld = new World ();
            wld.AddChild (node);

            // 即時ファイナライズ
            node.Destroy (-1);

            Assert.AreEqual (true, node.IsDestroyed);
            Assert.AreEqual (true, node.IsFinalized);
            Assert.AreEqual (0, wld.NodeDestroyer.ReservationCount);
        }

        [TestMethod]
        public void Test_Destroy_Defered () {
            var node = new Node ("DeadSoul");

            var wld = new World ();
            wld.AddChild (node);

            // 遅延ファイナライズ
            node.Destroy (0);

            Assert.AreEqual (true, node.IsDestroyed);
            Assert.AreEqual (false, node.IsFinalized);
            Assert.AreEqual (1, wld.NodeDestroyer.ReservationCount);
        }

        [TestMethod]
        public void Test_Destroy_and_Purge () {
            var node1 = new Node ("DeadSoul(PurgeTime=1)");  // パージされる
            var node2 = new Node ("DeadSoul(PurgeTime=2)");  // パージされない

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            // 遅延ファイナライズ
            node1.Destroy (1);
            node2.Destroy (2);

            // そしてパージ (time=1まで)
            wld.Update (1);
            wld.Purge ();

            Assert.AreEqual (true, node1.IsDestroyed);
            Assert.AreEqual (true, node2.IsDestroyed);
            Assert.AreEqual (true, node1.IsFinalized);
            Assert.AreEqual (false, node2.IsFinalized);
            Assert.AreEqual (1, wld.NodeDestroyer.ReservationCount);
            Assert.AreEqual (1, wld.ChildCount);
        }

    }
}
