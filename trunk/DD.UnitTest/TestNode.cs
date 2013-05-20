using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestNode {
        [TestMethod]
        public void Test_New () {
            var node = new Node ("Node1");

            Assert.AreEqual ("Node1", node.Name);
            Assert.AreEqual (0, node.X);
            Assert.AreEqual (0, node.Y);
            Assert.AreEqual (null, node.Parent);
            Assert.AreEqual (true, node.Visible);
            Assert.AreEqual (true, node.Clickable);
            Assert.AreEqual (0, node.ChildCount);
            Assert.AreEqual (0, node.ComponentCount);
        }

        [TestMethod]
        public void Test_AddChild () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");

            nod1.AddChild (nod2);
            nod1.AddChild (nod3);

            Assert.AreEqual (2, nod1.ChildCount);
            Assert.AreEqual (2, nod1.Children.Count ());
            Assert.AreEqual (nod2, nod1.GetChild (0));
            Assert.AreEqual (nod3, nod1.GetChild (1));

            Assert.AreEqual (nod1, nod2.Parent);
            Assert.AreEqual (nod1, nod3.Parent);
        }

        [TestMethod]
        public void Test_RemoveChild () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");

            nod1.AddChild (nod2);
            nod1.AddChild (nod3);

            Assert.AreEqual (2, nod1.ChildCount);

            nod1.RemoveChild (nod2);
            nod1.RemoveChild (nod3);

            Assert.AreEqual (0, nod1.ChildCount);
        }

        [TestMethod]
        public void Test_Downwards () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");
            var nod4 = new Node ("Node4");
            var nod5 = new Node ("Node5");

            nod1.AddChild (nod2);
            nod1.AddChild (nod3);
            nod2.AddChild (nod4);
            nod3.AddChild (nod5);

            // 幅優先
            var nodes = nod1.Downwards.ToArray ();
            Assert.AreEqual (5, nodes.Count ());
            Assert.AreEqual (nod1, nodes[0]);
            Assert.AreEqual (nod2, nodes[1]);
            Assert.AreEqual (nod3, nodes[2]);
            Assert.AreEqual (nod4, nodes[3]);
            Assert.AreEqual (nod5, nodes[4]);
        }

        [TestMethod]
        public void Test_Upwards () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");
            var nod4 = new Node ("Node4");
            var nod5 = new Node ("Node5");

            nod1.AddChild (nod2);
            nod1.AddChild (nod3);
            nod2.AddChild (nod4);
            nod3.AddChild (nod5);

            var nodes = nod5.Upwards.ToArray ();
            Assert.AreEqual (3, nodes.Count ());
            Assert.AreEqual (nod5, nodes[0]);
            Assert.AreEqual (nod3, nodes[1]);
            Assert.AreEqual (nod1, nodes[2]);
        }

        [TestMethod]
        public void Test_Root () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");
            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            Assert.AreEqual (nod1, nod1.Root);
            Assert.AreEqual (nod1, nod2.Root);
            Assert.AreEqual (nod1, nod3.Root);
        }

        [TestMethod]
        public void Test_GlobalX () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");

            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            nod1.X = 1;
            nod2.X = 2;
            nod3.X = 4;

            Assert.AreEqual (1, nod1.WindowX);
            Assert.AreEqual (3, nod2.WindowX);
            Assert.AreEqual (7, nod3.WindowX);
        }

        [TestMethod]
        public void Test_GlobalY () {
            var nod1 = new Node ("Node1");
            var nod2 = new Node ("Node2");
            var nod3 = new Node ("Node3");

            nod1.AddChild (nod2);
            nod2.AddChild (nod3);

            nod1.Y = 1;
            nod2.Y = 2;
            nod3.Y = 4;

            Assert.AreEqual (1, nod1.WindowY);
            Assert.AreEqual (3, nod2.WindowY);
            Assert.AreEqual (7, nod3.WindowY);
        }

        [TestMethod]
        public void Test_Attach () {
            var node = new Node ();
            var comp1 = new Component ();
            var comp2 = new Component ();

            node.Attach (comp1);
            node.Attach (comp2);

            Assert.AreEqual (2, node.ComponentCount);
            Assert.AreEqual (2, node.Components.Count ());
            Assert.AreEqual (comp1, node.GetComponent (0));
            Assert.AreEqual (comp2, node.GetComponent (1));

            Assert.AreEqual (node, comp1.Node);
            Assert.AreEqual (node, comp2.Node);
        }

        [TestMethod]
        public void Test_Detach () {
            var node = new Node ();
            var comp1 = new Component ();
            var comp2 = new Component ();

            node.Attach (comp1);
            node.Attach (comp2);
            node.Detach (comp1);
            node.Detach (comp2);

            Assert.AreEqual (0, node.ComponentCount);
            Assert.AreEqual (null, comp1.Node);
            Assert.AreEqual (null, comp2.Node);
        }

        [TestMethod]
        public void Test_SetBoundingBox () {
            var node = new Node ();
            node.SetBoundingBox (1, 2, 3, 4);

            Assert.AreEqual (1, node.BoundingBox.X);
            Assert.AreEqual (2, node.BoundingBox.Y);
            Assert.AreEqual (3, node.BoundingBox.Width);
            Assert.AreEqual (4, node.BoundingBox.Height);
            Assert.AreEqual (4, node.BoundingBox.X2);
            Assert.AreEqual (6, node.BoundingBox.Y2);
        }

        [TestMethod]
        public void Test_TransformToLocal () {
            var nod1 = new Node ();
            var nod2 = new Node ();
            nod1.AddChild (nod2);
            nod1.X = 1;
            nod1.Y = 2;
            nod2.X = 3;
            nod2.Y = 4;

            int x = 10;
            int y = 10;
            nod2.TransformToLocal(ref x,ref y);

            Assert.AreEqual (6, x);
            Assert.AreEqual (4, y);
        }
    }
}
