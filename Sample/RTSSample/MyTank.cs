using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD.Physics;

namespace DD.Sample.RTSSample {
    public class MyTank : Component {

        string tankName;
        string weaponName;
        Texture turretTexture;
        Texture bodyTexture;
        Texture tankTexture;
        float forwardSpeed;
        float backSpeed;
        float angularSpeed;
        float effectiveRange;
        int maxHp;
        int hp;
        bool selected;
        Node mychar;

        public Node MyCharacter {
            get { return mychar; }
            set { this.mychar = value; }
        }

        public string TankName { get { return tankName; } }
        public string WeaponName { get { return weaponName; } }

        public float ForwardSpeed { get { return forwardSpeed; } }
        public float BackSpeed { get { return backSpeed; } }
        public float AngularSpeed { get { return angularSpeed; } }
        public float EffectiveRange { get { return effectiveRange; } }
        public Texture TulletTexture { get { return turretTexture; } }
        public Texture BodyTexture { get { return bodyTexture; } }
        public Texture TankTexture { get { return tankTexture; } }
        public bool Selected { get { return selected; } set { this.selected = value; } }

        public int MaxHp { get { return maxHp; } }
        public int Hp { get { return hp; } }

        public MyTank (string tankName) {
            this.forwardSpeed = 5;
            this.backSpeed = 5;
            this.angularSpeed = 4;
            this.effectiveRange = 200;
            this.maxHp = 100;
            this.hp = 100;
            this.tankName = tankName;
            this.turretTexture = Resource.GetDefaultTexture ();
            this.bodyTexture = Resource.GetDefaultTexture ();
            this.tankTexture = Resource.GetDefaultTexture ();
            switch (tankName) {
                case "T-34": {
                        this.tankTexture = new Texture ("media/Tank-T34.png");
                        this.bodyTexture = new Texture ("media/Tank-T34(Body).png");
                        this.turretTexture = new Texture ("media/Tank-T34(Turret).png");
                        this.weaponName = "";
                        break;
                    }
                case "PanzerIV": {
                        this.tankTexture = new Texture ("media/Tank-PanzerIV.png");
                        this.bodyTexture = new Texture ("media/Tank-PanzerIV(Body).png");
                        this.turretTexture = new Texture ("media/Tank-PanzerIV(Turret).png");
                        this.weaponName = "";
                        break;
                    }
                case "Panther": {
                        this.tankTexture = new Texture ("media/Tank-Panther.png");
                        this.bodyTexture = new Texture ("media/Tank-Panther(Body).png");
                        this.turretTexture = new Texture ("media/Tank-Panther(Turret).png");
                        this.weaponName = "";
                        break;
                    }
                case "Hotchkiss": {
                        this.tankTexture = new Texture ("media/Tank-Hotchkiss.png");
                        this.bodyTexture = new Texture ("media/Tank-Hotchkiss(Body).png");
                        this.turretTexture = new Texture ("media/Tank-Hotchkiss(Turret).png");
                        this.weaponName = "";
                        break;
                    }
                default: throw new NotImplementedException ("Unknown tank name");
            }
            this.mychar = null;
        }

        public static Node CreateFriend (string tankName, Vector3 pos, Quaternion rot) {

            var node = Create ("MyTank", tankName, pos, rot);
            var cmp = new MyController (node.GetComponent<MyTank> ());
            node.Attach (cmp);

            return node;
        }

        public static Node CreateEnemy (string tankName, Vector3 pos, Quaternion rot) {

            var node = Create ("EnemyTank", tankName, pos, rot);
            var cmp = new MyEnemyAI (node.GetComponent<MyTank> ());
            node.Attach (cmp);

            return node;
        }

        private static Node Create (string nodeName, string tankName, Vector3 pos, Quaternion rot) {

            // タンク
            var cmp = new MyTank (tankName);

            // 車両(Body)
            var spr = new Sprite (40, 64);
            spr.SetOffset (-20, -34);
            spr.AddTexture (cmp.BodyTexture);
            spr.AddTexture (Resource.GetDefaultTexture ());

            // コリジョン他
            var col = new BoxCollisionShape (20, 32, 0);

            var node = new Node (nodeName);
            node.Attach (cmp);
            node.Attach (spr);
            node.Attach (col);

            // 砲台(Turret)
            var tullet = MyTullet.Create ();
            tullet.GetComponent<Sprite> ().AddTexture (cmp.turretTexture);
            node.AddChild (tullet);

            node.DrawPriority = -2;
            node.Translation = pos;
            node.Rotation = rot;

            return node;
        }

        public override void OnDestroyed () {
            var exp = MyBigExplosion.Create (Node.Position);
            World.AddChild (exp);
        }

        public void Damage (int value, Node sender) {
            this.hp = MyMath.Clamp (hp - value, 0, maxHp);
            if (hp == 0) {
                var mychar = sender.GetComponent<MyCharacter> ();
                mychar.Say (BalloonMessage.Destroy);
                Destroy (this);
            }
        }


        public override void OnPreDraw (object window) {
            var spr = GetComponent<Sprite> ();
            spr.Color = selected ? Color.Red : Color.White;
        }

    }
}
