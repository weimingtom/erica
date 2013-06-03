using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    public enum MyState {
        State1,
        State2,
        State3,
        State4,
    }

    [TestClass]
    public class TestStateMachine {
        [TestMethod]
        public void Test_New () {
            var sm = new StateMachine<MyState> ();

            Assert.AreEqual (4, sm.StateCount);
            Assert.AreEqual (4, sm.States.Count());
            Assert.AreEqual (MyState.State1, sm.CurrentState);
        }

        [TestMethod]
        public void Test_Transition () {
            var sm = new StateMachine<MyState> ();

            Assert.AreEqual (4, sm.StateCount);
            Assert.AreEqual (4, sm.States.Count ());
            Assert.AreEqual (MyState.State1, sm.CurrentState);
        }


        [TestMethod]
        public void Test_SetCurrentState () {
            var sm = new StateMachine<MyState> ();
            var s1 = sm[MyState.State1];
            var s2 = sm[MyState.State2];
            var s3 = sm[MyState.State3];
            var s4 = sm[MyState.State4];

            // s1 = defulat(T)
            Assert.AreEqual (s1, sm.CurrentState);

            sm.CurrentState = s2;
            Assert.AreEqual (s2, sm.CurrentState);

            sm.SetCurrentState (s3);
            Assert.AreEqual (s3, sm.CurrentState);
        }

        [TestMethod]
        public void Test_AddTransitions () {
            var sm = new StateMachine<MyState> ();
            var s1 = sm[MyState.State1];
            var s2 = sm[MyState.State2];
            sm.AddTransition (s1, s2, ()=>true);
            sm.AddTransition (s2, s1, () => true);

            Assert.AreEqual (2, sm.TransitionCount);
            Assert.AreEqual (2, sm.Transitions.Count ());
        }

        [TestMethod]
        public void Test_Next_1 () {
            var sm = new StateMachine<MyState> ();
            var s1 = sm[MyState.State1];
            var s2 = sm[MyState.State2];
            sm.AddTransition (s1, s2, () => true);
            sm.AddTransition (s2, s1, () => true);

            Assert.AreEqual (s1, sm.CurrentState);
            
            sm.Process ();
            Assert.AreEqual (s2, sm.CurrentState);

            sm.Process ();
            Assert.AreEqual (s1, sm.CurrentState);
        }

        [TestMethod]
        public void Test_Next_2 () {
            var count = 0;
            
            var sm = new StateMachine<MyState> ();
            var s1 = sm[MyState.State1];
            var s2 = sm[MyState.State2];
            var s3 = sm[MyState.State3];
            sm.AddTransition (s1, s2, () => count % 2 == 0);
            sm.AddTransition (s1, s3, () => count % 2 == 1);

            count = 1;
            sm.Process ();  // s1 -> s3

            Assert.AreEqual (s3, sm.CurrentState);
        }

        [TestMethod]
        public void Test_Next_3 () {
            var sm = new StateMachine<MyState> ();
            var s1 = sm[MyState.State1];
            var s2 = sm[MyState.State2];
            var s3 = sm[MyState.State3];
            sm.AddTransition (s1, s2, () => false);
            sm.AddTransition (s1, s3, () => false);

            sm.Process ();  // s1 -> s1 (条件に一致する遷移がない）

            Assert.AreEqual (s1, sm.CurrentState);
        }

        [TestMethod]
        public void Test_SetEvent () {
            var sm = new StateMachine<MyState> ();
            var s1 = sm[MyState.State1];
            var s2 = sm[MyState.State2];

            var enter1 = 0;
            var enter2 = 0;
            var exit1 = 0;
            var exit2 = 0;

            sm.AddEvent (s1, x => enter1++, x => exit1++);
            sm.AddEvent (s2, x => enter2++, x => exit2++);
            sm.AddTransition (s1, s2, () => true);
            sm.AddTransition (s2, s1, () => true);

            sm.Process ();
            Assert.AreEqual (1, exit1);
            Assert.AreEqual (1, enter2);

            sm.Process ();
            Assert.AreEqual (1, exit2);
            Assert.AreEqual (1, enter1);
        }

    }
    
}
