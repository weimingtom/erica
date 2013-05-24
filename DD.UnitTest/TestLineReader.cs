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
            var ply = new LineReader (600, 480);

            Assert.AreEqual (0, ply.LineCount);
            Assert.AreEqual (0, ply.CurrentPosition);
            Assert.AreEqual (null, ply.CurrentLine);
            Assert.AreEqual (600, ply.Width);
            Assert.AreEqual (480, ply.Height);
        }

        [TestMethod]
        public void Test_LoadLine () {
            var ply = new LineReader (600, 480);

            ply.LoadLine ("HelloMiku.txt");

            Assert.AreEqual (10, ply.LineCount);
        }

        [TestMethod]
        public void Test_Jump () {
            var ply = new LineReader (600, 480);

            ply.LoadLine ("HelloMiku.txt");

            ply.Jump (5);
            Assert.AreEqual (5, ply.CurrentPosition);

            ply.Next ();
            Assert.AreEqual (6, ply.CurrentPosition);

            ply.Prev ();
            Assert.AreEqual (5, ply.CurrentPosition);

            ply.Rewind ();
            Assert.AreEqual (0, ply.CurrentPosition);

        }
    }
}
