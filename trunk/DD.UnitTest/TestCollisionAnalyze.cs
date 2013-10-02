using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestCollisionAnalyze {

        [TestMethod]
        public void Test_CollideWith () {
            var node1 = new Node ("Node1");
            node1.Attach(new CollisionObject ());
            node1.CollisionObject.Shape = new BoxShape (1, 1, 1);
            node1.GroupID = 1;

            var node2 = new Node ("Node2");
            node2.Attach (new CollisionObject ());
            node2.CollisionObject.Shape = new BoxShape (1, 1, 1);
            node2.GroupID = 2;

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            node1.CollisionObject.CollideWith = -1;
            Assert.AreEqual (true, wld.Overlap (node1, node2));

            node1.CollisionObject.CollideWith = 1;
            Assert.AreEqual (false, wld.Overlap (node1, node2));

            node1.CollisionObject.CollideWith = 2;
            Assert.AreEqual (true, wld.Overlap (node1, node2));

            node1.CollisionObject.CollideWith = 2;
            node1.CollisionObject.IgnoreWith = 2;
            Assert.AreEqual (false, wld.Overlap (node1, node2));
        }


        [TestMethod]
        public void Test_Distance () {
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

            node1.Translate (0, 0, 0);
            node2.Translate (0, 0, 0);
            Assert.AreEqual (0, wld.Distance (node1, node2));

            node1.Translate (0, 0, 0);
            node2.Translate (10, 0, 0);
            Assert.AreEqual (8, wld.Distance (node1, node2), 0.01f);

            node1.Rotate (0, 0, 0, 0);
            node2.Rotate (45, 0, 0, 1);
            Assert.AreEqual (7.6f, wld.Distance (node1, node2), 0.01f);

            node1.Detach (col1);
            node2.Detach (col2);
            Assert.AreEqual (Single.NaN, wld.Distance (node1, node2), 0.01f);

        }

        [TestMethod]
        public void Test_Overlap () {
            var node1 = new Node ("Node1");
            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (1, 1, 1);
            node1.Attach (col1);

            var node2 = new Node ("Node1");
            var col2 = new CollisionObject ();
            col2.Shape = new BoxShape (1, 1, 1);
            node2.Attach (col2);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);


            node1.Translate (0, 0, 0);
            node2.Translate (0, 0, 0);
            Assert.AreEqual (true, wld.Overlap (node1, node2));

            node1.Translate (0, 0, 0);
            node2.Translate (10, 0, 0);
            Assert.AreEqual (false, wld.Overlap (node1, node2));

            node1.Rotate (0, 0, 0, 0);
            node2.Rotate (45, 0, 0, 1);
            Assert.AreEqual (false, wld.Overlap (node1, node2));

            node1.Detach (col1);
            node2.Detach (col2);
            Assert.AreEqual (false, wld.Overlap (node1, node2));

        }

        [TestMethod]
        public void Test_RayCast () {
            var node1 = new Node ("Node1");
            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (1, 1, 1);
            node1.Attach (col1);

            var node2 = new Node ("Node1");
            var col2 = new CollisionObject ();
            col2.Shape = new BoxShape (1, 1, 1);
            node2.Attach (col2);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            node1.Translate (1, 0, 0);
            node2.Translate (-1, 0, 0);

            wld.CollisionUpdate ();

            var start = new Vector3 (10, 0, 0);
            var end = new Vector3 (-10, 0, 0);
            var results = wld.RayCast (start, end).ToArray ();

            Assert.AreEqual (2, results.Count ());
            var result1 = results[0];
            var result2 = results[1];

            Assert.AreEqual (node1, result1.Node);
            Assert.AreEqual (0.39394f, result1.Fraction, 0.01f);
            Assert.AreEqual (new Vector3 (2, 0, 0), result1.Point);

            Assert.AreEqual (node2, result2.Node);
            Assert.AreEqual (0.49495f, result2.Fraction, 0.01f);
            Assert.AreEqual (new Vector3 (0, 0, 0), result2.Point);
        }

        [TestMethod]
        public void Test_Pick () {
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

            node1.Translate (1, 0, 0);
            node2.Translate (-1, 0, 0);

            wld.CollisionUpdate ();


            Assert.AreEqual (null, wld.Pick (new Vector3 (3, -10, 0), new Vector3 (3, 10, 0)));
            Assert.AreEqual (node1, wld.Pick (new Vector3 (2, -10, 0), new Vector3 (2, 10, 0)));
            Assert.AreEqual (node1, wld.Pick (new Vector3 (1, -10, 0), new Vector3 (1, 10, 0)));
            Assert.AreEqual (node1, wld.Pick (new Vector3 (0, -10, 0), new Vector3 (0, 10, 0)));
            Assert.AreEqual (node2, wld.Pick (new Vector3 (-1, -10, 0), new Vector3 (-1, 10, 0)));
            Assert.AreEqual (node2, wld.Pick (new Vector3 (-2, -10, 0), new Vector3 (-2, 10, 0)));
            Assert.AreEqual (null, wld.Pick (new Vector3 (-3, -10, 0), new Vector3 (-3, 10, 0)));



        }

        // （注意）
        // 接触面のどこが衝突点になるかは未定義
        // BulletPhysics等のバージョンアップによって変わる可能性がある
        [TestMethod]
        public void Test_Sweep () {
            var node1 = new Node ("Node1");
            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (1, 1, 1);
            node1.Attach (col1);
            node1.Rotate (45, 0, 0, 1);
            node1.Rotate (45, 1, 0, 0);

            var node2 = new Node ("Node2");
            var col2 = new CollisionObject ();
            col2.Shape = new BoxShape (2, 2, 2);
            col2.Offset = new Vector3 (2, 0, 0);
            node2.Attach (col2);

            var node3 = new Node ("Node3");
            var col3 = new CollisionObject ();
            col3.Shape = new BoxShape (1, 1, 1);
            col3.SetOffset (-1, 0, 0);
            node3.Attach (col3);

            var wld = new World ("World");
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);

            node1.Translate (0, 0, 0);
            node2.Translate (10, 0, 0);
            node3.Translate (-10, 0, 0);

            wld.CollisionUpdate ();

            var result = wld.Sweep (node1, new Vector3 (100, 0, 0));
            Assert.AreEqual (true, result.Hit);
            Assert.AreEqual (new Vector3 (10, 0, 0), result.Point);
            Assert.AreEqual (new Vector3 (-1, 0, 0), result.Normal);
            Assert.AreEqual (8.602f, result.Distance, 0.01f);
            Assert.AreEqual (0.09f, result.Fraction, 0.01f);

            result = wld.Sweep (node1, new Vector3 (-100, 0, 0));
            Assert.AreEqual (true, result.Hit);
            Assert.AreEqual (new Vector3 (-10, 0.2812f, -0.2812f), result.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), result.Normal);
            Assert.AreEqual (8.602f, result.Distance, 0.01f);
            Assert.AreEqual (0.09f, result.Fraction, 0.01f);
        }





    }
}
