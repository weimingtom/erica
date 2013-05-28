using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestAnimationClip {
        [TestMethod]
        public void Test_New () {
            var clip = new AnimationClip ("ClipName");

            Assert.AreEqual ("ClipName", clip.Name);
            Assert.AreEqual (0, clip.Duration);
            Assert.AreEqual (0, clip.TrackCount);
            Assert.AreEqual (1, clip.Speed);
            Assert.AreEqual (WrapMode.Loop, clip.WrapMode);
        }

        [TestMethod]
        public void Test_AddTrack () {
            var clip = new AnimationClip ("ClipName");
            var track1 = new AnimationTrack ("PropName", InterpolationType.Linear);
            var track2 = new AnimationTrack ("PropName", InterpolationType.Linear);
            var dummy = new Node ();

            clip.AddTrack (dummy, track1);
            clip.AddTrack (dummy, track2);

            Assert.AreEqual (2, clip.TrackCount);
            Assert.AreEqual (2, clip.Tracks.Count ());
            Assert.AreEqual (track1, clip.GetTrack (0).Item2);
            Assert.AreEqual (track2, clip.GetTrack (1).Item2);
        }

        [TestMethod]
        public void Test_RemoveTrack () {
            var clip = new AnimationClip ("ClipName");
            var track1 = new AnimationTrack ("PropName",  InterpolationType.Linear);
            var track2 = new AnimationTrack ("PropName", InterpolationType.Linear);
            var dummy = new Node ();

            clip.AddTrack (dummy, track1);
            clip.AddTrack (dummy, track2);

            Assert.AreEqual (2, clip.TrackCount);
            Assert.AreEqual (2, clip.Tracks.Count());
            
            clip.RemoveTrack (track1);
            clip.RemoveTrack (track2);

            Assert.AreEqual (0, clip.TrackCount);
            Assert.AreEqual (0, clip.Tracks.Count ());
        }

        [TestMethod]
        public void Test_SetDuration() {
            var clip = new AnimationClip ("ClipName");

            clip.SetDuration (100);
            Assert.AreEqual (100, clip.Duration);

            clip.Duration = 200;
            Assert.AreEqual (200, clip.Duration);

        }

        [TestMethod]
        public void Test_SetWrapMode () {
            var clip = new AnimationClip ("ClipName");
            clip.SetWrapMode (WrapMode.Loop);

            Assert.AreEqual (WrapMode.Loop, clip.WrapMode);
        }

        [TestMethod]
        public void Test_GetPlaybackPoisition () {
            var clip = new AnimationClip ("ClipName");
            clip.Duration = 2;
        
            clip.WrapMode = WrapMode.Once;
            Assert.AreEqual (1, clip.GetPlaybackPosition (1));
            Assert.AreEqual (2, clip.GetPlaybackPosition (2));
            Assert.AreEqual (3, clip.GetPlaybackPosition (3));

            clip.WrapMode = WrapMode.Loop;
            Assert.AreEqual (1, clip.GetPlaybackPosition (1));
            Assert.AreEqual (0, clip.GetPlaybackPosition (2));
            Assert.AreEqual (1, clip.GetPlaybackPosition (3));
        }


        [TestMethod]
        public void Test_SetPlaybackPoisition () {
            var clip = new AnimationClip ("ClipName");
            clip.Duration = 1000;
            clip.SetPlaybackPoisition (10, 1);

            Assert.AreEqual (10, clip.GetPlaybackPosition (1));
            Assert.AreEqual (11, clip.GetPlaybackPosition (2));
            Assert.AreEqual (12, clip.GetPlaybackPosition (3));
        }

        [TestMethod]
        public void Test_SetSpeed () {
            var clip = new AnimationClip ("ClipName");
            clip.Duration = 1000;
            clip.SetPlaybackPoisition (10, 1);
            clip.SetSpeed (2, 1);

            Assert.AreEqual (10, clip.GetPlaybackPosition (1));
            Assert.AreEqual (12, clip.GetPlaybackPosition (2));
            Assert.AreEqual (14, clip.GetPlaybackPosition (3));
        }




    }
}
