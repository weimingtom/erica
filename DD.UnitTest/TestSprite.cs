using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {


    [TestClass]
    public class TestSprite {


        [TestMethod]
        public void Test_New_1 () {
            var spr = new Sprite ();

            Assert.AreEqual (0, spr.TextureCount);
            Assert.AreEqual (null, spr.ActiveTexture);
            Assert.AreEqual (0, spr.OffsetX);
            Assert.AreEqual (0, spr.OffsetY);
            Assert.AreEqual (Color.White, spr.Color);
        }

        [TestMethod]
        public void Test_New_2 () {
            var tex = new Texture ("image2x2.png");
            var spr = new Sprite (tex);

            Assert.AreEqual (1, spr.TextureCount);
            Assert.AreEqual (tex, spr.ActiveTexture);
            Assert.AreEqual (0, spr.OffsetX);
            Assert.AreEqual (0, spr.OffsetY);
            Assert.AreEqual (Color.White, spr.Color);
        }

        [TestMethod]
        public void Test_SetColor () {
            var spr = new Sprite ();

            spr.Color = new Color (1, 2, 3, 4);
            Assert.AreEqual (new Color (1, 2, 3, 4), spr.Color);

            spr.SetColor (5, 6, 7, 8);
            Assert.AreEqual (new Color (5, 6, 7, 8), spr.Color);

        }

        [TestMethod]
        public void Test_SetTexture () {
            var spr = new Sprite ();
            spr.AddTexture (new Texture ("abstract7.png"));
            spr.AddTexture (new Texture ("image2x2.png"));

            Assert.AreEqual (2, spr.TextureCount);

            var tex0 = spr.GetTexture (0);
            Assert.AreEqual ("abstract7.png", tex0.Name);
            Assert.AreEqual (614, tex0.ImageWidth);
            Assert.AreEqual (1024, tex0.ImageHeight);

            var tex1 = spr.GetTexture (1);
            Assert.AreEqual ("image2x2.png", tex1.Name);
            Assert.AreEqual (2, tex1.ImageWidth);
            Assert.AreEqual (2, tex1.ImageHeight);
        }


        [TestMethod]
        public void Test_SetActiveTexture () {
            var spr = new Sprite ();
            spr.AddTexture (new Texture ("abstract7.png"));
            spr.AddTexture (new Texture ("image2x2.png"));

            spr.ActiveTextureIndex = 1;
            Assert.AreEqual (1, spr.ActiveTextureIndex);

            var tex1 = spr.GetActiveTexture ();
            Assert.AreEqual ("image2x2.png", tex1.Name);
            Assert.AreEqual (2, tex1.ImageWidth);
            Assert.AreEqual (2, tex1.ImageHeight);
        }

        [TestMethod]
        public void Test_RemoveTexture () {
            var spr = new Sprite ();
            var tex1 = new Texture ("abstract7.png");
            var tex2 = new Texture ("image2x2.png");
            spr.AddTexture (tex1);
            spr.AddTexture (tex2);

            spr.RemoveTexture (tex1);

            Assert.AreEqual (tex2, spr.ActiveTexture);
        }

        [TestMethod]
        public void Test_Width_Height () {
            var spr = new Sprite ();
            spr.AddTexture (new Texture ("abstract7.png"));

            Assert.AreEqual (614, spr.Width);
            Assert.AreEqual (1024, spr.Height);
        }

        [TestMethod]
        public void Test_SetOffset () {
            var spr = new Sprite ();
 
            spr.OffsetX = 1;
            spr.OffsetY = 2;

            Assert.AreEqual (1, spr.OffsetX);
            Assert.AreEqual (2, spr.OffsetY);

            spr.SetOffset (3, 4);

            Assert.AreEqual (3, spr.OffsetX);
            Assert.AreEqual (4, spr.OffsetY);
        }


    }
}
