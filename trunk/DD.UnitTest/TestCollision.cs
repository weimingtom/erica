using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestCollision {

        [TestMethod]
        public void Test_Overlap_Null_to_Null () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node2.Attach (new BoxCollision (1, 1, 1));

            Assert.AreEqual (false, Node.Overlap (node1, null));
            Assert.AreEqual (false, Node.Overlap (null, node2));
            Assert.AreEqual (false, Node.Overlap (node1, node2));
        }
        
        [TestMethod]
        public void Test_Overlap_Box_to_Box () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new BoxCollision (1, 1, 1));
            node2.Attach (new BoxCollision (1, 1, 1));

            node2.SetTranslation (1, 0, 0);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));

            node2.SetTranslation (2, 0, 0);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));

            node2.SetTranslation (3, 0, 0);
            Assert.AreEqual (false, Node.Overlap (node1, node2));
            Assert.AreEqual (false, Node.Overlap (node2, node1));

            node2.Collision.SetOffset (-1, 0, 0);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));

        }

        [TestMethod]
        public void Test_Overlap_Sphere_to_Sphere () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new SphereCollision (1));
            node2.Attach (new SphereCollision (1));

            node2.SetTranslation (1, 0, 0);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));

            node2.SetTranslation (2, 0, 0);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));

            node2.SetTranslation (3, 0, 0);
            Assert.AreEqual (false, Node.Overlap (node1, node2));
            Assert.AreEqual (false, Node.Overlap (node2, node1));

            node2.Collision.SetOffset (-1, 0, 0);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));
        }

        [TestMethod]
        public void Test_Overlap_Sphere_to_Box () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new SphereCollision (1));
            node2.Attach (new BoxCollision (1,1,1));

            node2.SetTranslation (1, 0, 0);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));

            node2.SetTranslation (2, 0, 0);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));

            node2.SetTranslation (3, 0, 0);
            Assert.AreEqual (false, Node.Overlap (node1, node2));
            Assert.AreEqual (false, Node.Overlap (node2, node1));

            node2.Collision.SetOffset (-1, 0, 0);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));
        }

        /// <summary>
        /// </summary>
        /// <remarks>
        /// ポリゴン形状は標準で 0.005 の”スキン”で被われている。 
        /// </remarks>
        [TestMethod]
        public void Test_Overlap_Rotated_Objects () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new BoxCollision (0.9f, 0.9f, 0.9f));
            node2.Attach (new BoxCollision (0.9f, 0.9f, 0.9f));

            node2.SetTranslation (2, 0, 0);

            node2.SetRotation (0, 0, 0, 1);
            Assert.AreEqual (false, Node.Overlap (node1, node2));
            Assert.AreEqual (false, Node.Overlap (node2, node1));

            node2.SetRotation (45, 0, 0, 1);
            Assert.AreEqual (true, Node.Overlap (node1, node2));
            Assert.AreEqual (true, Node.Overlap (node2, node1));

            node2.SetRotation (90, 0, 0, 1);
            Assert.AreEqual (false, Node.Overlap (node1, node2));
            Assert.AreEqual (false, Node.Overlap (node2, node1));
        }

        [TestMethod]
        public void Test_Distance_Null_to_Null () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");

            Assert.AreEqual (Single.NaN, Node.Distance (node1, node2));
        }


        [TestMethod]
        public void Test_Distance_Box_to_Box () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new BoxCollision (1, 1, 1));
            node2.Attach (new BoxCollision (1, 1, 1));

            // 重複
            node2.SetTranslation (1, 0, 0);
            Assert.AreEqual (0, Node.Distance (node1, node2));
            Assert.AreEqual (0, Node.Distance (node2, node1));

            // 重複
            node2.SetTranslation (2, 0, 0);
            Assert.AreEqual (0, Node.Distance (node1, node2));
            Assert.AreEqual (0, Node.Distance (node2, node1));

            // 離れている
            node2.SetTranslation (3, 0, 0);
            Assert.AreEqual (1, Node.Distance (node1, node2), 0.02f);
            Assert.AreEqual (1, Node.Distance (node2, node1), 0.02f);
        }

        [TestMethod]
        public void Test_Distance_Sphere_to_Sphere () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new SphereCollision (1));
            node2.Attach (new SphereCollision (1));

            // 重複
            node2.SetTranslation (1, 0, 0);
            Assert.AreEqual (0, Node.Distance (node1, node2));
            Assert.AreEqual (0, Node.Distance (node2, node1));

            // 重複
            node2.SetTranslation (2, 0, 0);
            Assert.AreEqual (0, Node.Distance (node1, node2));
            Assert.AreEqual (0, Node.Distance (node2, node1));

            // 離れている
            node2.SetTranslation (3, 0, 0);
            Assert.AreEqual (1, Node.Distance (node1, node2), 0.02f);
            Assert.AreEqual (1, Node.Distance (node2, node1), 0.02f);
        }

        [TestMethod]
        public void Test_Distance_Box_to_Sphere () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new SphereCollision (1));
            node2.Attach (new BoxCollision (1,1,1));

            // 重複
            node2.SetTranslation (1, 0, 0);
            Assert.AreEqual (0, Node.Distance (node1, node2));
            Assert.AreEqual (0, Node.Distance (node2, node1));

            // 重複
            node2.SetTranslation (2, 0, 0);
            Assert.AreEqual (0, Node.Distance (node1, node2));
            Assert.AreEqual (0, Node.Distance (node2, node1));

            // 離れている
            node2.SetTranslation (3, 0, 0);
            Assert.AreEqual (1, Node.Distance (node1, node2), 0.02f);
            Assert.AreEqual (1, Node.Distance (node2, node1), 0.02f);
        }

        [TestMethod]
        public void Test_Distance_Rotated_Objects () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new SphereCollision (1));
            node2.Attach (new BoxCollision (1, 1, 1));

            node2.SetTranslation (3, 0, 0);

            node2.SetRotation (0, 0, 0, 1);
            Assert.AreEqual (1, Node.Distance (node1, node2), 0.02f);
            Assert.AreEqual (1, Node.Distance (node2, node1), 0.02f);

            node2.SetRotation (45, 0, 0, 1);
            Assert.AreEqual (0.576f, Node.Distance (node1, node2), 0.02f);
            Assert.AreEqual (0.576f, Node.Distance (node2, node1), 0.02f);

            node2.SetRotation (90, 0, 0, 1);
            Assert.AreEqual (1, Node.Distance (node1, node2), 0.02f);
            Assert.AreEqual (1, Node.Distance (node2, node1), 0.02f);
        }

        [TestMethod]
        public void Test_RayCast_Box () {
            var node = new Node ("Node1");
            node.Attach (new BoxCollision (1, 1, 1));

            var dist = Node.RayCast (node, new Vector3(0,-2,0), new Vector3(0,2,0));
            Assert.AreEqual (1f, dist, 0.02f);

            dist = Node.RayCast (node, new Vector3 (0, -4, 0), new Vector3 (0, -2, 0));
            Assert.AreEqual (0, dist);

            dist = Node.RayCast (node, new Vector3 (0, 0, 0), new Vector3 (0, 2, 0));
            Assert.AreEqual (0, dist);
        }

        [TestMethod]
        public void Test_RayCast_Sphere () {
            var node = new Node ("Node1");
            node.Attach (new SphereCollision (1));

            var dist = Node.RayCast (node, new Vector3 (0, -2, 0), new Vector3 (0, 2, 0));
            Assert.AreEqual (1f, dist, 0.02f);

            dist = Node.RayCast (node, new Vector3 (0, -4, 0), new Vector3 (0, -2, 0));
            Assert.AreEqual (0, dist);

            dist = Node.RayCast (node, new Vector3 (0, 0, 0), new Vector3 (0, 2, 0));
            Assert.AreEqual (0, dist);
        }

        [TestMethod]
        public void Test_RayCast_Null () {
            var f = Node.RayCast (null, new Vector3 (0, 0, 0), new Vector3 (0, 1, 0));
            Assert.AreEqual (Single.NaN, f);
        }

   

        [TestMethod]
        public void Test_Sweep_Box () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new BoxCollision (1, 1, 1));
            node2.Attach (new BoxCollision (1, 1, 1));

            node2.SetTranslation (5, 1, 0);

            var dist = Node.Sweep (node1, node2, new Vector3 (-1, 0, 0));
            Assert.AreEqual (0, dist, 0.02f);

            dist = Node.Sweep (node1, node2, new Vector3 (-10, 0, 0));
            Assert.AreEqual (3, dist, 0.02f);

            dist = Node.Sweep (node1, node2, new Vector3 (0, 10, 0));
            Assert.AreEqual (0, dist, 0.02f);
        }

        [TestMethod]
        public void Test_Sweep_Sphere () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            node1.Attach (new SphereCollision (1));
            node2.Attach (new SphereCollision (1));

            node2.SetTranslation (5, 1, 0);

            var f = Node.Sweep (node1, node2, new Vector3 (-1, 0, 0));
            Assert.AreEqual (0, f, 0.02f);

            f = Node.Sweep (node1, node2, new Vector3 (-10, 0, 0));
            Assert.AreEqual (3.336749f, f, 0.02f);

            f = Node.Sweep (node1, node2, new Vector3 (0, 10, 0));
            Assert.AreEqual (0, f, 0.02f);
       
        }

        [TestMethod]
        public void Test_Sweep_Null () {
            var node1 = new Node("Node1");
            var node2 = new Node("Node2");

            var f = Node.Sweep (node1, null, new Vector3 (-1, 0, 0));
            Assert.AreEqual (Single.NaN, f);

            f = Node.Sweep (null, node2, new Vector3 (-1, 0, 0));
            Assert.AreEqual (Single.NaN, f);
}        
    }
}
