using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestButton {
        [TestMethod]
        public void Test_New1 () {
            var btn = new Button (16, 16);

            Assert.AreEqual (ButtonState.Normal, btn.State);
            Assert.AreEqual (16, btn.Width);
            Assert.AreEqual (16, btn.Height);
            Assert.AreEqual (null, btn.Normal);
            Assert.AreEqual (null, btn.Focused);
            Assert.AreEqual (null, btn.Pressed);
            Assert.AreEqual (null, btn.PressedFocused);
        }

        [TestMethod]
        public void Test_New2 () {
            var btn = new Button ("abstract7.png");

            Assert.AreEqual (ButtonState.Normal, btn.State);
            Assert.AreEqual (614, btn.Width);
            Assert.AreEqual (1024, btn.Height);
            Assert.AreEqual ("abstract7.png", btn.Normal);
            Assert.AreEqual (null, btn.Focused);
            Assert.AreEqual (null, btn.Pressed);
            Assert.AreEqual (null, btn.PressedFocused);
        }

        [TestMethod]
        public void Test_LoadTexture () {
            var tex = new Button (16, 16);
            tex.LoadTexutre (ButtonState.Normal, "abstract7.png");

            Assert.AreEqual ("abstract7.png", tex.Normal);
        }
    }
}
