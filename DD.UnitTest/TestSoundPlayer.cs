using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestSoundPlayer {
        [TestMethod]
        public void Test_New () {
            var ply = new SoundPlayer ();

            Assert.AreEqual (0, ply.ClipCount);
            Assert.AreEqual (0, ply.Clips.Count ());
        }

        [TestMethod]
        public void Test_AddClip () {
            var ply = new SoundPlayer ();
            var clip1 = new SoundClip ("PinPon.wav");
            var clip2 = new SoundClip ("nice_music.ogg");
            ply.AddClip (clip1);
            ply.AddClip (clip2);

            Assert.AreEqual (2, ply.ClipCount);
            Assert.AreEqual (2, ply.Clips.Count ());
            Assert.AreEqual (clip1, ply["PinPon.wav"]);
            Assert.AreEqual (clip2, ply["nice_music.ogg"]);
        }

        [TestMethod]
        public void Test_RemoveClip () {
            var ply = new SoundPlayer ();
            var clip1 = new SoundClip ("PinPon.wav");
            var clip2 = new SoundClip ("nice_music.ogg");
            ply.AddClip (clip1);
            ply.AddClip (clip2);

            Assert.AreEqual (2, ply.ClipCount);
            Assert.AreEqual (2, ply.Clips.Count ());

            ply.RemoveClip (clip1);
            ply.RemoveClip (clip2);
        }

    }
}
