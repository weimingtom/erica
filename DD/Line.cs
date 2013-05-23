using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DD {
    /// <summary>
    /// ライン クラス
    /// </summary>
    /// <remarks>
    /// ラインは日本語で言う台詞（セリフ）の事です。
    /// ラインは発言者の名前と2～3行のテキスト、それに（あるなら）音声ファイルと
    /// 関連するイベントを1つにまとめた物です。
    /// ラインは通常はテキストファイルからまとめて一気に読み込まれます。
    /// </remarks>
    public class Line {

        #region Field
        string actor;
        string words;
        string sound;
        string evnt;
        #endregion

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="actor">発言者の名前</param>
        /// <param name="words">発言テキスト</param>
        /// <param name="sound">音声ファイル名</param>
        /// <param name="evnt">関連イベント</param>
        public Line (string actor, string words, string sound, string evnt) {
            if (actor == null || actor == "") {
                throw new ArgumentNullException ("Actor is null or empty");
            }
            if (words == null || words == "") {
                throw new ArgumentNullException ("Words is null or empty");
            }
            this.actor = actor;
            this.words = words;
            this.sound = sound;
            this.evnt = evnt;
        }

        /// <summary>
        /// 発言者の名前
        /// </summary>
        public string Actor {
            get { return actor; }
        }

        /// <summary>
        /// 発言テキスト
        /// </summary>
        public string Words  {
            get { return words; }
        }

        /// <summary>
        /// 音声ファイル名
        /// </summary>
        public string Sound  {
            get { return sound; }
        }

        /// <summary>
        /// 関連するイベント
        /// </summary>
        /// <remarks>
        /// YAML形式で書かれた <see cref="EventArgs"/> の派生型オブジェクト。
        /// </remarks>
        public string Event  {
            get { return evnt; }
        }

        /// <summary>
        /// ライン ファイルの読み込み
        /// </summary>
        /// <param name="name">ライン ファイル名</param>
        /// <returns>ラインの配列</returns>
        public static Line[] Load (string name) {
            if (name == null || name == "") {
                throw new ArgumentNullException ("Name is null or empty");
            }
            using (var sr = new StreamReader (name)) {
                return LineParser.Parse (sr.ReadToEnd ());
            }
        }
    }
}
