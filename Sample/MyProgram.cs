using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample {
    class MyProgram {
        static void Main2 (string[] args) {

            var th = Math.Acos (-1);
            Console.WriteLine ("th = " + th);

            var q1 = new Quaternion (0, 0, 0, 1);
            var q2 = new Quaternion (180, 0, 0, 1);
            var q3 = new Quaternion (360, 0, 0, 1);

            var q = Quaternion.Slerp (0.99f, q2, q3);

            Console.WriteLine ("q = " + q + ", Angle = " + q.Angle + ", Axis = " + q.Axis);
       
        }
    }
}
