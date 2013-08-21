using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestMusicTrack {
        [TestMethod]
        public void Test_New () {
            var track = new MusicTrack ("nice_music.ogg");

            Assert.AreEqual ("nice_music.ogg", track.FileName);
            Assert.AreEqual (1.0f, track.Volume);
            Assert.AreEqual (92891, track.Duration);
        }

        [TestMethod]
        public void Test_SetVolume () {
            var track = new MusicTrack ("nice_music.ogg");

            track.Volume = 0.1f;
            Assert.AreEqual (0.1f, track.Volume, 0.1f);

            track.Volume = 0.2f;
            Assert.AreEqual (0.2f, track.Volume, 0.1f);
        }

        [TestMethod]
        public void Test_Play_and_Stop () {
            var track = new MusicTrack ("nice_music.ogg");

            track.Play ();
            Assert.AreEqual (true, track.IsPlaying);

            track.Stop ();
            Assert.AreEqual (false, track.IsPlaying);
        }


    }
}
