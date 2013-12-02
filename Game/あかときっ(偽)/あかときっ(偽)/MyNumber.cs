using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
    public class MyNumber : Component {
        public enum Type {
            Yellow,
            Green,
         }

        int value;
        int order;

        public MyNumber (int order) {
            this.value =28;
            this.order = order;
        }

        public int Value {
            get { return value; }
            set { this.value = value; }
        }

        public int Order {
            get { return order; }
            set { this.order = value; }
        }

        public static Node Create (Vector3 pos, int order, Type type) {

            var cmp = new MyNumber (order);

            var spr = new Sprite[order];
            for (var i = 0; i < order; i++) {
                spr[i] = new Sprite (11, 15);
                spr[i].SetOffset (-11*i, 0);
                switch (type) {
                    case Type.Yellow: spr[i].AddTexture (Resource.GetTexture ("共通/Numbers(Yellow).png")); break;
                    case Type.Green: spr[i].AddTexture (Resource.GetTexture ("共通/Numbers(Green).png")); break;
                    default: spr[i].AddTexture (Resource.GetTexture ("共通/Numbers(Green).png")); break;
                }
            }

            var node = new Node ("MyNumber");
            node.Attach (cmp);
            for (var i = 0; i < order; i++) {
                node.Attach (spr[i]);
            }

            node.Translation = pos;

            return node;
        }


        public override void OnUpdate (long msec) {
            
            var value = Math.Min((int)Math.Pow(10,order)-1, Value);
            Console.WriteLine ("Value = " + value);

            var i = 0;
            while (value > 0) { 
                var spr = GetComponent<Sprite> (i);
                spr.SetTextureOffset (11 * (value % 10 + 1), 0);
                value /= 10;
                i += 1;
            }

            this.value += 1;
        }
    }
}
