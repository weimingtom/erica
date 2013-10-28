using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest {
    [TestClass]
    public class TestComponent {
        class MyComponent : Component, IDisposable {
            public bool IsFinalized { get; private set; }
            public bool IsDisposed { get; private set; }
            public bool IsMailed { get; private set; }
            public object Letter { get; private set; }
            public Node LetterFrom { get; private set; }
            public string LetterTo { get; private set; }
            public int UpdateInitCount { get; private set; }
            public int PhysicsUpdateInitCount { get; private set; }
            public new T GetComponent<T> () where T : Component {
                return base.GetComponent<T> ();
            }
            public new T GetComponent<T> (int index) where T : Component {
                return base.GetComponent<T> (index);
            }
            public new Node GetNode (Func<Node, bool> pred) {
                return base.GetNode (pred);
            }
            public new void SendMessage (string address, object letter) {
                base.SendMessage (address, letter);
            }
            public void Dispose () {
                this.IsDisposed = true;
            }
            public override void OnFinalize () {
                this.IsFinalized = true;
            }
            public override void OnMailBox (Node from, string to, object letter) {
                this.IsMailed = true;
                this.LetterFrom = from;
                this.LetterTo = to;
                this.Letter = letter;
            }
            public override void OnUpdateInit (long msec) {
                this.UpdateInitCount += 1;
            }
            public override void OnPhysicsUpdateInit (long msec) {
                this.PhysicsUpdateInitCount += 1;
            }
        }

        [TestMethod]
        public void Test_New () {
            var comp = new Component ();

            Assert.AreEqual (null, comp.Node);
        }

        [TestMethod]
        public void Test_Node () {
            var comp = new Component ();
            var node = new Node ();

            node.Attach (comp);
            Assert.AreEqual (node, comp.Node);

            node.Detach (comp);
            Assert.AreEqual (null, comp.Node);
        }

        [TestMethod]
        public void Test_IsUpdateInitCalled () {
            var comp = new Component ();

            comp.IsUpdateInitCalled = true;
            Assert.AreEqual (true, comp.IsUpdateInitCalled);

            comp.IsUpdateInitCalled = false;
            Assert.AreEqual (false, comp.IsUpdateInitCalled);

        }

        [TestMethod]
        public void Test_IsPhysicsUpdateInitCalled () {
            var comp = new Component ();

            comp.IsPhysicsUpdateInitCalled = true;
            Assert.AreEqual (true, comp.IsPhysicsUpdateInitCalled);

            comp.IsPhysicsUpdateInitCalled = false;
            Assert.AreEqual (false, comp.IsPhysicsUpdateInitCalled);

        }

        [TestMethod]
        public void Test_NodeName () {
            var comp = new Component ();
            var node = new Node ("Node");

            Assert.AreEqual ("null", comp.NodeName);

            node.Attach (comp);
            Assert.AreEqual ("Node", comp.NodeName);

            node.Detach (comp);
            Assert.AreEqual ("null", comp.NodeName);
        }

        [TestMethod]
        public void Test_World () {
            var comp = new Component ();
            var node = new Node ();
            var wld = new World ();


            Assert.AreEqual (null, comp.World);

            node.Attach (comp);

            Assert.AreEqual (null, comp.World);

            wld.AddChild (node);

            Assert.AreEqual (wld, comp.World);

            wld.RemoveChild (node);
            Assert.AreEqual (null, comp.World);

            node.Detach (comp);
            Assert.AreEqual (null, comp.World);
        }

        [TestMethod]
        public void Test_InputReceiver () {
            var input = new InputReceiver ();
            var comp = new Component ();

            var node = new Node ();
            node.Attach (input);
            node.Attach (comp);

            Assert.AreEqual (input, comp.Input);

            var wld = new World ();
            wld.AddChild (node);

            Assert.AreNotEqual (wld.InputReceiver, comp.Input);
        }

        [TestMethod]
        public void Test_ClockTower () {
            var tower = new ClockTower ();
            var comp = new Component ();

            var node = new Node ();
            node.Attach (tower);
            node.Attach (comp);

            Assert.AreEqual (tower, comp.Time);

            var wld = new World ();
            wld.AddChild (node);

            Assert.AreNotEqual (wld.ClockTower, comp.Time);
        }

        [TestMethod]
        public void Test_AnimationController () {
            var anim = new AnimationController ();
            var comp = new Component ();

            var node = new Node ();
            node.Attach (anim);
            node.Attach (comp);

            Assert.AreEqual (anim, comp.Animation);

            var wld = new World ();
            wld.AddChild (node);

            Assert.AreNotEqual (wld.AnimationController, comp.Animation);
        }

        [TestMethod]
        public void Test_SoundPlayer () {
            var sound = new SoundPlayer ();
            var comp = new MyComponent ();

            var node = new Node ();
            node.Attach (sound);
            node.Attach (comp);

            Assert.AreEqual (sound, comp.Sound);

            var wld = new World ();
            wld.AddChild (node);

            Assert.AreNotEqual (wld.SoundPlayer, comp.Sound);
        }

        [TestMethod]
        public void Test_PostOffice () {
            var post = new PostOffice ();
            var cmp = new MyComponent ();

            var node = new Node ();
            node.Attach (post);
            node.Attach (cmp);

            Assert.AreEqual (post, cmp.PostOffice);

            var wld = new World ();
            wld.AddChild (node);

            Assert.AreNotEqual (wld.PostOffice, cmp.PostOffice);
        }

        [TestMethod]
        public void Test_Logger () {
            var log = new Logger ();
            var cmp = new Component ();

            var node = new Node ();
            node.Attach (log);
            node.Attach (cmp);

            Assert.AreEqual (log, cmp.Logger);

            var wld = new World ();
            wld.AddChild (node);

            Assert.AreNotEqual (wld.Logger, cmp.Logger);
        }

        [TestMethod]
        public void Test_CollisionAnalyzer () {
            var ca = new CollisionAnalyzer ();
            var cmp = new MyComponent ();

            var node = new Node ();
            node.Attach (ca);
            node.Attach (cmp);

            var wld = new World ();

            wld.AddChild (node);
            Assert.AreNotEqual (wld.CollisionAnalyzer, cmp.CollisionAnalyzer);

            wld.RemoveChild (node);
            Assert.AreEqual (ca, cmp.CollisionAnalyzer);
        }

        [TestMethod]
        public void Test_PhysicsSimulator () {
            var phys = new PhysicsSimulator ();
            var cmp = new MyComponent ();

            var node = new Node ();
            node.Attach (phys);
            node.Attach (cmp);

            var wld = new World ();

            wld.AddChild (node);
            Assert.AreNotEqual (wld.PhysicsSimulator, cmp.PhysicsSimulator);

            wld.RemoveChild (node);
            Assert.AreEqual (phys, cmp.PhysicsSimulator);
        }

 
        [TestMethod]
        public void Test_GetComponent () {
            var comp1 = new MyComponent ();
            var comp2 = new MyComponent ();
            var node = new Node ();
            node.Attach (comp1);
            node.Attach (comp2);

            Assert.AreEqual (comp1, comp1.GetComponent<MyComponent> ());
            Assert.AreEqual (comp1, comp1.GetComponent<MyComponent> (0));
            Assert.AreEqual (comp2, comp1.GetComponent<MyComponent> (1));
        }

        [TestMethod]
        public void Test_GetNode () {
            var comp = new MyComponent ();
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            var node = new Node ();
            node.Attach (comp);

            var wld = new World ();
            wld.AddChild (node);
            wld.AddChild (node1);
            wld.AddChild (node2);

            Assert.AreEqual (node1, comp.GetNode (x => x.Name == "Node1"));
            Assert.AreEqual (node2, comp.GetNode (x => x.Name == "Node2"));

        }

    
        [TestMethod]
        public void Test_SendMessage () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");

            var cmp1 = new MyComponent ();
            var cmp2 = new MyComponent ();
            node1.Attach (cmp1);
            node2.Attach (cmp2);

            var mbox1 = new MailBox ("Node1");
            var mbox2 = new MailBox ("Node2");
            node1.Attach (mbox1);
            node2.Attach (mbox2);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            cmp1.SendMessage ("Node2", "Node: 1 --> 2");
            cmp2.SendMessage ("Node1", "Node: 2 --> 1");

            wld.Deliver ();

            Assert.AreEqual ("Node1", cmp1.LetterTo);
            Assert.AreEqual (node2, cmp1.LetterFrom);
            Assert.AreEqual ("Node: 2 --> 1", (string)cmp1.Letter);

            Assert.AreEqual ("Node2", cmp2.LetterTo);
            Assert.AreEqual (node1, cmp2.LetterFrom);
            Assert.AreEqual ("Node: 1 --> 2", (string)cmp2.Letter);
        }

        [TestMethod]
        public void Test_UpdateInit () {
            var cmp = new MyComponent ();
            var node = new Node ();
            node.Attach (cmp);

            var wld = new World ();
            wld.AddChild (node);

            wld.Update (0);
            Assert.AreEqual (true, cmp.IsUpdateInitCalled);
            Assert.AreEqual (1, cmp.UpdateInitCount);

            wld.Update (0);
            Assert.AreEqual (true, cmp.IsUpdateInitCalled);
            Assert.AreEqual (1, cmp.UpdateInitCount);
        }

        [TestMethod]
        public void Test_PhysicsUpdateInit () {
            var cmp = new MyComponent ();
            var node = new Node ();
            node.Attach (cmp);

            var wld = new World ();
            wld.AddChild (node);

            wld.PhysicsUpdate (0);
            Assert.AreEqual (true, cmp.IsPhysicsUpdateInitCalled);
            Assert.AreEqual (1, cmp.PhysicsUpdateInitCount);

            wld.PhysicsUpdate (0);
            Assert.AreEqual (true, cmp.IsPhysicsUpdateInitCalled);
            Assert.AreEqual (1, cmp.PhysicsUpdateInitCount);
        }

    }
}
