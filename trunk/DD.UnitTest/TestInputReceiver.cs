using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    [TestClass]
    public class TestInputReceiver {
        [TestMethod]
        public void Test_New () {
            var input = new InputReceiver ();

            Assert.AreEqual (false, input.Alt);
            Assert.AreEqual (false, input.Control);
            Assert.AreEqual (false, input.Shift);
            Assert.AreEqual (false, input.AnyKey);
            Assert.AreEqual (false, input.AnyKeyDown);
            Assert.AreEqual (false, input.GetKey (KeyCode.Return));
            Assert.AreEqual (false, input.GetKeyUp (KeyCode.Return));
            Assert.AreEqual (false, input.GetKeyDown (KeyCode.Return));
            Assert.AreEqual (0, input.AliasCount);
            Assert.AreEqual (0, input.KeyCount);
        }

        [TestMethod]
        public void Test_AddAlias () {
            var input = new InputReceiver ();

            input.AddAlias ("Right", KeyCode.Mouse0);
            input.AddAlias ("Left", KeyCode.Mouse1);
            input.AddAlias ("Middle", KeyCode.Mouse2);
            input.AddAlias ("Yet another Middle", KeyCode.Mouse2);

            Assert.AreEqual (4, input.AliasCount);
            Assert.AreEqual ("Right", input.GetAlias (0).Name);
            Assert.AreEqual ("Left", input.GetAlias (1).Name);
            Assert.AreEqual ("Middle", input.GetAlias (2).Name);
            Assert.AreEqual ("Yet another Middle", input.GetAlias (3).Name);
            Assert.AreEqual (KeyCode.Mouse0, input.GetAlias (0).KeyCode);
            Assert.AreEqual (KeyCode.Mouse1, input.GetAlias (1).KeyCode);
            Assert.AreEqual (KeyCode.Mouse2, input.GetAlias (2).KeyCode);
            Assert.AreEqual (KeyCode.Mouse2, input.GetAlias (3).KeyCode);
        }

        [TestMethod]
        public void Test_RemoveAlias () {
            var input = new InputReceiver ();

            input.AddAlias ("Right", KeyCode.Mouse0);
            input.AddAlias ("Left", KeyCode.Mouse1);
            input.AddAlias ("Middle", KeyCode.Mouse2);

            input.RemoveAlias (2);
            input.RemoveAlias (1);
            input.RemoveAlias (0);

            Assert.AreEqual (0, input.AliasCount);
        }


        [TestMethod]
        public void Test_AddKeyCode () {
            var input = new InputReceiver ();

            input.AddKeyInput (KeyCode.Keypad0);
            input.AddKeyInput (KeyCode.Keypad1);
            input.AddKeyInput (KeyCode.Keypad2);
            input.AddKeyInput (KeyCode.Keypad2);

            Assert.AreEqual (4, input.KeyCount);
            Assert.AreEqual (4, input.Keys.Count ());
            Assert.AreEqual (true, input.AnyKey);
        }

        [TestMethod]
        public void Test_GetKey_1 () {
            var input = new InputReceiver ();

            input.AddKeyInput (KeyCode.Keypad0);
            input.AddKeyInput (KeyCode.Keypad1);
            input.AddKeyInput (KeyCode.Keypad2);
            input.AddKeyInput (KeyCode.Keypad2);

            Assert.AreEqual (true, input.GetKey(KeyCode.Keypad0));
            Assert.AreEqual (true, input.GetKey (KeyCode.Keypad1));
            Assert.AreEqual (true, input.GetKey (KeyCode.Keypad2));
            Assert.AreEqual (false, input.GetKey (KeyCode.Keypad3));
        }

        [TestMethod]
        public void Test_GetKey_2 () {
            var input = new InputReceiver ();

            input.AddAlias("Number0", KeyCode.Keypad0);
            input.AddAlias("Number1", KeyCode.Keypad1);
            input.AddAlias("Number2", KeyCode.Keypad2);
            input.AddAlias("Number3", KeyCode.Keypad3);

            input.AddKeyInput (KeyCode.Keypad0);
            input.AddKeyInput (KeyCode.Keypad1);
            input.AddKeyInput (KeyCode.Keypad2);

            Assert.AreEqual (true, input.GetKey("Number0"));
            Assert.AreEqual (true, input.GetKey ("Number1"));
            Assert.AreEqual (true, input.GetKey ("Number2"));
            Assert.AreEqual (false, input.GetKey ("Number3"));
            Assert.AreEqual (false, input.GetKey ("Number4"));
        }

        [TestMethod]
        public void Test_Indexer () {
            var input = new InputReceiver ();

            input.AddAlias ("Number0", KeyCode.Keypad0);
            input.AddAlias ("Number1", KeyCode.Keypad1);
            input.AddAlias ("Number2", KeyCode.Keypad2);
            input.AddAlias ("Number3", KeyCode.Keypad3);

            input.AddKeyInput (KeyCode.Keypad0);
            input.AddKeyInput (KeyCode.Keypad1);
            input.AddKeyInput (KeyCode.Keypad2);

            Assert.AreEqual (true, input["Number0"]);
            Assert.AreEqual (true, input["Number1"]);
            Assert.AreEqual (true, input["Number2"]);
            Assert.AreEqual (true, input[KeyCode.Keypad0]);
            Assert.AreEqual (true, input[KeyCode.Keypad1]);
            Assert.AreEqual (true, input[KeyCode.Keypad2]);
        }

        [TestMethod]
        public void Test_Keys () {
            var input = new InputReceiver ();

            input.AddKeyInput (KeyCode.Keypad0);
            input.AddKeyInput (KeyCode.Keypad1);
            input.AddKeyInput (KeyCode.Keypad2);

            var expected = new[] { KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2 };

            CollectionAssert.AreEqual (expected, input.Keys.ToArray());
        }

        [TestMethod]
        public void Test_Alt_Ctr_Shift () {
            var input = new InputReceiver ();

            input.AddKeyInput (KeyCode.LeftAlt);
            input.AddKeyInput (KeyCode.LeftControl);
            input.AddKeyInput (KeyCode.LeftShift);

            Assert.AreEqual (true, input.GetKey(KeyCode.LeftAlt));
            Assert.AreEqual (true, input.GetKey (KeyCode.LeftControl));
            Assert.AreEqual (true, input.GetKey (KeyCode.LeftShift));
            Assert.AreEqual (false, input.GetKey (KeyCode.RightAlt));
            Assert.AreEqual (false, input.GetKey (KeyCode.RightControl));
            Assert.AreEqual (false, input.GetKey (KeyCode.RightShift));
        }

        [TestMethod]
        public void Test_OnUpdate () {
            var input = new InputReceiver ();


            input.AddKeyInput (KeyCode.Alpha0);
            input.AddKeyInput (KeyCode.Alpha1);
            input.AddKeyInput (KeyCode.Alpha2);

            Assert.AreEqual (true, input.AnyKey);
            Assert.AreEqual (true, input.AnyKeyDown);


            // 1フレーム進める
            input.OnUpdate (0);

            Assert.AreEqual (false, input.AnyKey);
            Assert.AreEqual (false, input.AnyKeyDown);

            Assert.AreEqual (0, input.KeyCount);
            Assert.AreEqual (false, input.GetKey(KeyCode.Alpha0));
            Assert.AreEqual (false, input.GetKey (KeyCode.Alpha1));
            Assert.AreEqual (false, input.GetKey (KeyCode.Alpha2));
        }


        [TestMethod]
        public void Test_KeyUp () {
            var input = new InputReceiver ();

            input.AddKeyInput (KeyCode.Return);
            input.AddKeyInput (KeyCode.Space);
            input.AddKeyInput (KeyCode.UpArrow);

            // 1フレーム進める
            input.OnUpdate (0);

            input.AddKeyInput (KeyCode.Backspace);
            input.AddKeyInput (KeyCode.F1);
            input.AddKeyInput (KeyCode.DownArrow);


            Assert.AreEqual (true, input.GetKeyUp (KeyCode.Return));
            Assert.AreEqual (true, input.GetKeyUp (KeyCode.Space));
            Assert.AreEqual (true, input.GetKeyUp (KeyCode.UpArrow));

            Assert.AreEqual (false, input.GetKeyUp (KeyCode.Backspace));
            Assert.AreEqual (false, input.GetKeyUp (KeyCode.F1));
            Assert.AreEqual (false, input.GetKeyUp (KeyCode.DownArrow));

        }

        [TestMethod]
        public void Test_KeyDown () {
            var input = new InputReceiver ();

            input.AddKeyInput (KeyCode.Return);
            input.AddKeyInput (KeyCode.Space);
            input.AddKeyInput (KeyCode.UpArrow);

            // 1フレーム進める
            input.OnUpdate (0);

            input.AddKeyInput (KeyCode.Backspace);
            input.AddKeyInput (KeyCode.F1);
            input.AddKeyInput (KeyCode.DownArrow);
            
            Assert.AreEqual (false, input.GetKeyDown (KeyCode.Return));
            Assert.AreEqual (false, input.GetKeyDown (KeyCode.Space));
            Assert.AreEqual (false, input.GetKeyDown (KeyCode.UpArrow));

            Assert.AreEqual (true, input.GetKeyDown (KeyCode.Backspace));
            Assert.AreEqual (true, input.GetKeyDown (KeyCode.F1));
            Assert.AreEqual (true, input.GetKeyDown (KeyCode.DownArrow));
        }
    
    
    
    }
}
