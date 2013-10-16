using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    public struct Log {
        public Log (string node, int priority, string message)
            : this () {
                this.Node = node;
            this.Priority = priority;
            this.Message = message;
        }
        public string Node { get; private set; }
        public int Priority { get; private set; }
        public string Message { get; private set; }
    }

}
