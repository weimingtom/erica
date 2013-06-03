using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestTimeCounter {

        [TestMethod]
        public void Test_New_1 () {
            int interval = 0;

            using (var c = new TimeCounter (5, 1)) {
                c.Elapsed += () => interval += 1;
                c.Start ();

                while (c.IsRunning) {
                    System.Threading.Thread.Sleep (1);
                }
                System.Threading.Thread.Sleep (1);

                Assert.AreEqual (5, c.Count);
                Assert.AreEqual (5, interval);
            }
        }

        /// <summary>
        /// インターバル = 0
        /// </summary>
        /// <remarks>
        /// すべてのイベントは呼ばれずにカウントアップが終わる。
        /// </remarks>
        [TestMethod]
        public void Test_New_3 () {
            int interval = 0;

            using (var c = new TimeCounter (5, 0)) {
                c.Elapsed += () => interval += 1;
              c.Start ();

                while (c.IsRunning) {
                    System.Threading.Thread.Sleep (1);
                }
                System.Threading.Thread.Sleep (1);

                Assert.AreEqual (5, c.Count);
                Assert.AreEqual (0, interval);
             }
        }


    }
}
