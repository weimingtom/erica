using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    public class TiledMapObjectGroup : Component {
        #region Field
        List<Node> objs;
        #endregion

        #region Constructor
        public TiledMapObjectGroup () {
            this.objs = new List<Node> ();
        }
        #endregion

        #region Property
        public int ObjectCount {
            get { return objs.Count (); }
        }

        public IEnumerable<Node> Objects {
            get { return objs; }
        }
        #endregion


        #region Method
        public Node GetObject (int index) {
            return objs[index];
        }

        public void AddObject (Node node) {
            objs.Add(node);
        }

        public bool RemoveObject (Node node) {
            return objs.Remove (node);
        }
        #endregion

    }
}
