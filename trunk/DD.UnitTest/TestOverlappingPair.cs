using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestOverlappingPair {
        [TestMethod]
        public void Test_New () {
            var nodeA = new Node("NodeA");
            var nodeB = new Node("NodeB");
            var pair = new OverlappingPair (nodeA, nodeB);

            Assert.AreEqual (nodeA, pair.NodeA);
            Assert.AreEqual (nodeB, pair.NodeB);

        }

        [TestMethod]
        public void Test_Equal () {
            var nodeA = new Node ("NodeA");
            var nodeB = new Node ("NodeB");
            var pair1 = new OverlappingPair (nodeA, nodeB);
            var pair2 = new OverlappingPair (nodeB, nodeA);

            Assert.IsTrue(pair1 == pair2);
            Assert.IsFalse (pair1 != pair2);
            Assert.IsTrue (((object)pair1).Equals((object)pair2));
            Assert.IsTrue (pair1.Equals (pair2));
            var a = pair1.GetHashCode ();
            var b = pair2.GetHashCode ();
        }

        [TestMethod]
        public void Test_GetHashCode () {
            var nodeA = new Node ("NodeA");
            var nodeB = new Node ("NodeB");
            var pair1 = new OverlappingPair (nodeA, nodeB);
            var pair2 = new OverlappingPair (nodeB, nodeA);

            var hash1 = pair1.GetHashCode ();
            var hash2 = pair2.GetHashCode ();

            Assert.AreEqual (hash1, hash2);
        }
    }
}
