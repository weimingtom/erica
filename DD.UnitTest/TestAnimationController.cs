using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestAnimationController {

        public class MyTarget {
            public float Speed { get; set; }
            public Vector3 Point { get; set; }
        }

        [TestMethod]
        public void Test_New () {
            var anim = new AnimationController ();

            Assert.AreEqual (0, anim.ClipCount);
            Assert.AreEqual (0, anim.Clips.Count ());
        }

        [TestMethod]
        public void Test_Indexer () {
            var anim = new AnimationController ();
            var clip = new AnimationClip (100, "Clip");
            anim.AddClip (clip);

            Assert.AreEqual (clip, anim["Clip"]);
        }

        [TestMethod]
        public void Test_AddClip () {
            var anim = new AnimationController ();
            var clip1 = new AnimationClip (100, "Name");
            var clip2 = new AnimationClip (100, "Name");

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
        public void Test_RemoveClip () {


        }
        
        /// <summary>
        /// 対象オブジェクトが弱い参照である事のテスト
        /// </summary>
        [TestMethod]
        public void Test_WeakReferenceTrack () {
            var target = new MyTarget ();
            var track = new AnimationTrack ("Speed", InterpolationType.Linear);
            track.AddKeyframe (0, 100.0f);
            track.AddKeyframe (100, 100.0f);

            var clip = new AnimationClip (100, "Clip");
            clip.AddTrack (target, track);
          
            var anim = new AnimationController ();
            anim.AddClip (clip);

            // ターゲットの解放
            target = null;
            GC.Collect ();

            Assert.AreEqual (1, clip.TrackCount);
            Assert.AreEqual (false, clip.GetTrack (0).Item1.IsAlive);

            // 解放済みのターゲットを含むアニメーションの実行。
            // エラーが起きてはならない
            anim.OnAnimate (0);

        }




        [TestMethod]
        public void Test_OnAnimate_Float_Once () {
            var track = new AnimationTrack ("Speed", InterpolationType.Step);
            track.AddKeyframe (1, 1.0f);
            track.AddKeyframe (2, 2.0f);

            var clip = new AnimationClip (3, "TestClip");
            clip.WrapMode = WrapMode.Once;
            clip.Duration = 3;
            clip.Play ();

            var target = new MyTarget ();

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

            var clip = new AnimationClip (3, "TestClip");
            clip.WrapMode = WrapMode.Loop;
            clip.Play();

            var track = new AnimationTrack ("Speed", InterpolationType.Step);
            track.AddKeyframe (1, 1.0f);
            track.AddKeyframe (2, 2.0f);

            var target = new MyTarget ();

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

            var clip = new AnimationClip (3, "TestClip");
            clip.WrapMode = WrapMode.Once;
            clip.Duration = 3;
            clip.Play ();

            var track = new AnimationTrack ("Point", InterpolationType.Step);
            track.AddKeyframe (1, new Vector3 (1, 2, 0));
            track.AddKeyframe (2, new Vector3 (2, 3, 0));

            var target = new MyTarget ();

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

            var clip = new AnimationClip (3, "TestClip");
            clip.WrapMode = WrapMode.Loop;
            clip.Duration = 3;
            clip.Play ();

            var track = new AnimationTrack ("Point", InterpolationType.Step);
            track.AddKeyframe (1, new Vector3 (1, 2, 0));
            track.AddKeyframe (2, new Vector3 (2, 3, 0));

            var target = new MyTarget ();

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
