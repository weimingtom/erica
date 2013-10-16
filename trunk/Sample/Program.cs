using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample {
    public class Program {
        static void Main (string[] args) {
            var select = 0;

            switch (select) {
                case 0: DD.Sample.SimpleSample.Program.Main (args); break;
                case 1: DD.Sample.MessageSample.Program.Main (args); break;
                case 2: DD.Sample.AnimationSample.Program.Main (args); break;
                case 3: DD.Sample.ScrollSample.Program.Main (args); break;
                case 4: DD.Sample.TiledMapSample.Program.Main (args); break;
                case 5: DD.Sample.CollisionDetectSample.Program.Main (args); break;
                case 6: DD.Sample.PlatformSample.Program.Main (args); break;
                case 7: DD.Sample.TiledMapSample.Program.Main (args); break;
                case 8: DD.Sample.IsometricSample.Program.Main (args); break;
                case 9: DD.Sample.PhysicsSample.Program.Main (args); break;
                case 10: DD.Sample.GUISample.Program.Main (args); break;
                case 11: DD.Sample.MouseSample.Program.Main (args); break;
                case 12: DD.Sample.KeyboardSample.Program.Main (args); break;
                case 13: DD.Sample.DebugToolsSample.DebugToolsSampleProgram.Main (args); break;
                default: throw new NotImplementedException ("Sorry");
            }
        }
    }
}
