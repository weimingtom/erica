using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestLabel {
        [TestMethod]
        public void Test_New () {
            var label = new Label ("Text");

            Assert.AreEqual ("Text", label.Text);
            Assert.AreEqual (16, label.CharacterSize);
            Assert.AreEqual (2, label.ShadowOffset);
            Assert.AreEqual (Color.White, label.Color);
            Assert.AreEqual (CharacterStyle.Regular, label.Style);
        }

        [TestMethod]
        public void Test_SetText () {
            var label = new Label ();
            label.SetText ("Text");

            Assert.AreEqual ("Text", label.Text);
        }

        [TestMethod]
        public void Test_SetCharacterStyle () {
            var label = new Label ("Text");
            var style = CharacterStyle.Italic | CharacterStyle.Bold | CharacterStyle.Shadow | CharacterStyle.Underlined;
            
            label.Style = style;
            Assert.AreEqual (style, label.Style);
        }

        [TestMethod]
        public void Test_SetColor () {
            var label = new Label ();

            label.Color = Color.White;
            Assert.AreEqual (Color.White, label.Color);

            label.SetColor (0,0,0,255);
            Assert.AreEqual (Color.Black, label.Color);
        }

        [TestMethod]
        public void Test_SetOffset () {
            var label = new Label ();

            label.Offset = new Vector2 (1, 2);
            Assert.AreEqual (new Vector2 (1, 2), label.Offset);

            label.SetOffset (3, 4);
            Assert.AreEqual (new Vector2 (3, 4), label.Offset);
        }


        [TestMethod]
        public void Test_SetShadowOffset () {
            var label = new Label ();

            label.ShadowOffset = 1;
            Assert.AreEqual (1, label.ShadowOffset);

            label.SetShadowOffset (2);
            Assert.AreEqual (2, label.ShadowOffset);
        }
    }
}
