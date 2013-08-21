using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestSoundTrack {

        [TestMethod]
        public void Test_New () {
            var track = new SoundEffectTrack ("PinPon.wav");

            Assert.AreEqual ("PinPon.wav", track.FileName);
            Assert.AreEqual (1.0f, track.Volume);
            Assert.AreEqual (1520, track.Duration);
        }

        [TestMethod]
        public void Test_SetVolume () {
            var track = new SoundEffectTrack ("PinPon.wav");

            track.Volume = 0.1f;
            Assert.AreEqual (0.1f, track.Volume, 0.1f);

            track.Volume = 0.2f;
            Assert.AreEqual (0.2f, track.Volume, 0.1f);
        }

        [TestMethod]
        public void Test_Play_and_Stop () {
            var track = new SoundEffectTrack ("PinPon.wav");

            track.Play();
            Assert.AreEqual (true, track.IsPlaying);

            track.Stop ();
            Assert.AreEqual (false, track.IsPlaying);
        }


    }
}
