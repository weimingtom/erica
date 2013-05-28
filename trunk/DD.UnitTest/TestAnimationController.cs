using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestAnimationController {

        public class MyDummyTarget {
            public float Speed { get; set; }
            public Point Point { get; set; }
        }

        [TestMethod]
        public void Test_New () {
            var anim = new AnimationController ();

            Assert.AreEqual (0, anim.ClipCount);
            Assert.AreEqual (0, anim.Clips.Count ());
        }

        [TestMethod]
        public void Test_AddClip () {
            var anim = new AnimationController ();
            var clip1 = new AnimationClip ("Name");
            var clip2 = new AnimationClip ("Name");

            anim.AddClip (clip1);
            anim.AddClip (clip2);

            Assert.AreEqual (2, anim.ClipCount);
            Assert.AreEqual (2, anim.Clips.Count ());

            anim.RemoveClip (clip1);
            anim.RemoveClip (clip2);

            Assert.AreEqual (0, anim.ClipCount);
            Assert.AreEqual (0, anim.Clips.Count ());
        }

        [TestMethod]
        public void Test_OnAnimate_Float_Once () {
            var track = new AnimationTrack ("Speed", InterpolationType.Step);
            track.AddKeyframe (1, 1.0f);
            track.AddKeyframe (2, 2.0f);

            var clip = new AnimationClip ("TestClip");
            clip.WrapMode = WrapMode.Once;
            clip.Duration = 3;

            var target = new MyDummyTarget ();

            clip.AddTrack (target, track);

            var anim = new AnimationController ();
            anim.AddClip (clip);

            Assert.AreEqual (0.0f, target.Speed);

            anim.OnAnimate (1);
            Assert.AreEqual (1.0f, target.Speed);

            anim.OnAnimate (2);
            Assert.AreEqual (2.0f, target.Speed);

            anim.OnAnimate (3);
            Assert.AreEqual (2.0f, target.Speed);
        }

        [TestMethod]
        public void Test_OnAnimate_Float_Loop () {

            var clip = new AnimationClip ("TestClip");
            clip.WrapMode = WrapMode.Loop;
            clip.Duration = 3;

            var track = new AnimationTrack ("Speed", InterpolationType.Step);
            track.AddKeyframe (1, 1.0f);
            track.AddKeyframe (2, 2.0f);

            var target = new MyDummyTarget ();

            clip.AddTrack (target, track);

            var anim = new AnimationController ();
            anim.AddClip (clip);

            Assert.AreEqual (0.0f, target.Speed);

            anim.OnAnimate (1);
            Assert.AreEqual (1.0f, target.Speed);

            anim.OnAnimate (2);
            Assert.AreEqual (2.0f, target.Speed);

            anim.OnAnimate (3);
            Assert.AreEqual (1.0f, target.Speed);    // 折り返し
        }

        [TestMethod]
        public void Test_OnAnimate_Point_Once () {

            var clip = new AnimationClip ("TestClip");
            clip.WrapMode = WrapMode.Once;
            clip.Duration = 3;

            var track = new AnimationTrack ("Point", InterpolationType.Step);
            track.AddKeyframe (1, new Point (1, 2));
            track.AddKeyframe (2, new Point (2, 3));

            var target = new MyDummyTarget ();

            clip.AddTrack (target, track);

            var anim = new AnimationController ();
            anim.AddClip (clip);

            Assert.AreEqual (0f, target.Point.X, 0.0001f);
            Assert.AreEqual (0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (1);
            Assert.AreEqual (1.0f, target.Point.X, 0.0001f);
            Assert.AreEqual (2.0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (2);
            Assert.AreEqual (2.0f, target.Point.X, 0.0001f);
            Assert.AreEqual (3.0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (3);
            Assert.AreEqual (2.0f, target.Point.X, 0.0001f);
            Assert.AreEqual (3.0f, target.Point.Y, 0.0001f);
        }

        [TestMethod]
        public void Test_OnAnimate_Point_Loop () {

            var clip = new AnimationClip ("TestClip");
            clip.WrapMode = WrapMode.Loop;
            clip.Duration = 3;

            var track = new AnimationTrack ("Point", InterpolationType.Step);
            track.AddKeyframe (1, new Point (1, 2));
            track.AddKeyframe (2, new Point (2, 3));

            var target = new MyDummyTarget ();

            clip.AddTrack (target, track);

            var anim = new AnimationController ();
            anim.AddClip (clip);

            Assert.AreEqual (0f, target.Point.X, 0.0001f);
            Assert.AreEqual (0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (1);
            Assert.AreEqual (1.0f, target.Point.X, 0.0001f);
            Assert.AreEqual (2.0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (2);
            Assert.AreEqual (2.0f, target.Point.X, 0.0001f);
            Assert.AreEqual (3.0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (3);
            Assert.AreEqual (1.0f, target.Point.X, 0.0001f);   // 折り返し
            Assert.AreEqual (2.0f, target.Point.Y, 0.0001f);   // 折り返し
        }
    }

}
