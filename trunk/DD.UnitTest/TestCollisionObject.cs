using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestCollisionObject {
        public class MyComponent : Component{
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
            var col = new CollisionObject ();

            Assert.AreEqual (null, col.Shape);
            Assert.AreEqual (-1, col.CollideWith);
            Assert.AreEqual (0, col.IgnoreWith);
            Assert.AreEqual (0, col.OverlapCount);
            Assert.AreEqual (0, col.Overlaps.Count ());
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

            col.CollideWith = 1 << 1 | 1<<2 | 1<<3;

            Assert.AreEqual (1 << 1 | 1 << 2 | 1 << 3, col.CollideWith);
        }

        [TestMethod]
        public void Test_SetIgnoreWith () {
            var col = new CollisionObject ();

            col.IgnoreWith = 1 << 1 | 1 << 2 | 1 << 3;

            Assert.AreEqual (1 << 1 | 1 << 2 | 1 << 3, col.IgnoreWith);
        }


        [TestMethod]
        public void Test_OnPrepareCollisions () {
            var node = new Node();
            var col = new CollisionObject ();
            col.Shape = new BoxShape (1, 1, 1);
            node.Attach(col);
            node.Translate (1, 2, 3);

            var wld = new World();
            wld.AddChild(node);

            col.OnPrepareCollisions ();

            
            // (1) コリジョン ワールドへ登録済み
            Assert.AreEqual (1, wld.CollisionAnlyzer.CollisionObjectCount);

            // (2) DDワールドからコリジョン ワールドへ座標を反映
            Assert.AreEqual (1f, col.Data.WorldTransform.M41);
            Assert.AreEqual (2f, col.Data.WorldTransform.M42);
            Assert.AreEqual (3f, col.Data.WorldTransform.M43);

        }

        [TestMethod]
        public void Test_Overlaps () {
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

            // コリジョン発生
            wld.Analyze ();

            Assert.AreEqual (1, col1.OverlapCount);
            Assert.AreEqual (1, col2.OverlapCount);

            // コリジョン消失
            node2.Translate (10, 0, 0);
            wld.Analyze ();

            Assert.AreEqual (0, col1.OverlapCount);
            Assert.AreEqual (0, col2.OverlapCount);
        }

        [TestMethod]
        public void Test_Invoke_ColisionExit () {
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

            // コリジョン発生
            wld.Analyze ();

            Assert.AreEqual (true, cmp1.IsCollisionEnterCalled);
            Assert.AreEqual (true, cmp2.IsCollisionEnterCalled);

            // コリジョン消失
            node2.Translate (10, 0, 0);
            wld.Analyze ();

            Assert.AreEqual (true, cmp1.IsCollisionExitCalled);
            Assert.AreEqual (true, cmp2.IsCollisionExitCalled);
        }


    }
}
