using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyStatusPanel : Component {

        Label name;
        Label level;
        Bar hp;
        Bar attack;
        Bar armor;
        Bar dex;
        Bar luck;
        Bar exp;
        Label hpValue;
        Label attackValue;
        Label armorValue;
        Label dexValue;
        Label luckValue;
        Label expValue;

        public MyStatusPanel () {
        }

        public Label Name {
            get { return name; }
            private set { this.name = value; }
        }

        public Label Level {
            get { return level; }
            private set { this.level = value; }
        }

        public Bar Hp {
            get { return hp; }
            private set { this.hp = value; }
        }

        public Bar Attack {
            get { return attack; }
            private set { this.attack = value; }
        }

        public Bar Armor {
            get { return armor; }
            private set { this.armor = value; }
        }

        public Bar Dexterity {
            get { return dex; }
            private set { this.dex = value; }
        }

        public Bar Luck {
            get { return luck; }
            private set { this.luck = value; }
        }

        public Bar Exp {
            get { return exp; }
            private set { this.exp = value; }
        }

        public Label HpValue {
            get { return hpValue; }
            private set { this.hpValue = value; }
        }

        public Label AttackValue {
            get { return attackValue; }
            private set { this.attackValue = value; }
        }

        public Label ArmorValue {
            get { return armorValue; }
            private set { this.armorValue = value; }
        }

        public Label DexterityValue {
            get { return dexValue; }
            private set { this.dexValue = value; }
        }

        public Label LuckValue {
            get { return luckValue; }
            private set { this.luckValue = value; }
        }

        public Label ExpValue {
            get { return expValue; }
            private set { this.expValue = value; }
        }

        public static Node Create (Vector3 pos) {
            var cmp = new MyStatusPanel ();

            var spr = new Sprite (600, 200);
            spr.AddTexture (Resource.GetDefaultTexture ());
            spr.Color = Color.Gray;

            var mbox = new MailBox ("CharacterChanged");


            var nameLabel = new Label ("Name");
            var levelLabel = new Label ("Lv");
            var hpLabel = new Label ("HP");
            var attackLabel = new Label ("Ata");
            var armorLabel = new Label ("Arm");
            var dexLabel = new Label ("Dex");
            var luckLabel = new Label ("Luck");
            var expLabel = new Label ("Exp");

            var x = 410;
            var y = 10;
            nameLabel.SetOffset (x, y + 0);
            levelLabel.SetOffset (spr.Width - 50, y + 0);
            hpLabel.SetOffset (x, y + 20);
            attackLabel.SetOffset (x, y + 40);
            armorLabel.SetOffset (x, y + 60);
            dexLabel.SetOffset (x, y + 80);
            luckLabel.SetOffset (x, y + 100);
            expLabel.SetOffset (x, y + 120);

            cmp.Name = new Label ("デフォルト");
            cmp.Level = new Label ("-1");
            cmp.Hp = new Bar (16, 120, BarOrientation.Horizontal, 20);
            cmp.Attack = new Bar (16, 120, BarOrientation.Horizontal, 20);
            cmp.Armor = new Bar (16, 120, BarOrientation.Horizontal, 20);
            cmp.Dexterity = new Bar (16, 120, BarOrientation.Horizontal, 20);
            cmp.Luck = new Bar (16, 120, BarOrientation.Horizontal, 20);
            cmp.Exp = new Bar (16, 120, BarOrientation.Horizontal, 100);

            cmp.hpValue = new Label ("100");
            cmp.attackValue = new Label ("100");
            cmp.armorValue = new Label ("100");
            cmp.dexValue = new Label ("100");
            cmp.luckValue = new Label ("100");
            cmp.expValue = new Label ("100");


            x = 450;
            y = 10;
            cmp.Name.SetOffset (x, y + 0);
            cmp.Level.SetOffset (spr.Width - 30, y + 0);
            cmp.Hp.SetOffset (x, y + 20);
            cmp.Attack.SetOffset (x, y + 40);
            cmp.Armor.SetOffset (x, y + 60);
            cmp.Dexterity.SetOffset (x, y + 80);
            cmp.Luck.SetOffset (x, y + 100);
            cmp.Exp.SetOffset (x, y + 120);

            x = 572;
            y = 10;
            cmp.HpValue.SetOffset (x, y + 20);
            cmp.AttackValue.SetOffset (x, y + 40);
            cmp.ArmorValue.SetOffset (x, y + 60);
            cmp.DexterityValue.SetOffset (x, y + 80);
            cmp.LuckValue.SetOffset (x, y + 100);
            cmp.ExpValue.SetOffset (x, y + 120);


            var node = new Node ("StatusPanel");
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (mbox);

            node.Attach (nameLabel);
            node.Attach (levelLabel);
            node.Attach (hpLabel);
            node.Attach (armorLabel);
            node.Attach (attackLabel);
            node.Attach (dexLabel);
            node.Attach (luckLabel);
            node.Attach (expLabel);

            node.Attach (cmp.name);
            node.Attach (cmp.level);

            node.Attach (cmp.Hp);
            node.Attach (cmp.Armor);
            node.Attach (cmp.Attack);
            node.Attach (cmp.Dexterity);
            node.Attach (cmp.Luck);
            node.Attach (cmp.Exp);

            node.Attach (cmp.HpValue);
            node.Attach (cmp.ArmorValue);
            node.Attach (cmp.AttackValue);
            node.Attach (cmp.DexterityValue);
            node.Attach (cmp.LuckValue);
            node.Attach (cmp.ExpValue);

            node.AddChild (MyBodySprite.Create (new Vector3 (0, -100, 0)));

            node.Position = pos;


            return node;
        }

        public override void OnMailBox (Node from, string address, object letter) {

            switch (address) {
                case "CharacterChanged": {
                        var ch = ((Node)letter).GetComponent<MyCharacter> ();
                        this.name.Text = ch.Name;
                        this.level.Text = ch.Level.ToString ();

                        this.hp.CurrentValue = ch.Hp;
                        this.attack.CurrentValue = ch.Attack;
                        this.armor.CurrentValue = ch.Armor;
                        this.dex.CurrentValue = ch.Dexterity;
                        this.luck.CurrentValue = ch.Luck;
                        this.exp.CurrentValue = ch.Exp;

                        this.hpValue.Text = ch.Hp.ToString ();
                        this.attackValue.Text = ch.Attack.ToString ();
                        this.armorValue.Text = ch.Armor.ToString ();
                        this.dexValue.Text = ch.Dexterity.ToString ();
                        this.luckValue.Text = ch.Luck.ToString ();
                        this.expValue.Text = ch.Exp.ToString ();

                        Log (0, "Change status to " + ch.Name);
                        break;
                    }
            }
        }
    }
}
