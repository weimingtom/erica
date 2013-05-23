using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DD.UnitTest {
    public class MyEventArgs : EventArgs {
        public MyEventArgs ()
            : base () {
        }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
