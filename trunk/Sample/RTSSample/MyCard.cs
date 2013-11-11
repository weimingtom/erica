using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.RTSSample {
    public class MyCard : Component{
        Node tank;
        Node character;

        public MyCard () {
        }

        public static Node Create (int slot, Node charNode, Node tankNode) {
            var cha = charNode.GetComponent<MyCharacter> ();
            var tank = tankNode.GetComponent<MyTank> ();
            var cmp = new MyCard ();

            cmp.tank = tankNode;
            cmp.character = charNode;

            // 背景
            var spr1 = new Sprite (256, 128);
            spr1.Color = Color.Gray;

            // 顔
            var spr2 = new Sprite (128, 128);
            spr2.AddTexture (cha.FaceTexture);

            // 戦車
            var spr3 = new Sprite (32, 64);
            spr3.AddTexture (tank.TankTexture);
            spr3.SetOffset (128 + 6, 6);

            // 戦車名
            var label1 = new Label ();
            label1.Text = tank.TankName;
            label1.SetOffset (128 + 6 + 32 + 6, 6);

            // HP
            var hp = new Bar (4, 96, BarOrientation.Horizontal);
            hp.MaxValue = tank.MaxHp;
            hp.CurrentValue = tank.Hp;
            hp.SetOffset (128 + 6, 64 + 6);

            // クリック
            var col = new BoxCollision (128, 64, 0);
            col.SetOffset (128, 64, 0);

            var node = new Node ("Card");
            node.Attach (cmp);
            node.Attach (spr1);
            node.Attach (spr2);
            node.Attach (spr3);
            node.Attach (label1);
            node.Attach (hp);
            node.Attach (col);

            node.DrawPriority = -10;

            node.Translate (12+(256+4)*slot, 600, 0);

            // キャラクター -> マイカード
            cha.MyCard = node;

            return node;
        }

        public override void OnMouseButtonPressed (MouseButton button, float x, float y) {
            var t = tank.GetComponent<MyTank> ();
            t.Selected = !t.Selected;
        }

    }
}
