using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;


namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestPhysics2D {
        [TestMethod]
        public void Test_CreatWorld () {
            var p2d = Physics2D.GetInstance ();
            p2d.CreateWorld (32);

            Assert.IsNotNull (p2d.GetWorld ());

        }

        [TestMethod]
        public void Test_SetGravity () {
            var p2d = Physics2D.GetInstance ();

            p2d.Gravity = new Vector2 (1, 2);
            Assert.AreEqual (new Vector2 (1, 2), p2d.Gravity);

            p2d.SetGravity (3, 4);
            Assert.AreEqual (new Vector2 (3, 4), p2d.Gravity);
        }

        /// <summary>
        /// オフセットの変更に伴うコリジョン検出のテスト
        /// </summary>
        [TestMethod]
        public void Test_Collide_1 () {
            var shape1 = new SphereCollisionShape (1);
            var shape2 = new SphereCollisionShape (1);

            Collision col;

            shape2.SetOffset (1, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, null, shape2, null, out col));
            Assert.AreEqual (new Vector3 (0.5f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            shape2.SetOffset (2, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, null, shape2, null, out col));
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            shape2.SetOffset (3, 0, 0);
            Assert.AreEqual (false, Physics2D.Collide (shape1, null, shape2, null));
        }


        /// <summary>
        /// 変換行列の変更に伴うコリジョン検出のテスト
        /// </summary>
        [TestMethod]
        public void Test_Collide_2 () {
            var shape1 = new SphereCollisionShape (1);
            var shape2 = new SphereCollisionShape (1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;

            Collision col;

            tra2 = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (0.5f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (3, 0, 0);
            Assert.AreEqual (false, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
        }

        /// <summary>
        /// Box-Boxコリジョン検出のテスト
        /// </summary>
        [TestMethod]
        public void Test_Collide_3 () {
            var shape1 = new BoxCollisionShape (1, 1, 1);
            var shape2 = new BoxCollisionShape (1, 1, 1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;


            Collision col;

            tra2 = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (0.5f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (3, 0, 0);
            Assert.AreEqual (false, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
        }




        /// <summary>
        /// Box-Sphereコリジョン検出のテスト
        /// </summary>
        [TestMethod]
        public void Test_Collide_4 () {
            var shape1 = new BoxCollisionShape (1, 1, 1);
            var shape2 = new SphereCollisionShape (1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;

            Collision col;

            tra2 = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (0.505f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (1.005f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (3, 0, 0);
            Assert.AreEqual (false, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
        }



        /// <summary>
        /// Sphere-Boxコリジョン検出のテスト
        /// </summary>
        /// <remarks>
        /// (4)と同じに見えるが Farsser のコリジョン検出は Box - Sphere の順番で指定しないといけないので
        /// 内部でひっくり返しているのがテスト対象。
        /// </remarks>
        [TestMethod]
        public void Test_Collide_5 () {
            var shape1 = new SphereCollisionShape (1);
            var shape2 = new BoxCollisionShape (1, 1, 1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;

            Collision col;

            tra2 = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (0.495f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (-1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (0.995f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (-1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (3, 0, 0);
            Assert.AreEqual (false, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
        }

        /// <summary>
        /// Sphere-Sphere距離のテスト
        /// </summary>
        [TestMethod]
        public void Test_Distance_1 () {
            var shape1 = new SphereCollisionShape (1);
            var shape2 = new SphereCollisionShape (1);

            // overlap            
            shape2.SetOffset (1, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, null, shape2, null), 0.1f);

            // touch          
            shape2.SetOffset (2, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, null, shape2, null), 0.1f);


            // sepalate 
            shape2.SetOffset (3, 0, 0);
            Assert.AreEqual (1.0f, Physics2D.Distance (shape1, null, shape2, null), 0.1f);
        }

        /// <summary>
        /// Box-Box距離のテスト
        /// </summary>
        [TestMethod]
        public void Test_Distance_2 () {
            var shape1 = new BoxCollisionShape (1,1,1);
            var shape2 = new BoxCollisionShape (1,1,1);

            // overlap            
            shape2.SetOffset (1, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, null, shape2, null), 0.1f);

            // touch
            shape2.SetOffset (2, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, null, shape2, null), 0.1f);


            // sepalate
            shape2.SetOffset (3, 0, 0);
            Assert.AreEqual (1.0f, Physics2D.Distance (shape1, null, shape2, null), 0.1f);
        }

        /// <summary>
        /// 変換行列（平行移動）を伴う距離のテスト
        /// </summary>
        [TestMethod]
        public void Test_Distance_3 () {
            var shape1 = new SphereCollisionShape (1);
            var shape2 = new SphereCollisionShape (1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;

            // overlap           
            tra2 = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, tra1, shape2, tra2), 0.1f);

            // touch          
            tra2 = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, tra1, shape2, tra2), 0.1f);


            // sepalate 
            tra2 = Matrix4x4.CreateFromTranslation (3, 0, 0);
            Assert.AreEqual (1.0f, Physics2D.Distance (shape1, tra1, shape2, tra2), 0.1f);
        }

        /// <summary>
        /// 変換行列（回転）を伴う距離のテスト
        /// </summary>
        [TestMethod]
        public void Test_Distance_4 () {
            var shape1 = new SphereCollisionShape (1);
            var shape2 = new BoxCollisionShape (1,1,1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;

            var T = Matrix4x4.CreateFromTranslation (3, 0, 0);
            var R = Matrix4x4.CreateFromRotation(45, 0,0,1);
            tra2 = T*R;
            Assert.AreEqual (0.586f, Physics2D.Distance (shape1, tra1, shape2, tra2), 0.1f);

        }

    }
}
