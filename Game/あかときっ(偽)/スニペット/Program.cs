using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace スニペット {
    class Program {
        static void Main (string[] args) {
            int select = 0;

            switch (select) {
                case 0: FocusAnimation.Program.Main (args); break;
                default: throw new NotImplementedException ("Sorry");
            }
        }
    }
}
