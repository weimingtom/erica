using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TGUI;
using SFML.Graphics;

namespace DD {
   public class GUI :Component {
       TGUI.Gui data;

       public GUI () {
           var g2d = Graphics2D.GetInstance ();
           var win = g2d.GetWindow() as RenderWindow;

           this.data = new TGUI.Gui (win);
           data.GlobalFont = Resource.GetDefaultFont ();
       }

       public TGUI.Gui Data {
           get { return data; }
       }

       public Font Font { get; set; }

       public string Forcused { get; set; }

       public int WidgetCount { get; set; }
       public IEnumerable<Widget> Widgets { get; set; }
       public IEnumerable<string> WidgetNames { get; set; }


       public void FocusNext () {
       }

       public void FocusPrevious () {
       }

       public void FocusTo (string name) {
       }

       public void ForcusClear () {

       }

       public T Add<T> (T widget, string name = null) where T : Widget {
           return data.Add (widget, name ?? "");
       }

       public void Remove (string name) {

       }

       public void Clear () {

       }

       public T Get<T> (string name) where T : Widget {
           return data.Get<T> (name);
       }

       public Widget GetForcused () {
           return null;
       }

       public void MoveBack (string name) {
       }

       public void MoveFront (string name){
       }

       public override void OnDraw (object window) {
           data.Draw (true);
       }
    }
}
