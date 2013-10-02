using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using DD.Physics;

// メモ：
// 明示的にDestroy()を呼んで終了しないと BulletPhysics の動作が凄く怪しいので
// 必ず最後に wld.Destroy() を呼ぶこと。

namespace DD.UnitTest {
    [TestClass]
    public class TestRayCast {

        /// <summary>
        /// ボックス形状のテスト
        /// </summary>
        [TestMethod]
        public void Test_BoxShape () {

            var node = new Node ();

            var col = new CollisionObject ();
            col.Shape = new BoxShape (1, 1, 1);
            col.SetOffset (1, 0, 0);
            node.Attach (col);

            node.Translate (1, 0, 0);
            node.Rotate (45, 0, 0, 1);

            var wld = new World ();
            wld.AddChild (node);

            wld.CollisionUpdate ();


            // (1+1) - 1*√2
            var result = wld.RayCast (new Vector3 (-10, 0, 0), new Vector3 (10, 0, 0)).First ();
            Assert.AreEqual (true, result.Hit);
            Assert.AreEqual (new Vector3 (0.5858f, 0, 0), result.Point);
            Assert.AreEqual (new Vector3 (-1, 0, 0), result.Normal);
            Assert.AreEqual (10.58579f, result.Distance, 0.01f);
            Assert.AreEqual (0.5245f, result.Fraction, 0.01f);

            // (1+1) + 1*√2
            result = wld.RayCast (new Vector3 (10, 0, 0), new Vector3 (-10, 0, 0)).First ();
            Assert.AreEqual (true, result.Hit);
            Assert.AreEqual (new Vector3 (3.4142f, 0, 0), result.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), result.Normal);
            Assert.AreEqual (6.585786f, result.Distance, 0.01f);
            Assert.AreEqual (0.3225f, result.Fraction, 0.01f);

            wld.Destroy ();
        }

        /// <summary>
        /// スフィア形状のテスト
        /// </summary>
        [TestMethod]
        public void Test_SphereShape () {

            var node = new Node ();

            var col = new CollisionObject ();
            col.Shape = new SphereShape (1);
            col.SetOffset (1, 0, 0);
            node.Attach (col);

            node.Translate (1, 0, 0);
            node.Rotate (45, 0, 0, 1);

            var wld = new World ();
            wld.AddChild (node);

            wld.CollisionUpdate ();

            // (1+1) - 1
            var result = wld.RayCast (new Vector3 (-10, 0, 0), new Vector3 (10, 0, 0)).First ();
            Assert.AreEqual (true, result.Hit);
            Assert.AreEqual (new Vector3 (1, 0, 0), result.Point);
            Assert.AreEqual (new Vector3 (-1, 0, 0), result.Normal);
            Assert.AreEqual (11, result.Distance, 0.01f);
            Assert.AreEqual (0.55f, result.Fraction, 0.01f);

            // (1+1) + 1
            result = wld.RayCast (new Vector3 (10, 0, 0), new Vector3 (-10, 0, 0)).First ();
            Assert.AreEqual (true, result.Hit);
            Assert.AreEqual (new Vector3 (3, 0, 0), result.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), result.Normal);
            Assert.AreEqual (7, result.Distance, 0.01f);
            Assert.AreEqual (0.35f, result.Fraction, 0.01f);

            wld.Destroy ();
        }

        /// <summary>
        /// コリジョン マスクのテスト
        /// </summary>
        [TestMethod]
        public void Test_CollideWith () {

            var node = new Node ();

            var col = new CollisionObject ();
            col.Shape = new SphereShape (1);
            node.Attach (col);

            node.GroupID = 1 << 1;

            var wld = new World ();
            wld.AddChild (node);

            wld.CollisionUpdate ();

            // 1<<0
            var result = wld.RayCast (new Vector3 (-10, 0, 0), new Vector3 (10, 0, 0), 1 << 0).FirstOrDefault ();
            Assert.AreEqual (false, result.Hit);

            // 1<<1
            result = wld.RayCast (new Vector3 (-10, 0, 0), new Vector3 (10, 0, 0), 1 << 1).FirstOrDefault ();
            Assert.AreEqual (true, result.Hit);

            // 1<<2
            result = wld.RayCast (new Vector3 (-10, 0, 0), new Vector3 (10, 0, 0), 1 << 2).FirstOrDefault ();
            Assert.AreEqual (false, result.Hit);

            // 0
            result = wld.RayCast (new Vector3 (-10, 0, 0), new Vector3 (10, 0, 0), 0).FirstOrDefault ();
            Assert.AreEqual (false, result.Hit);

            // -1
            result = wld.RayCast (new Vector3 (-10, 0, 0), new Vector3 (10, 0, 0), -1).FirstOrDefault ();
            Assert.AreEqual (true, result.Hit);

            wld.Destroy ();
        }

        /// <summary>
        /// N個すべてヒットする事を確認するテスト
        /// </summary>
        [TestMethod]
        public void Test_AllNodes () {

            var wld = new World ();
            
            for (var i = 0; i < 10; i++) {
                var node = new Node ("" + i);

                var col = new CollisionObject ();
                col.Shape = new SphereShape (1);
                node.Attach (col);

                node.Translate (i, 0, 0);

                wld.AddChild (node);
            }
            

            wld.CollisionUpdate ();

            
            var results = wld.RayCast (new Vector3 (-10, 10, 0), new Vector3 (10, 10, 0));
            Assert.AreEqual (0, results.Count ());

            results = wld.RayCast (new Vector3 (-10, 0, 0), new Vector3 (10, 0, 0));

            Assert.AreEqual (10, results.Count ());
            Assert.AreEqual ("0", results.ElementAt (0).Node.Name);
            Assert.AreEqual ("1", results.ElementAt (1).Node.Name);
            Assert.AreEqual ("2", results.ElementAt (2).Node.Name);
            Assert.AreEqual ("3", results.ElementAt (3).Node.Name);
            Assert.AreEqual ("4", results.ElementAt (4).Node.Name);
            Assert.AreEqual ("5", results.ElementAt (5).Node.Name);
            Assert.AreEqual ("6", results.ElementAt (6).Node.Name);
            Assert.AreEqual ("7", results.ElementAt (7).Node.Name);
            Assert.AreEqual ("8", results.ElementAt (8).Node.Name);
            Assert.AreEqual ("9", results.ElementAt (9).Node.Name);
            

            wld.Destroy ();
        }

    }
}
