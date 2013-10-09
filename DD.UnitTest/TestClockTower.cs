using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestClockTower {
        [TestMethod]
        public void Test_New () {
            var fc = new ClockTower ();

            Assert.AreEqual (0, fc.CurrentTime);
            Assert.AreEqual (0, fc.PreviousTime);
            Assert.AreEqual (0, fc.DeltaTime);
            Assert.AreEqual (0, fc.FrameCount);
            Assert.AreEqual (0, fc.FrameRate);
        }

        [TestMethod]
        public void Test_GetTime () {
            var fc = new ClockTower ();

            fc.OnUpdate (33);
            fc.OnUpdate (66);
            fc.OnUpdate (100);

            Assert.AreEqual (100, fc.CurrentTime);
            Assert.AreEqual (66, fc.PreviousTime);
            Assert.AreEqual (34, fc.DeltaTime);
            Assert.AreEqual (3, fc.FrameCount);
        }

        [TestMethod]
        public void Test_GetFrameRate () {
            var fc = new ClockTower ();

            fc.OnUpdate (500);
            fc.OnUpdate (1000);
            Assert.AreEqual (2, fc.FrameRate);

            fc.OnUpdate (1500);
            fc.OnUpdate (1600);
            fc.OnUpdate (2000);
            Assert.AreEqual (3, fc.FrameRate);
        }

    }
}
