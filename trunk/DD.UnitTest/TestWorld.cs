using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD;

namespace DD.UnitTest {
    [TestClass]
    public class TestWorld {
        private class MyComponent : Component {
            public int Animated { get; private set; }
            public int Updated { get; private set; }
            public int OnUpdateInited { get; private set; }
            public override void OnAnimate (long msec, long dtime) {
                this.Animated += 1;
            }
            public override void OnUpdateInit (long msec) {
                this.OnUpdateInited += 1;
            }
            public override void OnUpdate (long msec) {
                this.Updated += 1;
            }
        }
        private class MyNode : Node, IDisposable {
            public bool IsDisposed { get; private set; }
            public MyNode(string name):base(name){}
            public void Dispose () {
                this.IsDisposed = true;
            }

        }
        private class MyWorld : World, IDisposable {
            public bool IsDisposed { get; private set; }
            public MyWorld(string name) : base(name){}
            public void Dispose () {
                this.IsDisposed = true;
            }

        }

        [TestMethod]
        public void Test_New () {
            var wld = new World ("World");

            Assert.AreEqual ("World", wld.Name);
            Assert.AreEqual (null, wld.ActiveCamera);
            Assert.IsNotNull (wld.InputReceiver);
            Assert.IsNotNull (wld.AnimationController);
            Assert.IsNotNull (wld.SoundPlayer);
            Assert.IsNotNull (wld.PostOffice);
            Assert.IsNotNull (wld.CollisionAnalyzer);
            Assert.IsNotNull (wld.PhysicsSimulator);
            Assert.IsNotNull (wld.NodeDestroyer);
            Assert.IsNotNull (wld.ClockTower);
            Assert.IsNotNull (wld.Logger);
        }

        [TestMethod]
        public void Test_Property () {
            var wld = new World ();
            wld.SetProperty ("Prop1", 0);
            wld.SetProperty ("Prop1", 1);  // 上書き

            Assert.AreEqual (1, wld.GetProperty<int> ("Prop1"));
            Assert.AreEqual (0, wld.GetProperty<int> ("Prop2"));    // not found
            Assert.AreEqual (1, wld.GetProperty<int> ("Prop2", 1)); // not found, explicit default value

        }

        [TestMethod]
        public void Test_ActiveCamera () {
            var cam = new Camera ();
            var node = new Node ();
            node.Attach (cam);

            var wld = new World ();
            wld.AddChild (node);

            wld.ActiveCamera = node;
            Assert.AreEqual (node, wld.ActiveCamera);
        }

        [TestMethod]
        public void Test_Animate () {
            var wld = new World ();
            var nod = new Node ();
            wld.AddChild (nod);

            var cmp1 = new MyComponent ();
            var cmp2 = new MyComponent ();
            wld.Attach (cmp1);
            nod.Attach (cmp2);

            wld.Animate (0, 0);

            Assert.AreEqual (1, cmp1.Animated);
            Assert.AreEqual (1, cmp2.Animated);

            nod.Animatable = false;
            wld.Animate (0, 0);

            Assert.AreEqual (2, cmp1.Animated);
            Assert.AreEqual (1, cmp2.Animated);

        }

        [TestMethod]
        public void Test_UpdateInit () {
            var wld = new World ();
            var nod = new Node ();
            wld.AddChild (nod);

            var cmp1 = new MyComponent ();
            var cmp2 = new MyComponent ();
            wld.Attach (cmp1);
            nod.Attach (cmp2);

            wld.Update (0);
            Assert.AreEqual (1, cmp1.OnUpdateInited);
            Assert.AreEqual (1, cmp2.OnUpdateInited);

            wld.Update (0);
            Assert.AreEqual (1, cmp1.OnUpdateInited);
            Assert.AreEqual (1, cmp2.OnUpdateInited);
        }

