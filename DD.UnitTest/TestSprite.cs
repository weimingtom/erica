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
            var spr = new Sprite (64, 64);

            Assert.AreEqual (64, spr.Width);
            Assert.AreEqual (64, spr.Height);
            Assert.AreEqual (0, spr.TextureCount);
            Assert.AreEqual (1, spr.Textures.Count ());
            Assert.AreEqual (null, spr.ActiveTexture);
            Assert.AreEqual (0, spr.Offset.X);
            Assert.AreEqual (0, spr.Offset.Y);
            Assert.AreEqual (Color.White, spr.Color);
            Assert.AreEqual (false, spr.AutoScale);
        }

        [TestMethod]
        public void Test_New_2 () {
            var tex = new Texture ("image2x2.png");
            var spr = new Sprite ();
            spr.AddTexture (tex);

            Assert.AreEqual (2, spr.Width);
            Assert.AreEqual (2, spr.Height);
            Assert.AreEqual (1, spr.TextureCount);
            Assert.AreEqual (1, spr.Textures.Count());
            Assert.AreEqual (tex, spr.ActiveTexture);
            Assert.AreEqual (0, spr.Offset.X);
            Assert.AreEqual (0, spr.Offset.Y);
            Assert.AreEqual (Color.White, spr.Color);
            Assert.AreEqual (false, spr.AutoScale);
        }

        [TestMethod]
        public void Test_New_3 () {
            // jpegのロード
        }



        [TestMethod]
        public void Test_SetColor () {
            var spr = new Sprite (64, 64);

            spr.Color = new Color (1, 2, 3, 4);
            Assert.AreEqual (new Color (1, 2, 3, 4), spr.Color);

            spr.SetColor (5, 6, 7, 8);
            Assert.AreEqual (new Color (5, 6, 7, 8), spr.Color);
        }

        [TestMethod]
        public void Test_SetAutoScale () {
            var spr = new Sprite (64, 64);

            spr.AutoScale = true;
            Assert.AreEqual (true, spr.AutoScale);

            spr.AutoScale = false;
            Assert.AreEqual (false, spr.AutoScale);

            }

        [TestMethod]
        public void Test_AddTexture_1 () {
            var spr = new Sprite (64, 64);
            spr.AddTexture (new Texture ("abstract7.png"));
            spr.AddTexture (new Texture ("image2x2.png"));

            Assert.AreEqual (64, spr.Width);
            Assert.AreEqual (64, spr.Height);
            Assert.AreEqual (2, spr.TextureCount);
            Assert.AreEqual ("abstract7.png", spr.GetTexture(0).Name);
            Assert.AreEqual ("image2x2.png", spr.GetTexture(1).Name);
        }

        [TestMethod]
        public void Test_AddTexture_2 () {
            var spr = new Sprite ();
            spr.AddTexture (new Texture ("abstract7.png"));
            spr.AddTexture (new Texture ("image2x2.png"));

            // サイズ未指定のスプライトは
            // 最初のテクスチャーにあわせられる

            Assert.AreEqual (614, spr.Width);
            Assert.AreEqual (1024, spr.Height);
            Assert.AreEqual (0, spr.ActiveTexture);
            Assert.AreEqual (2, spr.TextureCount);
        }


        [TestMethod]
        public void Test_RemoveTexture () {
            var spr = new Sprite (64, 64);
            var tex1 = new Texture ("abstract7.png");
            var tex2 = new Texture ("image2x2.png");
            spr.AddTexture (tex1);
            spr.AddTexture (tex2);

            spr.RemoveTexture (tex1);

            Assert.AreEqual (tex2, spr.ActiveTexture);
        }

        [TestMethod]
        public void Test_SetActiveTexture () {
            var spr = new Sprite (64, 64);
            spr.AddTexture (new Texture ("abstract7.png"));
            spr.AddTexture (new Texture ("image2x2.png"));

            spr.ActiveTexture = 0;
            Assert.AreEqual (0, spr.ActiveTexture);

            spr.ActiveTexture = 1;
            Assert.AreEqual (1, spr.ActiveTexture);
        }

        [TestMethod]
        public void Test_SetOffset () {
            var spr = new Sprite (64, 64);
            spr.Offset = new Vector2(1,2);

            Assert.AreEqual (1, spr.Offset.X);
            Assert.AreEqual (2, spr.Offset.Y);

            spr.SetOffset (3, 4);

            Assert.AreEqual (3, spr.Offset.X);
            Assert.AreEqual (4, spr.Offset.Y);
        }

        [TestMethod]
        public void Test_SetTextureOffset () {
            var spr = new Sprite (64, 64);
            spr.TextureOffset = new Vector2 (1, 2);

            Assert.AreEqual (1, spr.TextureOffset.X);
            Assert.AreEqual (2, spr.TextureOffset.Y);

            spr.SetTextureOffset (3, 4);
            Assert.AreEqual (3, spr.TextureOffset.X);
            Assert.AreEqual (4, spr.TextureOffset.Y);
        }

        [TestMethod]
        public void Test_Resize () {
            var spr = new Sprite (0, 0);

            spr.Resize (1, 2);
            Assert.AreEqual (1, spr.Width);
            Assert.AreEqual (2, spr.Height);

            spr.Resize (-1, -2);
            Assert.AreEqual (-1, spr.Width);
            Assert.AreEqual (-2, spr.Height);

        }
    }
}
