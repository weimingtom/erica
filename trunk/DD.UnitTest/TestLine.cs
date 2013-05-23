using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestLine {
        [TestMethod]
        public void Test_New () {
            var line = new Line ("Actor", "Words", "Sound", "Event");

            Assert.AreEqual ("Actor", line.Actor);
            Assert.AreEqual ("Words", line.Words);
            Assert.AreEqual ("Sound", line.Sound);
            Assert.AreEqual ("Event", line.Event);
        }

    }
}
