using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.RTSSample {
    public class MyEnemyAI: Component {
        MyTank tank;

        public MyEnemyAI (MyTank tank) {
            this.tank = tank;
        }

        public override void OnUpdate (long msec) {
            Node.Translate (0, 1, 0);
        }


    }
}
