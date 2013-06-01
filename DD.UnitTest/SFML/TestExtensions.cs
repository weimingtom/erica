using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFML.Graphics;
using SFML.Window;

namespace DD.UnitTest {
    [TestClass]
    public class TestExtensions {
        [TestMethod]
        public void Test_TextStyle () {
            var dd = CharacterStyle.Bold |
                     CharacterStyle.Italic |
                     CharacterStyle.Underlined;
            var sfml = SFML.Graphics.Text.Styles.Bold |
                       SFML.Graphics.Text.Styles.Italic |
                       SFML.Graphics.Text.Styles.Underlined;
            Assert.AreEqual (sfml, dd.ToSFML ());
        }

        [TestMethod]
        public void Test_Color () {
            var dd = new Color (1, 2, 3, 4);
            var sfml = new SFML.Graphics.Color (1, 2, 3, 4);
            Assert.AreEqual (sfml, dd.ToSFML ());
        }

    }
}
