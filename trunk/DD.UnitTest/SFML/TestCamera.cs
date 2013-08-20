using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFML.Graphics;

namespace DD.UnitTest {
    [TestClass]
    public class TestCameraSFML {
        [TestMethod]
        public void Test_SetupCamera () {
            var g2d = Graphics2D.GetInstance ();
            var win = g2d.GetWindow () as RenderWindow;

            var cam = new Camera ();
            cam.SetScreen (0, 0, 640, 480);
            cam.Viewport = new Rectangle (0.25f, 0.25f, 0.5f, 0.5f);

            var node = new Node ();
            node.Attach (cam);

            cam.SetupView (win);

            var view = win.GetView ();

            Assert.AreEqual (640, view.Size.X);
            Assert.AreEqual (480, view.Size.Y);
            Assert.AreEqual (320, view.Center.X);
            Assert.AreEqual (240, view.Center.Y);

            Assert.AreEqual (0.25f, view.Viewport.Top);
            Assert.AreEqual (0.25f, view.Viewport.Left);
            Assert.AreEqual (0.5f, view.Viewport.Width);
            Assert.AreEqual (0.5f, view.Viewport.Height);
        }
    }
}
