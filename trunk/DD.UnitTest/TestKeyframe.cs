using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestKeyframe {
        [TestMethod]
        public void Test_New1 () {
            var frame = new Keyframe (1, 2);

            Assert.AreEqual (1, frame.Time);
            Assert.AreEqual (2, frame.Value);
        }

        [TestMethod]
        public void Test_New2 () {
            var frame = new Keyframe (1, new Point(1,2));

            Assert.AreEqual (1, frame.Time);
            Assert.AreEqual (new Point(1,2), frame.Value);
        }
    }
}