        [TestMethod]
        public void Test_Update () {
            var wld = new World ();
            var nod = new Node ();
            wld.AddChild (nod);

            var cmp1 = new MyComponent ();
            var cmp2 = new MyComponent ();
            wld.Attach (cmp1);
            nod.Attach (cmp2);

            wld.Update (0);

            Assert.AreEqual (1, cmp1.Updated);
            Assert.AreEqual (1, cmp2.Updated);

            nod.Updatable = false;
            wld.Update (0);

            Assert.AreEqual (2, cmp1.Updated);
            Assert.AreEqual (1, cmp2.Updated);
        }

        [TestMethod]
        public void Test_Deliver () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            var mbox1 = new MailBox ("All");
            var mbox2 = new MailBox ("All");
            node1.Attach (mbox1);
            node2.Attach (mbox2);

            var received1 = false;
            var received2 = false;
            
            mbox1.Action += (from, to, args) => {
                received1 = true;
            };
            mbox2.Action += (from, to, args) => {
                received2 = true;
            };

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            wld.PostOffice.Post (node1, "Node2", null);
            wld.PostOffice.Post (node2, "Node1", null);

            wld.Deliver ();

            Assert.AreEqual (true, received1);
            Assert.AreEqual (true, received2);
        }

        [TestMethod]
        public void Test_Downloads () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            var node3 = new Node ("Node3");
            var node4 = new Node ("Node4");

