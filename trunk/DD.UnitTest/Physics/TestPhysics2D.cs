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

            Assert.IsNotNull(p2d.GetWorld());

        }

        [TestMethod]
        public void Test_SetGravity () {
            var p2d = Physics2D.GetInstance ();

            p2d.Gravity = new Vector2 (1, 2);
            Assert.AreEqual (new Vector2 (1, 2), p2d.Gravity);

            p2d.SetGravity (3, 4);
            Assert.AreEqual (new Vector2 (3, 4), p2d.Gravity);
        }
    }
}
