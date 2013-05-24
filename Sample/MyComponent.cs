using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace Sample {
    public class MyEventArgs : EventArgs {
        public MyEventArgs ()
            : base () {
        }

        public int X {
            get;
            set;
        }
        public int Y {
            get;
            set;
        }
    }

    public class MyComponent : Component {
        Sprite spr;
        int index;
        long prev;

        public MyComponent (Sprite spr) {
            this.spr = spr;
            this.index = 0;
            this.prev = 0;
        }

        public override void OnUpdate (long msec) {
            if (msec > prev + 33) {
                var tex = spr.GetTexture (0) as TiledTexture;
                this.index = (index + 1) % tex.TileCount;
                tex.ActiveTile = index;
                prev = msec;
            }
        }

        /// <inheritdoc/>
        public override void OnMouseButtonPressed (MouseButton button, int x, int y) {
            if (button == MouseButton.Left) {
                var tex = spr.GetTexture (0) as TiledTexture;
                this.index = (index + 1) % tex.TileCount;
                tex.ActiveTile = index;
            }
        }


    }
}
