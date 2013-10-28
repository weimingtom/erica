using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestDatabaseManager {
        [TestMethod]
        public void Test_New () {
            using (var dbms = new DatabaseManager ()) {
                Assert.AreEqual (0, dbms.DataBaseCount);
                Assert.AreEqual (0, dbms.DataBases.Count ());
            }
        }

        [TestMethod]
        public void Test_AddDatabase () {
            using (var dbms = new DatabaseManager ()) {
                var db = new DB.AkatokiEntities ();
                dbms.AddDataBase ("あかときっ！", db);

                Assert.AreEqual (1, dbms.DataBaseCount);
                Assert.AreEqual (1, dbms.DataBases.Count ());
                Assert.AreEqual (db, dbms.GetDataBase ("あかときっ！"));
                Assert.AreEqual (db, dbms.GetDataBase<DB.AkatokiEntities> ());
            }
        }

        [TestMethod]
        public void Test_RemoveDatabase () {
            using (var dbms = new DatabaseManager ()) {
                var db = new DB.AkatokiEntities ();
                dbms.AddDataBase ("あかときっ！", db);
                dbms.RemoveDataBase ("あかときっ！");

                Assert.AreEqual (0, dbms.DataBaseCount);
            }
        }

        [TestMethod]
        public void Test_SaveChanges () {
            // 省略
            // なかなか難しい
        }

    }
}
