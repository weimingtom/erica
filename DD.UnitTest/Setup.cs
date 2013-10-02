using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SFML.Window;
using SFML.Graphics;
using DD.Physics;

namespace DD.UnitTest {
    [TestClass]
    public class TestSetup {
        static Graphics2D g2d;

        [AssemblyInitialize ()]
        public static void AssemblyInitialize (TestContext context)
        {
            var g2d = Graphics2D.GetInstance ();
            g2d.CreateWindow (800, 600, "UnitTest");

            // 標準の PPM=64 ではテストしにくいため
            // ユニットテストでは常に PPM=1 を使用する
            PhysicsSimulator.PPM = 1;
        }
    
        [AssemblyCleanup ()]
        public static void AssemblyCleanup () {
            if (g2d != null) {
                g2d.Dispose ();
                g2d = null;
            }
        }
    }
}

