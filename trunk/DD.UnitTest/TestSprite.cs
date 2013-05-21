using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {


    [TestClass]
    public class TestSprite {


        [TestMethod]
        public void Test_New1 () {
            var spr = new Sprite ();

            Assert.AreEqual ("", spr.TextureName);
            Assert.AreEqual (0, spr.Width);
            Assert.AreEqual (0, spr.Height);
        }

        [TestMethod]
        public void Test_New2 () {
            var spr = new Sprite ("abstract7.png");

            Assert.AreEqual ("abstract7.png", spr.TextureName);
            Assert.AreEqual (614, spr.Width);
            Assert.AreEqual (1024, spr.Height);
        }

        [TestMethod]
        public void Test_LoadTexture () {
            var spr = new Sprite ();
            spr.LoadTexture ("abstract7.png");

            Assert.AreEqual ("abstract7.png", spr.TextureName);
            Assert.AreEqual (614, spr.Width);
            Assert.AreEqual (1024, spr.Height);
        }

   
    }
}
