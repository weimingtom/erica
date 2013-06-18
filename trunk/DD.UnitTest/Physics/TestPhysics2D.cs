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
            p2d.CreateWorld (800,600,0);

            Assert.IsNotNull(p2d.GetWorld());

        }

        [TestMethod]
        public void Test_SetGravity () {
            var p2d = Physics2D.GetInstance ();

            p2d.Gravity = new Vector3 (1, 2, 3);
            Assert.AreEqual (new Vector3 (1, 2, 0), p2d.Gravity);

            p2d.SetGravity (4,5,6);
            Assert.AreEqual (new Vector3 (4,5, 0), p2d.Gravity);
        }
    }
}
