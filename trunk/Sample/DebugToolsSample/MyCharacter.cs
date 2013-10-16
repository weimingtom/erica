using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD.Sample.DebugToolsSample {
    public class MyCharacter : Component {

        string name;
        string clas;
        int level;
        int hp;
        float attack;
        float armor;
        float dex;
        float luck;
        int exp;
        Texture standingTexture;
        Texture faceTexture;
        string greeting;

        public MyCharacter () {
        }

        public string Name {
            get { return name; }
        }

        public string Class {
            get { return clas; }
        }

        public int Hp {
            get { return hp; }
        }

        public int Exp {
            get { return exp; }
        }

        public int Level {
            get { return level; }
        }

        public float Attack {
            get { return attack; }
        }

        public float Armor {
            get { return armor; }
        }

        public float Dexterity {
            get { return dex; }
        }

        public float Luck {
            get { return luck; }
        }

        public Texture StandingTexture {
            get { return standingTexture; }
        }

        public Texture FaceTexture {
            get { return faceTexture; }
        }

        public string Greeting {
            get { return greeting; }
        }

        public static Node Create (string name) {
            var cmp = new MyCharacter ();

            switch (name) {
                case "A子": {
                        cmp.name = name;
                        cmp.clas = "村人";
                        cmp.hp = 11;
                        cmp.level = 1;
                        cmp.attack = 7;
                        cmp.armor = 2;
                        cmp.dex = 9;
                        cmp.luck = 6;
                        cmp.exp = 0;
                        cmp.standingTexture = new Texture ("media/Character-Ako.png");
                        cmp.faceTexture = new Texture ("media/Face-Ako.png");
                        cmp.greeting = @"ここにはA子のキャラクター紹介文が入ります。";
                        break;
                    }
                case "B子": {
                        cmp.name = name;
                        cmp.clas = "村人";
                        cmp.hp = 8;
                        cmp.level = 2;
                        cmp.attack = 13;
                        cmp.armor = 4;
                        cmp.dex = 5;
                        cmp.luck = 4;
                        cmp.exp = 0;
                        cmp.standingTexture = new Texture ("media/Character-Bko.png");
                        cmp.faceTexture = new Texture ("media/Face-Bko.png");
                        cmp.greeting = @"ここにはB子のキャラクター紹介文が入ります。";
                        break;
                    }
                case "C子": {
                        cmp.name = name;
                        cmp.clas = "村人";
                        cmp.hp = 7;
                        cmp.level = 1;
                        cmp.attack = 5;
                        cmp.armor = 6;
                        cmp.dex = 7;
                        cmp.luck = 6;
                        cmp.exp = 0;
                        cmp.standingTexture = new Texture ("media/Character-Cko.png");
                        cmp.faceTexture = new Texture ("media/Face-Cko.png");
                        cmp.greeting = @"ここにはC子のキャラクター紹介文が入ります。";
                        break;
                    }
                default: throw new NotImplementedException ("Unknown name");
            }

            var node = new Node (name);
            node.Attach (cmp);

            return node;
        }

    }
}
