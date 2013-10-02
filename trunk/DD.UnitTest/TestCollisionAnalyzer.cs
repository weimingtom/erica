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
        public class MyComponent : Component {
            public bool IsCollisionEnterCalled { get; private set; }
            public bool IsCollisionExitCalled { get; private set; }
            public override void OnCollisionEnter (Node collidee) {
                this.IsCollisionEnterCalled = true;
            }
            public override void OnCollisionExit (Node collidee) {
                this.IsCollisionExitCalled = true;
            }
        }
        
        [TestMethod]
        public void Test_New () {
            var ca = new CollisionAnalyzer ();

            Assert.IsNotNull (ca.CollisionWorld);
            Assert.AreEqual (0, ca.CollisionObjectCount);
            Assert.AreEqual (0, ca.CollisionObjects.Count());
            Assert.AreEqual (0, ca.OverlapppingPairCount);
            Assert.AreEqual (0, ca.OverlappingPairs.Count ());
        }

        [TestMethod]
        public void Test_AddCollisionObject () {
            var ca = new CollisionAnalyzer ();

            var node = new Node ();
            var col = new CollisionObject ();
            col.Shape = new SphereShape (1);
            node.Attach (col);

            ca.AddGhostObject (node);

            Assert.AreEqual (1, ca.CollisionObjectCount);
            Assert.AreEqual (1, ca.CollisionObjects.Count ());
            Assert.AreEqual (true, ca.IsRegistered (node));
        }

        [TestMethod]
        public void Test_RemoveCollisionObject () {
            var ca = new CollisionAnalyzer ();

            var node1 = new Node ();
            var col1 = new CollisionObject ();
            col1.Shape = new SphereShape (1);
            node1.Attach (col1);

            var node2 = new Node ();
            var col2 = new CollisionObject ();
            col2.Shape = new SphereShape (1);
            node2.Attach (col2);

            ca.AddGhostObject (node1);
            ca.AddGhostObject (node2);

            ca.RemoveGhostObject (node1);

            Assert.AreEqual (1, ca.CollisionObjectCount);
            Assert.AreEqual (1, ca.CollisionObjects.Count ());
            Assert.AreEqual (true, ca.IsRegistered (node2));
        }


        [TestMethod]
        public void Test_RegisterCollisionObject () {
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
            wld.CollisionUpdate ();

            Assert.AreEqual (2, wld.CollisionAnalyzer.CollisionObjectCount);
            Assert.AreEqual (2, wld.CollisionAnalyzer.CollisionObjects.Count ());

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Distance () {
            var node1 = new Node ("Node1");
            node1.Attach (new CollisionObject ());
            node1.CollisionObject.Shape = new BoxShape (1, 1, 1);
            node1.CollisionObject.Offset = new Vector3 (2, 0, 0);

            var node2 = new Node ("Node2");
            node2.Attach (new CollisionObject ());
            node2.CollisionObject.Shape = new BoxShape (1, 1, 1);
            node2.Translate (-2, 0, 0);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            // Distance()はスタティック関数なので
            // コリジョンワールドは使用しない
            //wld.CollisionUpdate ();
      
            Assert.AreEqual(2, CollisionAnalyzer.Distance(node1.CollisionObject, node2.CollisionObject), 0.0001f);
         
            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Overlapped () {
            var node1 = new Node ("Node1");
            node1.Attach (new CollisionObject ());
            node1.CollisionObject.Shape = new BoxShape (1, 1, 1);
       
            var node2 = new Node ("Node2");
            node2.Attach (new CollisionObject ());
            node2.CollisionObject.Shape = new BoxShape (1, 1, 1);
 
            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            // Overlapped()はスタティック関数なので
            // コリジョンワールドは使用しない
            //wld.CollisionUpdate ();

            node2.SetTranslation (0, 0, 0);
            Assert.AreEqual (true, CollisionAnalyzer.Overlapped (node1.CollisionObject, node2.CollisionObject));

            node2.SetTranslation (1, 0, 0);
            Assert.AreEqual (true, CollisionAnalyzer.Overlapped (node1.CollisionObject, node2.CollisionObject));

            node2.SetTranslation (2, 0, 0);
            Assert.AreEqual (false, CollisionAnalyzer.Overlapped (node1.CollisionObject, node2.CollisionObject));

            node1.CollisionObject.SetOffset (1, 0, 0);
            Assert.AreEqual (true, CollisionAnalyzer.Overlapped (node1.CollisionObject, node2.CollisionObject));

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_OverlappingObejcts () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");
            var node3 = new Node ("Node3");
            node1.Attach (new CollisionObject ());
            node2.Attach (new CollisionObject ());
            node3.Attach (new CollisionObject ());
            node1.CollisionObject.Shape = new BoxShape (1, 1, 1);
            node2.CollisionObject.Shape = new BoxShape (1, 1, 1);
            node3.CollisionObject.Shape = new BoxShape (1, 1, 1);

            node1.SetTranslation (1, 0, 0);
            node2.SetTranslation (3, 0, 0);
            node3.SetTranslation (5, 0, 0);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);

            // ここで登録される
            // コリジョンの解析とオーバーラップの判定
            wld.CollisionUpdate ();

            Assert.AreEqual (2, wld.CollisionAnalyzer.OverlapppingPairCount);
            Assert.AreEqual (2, wld.CollisionAnalyzer.OverlappingPairs.Count ());

            Assert.AreEqual (new OverlappingPair(node1, node2), wld.CollisionAnalyzer.OverlappingPairs.ElementAt (0));
            Assert.AreEqual (new OverlappingPair (node2, node3), wld.CollisionAnalyzer.OverlappingPairs.ElementAt (1));

            Assert.AreEqual (1, node1.CollisionObject.OverlappingObjectCount);
            Assert.AreEqual (2, node2.CollisionObject.OverlappingObjectCount);
            Assert.AreEqual (1, node3.CollisionObject.OverlappingObjectCount);

            Assert.AreEqual (node2, node1.CollisionObject.GetOverlappingObject (0));
            Assert.AreEqual (node1, node2.CollisionObject.GetOverlappingObject (0));
            Assert.AreEqual (node3, node2.CollisionObject.GetOverlappingObject (1));
            Assert.AreEqual (node2, node3.CollisionObject.GetOverlappingObject (0));
        }

        [TestMethod]
        public void Test_OnColisionEnter () {
            var node1 = new Node ("Node1");
            var cmp1 = new MyComponent ();
            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (1, 1, 1);
            node1.Attach (cmp1);
            node1.Attach (col1);

            var node2 = new Node ("Node2");
            var cmp2 = new MyComponent ();
            var col2 = new CollisionObject ();
            col2.Shape = new BoxShape (1, 1, 1);
            node2.Attach (cmp2);
            node2.Attach (col2);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            node1.Translate (1, 0, 0);

            // ここでコリジョン発生
            wld.CollisionUpdate ();

            Assert.AreEqual (true, cmp1.IsCollisionEnterCalled);
            Assert.AreEqual (true, cmp2.IsCollisionEnterCalled);

            // 離れる
            node2.Translate (10, 0, 0);

            // ここでコリジョン消失
            wld.CollisionUpdate ();

            Assert.AreEqual (true, cmp1.IsCollisionExitCalled);
            Assert.AreEqual (true, cmp2.IsCollisionExitCalled);
        }

        [TestMethod]
        public void Test_OnColisionExit () {
            var node1 = new Node ("Node1");
            var cmp1 = new MyComponent ();
            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (1, 1, 1);
            node1.Attach (cmp1);
            node1.Attach (col1);

            var node2 = new Node ("Node2");
            var cmp2 = new MyComponent ();
            var col2 = new CollisionObject ();
            col2.Shape = new BoxShape (1, 1, 1);
            node2.Attach (cmp2);
            node2.Attach (col2);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            node1.Translate (1, 0, 0);

            // ここでコリジョン発生
            wld.CollisionUpdate ();

            Assert.AreEqual (true, cmp1.IsCollisionEnterCalled);
            Assert.AreEqual (true, cmp2.IsCollisionEnterCalled);

            // 離れる
            node2.Translate (10, 0, 0);

            // ここでコリジョン消失
            wld.CollisionUpdate ();

            Assert.AreEqual (true, cmp1.IsCollisionExitCalled);
            Assert.AreEqual (true, cmp2.IsCollisionExitCalled);

        }

        [TestMethod]
        public void Test_SyncWithCollisionWorld () {
            var node1 = new Node ("Node1");
            node1.Attach (new CollisionObject ());
            node1.CollisionObject.Shape = new BoxShape (1, 1, 1);
            node1.CollisionObject.Offset = new Vector3 (1, 2, 3);

            var node2 = new Node ("Node2");
            node2.Attach (new CollisionObject ());
            node2.CollisionObject.Shape = new BoxShape (1, 1, 1);
            node2.Translate (4, 5, 6);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            // コリジョン ワールドと同期
            wld.CollisionUpdate ();

            // BulletPhysics側ではオフセット込みの座標
            Assert.AreEqual (new Vector3 (1, 2, 3), node1.CollisionObject.Data.WorldTransform.Origin.ToDD());
            Assert.AreEqual (new Vector3 (4, 5, 6), node2.CollisionObject.Data.WorldTransform.Origin.ToDD ());

        }

    }
}
