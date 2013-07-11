using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestComponent {
        class MyComponent : Component, IDisposable  {
            public bool IsDisposed { get; private set; }
            public new T GetComponent<T> () where T : Component {
                return base.GetComponent<T> ();
            }
            public new T GetComponent<T> (int index) where T : Component {
                return base.GetComponent<T> (index);
            }
            public new Node GetNode (Func<Node, bool> pred) {
                return base.GetNode (pred);
            }
            public new void Destroy (Node node) {
                base.Destroy(node);
            }
            public new void Destroy (Component comp) {
                base.Destroy(comp);
            }
            public void Dispose () {
                this.IsDisposed = true;
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
        public void Test_NodeName  () {
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
            var wld = new World ();
            var node = new Node ();
            var input = new InputReceiver ();
            var comp = new MyComponent ();

            node.Attach (input);
            node.Attach (comp);

            Assert.AreEqual (input, comp.Input);
            
            wld.AddChild (node);

            Assert.AreNotEqual (wld.InputReceiver, comp.Input);
        }

        [TestMethod]
        public void Test_AnimationController () {
            var wld = new World ();
            var node = new Node ();
            var anim = new AnimationController ();
            var comp = new MyComponent ();

            node.Attach (anim);
            node.Attach (comp);

            Assert.AreEqual (anim, comp.Animation);

            wld.AddChild (node);

            Assert.AreNotEqual (wld.AnimationController, comp.Animation);
        }

        [TestMethod]
        public void Test_SoundPlayer () {
            var wld = new World ();
            var node = new Node ();
            var sound = new SoundPlayer ();
            var comp = new MyComponent ();

            node.Attach (sound);
            node.Attach (comp);

            Assert.AreEqual (sound, comp.Sound);

            wld.AddChild (node);

            Assert.AreNotEqual (wld.SoundPlayer, comp.Sound);
        }

        [TestMethod]
        public void Test_GetComponent () {
            var comp1 = new MyComponent ();
            var comp2 = new MyComponent ();
            var node = new Node ();
            node.Attach (comp1);
            node.Attach (comp2);

            Assert.AreEqual(comp1, comp1.GetComponent<MyComponent> ());
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
        public void Test_Destroy_by_Node () {
            var comp1 = new MyComponent ();
            var comp2 = new MyComponent ();
            
            var node1 = new Node();
            var node2 = new Node ();
            node1.Attach (comp1);
            node2.Attach (comp2);

            var wld = new World ();
            wld.AddChild (node1);

            comp1.Destroy (node1);
            comp2.Destroy (node2);

            Assert.AreEqual (true, comp1.IsDisposed);
            Assert.AreEqual (true, comp2.IsDisposed);
        }

        [TestMethod]
        public void Test_Destroy_by_Component () {
            var comp1 = new MyComponent ();
            var comp2 = new MyComponent ();

            var node1 = new Node ();
            var node2 = new Node ();
            node1.Attach (comp1);
            node2.Attach (comp2);

            var wld = new World ();
            wld.AddChild (node1);

            comp1.Destroy (comp1);
            comp2.Destroy (comp2);

            Assert.AreEqual (true, comp1.IsDisposed);
            Assert.AreEqual (true, comp2.IsDisposed);
        }

    }
}
