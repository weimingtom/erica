using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.RTSSample {
    public class MyRader : Component {
        Node points;

        public MyRader () {
            this.points = null;
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyRader ();
            cmp.points = new Node ();
            cmp.points.DrawPriority = -2;

            // 背景
            var spr = new Sprite (120, 180);
            spr.Color = new Color (255, 255, 255, 128);
            
            var node = new Node ("Rader");
            node.AddChild (cmp.points);
            node.Attach (cmp);
            node.Attach (spr);
            node.DrawPriority = -2;

            node.Translation = pos;
            
            return node;
        }

        public override void OnUpdate (long msec) {
            var tanks = from node in World.Downwards
                        where node.Name == "MyTank" || node.Name == "EnemyTank"
                        select node;

            foreach (var cmp in points.Components) {
                points.Detach (cmp);
            }

            foreach (var tank in tanks) {
                var spr = new Sprite (2, 2);
                spr.Color = Color.Black;
                spr.Offset = new Vector2 (tank.Translation.X / 10f,
                                          tank.Translation.Y / 10f);
                
                points.Attach (spr);
            }
            var cam = World.ActiveCamera;
            var frame = new Square (80, 60, 2);
            frame.Offset = new Vector3 (cam.Position.X / 10f, 
                                       cam.Position.Y / 10f,
                                       cam.Position.Z / 10f);
            
            points.Attach (frame);
        }
    }
}
