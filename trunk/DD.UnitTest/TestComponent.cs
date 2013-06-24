using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestComponent {
        public class MyComponent : Component  {
            public new T GetComponent<T> () where T:Component{
                return base.GetComponent<T> ();
            }
            public new Node GetNode (Func<Node, bool> pred) {
                return base.GetNode (pred);
            }
            public new InputReceiver Input {
                get { return base.Input; }
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
        public void Test_GetComponent () {
            var comp = new MyComponent ();
            var node = new Node ();
            node.Attach (comp);

            Assert.AreEqual(comp, comp.GetComponent<MyComponent> ());

        }

        [TestMethod]
        public void Test_GetNode () {
            var comp = new MyComponent ();
            var node1 = new Node ();
            var node2 = new Node ();
            var node = new Node ();
            node.Attach (comp);
            node1.UserID = 1;
            node2.UserID = 2;

            var wld = new World ();
            wld.AddChild (node);
            wld.AddChild (node1);
            wld.AddChild (node2);

            Assert.AreEqual (node1, comp.GetNode (x => x.UserID == 1));
            Assert.AreEqual (node2, comp.GetNode (x => x.UserID == 2));

        }

    }
}
