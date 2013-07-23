using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestTransformable {
        [TestMethod]
        public void Test_New () {
            var tr = (Transformable)new Node ();

            Assert.AreEqual (new Vector3 (0, 0, 0), tr.Translation);
            Assert.AreEqual (new Vector3 (1, 1, 1), tr.Scale);
            Assert.AreEqual (Quaternion.Identity, tr.Rotation);
        }

        [TestMethod]
        public void Test_Translation () {
            var tr = (Transformable)new Node ();

            tr.Translation = new Vector3 (1, 2, 3);
            Assert.AreEqual (new Vector3 (1, 2, 3), tr.Translation);

            tr.Translate (1, 1, 1);
            Assert.AreEqual (new Vector3 (2, 3, 4), tr.Translation);
        }

        [TestMethod]
        public void Test_Scale () {
            var tr = (Transformable)new Node ();

            tr.Scale = new Vector3 (1, 2, 3);
            Assert.AreEqual (new Vector3 (1, 2, 3), tr.Scale);

            tr.Expand (2,2,2);
            Assert.AreEqual (new Vector3 (2, 4, 6), tr.Scale);
        }

        [TestMethod]
        public void Test_Rotation () {
            var node = (Transformable)new Node ();

            node.Rotation = new Quaternion (45, 0, 0, 1);
            Assert.AreEqual (new Quaternion(45, 0,0,1), node.Rotation);

            node.Rotate (45, 0, 0, 1);
            Assert.AreEqual (new Quaternion(90,0,0,1), node.Rotation);

            node.Rotate (new Quaternion(45,0,0,1));
            Assert.AreEqual (new Quaternion (135, 0, 0, 1), node.Rotation);
        }

        [TestMethod]
        public void Test_Transform_Get () {
            var tr = (Transformable)new Node ();
            tr.Translation = new Vector3 (1, 2, 3);
            tr.Rotation = new Quaternion (45, 0, 0, 1);
            tr.Scale = new Vector3 (1, 2, 3);            

            var expected = Matrix4x4.CreateFromTranslation (1, 2, 3) * 
                           Matrix4x4.CreateFromRotation (45, 0, 0, 1) * 
                           Matrix4x4.CreateFromScale (1, 2, 3);

            Assert.AreEqual (expected, tr.Transform);
        }

        [TestMethod]
        public void Test_Transform_Set () {
            var tr = (Transformable)new Node ();

            tr.Transform = Matrix4x4.CreateFromTranslation (1, 2, 3) *
                           Matrix4x4.CreateFromRotation (45, 0,0,1) *
                           Matrix4x4.CreateFromScale (1, 2, 3);

            var T = new Vector3 (1, 2, 3);
            var R = new Quaternion (45, 0, 0, 1);
            var S = new Vector3 (1, 2, 3);

            Assert.AreEqual (T, tr.Translation);
            Assert.AreEqual (R, tr.Rotation);
            Assert.AreEqual (S, tr.Scale);
        }

        [TestMethod]
        public void Test_Transform_Cache () {
            var tr = (Transformable)new Node ();
            tr.Translation = new Vector3 (1, 2, 3);
            tr.Rotation = new Quaternion (45, 0, 0, 1);
            tr.Scale = new Vector3 (1, 2, 3);

            var expected = Matrix4x4.CreateFromTranslation (1, 2, 3) *
                      Matrix4x4.CreateFromRotation (45, 0,0,1) *
                      Matrix4x4.CreateFromScale (1, 2, 3);

            // 2回目はキャッシュから
            Assert.AreEqual (expected, tr.Transform);
            Assert.AreEqual (expected, tr.Transform);

            // 変更するとキャッシュが無効化される
            tr.Translation = new Vector3 (2, 3, 4);
            tr.Rotation = new Quaternion (90, 0, 0, 1);
            tr.Scale = new Vector3 (2, 3, 4);
          
            expected = Matrix4x4.CreateFromTranslation (2, 3, 4) *
                       Matrix4x4.CreateFromRotation (90, 0,0,1) *
                       Matrix4x4.CreateFromScale (2, 3, 4);

            Assert.AreEqual (expected, tr.Transform);
            Assert.AreEqual (expected, tr.Transform);
        }

    }
}
