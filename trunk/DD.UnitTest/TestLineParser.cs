using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestLineParser {
        [TestMethod]
        public void Test_Parse () {
            var txt = @"
- Miku
+ voice-001.wave
* !MyEventArgs { X: 100, Y: 200 }
ミクだよー

- Miku
+ voice-002.wave
* !MyEventArgs { X: 300, Y: 400 }
ミクだよー（1行目）
ミクだよー（2行目）

";
            var lines = LineParser.Parse(txt);

            Assert.AreEqual (2, lines.Count());
            Assert.AreEqual ("Miku", lines[0].Actor);
            Assert.AreEqual ("voice-001.wave", lines[0].Sound);
            Assert.AreEqual ("!MyEventArgs { X: 100, Y: 200 }", lines[0].Event);
            Assert.AreEqual ("ミクだよー", lines[0].Words);
            Assert.AreEqual ("Miku", lines[1].Actor);
            Assert.AreEqual ("voice-002.wave", lines[1].Sound);
            Assert.AreEqual ("!MyEventArgs { X: 300, Y: 400 }", lines[1].Event);
            Assert.AreEqual ("ミクだよー（1行目）\nミクだよー（2行目）", lines[1].Words);
        }
    }
}
