using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestTexture {
        [TestMethod]
        public void Test_New () {
            var tex = new Texture("abstract7.png");

            Assert.AreEqual ("abstract7.png", tex.Name);
            Assert.AreEqual (614, tex.Width);
            Assert.AreEqual (1024, tex.Height);
            Assert.AreEqual (new Rectangle (0, 0, 614, 1024), tex.ActiveRegion);
        }

        [TestMethod]
        public void Test_SetActiveRegion () {
            var tex = new Texture ("abstract7.png");
            tex.SetActiveRegion (1, 1, 100, 100);

            Assert.AreEqual (new Rectangle (1, 1, 100, 100), tex.ActiveRegion);
        }
    }
}
