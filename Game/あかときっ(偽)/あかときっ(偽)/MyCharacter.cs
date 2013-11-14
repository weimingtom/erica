using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {
    public class MyCharacter : Component {
        string name;
        Data.AkatokiEntities db;
        Data.CharacterStatus status;

        public MyCharacter (string uniqueName) {
            var db = new Data.AkatokiEntities ();
            if (db.Characters.First (x => x.UniqueName == uniqueName) == null) {
                throw new ArgumentException ("Name is not found");
            }

            this.name = uniqueName;
            this.db = new Data.AkatokiEntities ();
            this.status = db.CharacterStatuses.First (x => x.Character == uniqueName);
        }

        public static Node Create (string uniqueName) {
            var cmp = new MyCharacter (uniqueName);

            var mbox1 = new MailBox ("IncreasePartsLevel");
            var mbox2 = new MailBox ("DecreasePartsLevel");

            var node = new Node (uniqueName);
            node.Attach (cmp);
            node.Attach (mbox1);
            node.Attach (mbox2);

            return node;
        }

        public float Attack {
            get { return status.Attack; }
        }

        public float Shield {
            get { return status.Shield; }
        }

        public float Speed {
            get { return status.Speed; }
        }

        public float Tech {
            get { return status.Tech; }
        }

        public int CoatLevel {
            get { return status.CoatLevel; }
            private set {
                if (value < 0) {
                    value = 0;
                }
                if (value > 4) {
                    value = 4;
                }
                status.CoatLevel = value;
            }
        }

        public int SkirtLevel {
            get { return status.SkirtLevel; }
            private set {
                if (value < 0) {
                    value = 0;
                }
                if (value > 4) {
                    value = 4;
                }
                status.SkirtLevel = value;
            }
        }

        public int InnerLevel {
            get { return status.InnerLevel; }
            private set {
                if (value < 0) {
                    value = 0;
                }
                if (value > 4) {
                    value = 4;
                }
                status.InnerLevel = value;
            }
        }

        public int BraLevel {
            get { return status.BraLevel; }
            private set {
                if (value < 0) {
                    value = 0;
                }
                if (value > 4) {
                    value = 4;
                }
                status.BraLevel = value;
            }
        }

        public int PantsLevel {
            get { return status.PantsLevel; }
            private set {
                if (value < 0) {
                    value = 0;
                }
                if (value > 4) {
                    value = 4;
                }
                status.PantsLevel = value;
            }
        }


        public int GetPartsLevel (string partsName) {
            switch (partsName) {
                case "上着": return CoatLevel;
                case "スカート": return SkirtLevel;
                case "インナー": return InnerLevel;
                case "上腕": return 4;
                case "下腕": return 4;
                case "ブラ": return BraLevel;
                case "パンツ": return PantsLevel;
                default: throw new InvalidOperationException ("Unknwon parts name");
            }
        }

        /// <summary>
        /// 基本テクスチャーの取得
        /// </summary>
        /// <param name="section">セクション（戦闘、会話など）</param>
        /// <param name="category">カテゴリー（メイン、スロットなど）</param>
        /// <param name="pose">ポーズ（標準待機、被弾など）</param>
        /// <returns></returns>
        public Texture GetTexture (string section, string category, string pose) {
            // 現在のキャラクターを表示するのに使用される基本テクスチャー
            var name = (from tex in db.Textures
                        where tex.Section == section
                        where tex.Category == category
                        where tex.Pose == pose
                        where tex.Parts == "基本絵"
                        select tex.FileName).FirstOrDefault ();
            return Resource.GetTexture (name);
        }

        /// <summary>
        /// テクスチャー(部位パーツ)の取得
        /// </summary>
        /// <param name="section"></param>
        /// <param name="category"></param>
        /// <param name="pose"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        public Texture GetPartsTexture (string section, string category, string pose, string parts) {
            // 現在のキャラクターを表示するのに使用される装飾テクスチャー
            // （内部のパーツ破壊度を考慮します）
            var partsLevel = GetPartsLevel (parts);

            var name = (from tex in db.Textures
                        where tex.Section == section
                        where tex.Category == category
                        where tex.Pose == pose
                        where tex.Parts == parts
                        where tex.Level == partsLevel
                        select tex.FileName).FirstOrDefault ();
            return Resource.GetTexture (name);
        }

        void IncreasePartsLevel (string partsName) {
            switch (partsName) {
                case "CoatLevel": CoatLevel = Math.Min (4, CoatLevel + 1); break;
                case "SkirtLevel": SkirtLevel = Math.Min (4, SkirtLevel + 1); break;
                case "InnerLevel": InnerLevel = Math.Min (4, InnerLevel + 1); break;
                case "BraLevel": BraLevel = Math.Min (4, BraLevel + 1); break;
                case "PantsLevel": PantsLevel = Math.Min (4, PantsLevel + 1); break;
                default: throw new InvalidOperationException ("Unknwon parts name");
            }
        }

        void DecreasePartsLevel (string partsName) {
            switch (partsName) {
                case "CoatLevel": CoatLevel = Math.Max (0, CoatLevel - 1); break;
                case "SkirtLevel": SkirtLevel = Math.Max (0, SkirtLevel - 1); break;
                case "InnerLevel": InnerLevel = Math.Max (0, InnerLevel - 1); break;
                case "BraLevel": BraLevel = Math.Max (0, BraLevel - 1); break;
                case "PantsLevel": PantsLevel = Math.Max (0, PantsLevel - 1); break;
                default: throw new InvalidOperationException ("Unknwon parts name");
            }
        }

        public override void OnMailBox (Node from, string address, object letter) {
            switch (address) {
                case "IncreasePartsLevel": IncreasePartsLevel ((string)letter); break;
                case "DecreasePartsLevel": DecreasePartsLevel ((string)letter); break;
            }
        }

    }
}
