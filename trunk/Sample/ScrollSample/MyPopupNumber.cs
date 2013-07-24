using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.ScrollSample {
    public class MyPopupNumber : Component {

        public Label Label { get; set; }
        public static int Count { get; set; }
        public int MyCount { get; set; }
        public long BirthTime { get; set; }
        public long LifeTime { get; set; }

        public MyPopupNumber () {
            this.LifeTime = 3000;
            this.MyCount = Count;
            MyPopupNumber.Count += 1;
        }

        public static Node Create () {
            var cmp = new MyPopupNumber ();
            var label = new Label ("こんにちは、世界");

            var node = new Node ();
            node.Attach (cmp);
            node.Attach (label);

            return node;
        }

        public override void OnUpdateInit (long msec) {
            this.BirthTime = msec;  
        }

        public override void OnDestroyed () {
            Console.WriteLine ("Destroyed " + MyCount);
        }


        public override void OnUpdate (long msec) {
            if (msec > BirthTime + LifeTime) {
                Destroy (this);
            }
        }

        public override void OnDraw (object window, EventArgs args) {

            Console.WriteLine("T = " + Node.Point );

        }
    }
}
