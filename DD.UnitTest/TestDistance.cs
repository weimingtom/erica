using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

// メモ：
// 明示的にDestroy()を呼んで終了しないと BulletPhysics の動作が凄く怪しいので
// 必ず最後に wld.Destroy() を呼ぶこと。

// メモ：
// BulletSharpの Box-Box の GetClosestPoints() の精度は果てしなく悪い。
// 小数点以下1桁目でもう違う。
// BulletNoXNAだと2桁目まで正しいのでBulletSharp固有の問題

namespace DD.UnitTest {
    [TestClass]
    public class TestDistance {
        [TestMethod]
        public void Test_Distance_Box_to_Box () {
            var node1 = new Node ("Node1");

            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (1, 1, 1);
            col1.SetOffset (0, 0, 0);

            node1.Attach (col1);

            var node2 = new Node ("Node2");
            var col2 = new CollisionObject ();
            col2.Shape = new BoxShape (1,1,1);
            col2.SetOffset (0, 10, 0);

            node2.Attach (col2);


            node1.Translate (0, 0, 0);
            node2.Translate (10, 0, 0);

            // 10*√2

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);
            
            wld.CollisionUpdate ();

            Assert.AreEqual(10*1.4142f - 2*1.4142f, wld.Distance (node1, node2), 0.05f);

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Distance_Sphere_to_Sphere () {
            var node1 = new Node ("Node1");

            var col1 = new CollisionObject ();
            col1.Shape = new SphereShape (1);
            col1.SetOffset (0, 0, 0);

            node1.Attach (col1);

            var node2 = new Node ("Node2");
            var col2 = new CollisionObject ();
            col2.Shape = new SphereShape (1);
            col2.SetOffset (0, 10, 0);

            node2.Attach (col2);


            node1.Translate (0, 0, 0);
            node2.Translate (10, 0, 0);

            // 10*√2

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            wld.CollisionUpdate ();

            Assert.AreEqual (10 * 1.4142f - 2, wld.Distance (node1, node2), 0.01f);

            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Distance_Overlapped_Objects () {
            var node1 = new Node ("Node1");

            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (1,1,1);

            node1.Attach (col1);

            var node2 = new Node ("Node2");
            var col2 = new CollisionObject ();
            col2.Shape = new BoxShape (1,1,1);

            node2.Attach (col2);

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            wld.CollisionUpdate ();


            node2.Translation = new Vector3 (3, 0, 0);
            node2.Rotation = new Quaternion (45, 0, 0, 1);

            var d = wld.Distance (node1, node2);
            Assert.AreEqual (3 - 1 - 1 * 1.4142f, d, 0.5f);

            node2.Translation = new Vector3 (2.1f, 0, 0);

            d = wld.Distance (node1, node2);
            Assert.AreEqual (0, d);

            node2.Translation = new Vector3 (0, 0, 0);

            d = wld.Distance (node1, node2);
            Assert.AreEqual (0, d);

            
            wld.Destroy ();
        }

        [TestMethod]
        public void Test_Distance_Invalid_Objects () {
            var node1 = new Node ("Node1");
            var node2 = new Node ("Node2");

            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);

            wld.CollisionUpdate ();

            var d = wld.Distance (node1, node2);
            Assert.AreEqual (Single.NaN, d);


            wld.Destroy ();
        }
    }
}
