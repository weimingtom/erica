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
                default: throw new NotImplementedException ("Sorry");
            }
        }
    }
}
