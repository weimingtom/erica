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
            var clip = new AnimationClip (100, "ClipName");

            Assert.AreEqual (100, clip.Duration);
            Assert.AreEqual ("ClipName", clip.Name);
            Assert.AreEqual (1, clip.Speed);
            Assert.AreEqual (WrapMode.Loop, clip.WrapMode);
            Assert.AreEqual (false, clip.IsPlaying);
            Assert.AreEqual (0, clip.TrackCount);
            Assert.AreEqual (0, clip.EventCount);
        }

        [TestMethod]
        public void Test_AddTrack () {
            var clip = new AnimationClip (100, "ClipName");
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
            var clip = new AnimationClip (100, "ClipName");
            var track1 = new AnimationTrack ("PropName",  InterpolationType.Linear);
            var track2 = new AnimationTrack ("PropName", InterpolationType.Linear);
            var target = new Node ();

            clip.AddTrack (target, track1);
            clip.AddTrack (target, track2);

            Assert.AreEqual (2, clip.TrackCount);
            Assert.AreEqual (2, clip.Tracks.Count());
            
            clip.RemoveTrack (track1);
            clip.RemoveTrack (track2);

            Assert.AreEqual (0, clip.TrackCount);
            Assert.AreEqual (0, clip.Tracks.Count ());
        }

        [TestMethod]
        public void Test_AddEvent () {
            var clip = new AnimationClip (100, "Clip");
            var count = 0;
            var args1 = new EventArgs ();
            var args2 = new EventArgs ();
            clip.AddEvent (0, (x, y) => count++, args1);
            clip.AddEvent (1, (x, y) => count++, args2);

            Assert.AreEqual (2, clip.EventCount);
            Assert.AreEqual (2, clip.Events.Count ());

            Assert.AreEqual (0, clip.GetEvent (0).Position);
            Assert.AreEqual (1, clip.GetEvent (1).Position);
            Assert.AreEqual (args1, clip.GetEvent (0).Args);
            Assert.AreEqual (args2, clip.GetEvent (1).Args);
        }

        [TestMethod]
        public void Test_RemoveEvent () {
            var clip = new AnimationClip (100, "Clip");
            var count = 0;
            var args1 = new EventArgs ();
            var args2 = new EventArgs ();
            clip.AddEvent (0, (x, y) => count++, args1);
            clip.AddEvent (1, (x, y) => count++, args2);

            clip.RemoveEvent (0);
            clip.RemoveEvent (0);

            Assert.AreEqual (0, clip.EventCount);
            Assert.AreEqual (0, clip.Events.Count ());
        }

        [TestMethod]
        public void Test_SetDuration() {
            var clip = new AnimationClip (100, "ClipName");

            Assert.AreEqual (100, clip.Duration);

            clip.Duration = 200;
            Assert.AreEqual (200, clip.Duration);

        }

        [TestMethod]
        public void Test_SetWrapMode () {
            var clip = new AnimationClip (100, "ClipName");
            clip.WrapMode = WrapMode.Loop;

            Assert.AreEqual (WrapMode.Loop, clip.WrapMode);
        }

        [TestMethod]
        public void Test_GetPlaybackPoisition () {
            var clip = new AnimationClip (100, "ClipName");
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
            var clip = new AnimationClip (100, "ClipName");
            clip.SetPlaybackPoisition (10, 1);

            Assert.AreEqual (10, clip.GetPlaybackPosition (1));
            Assert.AreEqual (11, clip.GetPlaybackPosition (2));
            Assert.AreEqual (12, clip.GetPlaybackPosition (3));
        }

        [TestMethod]
        public void Test_SetSpeed () {
            var clip = new AnimationClip (100, "ClipName");
            clip.SetPlaybackPoisition (10, 1);
            clip.SetSpeed (2, 1);

            Assert.AreEqual (10, clip.GetPlaybackPosition (1));
            Assert.AreEqual (12, clip.GetPlaybackPosition (2));
            Assert.AreEqual (14, clip.GetPlaybackPosition (3));
        }

        [TestMethod]
        public void Test_Play () {
            var clip = new AnimationClip (100, "ClipName");
            Assert.AreEqual (false, clip.IsPlaying);

            clip.Play ();

            Assert.AreEqual (true, clip.IsPlaying);
        }

        [TestMethod]
        public void Test_Stop () {
            var clip = new AnimationClip (100, "ClipName");
            Assert.AreEqual (false, clip.IsPlaying);

            clip.Play ();
            clip.Stop ();

            Assert.AreEqual (false, clip.IsPlaying);


        }

    }
}
