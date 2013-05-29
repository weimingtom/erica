using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestSaveDataContainer {
        public struct MyStruct {
            public MyStruct (int x, float y, string name)
                : this () {
                this.X = x;
                this.Y = y;
                this.Name = name;
            }
            public int X { get; set; }
            public float Y { get; set; }
            public string Name { get; set; }
        }
        [TestMethod]
        public void Test_New () {
            var svd = new SaveDataContainer ();

            Assert.AreEqual (0, svd.ItemCount);
            Assert.AreEqual (0, svd.Items.Count ());
            Assert.AreEqual (false, svd.Encryption);
            Assert.AreEqual ("./", svd.Path);
            Assert.AreEqual ("Hello world", svd.Password);
        }

        [TestMethod]
        public void Test_Add () {
            var svd = new SaveDataContainer ();
            var key = "Item0";
            var value = new MyStruct (1, 2f, "3");

            svd.Add (key, value);

            Assert.AreEqual (1, svd.ItemCount);
            Assert.AreEqual (1, svd.Items.Count ());
        }

        [TestMethod]
        public void Test_Remove () {
            var svd = new SaveDataContainer ();
            var key = "Item0";
            var value = new MyStruct (1, 2f, "3");

            svd.Add (key, value);
            svd.Remove (key);

            Assert.AreEqual (0, svd.ItemCount);
            Assert.AreEqual (0, svd.Items.Count ());
        }

        [TestMethod]
        public void Test_Get () {
            var svd = new SaveDataContainer ();
            var key = "Item0";
            var value = new MyStruct (1, 2f, "3");

            svd.Add (key, value);

            Assert.AreEqual (1, svd.ItemCount);
            Assert.AreEqual (value, svd.Get (key));
            Assert.AreEqual (value, svd[key]);
        }

        [TestMethod]
        public void Test_GetOrCreate () {
            var svd = new SaveDataContainer ();
            var key = "Item0";
            var value = new MyStruct (1, 2f, "3");

            svd.GetOrCreate (key, value);

            Assert.AreEqual (1, svd.ItemCount);
            Assert.AreEqual (value, svd.Get (key));
            Assert.AreEqual (value, svd[key]);
        }

        [TestMethod]
        public void Test_SetEncryptionEnable () {
            var svd = new SaveDataContainer ();

            svd.SetEncryptionEnable (true, "New password");

            Assert.AreEqual (true, svd.Encryption);
            Assert.AreEqual ("New password", svd.Password);
        }


        [TestMethod]
        public void Test_Save_and_Load () {
            var svd1 = new SaveDataContainer ();
            svd1.Add ("Item", new MyStruct (1, 2f, "3"));
            svd1.Save ("TestSaveDataContainer-001.txt");

            var svd2 = new SaveDataContainer ();
            svd2.Load ("TestSaveDataContainer-001.txt");

            Assert.AreEqual (1, svd2.ItemCount);
            Assert.AreEqual (new MyStruct (1, 2f, "3"), svd2.Get ("Item"));
        }


        /// <summary>
        /// 暗号化ファイルのテスト
        /// </summary>
        [TestMethod]
        public void Test_Save_and_Load_with_Encryption () {
            var svd1 = new SaveDataContainer ();
            svd1.Encryption = true;
            svd1.Add ("Item", new MyStruct (1, 2f, "3"));
            svd1.Save ("TestSaveDataContainer-002.txt");

            var svd2 = new SaveDataContainer ();
            svd2.Encryption = true;
            svd2.Load ("TestSaveDataContainer-002.txt");

            Assert.AreEqual (1, svd2.ItemCount);
            Assert.AreEqual (new MyStruct (1, 2f, "3"), svd2.Get ("Item"));
        }

        /// <summary>
        /// ディレクトリの作成を伴うセーブ
        /// </summary>
        [TestMethod]
        public void Test_SaveOrCreate () {

            var svd1 = new SaveDataContainer ();
            svd1.Path = "./savedata1/";   // 存在しない
            svd1.Add ("Item", 1);
            svd1.Save ("TestSaveDataContainer-003.txt");

            var svd2 = new SaveDataContainer ();
            svd2.Path = "./savedata1/";
            svd2.Load ("TestSaveDataContainer-003.txt");

            Assert.AreEqual (1, svd2.ItemCount);
        }

        /// <summary>
        /// ディレクトリの作成を伴うロード
        /// </summary>
        [TestMethod]
        public void Test_LoadOrCreate () {

            var svd1 = new SaveDataContainer ();
            svd1.Path = "./savedata2/";    // 存在しない
            svd1.LoadOrCreate ("TestSaveDataContainer-004.txt");
            svd1.Add ("Item", 1);
            svd1.Save ("TestSaveDataContainer-004.txt");

            var svd2 = new SaveDataContainer ();
            svd2.Path = "./savedata2/";
            svd2.Load ("TestSaveDataContainer-004.txt");

            Assert.AreEqual (1, svd2.ItemCount);
        }
      
    }
}
