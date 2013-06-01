using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// 現在のところライン イベントの単体テストはない
// （実装が難しい・・・）
// 今後の課題

namespace DD.UnitTest {


    [TestClass]
    public class TestLineReader {
        [TestMethod]
        public void Test_New () {
            var reader = new LineReader (600, 480);

            Assert.AreEqual (0, reader.LineCount);
            Assert.AreEqual (0, reader.CurrentPosition);
            Assert.AreEqual (null, reader.CurrentLine);
            Assert.AreEqual (600, reader.Width);
            Assert.AreEqual (480, reader.Height);
        }


        [TestMethod]
        public void Test_AddLine () {
            var reader = new LineReader (600, 480);
            reader.AddLine(new Line ("Actor", "Words", "Sound", "Event"));

            Assert.AreEqual (1, reader.LineCount);
        }


        [TestMethod]
        public void Test_LoadLine () {
            var reader = new LineReader (600, 480);

            reader.LoadLine ("HelloMiku.txt");

            Assert.AreEqual (10, reader.LineCount);
        }

        [TestMethod]
        public void TestSetCharacterSize () {
            var reader = new LineReader (600, 480);

            reader.CharacterSize = 1;
            Assert.AreEqual (1, reader.CharacterSize);
            
            reader.SetCharacterSize (2);
            Assert.AreEqual (2, reader.CharacterSize);
        }

        [TestMethod]
        public void Test_Color () {
            var reader = new LineReader (600, 480);

            reader.Color = new Color (1, 2, 3, 4);
            Assert.AreEqual(new Color(1,2,3,4), reader.Color);
            
            reader.SetColor(5,6,7,8);
            Assert.AreEqual(new Color(5,6,7,8), reader.Color);
        }


        [TestMethod]
        public void Test_Jump () {
            var reader = new LineReader (600, 480);

            reader.LoadLine ("HelloMiku.txt");

            reader.Jump (5);
            Assert.AreEqual (5, reader.CurrentPosition);

            reader.Next ();
            Assert.AreEqual (6, reader.CurrentPosition);

            reader.Prev ();
            Assert.AreEqual (5, reader.CurrentPosition);

            reader.Rewind ();
            Assert.AreEqual (0, reader.CurrentPosition);

        }

        [TestMethod]
        public void Test_SetFeedMode () {
            var reader = new LineReader (600, 480);

            reader.SetFeedMode (FeedMode.Automatic, new LineReader.FeedParameters (1, 2));

            Assert.AreEqual (FeedMode.Automatic, reader.FeedMode);
            Assert.AreEqual (1, reader.FeedParameter.TimeAfterOneCharacter);
            Assert.AreEqual (2, reader.FeedParameter.TimeAfterOneSentense);

        }
    }
}
