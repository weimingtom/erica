using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.MessageSample {
   public  class MySender: Component {
       long interval;
       long prev;
       int count;
       string address;

       public MySender () {
           this.interval = 1000;
           this.prev = 0;
           this.count = 0;
           this.address = "";
       }

       public string Address {
           get { return address; }
           set { this.address = value; }
       }

       public static Node Create (string address, Vector3 pos) {
           var cmp = new MySender ();
           cmp.address = address;

           var spr = new Sprite (64, 64);
           spr.AddTexture (Resource.GetDefaultTexture ());
           spr.Color = Color.Purple;

           var label1 = new Label ();
           label1.Text = "Sender";
           label1.SetOffset (8, 16);

           var label2 = new Label ();
           label2.SetOffset (0, -24);
           label2.Text = "None";

           var node = new Node ("Sender");
           node.Attach (cmp);
           node.Attach (spr);
           node.Attach (label1);
           node.Attach (label2);

           node.Translation = pos;

           return node;
       }

       public override void OnUpdateInit (long msec) {
           this.prev = msec;    
       }

       public override void OnUpdate (long msec) {
           if (msec - prev > interval) {
               Notify ();
               this.prev = msec;
           }
       }

       void Notify () {
           var letter = count.ToString ();
            Console.WriteLine ("Send to {0} : {1}", address, letter);
           
           GetComponent<Label> (1).Text = letter;
           SendMessage (address, letter);
    
           this.count += 1;
       }
    }
}
