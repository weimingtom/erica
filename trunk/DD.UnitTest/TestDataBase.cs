using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestDataBase {
        [TestMethod]
        public void Test_Database () {
            var db = new DB.AkatokiEntities ();

            Assert.AreEqual(2, db.GetTableCount());
            Assert.AreEqual (2, db.GetTables().Count ());
        }

    }
}