            var wld = new World ();

            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);
            Assert.AreEqual (4, wld.Downwards.Count ());

            // Update()するまでは反映されない
            wld.AddChild (node4);
            Assert.AreEqual (4, wld.Downwards.Count ());

            wld.Update (0);
            Assert.AreEqual (5, wld.Downwards.Count ());
        }

        [TestMethod]
        public void Test_Root () {
            var wld = new World ();
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            wld.AddChild (node1);
            node1.AddChild (node2);

            Assert.AreEqual (wld, node1.Root);
            Assert.AreEqual (wld, node2.Root);
        }

        [TestMethod]
        public void Test_World () {
            var wld = new World ();
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            wld.AddChild (node1);
            node1.AddChild (node2);

            Assert.AreEqual (wld, node1.Root);
            Assert.AreEqual (wld, node2.Root);
        }

        /// <summary>
        /// World.Find は Node.Find を置き換える高速版（キャッシュ）が上書き実装されている
        /// </summary>
        [TestMethod]
        public void Test_Find_by_Name () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            var node3 = new Node ("Node3");
            var node4 = new Node ("Node4");
            node1.AddChild (node2);
            node1.AddChild (node3);

            var wld = new World ();
            wld.AddChild (node1);

            Assert.AreEqual (node1, wld.Find ("Node1"));
            Assert.AreEqual (node2, wld.Find ("Node2"));
            Assert.AreEqual (node3, wld.Find ("Node3"));
            Assert.AreEqual (null, wld.Find ("Node4"));

            // Update()を呼ぶまで更新されない
            wld.AddChild (node4);
            Assert.AreEqual (null, wld.Find ("Node4"));

            wld.Update (0);
            Assert.AreEqual (node4, wld.Find ("Node4"));

        }

        /// <summary>
        /// World.Finds は Node.Finds を置き換える高速版（キャッシュ）が上書き実装されている
        /// </summary>
        [TestMethod]
        public void Test_Finds_by_Name () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            var node3 = new Node ("Node3");
            var node4 = new Node ("Node1");
            var node5 = new Node ("Node1");
            var node6 = new Node ("Node6");
            node1.AddChild (node2);
            node1.AddChild (node3);
            node2.AddChild (node4);
            node3.AddChild (node5);

            var wld = new World ();
            wld.AddChild (node1);

            Assert.AreEqual (3, wld.Finds ("Node1").Count ());
            Assert.AreEqual (1, wld.Finds ("Node2").Count ());
            Assert.AreEqual (1, wld.Finds ("Node3").Count ());
            Assert.AreEqual (0, wld.Finds ("Node4").Count ());

            // Update()を呼ぶまで更新されない
            wld.AddChild (node6);
            Assert.AreEqual (0, wld.Finds ("Node6").Count ());

            wld.Update (0);
            Assert.AreEqual (1, wld.Finds ("Node6").Count ());
        }

        [TestMethod]
        public void Test_Find_by_Predicate () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            var node3 = new Node ("Node3");
            var node4 = new Node ("Node4");
            node1.AddChild (node2);
            node1.AddChild (node3);

            var wld = new World ();
            wld.AddChild (node1);

            Assert.AreEqual (node1, wld.Find (x => x.Name == "Node1"));
            Assert.AreEqual (node2, wld.Find (x => x.Name == "Node2"));
            Assert.AreEqual (node3, wld.Find (x => x.Name == "Node3"));
            Assert.AreEqual (null, wld.Find (x => x.Name == "Node4"));

            // Update()を呼ぶまで更新されない
            wld.AddChild (node4);
            Assert.AreEqual (null, wld.Find (x => x.Name == "Node4"));

            wld.Update (0);
            Assert.AreEqual (node4, wld.Find (x => x.Name == "Node4"));
        }

        [TestMethod]
        public void Test_Finds_by_Predicate () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            var node3 = new Node ("Node3");
            var node4 = new Node ("Node1");
            var node5 = new Node ("Node1");
            var node6 = new Node ("Node6");
            node1.AddChild (node2);
            node1.AddChild (node3);
            node2.AddChild (node4);
            node3.AddChild (node5);

            var wld = new World ();
            wld.AddChild (node1);

            Assert.AreEqual (3, wld.Finds (x => x.Name == "Node1").Count());
            Assert.AreEqual (1, wld.Finds (x => x.Name == "Node2").Count ());
            Assert.AreEqual (1, wld.Finds (x => x.Name == "Node3").Count ());
            Assert.AreEqual (0, wld.Finds (x => x.Name == "Node4").Count ());

            // Update()を呼ぶまで更新されない
            wld.AddChild (node6);
            Assert.AreEqual (0, wld.Finds (x => x.Name == "Node6").Count ());

            wld.Update (0);
            Assert.AreEqual (1, wld.Finds (x => x.Name == "Node6").Count ());
        }

        [TestMethod]
        public void Test_Deliverable () {
            var node = new Node ("Node");
            var mbox = new MailBox ("Node");
            node.Attach (mbox);

            var wld = new World ();
            wld.AddChild (node);

            var recved = false;
            mbox.Action += (from, address, letter) => {
                recved = true;
            };

            
            node.Deliverable = true;
            wld.PostOffice.Post (node, "Node", "Hello World");
            wld.Deliver ();

            Assert.AreEqual (true, recved);

            recved = false;

            node.Deliverable = false;
            wld.PostOffice.Post (node, "Node", "Hello World");
            wld.Deliver ();

            Assert.AreEqual (false, recved);
        }

        [TestMethod]
        public void Test_Updatable () {
            // テストしたような、してないような

        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// World.Destroy は Node.Destroy とは別もの
        /// </remarks>
        [TestMethod]
        public void Test_Destroy () {
            var wld = new MyWorld ("World");
            var node1 = new MyNode ("Node1");
            var node2 = new MyNode ("Node2");
            var node3 = new MyNode ("Node3");
            wld.AddChild (node1);
            node1.AddChild (node2);
            node2.AddChild (node3);

            // 即時ファイナライズ
            wld.Destroy ();

            Assert.AreEqual (true, node1.IsDestroyed);
            Assert.AreEqual (true, node2.IsDestroyed);
            Assert.AreEqual (true, node3.IsDestroyed);
            
            Assert.AreEqual (true, node1.IsFinalized);
            Assert.AreEqual (true, node2.IsFinalized);
            Assert.AreEqual (true, node3.IsFinalized);

            Assert.AreEqual (true, node1.IsDisposed);
            Assert.AreEqual (true, node2.IsDisposed);
            Assert.AreEqual (true, node3.IsDisposed);

            Assert.AreEqual (true, wld.IsDestroyed);
            Assert.AreEqual (true, wld.IsFinalized);
            Assert.AreEqual (true, wld.IsDisposed);
            Assert.AreEqual (0, wld.ChildCount);
        }
    }
}
