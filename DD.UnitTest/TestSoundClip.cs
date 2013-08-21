using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD;

namespace DD.UnitTest {
    [TestClass]
    public class TestSoundClip {
        [TestMethod]
        public void Test_New () {
            var clip = new SoundClip ("Sound");

            Assert.AreEqual("Sound", clip.Name);
            Assert.AreEqual (0, clip.TrackCount);
            Assert.AreEqual (0, clip.Tracks.Count ());
            Assert.AreEqual (1, clip.Volume);
            Assert.AreEqual (0, clip.Duration);
            Assert.AreEqual (false, clip.IsPlaying);
        }

        [TestMethod]
        public void Test_AddTrack () {
            var clip = new SoundClip ("Sound");
            var track1 = new SoundEffectTrack ("PinPon.wav");
            var track2 = new MusicTrack ("nice_music.ogg");
            clip.AddTrack (track1);
            clip.AddTrack (track2);

            Assert.AreEqual (2, clip.TrackCount);
            Assert.AreEqual (2, clip.Tracks.Count ());
            Assert.AreEqual (track1, clip.GetTrack (0));
            Assert.AreEqual (track2, clip.GetTrack (1));
        }

        [TestMethod]
        public void Test_RemoveTrack () {
            var clip = new SoundClip ("Sound");
            var track1 = new SoundEffectTrack ("PinPon.wav");
            var track2 = new MusicTrack ("nice_music.ogg");
            clip.AddTrack (track1);
            clip.AddTrack (track2);
            clip.RemoveTrack (1);
            clip.RemoveTrack (0);
        }

        [TestMethod]
        public void Test_Play_and_Stop () {
            var clip = new SoundClip ("Sound");
            var track1 = new SoundEffectTrack ("PinPon.wav");
            var track2 = new MusicTrack ("nice_music.ogg");
            clip.AddTrack (track1);
            clip.AddTrack (track2);

            clip.Play ();
            Assert.AreEqual (true, clip.IsPlaying);
            Assert.AreEqual (true, track1.IsPlaying);
            Assert.AreEqual (true, track2.IsPlaying);

            clip.Stop ();
            Assert.AreEqual (false, clip.IsPlaying);
            Assert.AreEqual (false, track1.IsPlaying);
            Assert.AreEqual (false, track2.IsPlaying);
        }

        [TestMethod]
        public void Test_SetVolume () {

            var clip = new SoundClip ("Sound");
            var track1 = new SoundEffectTrack ("PinPon.wav");
            var track2 = new MusicTrack ("nice_music.ogg");
            clip.AddTrack (track1);
            clip.AddTrack (track2);

            clip.Volume = 0.5f;

            Assert.AreEqual (0.5f, clip.Volume);
            Assert.AreEqual (0.5f, track1.Volume);
            Assert.AreEqual (0.5f, track2.Volume);

        }


    }
}
