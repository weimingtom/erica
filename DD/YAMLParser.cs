using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Yaml;
using System.Yaml.Serialization;

namespace DD {
    /// <summary>
    /// YAMLパーサー クラス
    /// </summary>
    /// <remarks>
    /// YAML形式のファイルをパースして該当するオブジェクトを返します。
    /// </remarks>
    public static class YAMLParser {
        static readonly YamlSerializer yaml = new YamlSerializer();

        /// <summary>
        /// <see cref="EventArgs"/> とその派生型のパース
        /// </summary>
        /// <remarks>
        /// YAML形式で記述されたオブジェクトのインスタンスを生成します。
        /// オブジェクトは引数なしのコンストラクターが必須です。
        /// また引き数で指定されたプロパティのsetterにアクセスできる必要があります。
        /// </remarks>
        /// <param name="ev">YAML形式の文字列</param>
        /// <returns>オブジェクトのインスタンス</returns>
        public static object Parse (string ev) {
            var objs = yaml.Deserialize (ev);
            if (objs == null) {
                throw new InvalidOperationException("Can't deserialize LineEvent. ev=" + ev);
            }
            
            return objs[0];
        }
    }
}
