using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestAnimationTrack {
      
        [TestMethod]
        public void Test_New () {
            var track = new AnimationTrack ("PropetyName", InterpolationType.Step);

            Assert.AreEqual ("PropetyName", track.TargetProperty);
            Assert.AreEqual (0, track.KeyframeCount);
            Assert.AreEqual (0, track.ComponentCount);
            Assert.AreEqual (null, track.ComponentType);
        }

        [TestMethod]
        public void Test_AddKeyframe_Primitive () {
            var track = new AnimationTrack ("PropetyName", InterpolationType.Step);

            // 内部でソート
            track.AddKeyframe (4, 4);
            track.AddKeyframe (3, 3);
            track.AddKeyframe (2, 2);
            track.AddKeyframe (1, 1);

            Assert.AreEqual (1, track.GetKeyframe (0).Time);
            Assert.AreEqual (1, track.GetKeyframe (0).Value);

            Assert.AreEqual (2, track.GetKeyframe (1).Time);
            Assert.AreEqual (2, track.GetKeyframe (1).Value);
            
            Assert.AreEqual (3, track.GetKeyframe (2).Value);
            Assert.AreEqual (3, track.GetKeyframe (2).Time);
            
            Assert.AreEqual (4, track.GetKeyframe (3).Value);
            Assert.AreEqual (4, track.GetKeyframe (3).Time);
        }

        [TestMethod]
        public void Test_AddKeyframe_ValueType () {
            var track = new AnimationTrack ("PropetyName", InterpolationType.Step);

            track.AddKeyframe (1, new Vector3 (1, 2, 3));
            track.AddKeyframe (2, new Vector3 (4, 5, 6));
            track.AddKeyframe (3, new Vector3 (7, 8, 9));
            track.AddKeyframe (4, new Vector3 (10, 11, 12));

            Assert.AreEqual (1, track.GetKeyframe (0).Time);
            Assert.AreEqual (new Vector3(1,2,3), track.GetKeyframe (0).Value);

            Assert.AreEqual (2, track.GetKeyframe (1).Time);
            Assert.AreEqual (new Vector3 (4,5,6), track.GetKeyframe (1).Value);

            Assert.AreEqual (3, track.GetKeyframe (2).Time);
            Assert.AreEqual (new Vector3 (7,8,9), track.GetKeyframe (2).Value);

            Assert.AreEqual (4, track.GetKeyframe (3).Time);
            Assert.AreEqual (new Vector3 (10,11,12), track.GetKeyframe (3).Value);
        }

        /// <summary>
        /// Step補完のテスト（プリミティブ型）
        /// </summary>
        [TestMethod]
        public void Test_Sample_Step_Primitive () {
            var track = new AnimationTrack ("PropetyName", InterpolationType.Step);

            track.AddKeyframe (1, 1.0f);
            track.AddKeyframe (2, 2.0f);

            Assert.AreEqual (1.0f, track.Sample (0.0f), 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (0.5f), 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (1.0f), 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (1.5f), 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (2.0f), 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (2.5f), 0.0001f);
        }

        /// <summary>
        /// Step補完のテスト（プリミティブ型）
        /// </summary>
        [TestMethod]
        public void Test_Sample_Linear_Primitive () {
            var track = new AnimationTrack ("PropetyName", InterpolationType.Linear);

            track.AddKeyframe (1, 1.0f);
            track.AddKeyframe (2, 2.0f);

            Assert.AreEqual (1.0f, track.Sample (0.0f), 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (0.5f), 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (1.0f), 0.0001f);
            Assert.AreEqual (1.5f, track.Sample (1.5f), 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (2.0f), 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (2.5f), 0.0001f);
        }

        /// <summary>
        /// Linear補完のテスト（構造体型）
        /// </summary>
        [TestMethod]
        public void Test_Sample_Step_ValueType () {
            var track = new AnimationTrack ("PropetyName", InterpolationType.Step);

            track.AddKeyframe (1, new Vector3 (1, 2, 0));
            track.AddKeyframe (2, new Vector3 (2, 3, 0));

            Assert.AreEqual (1.0f, track.Sample (0.0f).X, 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (0.5f).X, 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (1.0f).X, 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (1.5f).X, 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (2.0f).X, 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (2.5f).X, 0.0001f);

            Assert.AreEqual (2.0f, track.Sample (0.0f).Y, 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (0.5f).Y, 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (1.0f).Y, 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (1.5f).Y, 0.0001f);
            Assert.AreEqual (3.0f, track.Sample (2.0f).Y, 0.0001f);
            Assert.AreEqual (3.0f, track.Sample (2.5f).Y, 0.0001f);
        }

        /// <summary>
        /// Linear補完のテスト（構造体型）
        /// </summary>
        [TestMethod]
        public void Test_Sample_Linear_ValueType () {
            var track = new AnimationTrack ("PropetyName", InterpolationType.Linear);

            track.AddKeyframe (1, new Vector3 (1, 2, 0));
            track.AddKeyframe (2, new Vector3 (2, 3, 0));

            Assert.AreEqual (1.0f, track.Sample (0.0f).X, 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (0.5f).X, 0.0001f);
            Assert.AreEqual (1.0f, track.Sample (1.0f).X, 0.0001f);
            Assert.AreEqual (1.5f, track.Sample (1.5f).X, 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (2.0f).X, 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (2.5f).X, 0.0001f);

            Assert.AreEqual (2.0f, track.Sample (0.0f).Y, 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (0.5f).Y, 0.0001f);
            Assert.AreEqual (2.0f, track.Sample (1.0f).Y, 0.0001f);
            Assert.AreEqual (2.5f, track.Sample (1.5f).Y, 0.0001f);
            Assert.AreEqual (3.0f, track.Sample (2.0f).Y, 0.0001f);
            Assert.AreEqual (3.0f, track.Sample (2.5f).Y, 0.0001f);
        }

        /// <summary>
        /// 明示的な型変換（Foat -> Int）を伴うサンプリングのテスト（プリミティブ型）
        /// </summary>
        /// <remarks>
        /// 補完計算の結果は Float なので、内部的にそれを Int にする明示的な変換が存在する。
        /// それのチェック
        /// </remarks>
        [TestMethod]
        public void Test_Sample_ValueType_Convert_Float2Int () {
            var track = new AnimationTrack ("PropetyName", InterpolationType.Linear);

            track.AddKeyframe (1, 1);                         // Int
            track.AddKeyframe (100, 100);                     // Int

            Assert.AreEqual (1, track.Sample (1), 0.0001f);      // Int
            Assert.AreEqual (50, track.Sample (50), 0.0001f);    // Int
            Assert.AreEqual (100, track.Sample (100), 0.0001f);  // Int
        }

        /// <summary>
        /// 明示的な型変換（Foat -> Int）を伴うサンプリングのテスト（プリミティブ型）（構造体型）
        /// </summary>
        /// <remarks>
        /// 補完計算の結果は Float なので、内部的にそれを Int にする明示的な変換が存在する。
        /// それのチェック
        /// </remarks>
        [TestMethod]
        public void Test_Sample_ValueType_Float2Int () {
            var track = new AnimationTrack ("PropetyName", InterpolationType.Linear);

            track.AddKeyframe (1, new IPoint (1, 0));     // Int
            track.AddKeyframe (100, new IPoint (100, 0)); // Int

            Assert.AreEqual (1, track.Sample (1).X);         // Int
            Assert.AreEqual (50, track.Sample (50).X);       // Int
            Assert.AreEqual (100, track.Sample (100).X);     // Int
        }


    }
}
