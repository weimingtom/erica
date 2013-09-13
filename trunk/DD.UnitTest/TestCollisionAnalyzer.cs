using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Distance()やRayCast()のテストはWorldのテストの方で行う
// どうせWorldがないとテストできないので

namespace DD.UnitTest {
    [TestClass]
    public class TestCollisionAnalyzer {
        [TestMethod]
        public void Test_New () {
            var ca = new CollisionAnalyzer ();

            Assert.IsNotNull (ca.CollisionWorld);
            Assert.AreEqual (0, ca.CollisionObjectCount);
            Assert.AreEqual (0, ca.CollisionObjects.Count());
        }

        [TestMethod]
        public void Test_Register_CollisionObjects () {
            var node1 = new Node ("Node1");
            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (1, 1, 1);
            node1.Attach (col1);

            var node2 = new Node ("Node2");
            var col2 = new CollisionObject ();
            col2.Shape = new BoxShape (1, 1, 1);
            node2.Attach (col2);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            // ここで登録される
            wld.Analyze ();

            Assert.AreEqual (2, wld.CollisionAnlyzer.CollisionObjectCount);
            Assert.AreEqual (2, wld.CollisionAnlyzer.CollisionObjects.Count ());

            wld.Destroy ();
        }


    }
}
