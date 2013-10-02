using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestCollisionObject {


        [TestMethod]
        public void Test_New () {
            var col = new CollisionObject ();

            Assert.AreEqual (null, col.Shape);
            Assert.AreEqual (-1, col.CollideWith);
            Assert.AreEqual (0, col.IgnoreWith);
            Assert.AreEqual (0, col.OverlappingObjectCount);
            Assert.AreEqual (0, col.OverlapObjects.Count ());
            Assert.AreEqual (new Vector3 (0, 0, 0), col.Offset);
        }

        [TestMethod]
        public void Test_SetShape () {
            var col = new CollisionObject ();
            var box = new BoxShape (1, 1, 1);

            Assert.AreEqual (null, col.Shape);

            col.Shape = box;

            Assert.AreEqual (box, col.Shape);
            Assert.IsNotNull (col.Data);
        }

        [TestMethod]
        public void Test_SetOffset () {
            var col = new CollisionObject ();

            col.Offset = new Vector3 (1, 2, 3);
            Assert.AreEqual (new Vector3(1,2,3), col.Offset);

            col.SetOffset (4, 5, 6);
            Assert.AreEqual (new Vector3 (4, 5, 6), col.Offset);
        }

        [TestMethod]
        public void Test_SetCollideWith () {
            var col = new CollisionObject ();

            col.CollideWith = 15;

            Assert.AreEqual (15, col.CollideWith);
        }

        [TestMethod]
        public void Test_SetIgnoreWith () {
            var col = new CollisionObject ();

            col.IgnoreWith = 255;

            Assert.AreEqual (255, col.IgnoreWith);
        }

        [TestMethod]
        public void Test_CollisionMask () {
            var col = new CollisionObject ();

            col.CollideWith = 15;
            col.IgnoreWith = 3;

            Assert.AreEqual (12, col.CollisionMask);
    
        }
     

        [TestMethod]
        public void Test_Overlaps () {
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

            // コリジョン発生
            wld.CollisionUpdate ();

            Assert.AreEqual (1, col1.OverlappingObjectCount);
            Assert.AreEqual (1, col2.OverlappingObjectCount);

            // コリジョン消失
            node2.Translate (10, 0, 0);
            wld.CollisionUpdate ();

            Assert.AreEqual (0, col1.OverlappingObjectCount);
            Assert.AreEqual (0, col2.OverlappingObjectCount);
        }



    }
}
