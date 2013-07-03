using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD.Physics;

namespace DD.UnitTest.Physics {
    [TestClass]
    public class TestCollisionShape {
        [TestMethod]
        public void TestMethod_New () {
            var shape = new SphereCollisionShape (1);
            
            Assert.AreEqual (Vector3.Zero, shape.Offset);
        }

        [TestMethod]
        public void TestMethod_SetOffset () {
            var shape = new SphereCollisionShape (1);

            shape.Offset = new Vector3 (1, 2, 3);

            Assert.AreEqual (new Vector3(1,2,3), shape.Offset);

            shape.SetOffset (4, 5, 6);

            Assert.AreEqual (new Vector3 (4,5,6), shape.Offset);

        }

        [TestMethod]
        public void TestMethod_Pick () {
            var wld = new World ();

            var nod1 = new Node ();
            var shp1 = new SphereCollisionShape (1);
            shp1.SetOffset (1, 0, 0);
            nod1.Attach (shp1);
            nod1.SetTranslation (1, 0, 0);
            nod1.DrawPriority = 2;

            var nod2 = new Node ();
            var shp2 = new BoxCollisionShape (1, 1, 1);
            shp2.SetOffset (2, 0, 0);
            nod2.Attach (shp2);
            nod2.SetTranslation (2, 0, 0);
            nod2.DrawPriority = 1;

            wld.AddChild (nod1);
            wld.AddChild (nod2);

            Assert.AreEqual (null, Graphics2D.Pick (wld, 0, 0).FirstOrDefault ());
            Assert.AreEqual (nod1, Graphics2D.Pick (wld, 1, 0).FirstOrDefault ());
            Assert.AreEqual (nod1, Graphics2D.Pick (wld, 2, 0).FirstOrDefault ());
            Assert.AreEqual (nod2, Graphics2D.Pick (wld, 3, 0).FirstOrDefault ());
            Assert.AreEqual (nod2, Graphics2D.Pick (wld, 4, 0).FirstOrDefault ());
            Assert.AreEqual (nod2, Graphics2D.Pick (wld, 5, 0).FirstOrDefault ());
            Assert.AreEqual (null, Graphics2D.Pick (wld, 6, 0).FirstOrDefault ());
        }
    }
}
