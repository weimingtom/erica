using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestCamera {
        [TestMethod]
        public void Test_New () {
            var cam = new Camera ();

            Assert.AreEqual (ProjectionType.Undefined, cam.Type);
            Assert.AreEqual (true, cam.ClearEnabled);
            Assert.AreEqual (Color.Blue, cam.ClearColor);
            Assert.AreEqual (new Rectangle (0, 0, 1, 1), cam.Viewport);
        }

        [TestMethod]
        public void Test_ClearColor () {
            var cam = new Camera ();

            cam.ClearEnabled = false;
            cam.ClearColor = Color.Red;

            Assert.AreEqual (false, cam.ClearEnabled);
            Assert.AreEqual (Color.Red, cam.ClearColor);
        }

        [TestMethod]
        public void Test_SetScreen () {
            var cam = new Camera ();

            cam.SetScreen (1, 2, 3, 4);

            Assert.AreEqual (ProjectionType.Screen, cam.Type);
            Assert.AreEqual (1, cam.Screen.X);
            Assert.AreEqual (2, cam.Screen.Y);
            Assert.AreEqual (3, cam.Screen.Width);
            Assert.AreEqual (4, cam.Screen.Height);
        }

        [TestMethod]
        public void Test_SetViewport () {
            var cam = new Camera ();

            cam.SetViewport (0.1f, 0.2f, 0.3f, 0.4f);

            Assert.AreEqual (0.1f, cam.Viewport.X);
            Assert.AreEqual (0.2f, cam.Viewport.Y);
            Assert.AreEqual (0.3f, cam.Viewport.Width);
            Assert.AreEqual (0.4f, cam.Viewport.Height);
        }


    }
}
