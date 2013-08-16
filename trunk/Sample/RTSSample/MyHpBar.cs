using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    /*
    public class MyHpBar : Component {
        Bar bar;

        public MyHpBar () {
        }

        public static Node Create (int maxValue) {
            var cmp = new MyHpBar ();

            var bar = new Bar (3, 30, BarOrientation.Horizontal);
            bar.MaxValue = maxValue;
            bar.CurrentValue = maxValue;
            bar.BackgroundColor = Color.Black;
            bar.ForegroundColor = Color.Green;

            var node = new Node ("HpBar");
            node.Attach (cmp);
            node.Attach (bar);

            node.SetTranslation (-15, 30, 0);

            cmp.bar = bar;

            return node;
        }

        public override void OnPreDraw (object window) {
            // ワールド固定
            Node.SetGlobalRotation (Quaternion.Identity);
        }

    }
     * */
}
