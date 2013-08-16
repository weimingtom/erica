using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestBar {
        [TestMethod]
        public void TestMethod_New () {
            var bar = new Bar (1, 2, BarOrientation.Horizontal);

            Assert.AreEqual (1, bar.Width);
            Assert.AreEqual (2, bar.Height);
            Assert.AreEqual (BarOrientation.Horizontal, bar.Orientation);
            Assert.AreEqual (0, bar.MaxValue);
            Assert.AreEqual (0, bar.CurrentValue);
            Assert.AreEqual (1, bar.CurrentRate);
            Assert.AreEqual (Color.Red, bar.BackgroundColor);
            Assert.AreEqual (Color.Green, bar.ForegroundColor);
            Assert.AreEqual (null, bar.BackgroundTexture);
            Assert.AreEqual (null, bar.ForegroundTexture);
            Assert.AreEqual (new Vector2 (0, 0), bar.Offset);
        }

        [TestMethod]
        public void TestMethod_SetValue () {
            var bar = new Bar (1, 2, BarOrientation.Horizontal);

            bar.CurrentValue = 100;
            Assert.AreEqual (0, bar.CurrentValue);

            bar.MaxValue = 100;
            bar.CurrentValue = 100;
            Assert.AreEqual (100, bar.CurrentValue);
            Assert.AreEqual (1, bar.CurrentRate);

            bar.CurrentValue = 50;
            Assert.AreEqual (50, bar.CurrentValue);
            Assert.AreEqual (0.5f, bar.CurrentRate, 0.0001f);

        }

        [TestMethod]
        public void TestMethod_SetColor () {
            var bar = new Bar (1, 2, BarOrientation.Horizontal);

            bar.ForegroundColor = Color.White;
            bar.BackgroundColor = Color.Black;
            

            Assert.AreEqual (Color.White, bar.ForegroundColor);
            Assert.AreEqual (Color.Black, bar.BackgroundColor);
        }

        [TestMethod]
        public void TestMethod_SetTexture () {
            var bar = new Bar (1, 2, BarOrientation.Horizontal);

            var tex1 = new Texture ("abstract7.png");
            var tex2 = new Texture ("explosion4x8.png");

            bar.ForegroundTexture = tex1;
            bar.BackgroundTexture = tex2;
        
            Assert.AreEqual (tex1, bar.ForegroundTexture);
            Assert.AreEqual (tex2, bar.BackgroundTexture);
        
        }
        
        [TestMethod]
        public void TestMethod_SetOffset () {
            var bar = new Bar (1, 2, BarOrientation.Horizontal);

            bar.Offset = new Vector2 (1, 2);
            Assert.AreEqual (new Vector2 (1, 2), bar.Offset);

            bar.SetOffset (3, 4);
            Assert.AreEqual (new Vector2 (3, 4), bar.Offset);
        }


    }
}
