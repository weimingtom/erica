using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    public class Logger : Component {

        const int MaxLogLine = 100;

        Queue<Log> logs;

        public Logger () {
            this.logs = new Queue<Log> ();
        }

        public int LogCount {
            get { return logs.Count (); }
        }

        public IEnumerable<Log> Logs {
            get { return logs; }
        }

        public void Write (Node node, int priority, string message) {
            var name = (node == null) ? "Null" : node.Name;
            this.logs.Enqueue (new Log (name, priority, message));
            if (logs.Count () > MaxLogLine) {
                logs.Dequeue ();
            }
        }

        public void Clear () {
            this.logs.Clear ();
        }
    }
}
