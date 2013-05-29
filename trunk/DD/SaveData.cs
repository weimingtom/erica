using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Yaml.Serialization;
using System.Security.Cryptography;
using System.IO;

namespace DD {
    /// <summary>
    /// セーブデータ クラス
    /// </summary>
    /// <remarks>
    /// ユーザー定義のゲーム内パラメーターを保存・復帰するのに適したクラスです。
    /// 文字列をキーとして任意の構造体をファイルに保存可能です。
    /// （ただし public プロパティのみ）
    /// また暗号化が有効な時はセーブ ファイル全体がAES方式によって暗号化され、
    /// さらにSHA256によるハッシュ値のチェックが行われます。
    /// 非暗号時は人間に読みやすいYAML形式のテキストで出力されます。
    /// </remarks>
    public class SaveDataContainer {
        #region Field
        bool encryption;
        string password;
        string directory;
        Dictionary<string, object> items;
        AesCryptoServiceProvider aes;
        SHA256CryptoServiceProvider sha256;
        #endregion

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// </remarks>
        public SaveDataContainer () {
            this.directory = "./";
            this.encryption = false;
            this.password = "";
            this.items = new Dictionary<string, object> ();
            this.aes = new AesCryptoServiceProvider ();
            this.sha256 = new SHA256CryptoServiceProvider ();

            aes.BlockSize = 128;
            aes.KeySize = 128;
            aes.Mode = System.Security.Cryptography.CipherMode.CBC;
            aes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            this.Password = "Hello world";  // 初期パスワード
            this.aes.GenerateIV ();         // 初期ベクトル
        }


        /// <summary>
        /// 暗号化の有無
        /// </summary>
        /// <remarks>
        /// AES暗号を使用しファイル全体を暗号化します。
        /// 暗号化で使用されるパスワード <see cref="Password"/> 
        /// （初期値："Hello world"）は適時変更してください。 
        /// 暗号化されていない時はYAML形式のテキストで出力します。
        /// </remarks>
        public bool Encryption {
            get { return encryption; }
            set { this.encryption = value; }
        }

        /// <summary>
        /// 暗号化パスワード
        /// </summary>
        /// <remarks>
        /// 暗号化で使用するパスワードです。
        /// かならず別途保管してください。
        /// 先頭16文字が有効です。
        /// </remarks>
        public string Password {
            get { return password; }
            set {
                if (value == null || value == "") {
                    throw new ArgumentNullException ("Password is null or empty");
                }
                SetEncryptionEnable (encryption, value);
            }
        }

        /// <summary>
        /// 項目数
        /// </summary>
        public int ItemCount {
            get { return items.Count (); }
        }

        /// <summary>
        /// すべての項目を列挙する列挙子
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> Items {
            get { return items; }
        }

        /// <summary>
        /// 項目にアクセスするインデクサー
        /// </summary>
        /// <remarks>
        /// 指定の項目が存在していない場合例外が発生します。
        /// </remarks>
        /// <param name="key">キー</param>
        /// <returns></returns>
        public object this[string key] {
            get { return items[key]; }
            set { this.items[key] = value; }
        }

        /// <summary>
        /// セーブファイルのディレクトリ
        /// </summary>
        /// <remarks>
        /// セーブファイルの置き場所です。
        /// ディレクトリの最後は必ず'/'で終わっている必要があります。
        /// </remarks>
        public string Path {
            get { return directory; }
            set {
                if (value == null || value == "") {
                    throw new ArgumentNullException ("Directory is null or empty");
                }
                if (!value.EndsWith ("/")) {
                    throw new ArgumentException ("Last character should be /");
                }
                
                this.directory = value;
            }
        }

        /// <summary>
        /// セーブ
        /// </summary>
        /// <remarks>
        /// ディレクトリ <see cref="Path"/> に指定のセーブ ファイルを作成し、
        /// すべての項目 <see cref="Items"/> を保存します。
        /// 指定のパスにディレクトリーまたはファイルが存在しない場合は自動的に作成します。
        /// <see cref="Encryption"/> が false の時はYAML形式の平文で、true の時はそれをAES暗号化して保存します。
        /// </remarks>
        /// <returns></returns>
        public void Save (string fileName) {
            if (fileName == null || fileName == "") {
                throw new ArgumentNullException ("FileName is null");
            }
            if (!Directory.Exists (directory)) {
                Directory.CreateDirectory (directory);
            }

            using (var wr = new StreamWriter (directory + fileName)) {
                Save (wr);
            }
        }

        /// <summary>
        /// セーブ
        /// </summary>
        /// <remarks>
        /// 指定のストリームにすべての項目を書き込みます。
        /// </remarks>
        /// <param name="wr">ストリーム</param>
        public void Save (StreamWriter wr) {
            var yaml = new YamlSerializer ();
            var plainTxt = yaml.Serialize (items);

            if (encryption) {
                var encoder = aes.CreateEncryptor ();
                var src = Encoding.UTF8.GetBytes (plainTxt);

                var iv = aes.IV;                                              // 16 byte
                var dst = encoder.TransformFinalBlock (src, 0, src.Length);   // ...
                var crc = sha256.ComputeHash (iv.Concat (dst).ToArray ());    // 32 byte

                var bytes = iv.Concat (dst).Concat (crc).ToArray ();
                plainTxt = Convert.ToBase64String (bytes);
            }

            wr.Write (plainTxt, Encoding.UTF8);
        }

