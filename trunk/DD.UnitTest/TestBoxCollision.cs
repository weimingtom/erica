using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestBoxCollision {
        /*
        [TestMethod]
        public void Test_New () {
            var col = new BoxCollision (1, 1, 1);

            Assert.AreEqual (2, col.Width);
            Assert.AreEqual (2, col.Height);
            Assert.AreEqual (2, col.Depth);
        }

        [TestMethod]
        public void Test_CreateShape () {
            var box = new BoxCollision (1, 1, 1);

            Assert.IsNotNull (box.CreateShapeBody (1));
        }

        [TestMethod]
        public void Test_VretexCount () {
            var box = new BoxCollision (1, 1, 1);

            Assert.AreEqual (4, box.VertexCount);
            Assert.AreEqual (4, box.Vertices.Count ());

            // Box型は頂点数を変更できない（当たり前）
            box.VertexCount = 16;
            Assert.AreEqual (4, box.VertexCount);
            Assert.AreEqual (4, box.Vertices.Count ());
        }

        [TestMethod]
        public void Test_Vretices () {
            var box = new BoxCollision (1, 1, 1);
            box.SetOffset (1, 1, 0);
    
            var boxVertices = box.Vertices.ToArray ();
    
            Assert.AreEqual (new Vector3 (0, 0, 0), boxVertices[0]);
            Assert.AreEqual (new Vector3 (2, 0, 0), boxVertices[1]);
            Assert.AreEqual (new Vector3 (2, 2, 0), boxVertices[2]);
            Assert.AreEqual (new Vector3 (0, 2, 0), boxVertices[3]);
        }

        [TestMethod]
        public void Test_Contain () {
            var box = new BoxCollision (1,1,1);
            box.SetOffset (1, 1, 0);

            Assert.AreEqual (false, box.Contain (-1, -1, 0));
            Assert.AreEqual (true, box.Contain (0, 0, 0));
            Assert.AreEqual (true, box.Contain (1, 1, 0));
            Assert.AreEqual (false, box.Contain (2, 2, 0));
        }
        */
    
    }
}
