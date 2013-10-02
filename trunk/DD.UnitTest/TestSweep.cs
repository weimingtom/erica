using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// メモ：
// 明示的にDestroy()を呼んで終了しないと BulletPhysics の動作が凄く怪しいので
// 必ず最後に wld.Destroy() を呼ぶこと。

// Sphereないけどまあいいか

namespace DD.UnitTest {
    [TestClass]
    public class TestSweep {
        /// <summary>
        /// ボックス形状のスィープ テスト
        /// </summary>
        [TestMethod]
        public void Test_BoxShape () {

            var node1 = new Node ("Sweeper");

            var col1 = new CollisionObject ();
            col1.Shape = new BoxShape (1, 1, 1);
            col1.SetOffset (0, 1, 0);

            node1.Attach (col1);

            var node2 = new Node ("Target1");
            var col2 = new CollisionObject ();
            col2.Shape = new SphereShape (1);
            col2.SetOffset (0, 1, 0);

            node2.Attach (col2);

            var node3 = new Node ("Target2");
            var col3 = new CollisionObject ();
            col3.Shape = new SphereShape (1);
            col3.SetOffset (0, 1, 0);


            node1.Translate (0, 0, 0);
            node2.Translate (10, 0, 0);
            node3.Translate (20, 0, 0);

            
            var wld = new World ();
            wld.AddChild (node1);
            wld.AddChild (node2);
            wld.AddChild (node3);

            wld.CollisionUpdate ();

            var result = wld.Sweep (node1, new Vector3 (100, 0, 0));
            Assert.AreEqual (true, result.Hit);
            Assert.AreEqual (new Vector3 (9, 1, 0), result.Point);
            Assert.AreEqual (new Vector3 (-1, 0, 0), result.Normal);
            Assert.AreEqual (8, result.Distance, 0.01f);
            Assert.AreEqual (0.08f, result.Fraction, 0.01f);

            result = wld.Sweep (node1, new Vector3 (0, 100, 0));
            Assert.AreEqual (false, result.Hit);

            result = wld.Sweep (node1, new Vector3 (-100, 100, 0));
            Assert.AreEqual (false, result.Hit);

            wld.Destroy ();
        }

    }
}
