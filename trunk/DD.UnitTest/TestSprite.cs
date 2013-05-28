using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {


    [TestClass]
    public class TestSprite {


        [TestMethod]
        public void Test_New () {
            var spr = new Sprite (4);

            Assert.AreEqual (4, spr.TextureCount);
            Assert.AreEqual (0, spr.ActiveTexture);
        }

        [TestMethod]
        public void Test_SetTexture () {
            var spr = new Sprite (4);
            spr.SetTexture (0,new Texture ("abstract7.png"));
            spr.SetTexture (1, new Texture ( "image2x2.png"));

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
             var spr = new Sprite (4);

            spr.SetTexture (0, new Texture("abstract7.png"));
            spr.SetTexture (1, new Texture("image2x2.png"));

            spr.ActiveTexture = 1;
            
            Assert.AreEqual(1, spr.ActiveTexture);
        
            var tex1 = spr.GetActiveTexture ();
            Assert.AreEqual ("image2x2.png", tex1.Name);
            Assert.AreEqual (2, tex1.ImageWidth);
            Assert.AreEqual (2, tex1.ImageHeight);
        
        }

   
    }
}
