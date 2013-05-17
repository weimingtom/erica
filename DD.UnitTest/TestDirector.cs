using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD;

namespace DD.UnitTest {
    [TestClass]
    public class TestDirector {
        [TestMethod]
        public void Test_IsAlive () {
            var direc = new Director ();

            Assert.AreEqual (true, direc.IsAlive);
        }
        
        [TestMethod]
        public void Test_Exit () {
            var direc = new Director ();
            direc.Exit ();

            Assert.AreEqual (false, direc.IsAlive);
        }

        [TestMethod]
        public void Test_CurrnetScene () {
            var direc = new Director ();
            var sce1 = new Script ("Scene1");

            direc.PushScript (sce1);

            Assert.AreEqual (sce1, direc.CurrentScript);
        }

        [TestMethod]
        public void Test_SceneCount () {
            var direc = new Director ();
            var sce1 = new Script ("Scene1");
            var sce2 = new Script ("Scene2");
            direc.PushScript (sce1);
            direc.PushScript (sce2);

            Assert.AreEqual (2, direc.ScriptCount);
        }

        [TestMethod]
        public void Test_Scenes () {
            var direc = new Director ();
            var sce1 = new Script ("Scene1");
            var sce2 = new Script ("Scene2");
            direc.PushScript (sce1);
            direc.PushScript (sce2);

            Assert.AreEqual (2, direc.Scripts.Count());
        }

        [TestMethod]
        public void Test_AddScene () {
            var director = new Director ();

            var scene1 = new Script ("Scene1");
            director.PushScript (scene1);
            Assert.AreEqual (1, director.ScriptCount);
            Assert.AreEqual (scene1, director.CurrentScript);

            var scene2 = new Script ("Scene2");
            director.PushScript (scene2);
            Assert.AreEqual (2, director.ScriptCount);
            Assert.AreEqual (scene2, director.CurrentScript);
        }
        
        [TestMethod]
        public void Test_RemoveScene () {
            var director = new Director ();

            var scene1 = new Script ("Scene1");
            var scene2 = new Script ("Scene2");
            director.PushScript (scene1);
            director.PushScript (scene2);

            Assert.AreEqual (2, director.ScriptCount);
            Assert.AreEqual (scene2, director.CurrentScript);

            director.PopScript ();

            Assert.AreEqual (1, director.ScriptCount);
            Assert.AreEqual (scene1, director.CurrentScript);
        }
    }
}
