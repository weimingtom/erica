using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestAnimationController {
        private class MyTarget {
            public float Speed { get; set; }
            public Vector3 Point { get; set; }
        }
        private class MyEventArgs : EventArgs{
            public MyEventArgs (int value){
                this.Value = value;
            }
            public int Value { get; set; }
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
            anim.OnAnimate (0, 0);

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

            anim.OnAnimate (1, 0);
            Assert.AreEqual (1.0f, target.Speed);

            anim.OnAnimate (2, 0);
            Assert.AreEqual (2.0f, target.Speed);

            anim.OnAnimate (3, 0);
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

            anim.OnAnimate (1, 0);
            Assert.AreEqual (1.0f, target.Speed);

            anim.OnAnimate (2, 0);
            Assert.AreEqual (2.0f, target.Speed);

            anim.OnAnimate (3, 0);
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

            anim.OnAnimate (1, 0);
            Assert.AreEqual (1.0f, target.Point.X, 0.0001f);
            Assert.AreEqual (2.0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (2, 0);
            Assert.AreEqual (2.0f, target.Point.X, 0.0001f);
            Assert.AreEqual (3.0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (3, 0);
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

            anim.OnAnimate (1, 0);
            Assert.AreEqual (1.0f, target.Point.X, 0.0001f);
            Assert.AreEqual (2.0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (2, 0);
            Assert.AreEqual (2.0f, target.Point.X, 0.0001f);
            Assert.AreEqual (3.0f, target.Point.Y, 0.0001f);

            anim.OnAnimate (3, 0);
            Assert.AreEqual (1.0f, target.Point.X, 0.0001f);   // 折り返し
            Assert.AreEqual (2.0f, target.Point.Y, 0.0001f);   // 折り返し
        }

        /// <summary>
        /// アニメーション イベントのハンドラー呼び出しのテスト
        /// </summary>
        [TestMethod]
        public void Test_OnAnimate_Event_Handler () {
            var clip = new AnimationClip (100, "TestClip");
            clip.WrapMode = WrapMode.Once;
            clip.Play ();

            var count = 0;
            clip.AddEvent (0, (x, y) => count = 1, null);
            clip.AddEvent (50, (x, y) => count = 2, null);
            clip.AddEvent (100, (x, y) => count = 3, null);

            var anim = new AnimationController ();
            anim.AddClip (clip);

            anim.OnAnimate (0, 0);
            Assert.AreEqual (1, count);

            anim.OnAnimate (50, 0);
            Assert.AreEqual (2, count);

            anim.OnAnimate (100, 0);
            Assert.AreEqual (3, count);
        }

        /// <summary>
        /// アニメーション イベントの引数のテスト
        /// </summary>
        [TestMethod]
        public void Test_OnAnimate_Event_Args () {
            var clip = new AnimationClip (100, "TestClip");
            clip.WrapMode = WrapMode.Once;
            clip.Play ();

            var args1 = new MyEventArgs (1);
            var args2 = new MyEventArgs (2);
            var args3 = new MyEventArgs (3);

            var count = 0;
            clip.AddEvent (0, (sender, args) => count = ((MyEventArgs)args).Value, args1);
            clip.AddEvent (50, (sender, args) => count = ((MyEventArgs)args).Value, args2);
            clip.AddEvent (100, (sender, args) => count = ((MyEventArgs)args).Value, args3);

            var anim = new AnimationController ();
            anim.AddClip (clip);

            anim.OnAnimate (0, 0);
            Assert.AreEqual (1, count);

            anim.OnAnimate (50, 0);
            Assert.AreEqual (2, count);

            anim.OnAnimate (100, 0);
            Assert.AreEqual (3, count);
        }

        /// <summary>
        /// アニメーション イベントの複雑なテスト
        /// </summary>
        [TestMethod]
        public void Test_OnAnimate_Event_2 () {
            var clip = new AnimationClip (100, "TestClip");
            clip.WrapMode = WrapMode.Once;
            clip.Play ();

            var args1 = new MyEventArgs (1);
            var args2 = new MyEventArgs (2);
            var args3 = new MyEventArgs (3);

            var count = 0;
            clip.AddEvent (0, (sender, args) => count = 1, null);
            clip.AddEvent (50, (sender, args) => count = 2, null);
            clip.AddEvent (100, (sender, args) => count += 1, null);
            clip.AddEvent (100, (sender, args) => count += 1, null);

            var anim = new AnimationController ();
            anim.AddClip (clip);

            // 停止中のクリップはイベントを発火しない
            clip.Stop ();
            anim.OnAnimate (0, 0);
            Assert.AreEqual (0, count);

            clip.Play ();
            
            // 逆再生中の dtime は逆方向
            // clip.SetSpeed (-1, 50);
            // anim.OnAnimate (40, 10);
            // Assert.AreEqual (2, count);

            count = 0;
            clip.Speed = 1;

            // 同じポジションで重複したイベントは両方発火
            anim.OnAnimate (100, 0);
            Assert.AreEqual (2, count);

        }
    }

}
