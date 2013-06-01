using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestSoundClip {
        /// <summary>
        /// オンメモリのサウンド クリップ
        /// SFMLで言うと Sound
        /// </summary>
        [TestMethod]
        public void Test_New_1 () {
            var clip = new SoundClip ("PinPon.wav");

            Assert.AreEqual ("PinPon.wav", clip.Name);
            Assert.AreEqual (false, clip.Streaming);
            Assert.AreEqual (false, clip.Loop);
            Assert.AreEqual (1.0f, clip.Volume);
            Assert.AreEqual (1520, clip.Duration);
        }

        /// <summary>
        /// ストリーミングのサウンド クリップ
        /// SFMLで言うと Music
        /// </summary>
        [TestMethod]
        public void Test_New_2 () {
            var clip = new SoundClip ("nice_music.ogg", true);

            Assert.AreEqual ("nice_music.ogg", clip.Name);
            Assert.AreEqual (true, clip.Streaming);
            Assert.AreEqual (true, clip.Loop);
            Assert.AreEqual (1.0f, clip.Volume);
            Assert.AreEqual (92891, clip.Duration);
        }


        [TestMethod]
        public void Test_SetLoop () {
            var clip = new SoundClip ("PinPon.wav");

            clip.Loop = true;
            Assert.AreEqual (true, clip.Loop);

            clip.SetLoop (false);
            Assert.AreEqual (false, clip.Loop);
        }

        [TestMethod]
        public void Test_SetVolume () {
            var clip = new SoundClip ("PinPon.wav");
            clip.Volume = 0.1f;

            Assert.AreEqual (0.1f, clip.Volume, 0.1f);
        }

        [TestMethod]
        public void Test_Play () {
            var clip = new SoundClip ("PinPon.wav");
            clip.Play();

            Assert.AreEqual (true, clip.IsPlaying);
        }


    }
}