        /// <summary>
        /// ロード
        /// </summary>
        /// <remarks>
        /// パスディレクトリ <see cref="Path"/> のファイルをロードし、
        /// すべての項目 <see cref="Items"/> を復帰します。
        /// 指定のパスまたはディレクトリが存在しない場合例外を発生します。
        /// <see cref="Encryption"/> が false の時はYAML形式の平文を、true の時はそれをAES暗号化されているものとして復帰します。
        /// 現在暗号化の有無を自動判別しません。
        /// </remarks>
        /// <param name="fileName">ファイル名</param>
        /// <returns></returns>
        public void Load (string fileName) {
            if (fileName == null || fileName == "") {
                throw new ArgumentNullException ("FileName is null or empty");
            }

            using (var sr = new StreamReader (directory + fileName, Encoding.UTF8)) {
                Load (sr);
            }
        }

        private byte[] Take (byte[] src, int start, int count) {
            var dst = new byte[count];
            Array.Copy (src, start, dst, 0, count);
            return dst;
        }

        /// <summary>
        /// ロード
        /// </summary>
        /// <remarks>
        /// 指定のストリームからロードします。
        /// </remarks>
        /// <param name="sr"></param>
        public void Load (StreamReader sr) {

            var yaml = new YamlSerializer ();
            var plainTxt = sr.ReadToEnd();

            if (encryption) {
                var bytes = Convert.FromBase64String (plainTxt);
                var iv = Take (bytes, 0, 16);                     // 16 byte
                var src = Take (bytes, 16, bytes.Length - 48);    // ...
                var crc = Take (bytes, bytes.Length - 32, 32);    // 32 byte

                var sha = sha256.ComputeHash (iv.Concat (src).ToArray());
                for (var i = 0; i < 32; i++) {
                    if(crc[i] != sha[i]) {
                        throw new InvalidDataException("Sha256 is different");
                    }
                }

                var decoder = aes.CreateDecryptor (aes.Key, iv);
                var dst = decoder.TransformFinalBlock (src, 0, src.Length);
                plainTxt = Encoding.UTF8.GetString (dst);
            }

            this.items = yaml.Deserialize (plainTxt).ElementAtOrDefault (0) as Dictionary<string, object> ?? new Dictionary<string, object> ();
        }

        /// <summary>
        /// ロード
        /// </summary>
        /// <remarks>
        /// 指定のパスにディレクトリーまたはファイルが存在しない場合は自動的に作成します。
        /// それ以外は <see cref="Load(string)"/> と同じです。
        /// </remarks>
        /// <param name="fileName">ファイル名</param>
        /// <returns></returns>
        public void LoadOrCreate (string fileName)     {
            if (!Directory.Exists (directory)) {
                Directory.CreateDirectory (directory);
            }
            if (!File.Exists (directory + fileName)) {
                var fs = File.Create (directory + fileName);
                fs.Close ();
            }

            Load (fileName);
        }

        /// <summary>
        /// 項目の追加
        /// </summary>
        /// <param name="key">キー</param>
        /// <param name="initValue">初期値</param>
        public void Add (string key, object initValue) {
            if (key == null || key == "") {
                throw new ArgumentNullException ("Key is null or empty");
            }
            if (initValue == null) {
                throw new ArgumentNullException ("Value is null");
            }

            this.items.Add (key, initValue);
        }

        /// <summary>
        /// 項目の削除
        /// </summary>
        /// <param name="key">キー</param>
        public bool Remove (string key) {
            return this.items.Remove (key);
        }

        /// <summary>
        /// 項目の取得
        /// </summary>
        /// <remarks>
        /// 指定したキー <paramref name="key"/> の項目が存在しない場合例外が発生します。
        /// 例外を発生させずに新規に項目を作りたい場合は <see cref="GetOrCreate"/> メソッドを使用してください。
        /// </remarks>
        /// <param name="key">キー</param>
        /// <returns></returns>
        public object Get (string key) {
            return items[key];
        }

        /// <summary>
        /// 項目の取得
        /// </summary>
        /// <remarks>
        /// 指定したキー <paramref name="key"/> の項目が存在しない場合、
        /// 新規に項目を作成し <paramref name="initValue"/> で初期化します。
        /// </remarks>
        /// <param name="key">キー</param>
        /// <param name="initValue">項目が存在しない場合の初期値</param>
        /// <returns></returns>
        public object GetOrCreate (string key, object initValue) {
            if (!items.ContainsKey (key)) {
                items.Add (key, initValue);
            }
            return items[key];
        }

        /// <summary>
        /// 暗号化の変更
        /// </summary>
        /// <remarks>
        /// 暗号化の有無を変更します。暗号化に使用するパスワードは先頭16文字が有効です。
        /// </remarks>
        /// <param name="enable">暗号化の有無</param>
        /// <param name="password">パスワード</param>
        public void SetEncryptionEnable (bool enable, string password) {
            if (password == null || password == "") {
                throw new ArgumentNullException ("Password is null or empty");
            }

            var key = new byte[16];
            for (var i = 0; i < 16; i++) {
                key[i] = Encoding.UTF8.GetBytes (password).ElementAtOrDefault (i);
            }
            this.encryption = enable;
            this.password = password;
            this.aes.Key = key;
        }

    }
}
