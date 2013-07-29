﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD;

namespace DD.UnitTest {
    [TestClass]
    public class TestWorld {
        private class MyComponent : Component {
            public int Animated { get; private set; }
            public int Updated { get; private set; }
            public int OnUpdateInited { get; private set; }
            public override void OnAnimate (long msec, long dtime) {
                this.Animated += 1;
            }
            public override void OnUpdateInit (long msec) {
                this.OnUpdateInited += 1;
            }
            public override void OnUpdate (long msec) {
                this.Updated += 1;
            }
        }

        [TestMethod]
        public void Test_New () {
            var wld = new World ("World");

            Assert.AreEqual ("World", wld.Name);
            Assert.AreEqual (null, wld.ActiveCamera);
            Assert.IsNotNull (wld.InputReceiver);
            Assert.IsNotNull (wld.AnimationController);
            Assert.IsNotNull (wld.SoundPlayer);
        }

        [TestMethod]
        public void Test_Property () {
            var wld = new World ();
            wld.SetProperty ("Prop1", 0);
            wld.SetProperty ("Prop1", 1);

            Assert.AreEqual (1, wld.GetProperty<int> ("Prop1"));
            Assert.AreEqual (0, wld.GetProperty<int> ("Prop2"));    // not found
            Assert.AreEqual (1, wld.GetProperty<int> ("Prop2", 1)); // not found

        }

        [TestMethod]
        public void Test_ActiveCamera () {
            var cam = new Camera ();
            var node = new Node ();
            node.Attach (cam);

            var wld = new World ();
            wld.AddChild (node);

            wld.ActiveCamera = node;
            Assert.AreEqual (node, wld.ActiveCamera);
        }

        [TestMethod]
        public void Test_Animate () {
            var wld = new World ();
            var nod = new Node ();
            wld.AddChild (nod);

            var cmp1 = new MyComponent ();
            var cmp2 = new MyComponent ();
            wld.Attach (cmp1);
            nod.Attach (cmp2);

            wld.Animate (0, 0);

            Assert.AreEqual (1, cmp1.Animated);
            Assert.AreEqual (1, cmp2.Animated);

            nod.Animatable = false;
            wld.Animate (0, 0);

            Assert.AreEqual (2, cmp1.Animated);
            Assert.AreEqual (1, cmp2.Animated);

        }

        [TestMethod]
        public void Test_UpdateInit () {
            var wld = new World ();
            var nod = new Node ();
            wld.AddChild (nod);

            var cmp1 = new MyComponent ();
            var cmp2 = new MyComponent ();
            wld.Attach (cmp1);
            nod.Attach (cmp2);

            wld.Update (0);
            Assert.AreEqual (1, cmp1.OnUpdateInited);
            Assert.AreEqual (1, cmp2.OnUpdateInited);

            wld.Update (0);
            Assert.AreEqual (1, cmp1.OnUpdateInited);
            Assert.AreEqual (1, cmp2.OnUpdateInited);
        }

        [TestMethod]
        public void Test_Update () {
            var wld = new World ();
            var nod = new Node ();
            wld.AddChild (nod);

            var cmp1 = new MyComponent ();
            var cmp2 = new MyComponent ();
            wld.Attach (cmp1);
            nod.Attach (cmp2);

            wld.Update (0);

            Assert.AreEqual (1, cmp1.Updated);
            Assert.AreEqual (1, cmp2.Updated);

            nod.Updatable = false;
            wld.Update (0);

            Assert.AreEqual (2, cmp1.Updated);
            Assert.AreEqual (1, cmp2.Updated);
        }

    }
}