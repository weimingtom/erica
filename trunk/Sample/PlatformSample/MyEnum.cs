using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.PlatformSample {
    [Flags]
    public enum MyGroup {
        All       = -1,
        Character = 1<<0,
        Platform     = 1<<1,
    }
}
