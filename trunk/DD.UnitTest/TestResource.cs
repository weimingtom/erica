﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD;
using System.Drawing;
using System.IO;

namespace DD.UnitTest {
    public class Foo {

    }

    [TestClass]
    public class TestResource {
        [TestMethod]
        public void Test_GetTexture_1_FromFile () {
            var tex1 = Resource.GetTexture ("abstract7.png");
            var tex2 = Resource.GetTexture ("abstract7.png");

            Assert.AreSame (tex2, tex1);
        }

        [TestMethod]
        public void Test_GetTexture_2_FromBitmap () {
            var bitmap = new Bitmap ("abstract7.png");
            var tex1 = Resource.GetTexture (bitmap, "abstract7.png");
            var tex2 = Resource.GetTexture (bitmap, "abstract7.png");

            Assert.AreSame (tex2, tex1);
        }

        [TestMethod]
        public void Test_GetTexture_3_FromStream () {
            var bytes = File.ReadAllBytes ("abstract7.png");
            var tex1 = Resource.GetTexture (new MemoryStream (bytes), "abstract7.png");
            var tex2 = Resource.GetTexture (new MemoryStream (bytes), "abstract7.png");

            Assert.AreSame (tex2, tex1);
        }

        [TestMethod]
        public void Test_GetDefaultTexture () {
            var tex1 = Resource.GetDefaultTexture ();
            var tex2 = Resource.GetDefaultTexture ();

            Assert.AreSame (tex2, tex1);
        }

        [TestMethod]
        public void Test_GetSoundTrack () {
            var track1 = Resource.GetSoundTrack ("PinPon.wav");
            var track2 = Resource.GetSoundTrack ("PinPon.wav");

            Assert.AreSame (track2, track1);
        }

        [TestMethod]
        public void Test_GetMusicTrack () {
            var track1 = Resource.GetMusicTrack ("nice_music.ogg");
            var track2 = Resource.GetMusicTrack ("nice_music.ogg");

            Assert.AreSame (track2, track1);
        }
    }
}
