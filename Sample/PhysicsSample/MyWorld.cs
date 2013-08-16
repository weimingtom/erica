﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.PhysicsSample
{
	public class MyWorld : Component {
        #region Field
        Node picked;
        Vector2 delta;
        #endregion

        public override void OnAttached () {
            this.picked = null;
            this.delta = new Vector2 (0, 0);
        }

        public static World Create () {
            var spr = new Sprite (new Texture ("media/DarkGalaxy.jpg"));
            var cmp = new MyWorld ();

            var wld = new World ("First Script");
            wld.Attach (spr);
            wld.Attach (cmp);
            wld.DrawPriority = 127;

            return wld;
        }

        public override void OnUpdate (long msec) {

            var g2d = Graphics2D.GetInstance ();
            var pos = g2d.GetMousePosition ();

            if (Input.GetKeyDown (KeyCode.Mouse0)) {
                var node = Graphics2D.Pick (World, pos.X, pos.Y).FirstOrDefault ();
                if (node != null) {
                    this.picked = node;
                    this.delta = pos - new Vector2 (node.Position.X, node.Position.Y);
                }
            }
            if (Input.GetKeyUp (KeyCode.Mouse0)) {
                this.picked = null;
            }

            if (picked != null) {
                var t = pos - delta;
                picked.Translation = new Vector3 (t.X, t.Y, 0);
            }

            base.OnUpdate (msec);
        }
    }
}
