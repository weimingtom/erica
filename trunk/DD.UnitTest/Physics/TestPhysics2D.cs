using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;


namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestPhysics2D {
        /*
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
            var shape1 = new SphereCollision (1);
            var shape2 = new SphereCollision (1);

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
            var shape1 = new SphereCollision (1);
            var shape2 = new SphereCollision (1);
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
            var shape1 = new BoxCollision (1, 1, 1);
            var shape2 = new BoxCollision (1, 1, 1);
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
            var shape1 = new BoxCollision (1, 1, 1);
            var shape2 = new SphereCollision (1);
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
            var shape1 = new SphereCollision (1);
            var shape2 = new BoxCollision (1, 1, 1);
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
        /// 変換行列を暗黙的に取得するテスト
        /// </summary>
        /// <remarks>
        /// 変換行列を渡すのを省略してノードの変換行列を暗黙的に使用するテスト
        /// </remarks>
        [TestMethod]
        public void Test_Collide_6 () {
            var shape1 = new SphereCollision (1);
            var shape2 = new BoxCollision (1, 1, 1);
            var node1 = new Node ();
            var node2 = new Node ();
           
            Collision col;

            Assert.AreEqual (true, Physics2D.Collide (shape1, shape2, out col));
            Assert.AreEqual (new Vector3 (0, -0.005f, 0), col.Point);
            Assert.AreEqual (new Vector3 (0, -1.000f, 0), col.Normal);

            
            node1.Attach (shape1);
            node2.Attach (shape2);

 
            node2.SetTranslation (1, 0, 0);            
            Assert.AreEqual (true, Physics2D.Collide (shape1, shape2, out col));
            Assert.AreEqual (new Vector3 (0.495f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (-1, 0, 0), col.Normal);

            node2.SetTranslation (2, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, shape2, out col));
            Assert.AreEqual (new Vector3 (0.995f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (-1, 0, 0), col.Normal);

            node2.SetTranslation (3, 0, 0);
            Assert.AreEqual (false, Physics2D.Collide (shape1, shape2, out col));
        }

        /// <summary>
        /// Sphere-Rhombusコリジョン検出のテスト
        /// </summary>
        [TestMethod]
        public void Test_Collide_7 () {
            var shape1 = new SphereCollision (1);
            var shape2 = new RhombusCollisionShape (1, 1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;

            Collision col;

            tra2 = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (0.35f, -0.35f, 0), col.Point);
            Assert.AreEqual (new Vector3 (-0.707106769f, 0.707106769f, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (0.995f, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (-1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (3, 0, 0);
            Assert.AreEqual (false, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
        }

        /// <summary>
        /// Box-Rhombusコリジョン検出のテスト
        /// </summary>
        [TestMethod]
        public void Test_Collide_8 () {
            var shape1 = new BoxCollision (1, 1, 1);
            var shape2 = new RhombusCollisionShape (1, 1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;

            Collision col;

            tra2 = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (0.75f, 0.5f, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (true, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Point);
            Assert.AreEqual (new Vector3 (1, 0, 0), col.Normal);

            tra2 = Matrix4x4.CreateFromTranslation (3, 0, 0);
            Assert.AreEqual (false, Physics2D.Collide (shape1, tra1, shape2, tra2, out col));
        }



        [TestMethod]
        public void Test_Contain_1 () {
            var shape = new SphereCollision (1);

            Assert.AreEqual(true, Physics2D.Contain(shape, null, new Vector2(0,0)));
            Assert.AreEqual (false, Physics2D.Contain (shape, null, new Vector2 (1,1)));
            Assert.AreEqual (false, Physics2D.Contain (shape, null, new Vector2 (-1, -1)));
        }

        [TestMethod]
        public void Test_Contain_2 () {
            var shape = new SphereCollision (1);

            var tra = Matrix4x4.CreateFromTranslation (0, 0, 0);
            Assert.AreEqual (true, Physics2D.Contain (shape, tra, new Vector2 (0, 0)));

            tra = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (true, Physics2D.Contain (shape, tra, new Vector2 (0, 0)));

            tra = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (false, Physics2D.Contain (shape, tra, new Vector2 (0, 0)));
        }

        [TestMethod]
        public void Test_Contain_3 () {
            var shape = new BoxCollision (1, 1, 1);

            Assert.AreEqual (true, Physics2D.Contain (shape, null, new Vector2 (0, 0)));
            Assert.AreEqual (false, Physics2D.Contain (shape, null, new Vector2 (2, 2)));
            Assert.AreEqual (false, Physics2D.Contain (shape, null, new Vector2 (-2, -2)));
        }

        [TestMethod]
        public void Test_Contain_4 () {
            var shape = new BoxCollision (1, 1, 1);

            var tra = Matrix4x4.CreateFromTranslation (0, 0, 0);
            Assert.AreEqual (true, Physics2D.Contain (shape, tra, new Vector2 (0, 0)));

            tra = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (true, Physics2D.Contain (shape, tra, new Vector2 (0, 0)));

            tra = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (false, Physics2D.Contain (shape, tra, new Vector2 (0, 0)));
        }

        /// <summary>
        /// Sphereのレイキャストのテスト
        /// </summary>
        [TestMethod]
        public void Test_RayCast_1 () {
            
            var shape = new SphereCollision(1.0f);
            RayIntersection output;

            var ray = new Ray (new Vector3 (10, 0, 0), new Vector3 (9, 0, 0), 10);
            var hit = Physics2D.RayCast (shape, null, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (1, 0, 0), output.Normal);
            Assert.AreEqual (9.0f, output.Distance);

            ray = new Ray (new Vector3 (0, 10, 0), new Vector3 (0, 9, 0), 10);
            hit = Physics2D.RayCast (shape, null, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (0, 1, 0), output.Normal);
            Assert.AreEqual (9.0f, output.Distance);

            ray = new Ray (new Vector3 (-10, 0, 0), new Vector3 (-9, 0, 0), 10);
            hit = Physics2D.RayCast (shape, null, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (-1, 0, 0), output.Normal);
            Assert.AreEqual (9.0f, output.Distance);

            ray = new Ray (new Vector3 (0, -10, 0), new Vector3 (0, -9, 0), 10);
            hit = Physics2D.RayCast (shape, null, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (0, -1, 0), output.Normal);
            Assert.AreEqual (9.0f, output.Distance);
        }

        /// <summary>
        /// Sphereのレイキャストのテスト
        /// </summary>
        [TestMethod]
        public void Test_RayCast_2 () {

            var shape = new BoxCollision (1,1,1);
            RayIntersection output;

            var ray = new Ray (new Vector3 (10, 0, 0), new Vector3 (9, 0, 0), 10);
            var hit = Physics2D.RayCast (shape, null, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (1, 0, 0), output.Normal);
            Assert.AreEqual (9.0f, output.Distance);

            ray = new Ray (new Vector3 (0, 10, 0), new Vector3 (0, 9, 0), 10);
            hit = Physics2D.RayCast (shape, null, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (0, 1, 0), output.Normal);
            Assert.AreEqual (9.0f, output.Distance);

            ray = new Ray (new Vector3 (-10, 0, 0), new Vector3 (-9, 0, 0), 10);
            hit = Physics2D.RayCast (shape, null, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (-1, 0, 0), output.Normal);
            Assert.AreEqual (9.0f, output.Distance);

            ray = new Ray (new Vector3 (0, -10, 0), new Vector3 (0, -9, 0), 10);
            hit = Physics2D.RayCast (shape, null, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (0, -1, 0), output.Normal);
            Assert.AreEqual (9.0f, output.Distance);
        }

        /// <summary>
        /// Matrixを外部から与えたレイキャストのテスト
        /// </summary>
        [TestMethod]
        public void Test_RayCast_3 () {

            var shape = new SphereCollision (1);
            RayIntersection output;

            var ray = new Ray (new Vector3 (0, 0, 0), new Vector3 (1, 0, 0), 2);

            var tra = Matrix4x4.CreateFromTranslation (2, 0, 0);
            var hit = Physics2D.RayCast (shape, tra, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (-1, 0, 0), output.Normal);
            Assert.AreEqual (1.0f, output.Distance);

            tra = Matrix4x4.CreateFromTranslation (3, 0, 0);
            hit = Physics2D.RayCast (shape, tra, ray, out output);
            Assert.AreEqual (true, hit);
            Assert.AreEqual (new Vector3 (-1, 0, 0), output.Normal);
            Assert.AreEqual (2.0f, output.Distance);

            tra = Matrix4x4.CreateFromTranslation (4, 0, 0);
            hit = Physics2D.RayCast (shape, tra, ray, out output);
            Assert.AreEqual (false, hit);
        }

        /// <summary>
        /// Matrixを省略したレイキャストのテスト
        /// （Nodeから暗黙的に取得）
        /// </summary>
        [TestMethod]
        public void Test_RayCast_4 () {
            var shape = new SphereCollision (1);
            var node = new Node ();
            node.Attach (shape);

            RayIntersection output;

            var ray = new Ray (new Vector3 (0, 0, 0), new Vector3 (2, 0, 0), 1);

            node.Translation = new Vector3 (2, 0, 0);
            Assert.AreEqual (true, Physics2D.RayCast (shape, ray, out output));
            Assert.AreEqual (new Vector3 (-1, 0, 0), output.Normal);
            Assert.AreEqual (1.0f, output.Distance);
            Assert.AreEqual (0.5f, output.Fraction);

            node.Translation = new Vector3 (3, 0, 0);
            Assert.AreEqual (true, Physics2D.RayCast (shape, ray, out output));
            Assert.AreEqual (new Vector3 (-1, 0, 0), output.Normal);
            Assert.AreEqual (2.0f, output.Distance);
            Assert.AreEqual (1.0f, output.Fraction);

            node.Translation = new Vector3 (4, 0, 0);
            Assert.AreEqual (false, Physics2D.RayCast (shape, ray, out output));
        }

        /// <summary>
        /// レイキャストの Fraction / Distance のテスト
        /// </summary>
        [TestMethod]
        public void Test_RayCast_5 () {

            var shape = new SphereCollision (1);
            shape.Offset = new Vector3 (10, 0, 0);

            var ray = new Ray (new Vector3 (0, 0, 0), new Vector3 (2, 0, 0), 10);
            RayIntersection output;

            Assert.AreEqual (true, Physics2D.RayCast (shape, null, ray, out output));
            Assert.AreEqual (9.0f, output.Distance);
            Assert.AreEqual (4.5f, output.Fraction);
        }

        /// <summary>
        /// レイキャストのすべての派生シリーズのテスト
        /// </summary>
        /// <remarks>
        /// エラーが出なければそれで良しとする
        /// </remarks>
        [TestMethod]
        public void Test_RayCast_6 () {
            var node = new Node();
            var shape = new SphereCollision (1);
            shape.Offset = new Vector3 (10, 0, 0);
            node.Attach(shape);

            var ray = new Ray(new Vector3(0,0,0), new Vector3(1,0,0), 10);
            var tra = node.Transform;
            RayIntersection output;

            Assert.AreEqual (true, Physics2D.RayCast (shape, tra, ray, out output));
            Assert.AreEqual (true, Physics2D.RayCast (shape, null, ray, out output));
            Assert.AreEqual (true, Physics2D.RayCast (shape, tra, ray));
            Assert.AreEqual (true, Physics2D.RayCast (shape, null, ray));

        }


        /// <summary>
        /// Sphere-Sphere距離のテスト
        /// </summary>
        /// <remarks>
        /// 球にスキン幅はない。
        /// </remarks>
        [TestMethod]
        public void Test_Distance_1 () {
            var shape1 = new SphereCollision (1);
            var shape2 = new SphereCollision (1);
            ClosestPoints cp;

            // overlap            
            shape2.SetOffset (1, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, null, shape2, null, out cp), 0.001f);
            Assert.AreEqual (new Vector3(0.5f,0,0), cp.PointA);
            Assert.AreEqual (new Vector3 (0.5f, 0, 0), cp.PointB);

            // touch          
            shape2.SetOffset (2, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, null, shape2, null, out cp), 0.001f);
            Assert.AreEqual (new Vector3 (1, 0, 0), cp.PointA);
            Assert.AreEqual (new Vector3 (1, 0, 0), cp.PointB);


            // sepalate 
            shape2.SetOffset (3, 0, 0);
            Assert.AreEqual (1.0f, Physics2D.Distance (shape1, null, shape2, null, out cp), 0.001f);
            Assert.AreEqual (new Vector3 (1, 0, 0), cp.PointA);
            Assert.AreEqual (new Vector3 (2, 0, 0), cp.PointB);
        }

        /// <summary>
        /// Box-Box距離のテスト
        /// </summary>
        /// <remarks>
        /// ポリゴンはスキン幅(0.01)の分広がっている事に注意
        /// </remarks>
        [TestMethod]
        public void Test_Distance_2 () {
            var shape1 = new BoxCollision (1,1,1);
            var shape2 = new BoxCollision (1,1,1);
            ClosestPoints cp;

            // overlap            
            shape2.SetOffset (1, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, null, shape2, null, out cp), 0.001f);
            Assert.AreEqual (new Vector3 (0, -1, 0), cp.PointA);
            Assert.AreEqual (new Vector3 (0, -1, 0), cp.PointB);

            // touch
            shape2.SetOffset (2, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, null, shape2, null, out cp), 0.001f);
            Assert.AreEqual (new Vector3 (1, -1, 0), cp.PointA);
            Assert.AreEqual (new Vector3 (1, -1, 0), cp.PointB);

            // sepalate
            // スキン幅は0.01
            shape2.SetOffset (3, 0, 0);
            Assert.AreEqual (1.0f, Physics2D.Distance (shape1, null, shape2, null, out cp), 0.02f);
            Assert.AreEqual (new Vector3 (1.01f, -1, 0), cp.PointA);
            Assert.AreEqual (new Vector3 (1.99f, -1, 0), cp.PointB);
        }

        /// <summary>
        /// 変換行列（平行移動）を伴う距離のテスト
        /// </summary>
        [TestMethod]
        public void Test_Distance_3 () {
            var shape1 = new SphereCollision (1);
            var shape2 = new SphereCollision (1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;
            ClosestPoints cp;

            // overlap           
            tra2 = Matrix4x4.CreateFromTranslation (1, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, tra1, shape2, tra2, out cp), 0.001f);
            Assert.AreEqual (new Vector3 (0.5f, 0, 0), cp.PointA);
            Assert.AreEqual (new Vector3 (0.5f, 0, 0), cp.PointB);

            // touch          
            tra2 = Matrix4x4.CreateFromTranslation (2, 0, 0);
            Assert.AreEqual (0.0f, Physics2D.Distance (shape1, tra1, shape2, tra2, out cp), 0.001f);
            Assert.AreEqual (new Vector3 (1, 0, 0), cp.PointA);
            Assert.AreEqual (new Vector3 (1, 0, 0), cp.PointB);

            // sepalate 
            tra2 = Matrix4x4.CreateFromTranslation (3, 0, 0);
            Assert.AreEqual (1.0f, Physics2D.Distance (shape1, tra1, shape2, tra2, out cp), 0.001f);
            Assert.AreEqual (new Vector3 (1, 0, 0), cp.PointA);
            Assert.AreEqual (new Vector3 (2, 0, 0), cp.PointB);
        }

        /// <summary>
        /// 変換行列（回転）を伴う距離のテスト
        /// </summary>
        [TestMethod]
        public void Test_Distance_4 () {
            var shape1 = new SphereCollision (1);
            var shape2 = new BoxCollision (1,1,1);
            var tra1 = Matrix4x4.Identity;
            var tra2 = Matrix4x4.Identity;
            ClosestPoints cp;

            var T = Matrix4x4.CreateFromTranslation (3, 0, 0);
            var R = Matrix4x4.CreateFromRotation(45, 0,0,1);
            tra2 = T*R;
            Assert.AreEqual (0.576f, Physics2D.Distance (shape1, tra1, shape2, tra2, out cp), 0.01f);
            Assert.AreEqual (new Vector3 (1, 0, 0), cp.PointA);
            Assert.AreEqual (new Vector3 (1.5758f, 0, 0), cp.PointB);
        }
        */
    }
}
