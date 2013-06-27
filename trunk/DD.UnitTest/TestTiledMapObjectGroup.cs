using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestTiledMapObjectGroup {
        [TestMethod]
        public void TestMethod_New () {
            var objGroup = new TiledMapObjectGroup ();

            Assert.AreEqual (0, objGroup.ObjectCount);
            Assert.AreEqual (0, objGroup.Objects.Count());
        }

        [TestMethod]
        public void TestMethod_AddObject () {
            var objGroup = new TiledMapObjectGroup ();
            var obj1 = new Node ();
            var obj2 = new Node ();

            objGroup.AddObject (obj1);
            objGroup.AddObject (obj2);

            Assert.AreEqual (2, objGroup.ObjectCount);
            Assert.AreEqual (2, objGroup.Objects.Count());

        }

        [TestMethod]
        public void TestMethod_RemoveObject () {
            var objGroup = new TiledMapObjectGroup ();
            var obj1 = new Node ();
            var obj2 = new Node ();

            objGroup.AddObject (obj1);
            objGroup.AddObject (obj2);

            objGroup.RemoveObject (obj1);
            objGroup.RemoveObject (obj2);

            Assert.AreEqual (0, objGroup.ObjectCount);
            Assert.AreEqual (0, objGroup.Objects.Count ());

        }

    }
}
