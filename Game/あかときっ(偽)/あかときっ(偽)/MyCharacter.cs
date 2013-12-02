using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DD;

namespace あかときっ_偽_ {

    /// <summary>
    /// キャラクター コンポーネント
    /// </summary>
    /// <remarks>
    /// キャラクター1体を表すコンポーネントです。
    /// データベースと連携しキャラクターの状態を管理します。
    /// </remarks>
    public class MyCharacter : Component {

        #region Field
        string name;
        Data.AkatokiEntities db;
        Data.CharacterStatus status;
        #endregion

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="uniqueName">キャラクターのユニーク名（例：mayuto）</param>
        public MyCharacter (string uniqueName) {
            var db = new Data.AkatokiEntities ();
            if (db.Characters.First (x => x.UniqueName == uniqueName) == null) {
                throw new ArgumentException ("Name is not found");
            }

            this.name = uniqueName;
            this.db = new Data.AkatokiEntities ();
            this.status = db.CharacterStatuses.First (x => x.Character == uniqueName);
        }

        /// <summary>
        /// キャラクター ノードの作成
        /// </summary>
        /// <param name="uniqueName">ユニークなキャラクター名(例："Maki")</param>
        /// <returns></returns>
        public static Node Create (string uniqueName) {
            if (uniqueName == null) {
                throw new ArgumentNullException ("UniqueName is null");
            }

            var cmp = new MyCharacter (uniqueName);

            var mbox1 = new MailBox ("IncreasePartsLevel");
            var mbox2 = new MailBox ("DecreasePartsLevel");

            var node = new Node (uniqueName);
            node.Attach (cmp);
            node.Attach (mbox1);
            node.Attach (mbox2);

            return node;
        }

        /// <summary>
        /// キャラクターの表示名（例：爽夏）
        /// </summary>
        public string Name {
            get { return name; }
        }

        /// <summary>
        /// キャラクターのHP（現在値）
        /// </summary>
        public float Hp {
            get { return status.HP; }
        }

        /// <summary>
        /// キャラクターのHP（最大値）
        /// </summary>
        public float MaxHp {
            get { return status.MaxHP; }
        }

        /// <summary>
        /// キャラクターのMP（現在値）
        /// </summary>
        public float Mp {
            get { return status.MP; }
        }

        /// <summary>
        /// キャラクターのMP（最大値）
        /// </summary>
        public float MaxMp {
            get { return status.MaxMP; }
        }

        /// <summary>
        /// キャラクターの防御力
        /// </summary>
        public float Attack {
            get { return status.Attack; }
        }

        /// <summary>
        /// キャラクターのシールド値
        /// </summary>
        public float Shield {
            get { return status.Shield; }
        }

        /// <summary>
        /// キャラクターの攻撃速度
        /// </summary>
        public float Speed {
            get { return status.Speed; }
        }

        /// <summary>
        /// キャラクターのテクニック値
        /// </summary>
        public float Tech {
            get { return status.Tech; }
        }

        /// <summary>
        /// 上着の破損レベル
        /// </summary>
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

        /// <summary>
        /// スカートの破損レベル
        /// </summary>
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

        /// <summary>
        /// 肌着の破損レベル
        /// </summary>
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

        /// <summary>
        /// ブラの破損レベル
        /// </summary>
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

        /// <summary>
        /// パンツの破損レベル
        /// </summary>
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

        #region Method
        /// <summary>
        /// 衣服の破損レベルの取得
        /// </summary>
        /// <remarks>
        /// 指定する衣服の名前は日本語です。
        /// 例：上着
        /// </remarks>
        /// <param name="partsName">部位の名前</param>
        /// <returns></returns>
        public int GetPartsLevel (string partsName) {
            switch (partsName) {
                case "上着": return CoatLevel;
                case "スカート": return SkirtLevel;
                case "インナー": return InnerLevel;
                case "上腕": return 4;
                case "下腕": return 4;
                case "ブラ": return BraLevel;
                case "パンツ": return PantsLevel;
                case "標準待機": return 0;
                default: throw new InvalidOperationException ("Unknwon parts name, " + partsName);
            }
        }

        /// <summary>
        /// テクスチャーの取得
        /// </summary>
        /// <remarks>
        /// セクション、カテゴリー、部位名、ポーズを指定してテクスチャーを取得します。
        /// 今だけ全員で真姫のテクスチャーを使います
        /// </remarks>
        /// <param name="section">セクション名</param>
        /// <param name="category">カテゴリー名</param>
        /// <param name="parts">パーツ名</param>
        /// <param name="pose">ポーズ名</param>
        /// <returns></returns>
        public Texture GetTexture (string section, string category, string parts, string pose) {
            var name = (from tex in db.Textures
                        where tex.Section == section
                        where tex.Category == category
                        where tex.Character == "Maki"   // this.name   今だけ強制的に真姫
                        where tex.Parts == parts
                        where tex.Pose == pose || pose == null
                        select tex.FileName).FirstOrDefault ();
            return Resource.GetTexture (name);
        }


        /// <summary>
        /// テクスチャー(部位パーツ)の取得
        /// </summary>
        /// <remarks>
        /// キャラクターの部位破損レベルを考慮します。
        /// </remarks>
        /// <param name="section"></param>
        /// <param name="category"></param>
        /// <param name="pose"></param>
        /// <param name="parts"></param>
        /// <returns></returns>
        /*
        public Texture GetPartsTexture (string section, string category, string parts, string pose) {



            // 現在のキャラクターを表示するのに使用される装飾テクスチャー
            // （内部のパーツ破壊度を考慮します）
            var partsLevel = GetPartsLevel (parts);

            var name = (from tex in db.Textures
                        where tex.Character == "Maki"  // this.name 今だけ強制的に真姫
                        where tex.Section == section
                        where tex.Category == category
                        where tex.Parts == parts
                        where tex.Pose == pose
                        where tex.Level == partsLevel
                        select tex.FileName).FirstOrDefault ();
            return Resource.GetTexture (name);
        }
        */

        /// <summary>
        /// 指定のパーツの部位レベルの上昇
        /// </summary>
        /// <param name="partsName"></param>
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

        /// <summary>
        /// 指定のパーツの部位レベルの下降
        /// </summary>
        /// <param name="partsName"></param>
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

        /// <summary>
        /// メールボックス
        /// </summary>
        /// <param name="from"></param>
        /// <param name="address"></param>
        /// <param name="letter"></param>
        public override void OnMailBox (Node from, string address, object letter) {
            switch (address) {
                case "IncreasePartsLevel": IncreasePartsLevel ((string)letter); break;
                case "DecreasePartsLevel": DecreasePartsLevel ((string)letter); break;
            }
        }
        #endregion


    }
}
