using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;
using System.Drawing;
using System.Data.Entity;

namespace DD {
    /// <summary>
    /// リソース管理 クラス
    /// </summary>
    /// <remarks>
    /// リソース マネージャーはファイル リソース（フォント、テクスチャーなど）を一括管理します。
    /// これらのリソースは1度だけロードされ、必要に応じて再利用されます。
    /// デフォルト フォントはユーザーがファイルを用意することなく利用可能です
    /// （実行ファイルに埋め込まれています）。
    /// 使用後は <see cref="Dispose"/> メソッドを読んでリソースを解放してください。
    /// </remarks>
    public static class Resource {

        #region Field
        static string fontDirectory = "./";
        static string textureDirectory = "./";
        static string lineDirectory = "./";
        static string audioDirectory = "./";
        static string databaseDirectory = "./";
        static Dictionary<string, SFML.Graphics.Font> fonts = new Dictionary<string, SFML.Graphics.Font> ();
        static Dictionary<string, Texture> textures = new Dictionary<string, Texture> ();
        static Dictionary<string, Line[]> lines = new Dictionary<string, Line[]> ();
        static Dictionary<string, SoundTrack> audios = new Dictionary<string, SoundTrack> ();
        static Dictionary<string, DbContext> databases = new Dictionary<string, DbContext> ();
        #endregion

        #region Constructor
        #endregion

        #region Property
        /// <summary>
        /// フォント ディレクトリ
        /// </summary>
        public static string FontDirectory {
            get { return fontDirectory; }
        }

        /// <summary>
        /// テクスチャー ディレクトリ
        /// </summary>
        public static string TextureDirectory {
            get { return textureDirectory; }
        }

        /// <summary>
        /// ライン ディレクトリ
        /// </summary>
        public static string LineDirectory {
            get { return lineDirectory; }
        }

        /// <summary>
        /// サウンド ディレクトリ
        /// </summary>
        public static string SoundDirectory {
            get { return audioDirectory; }
        }

        /// <summary>
        /// データベース ディレクトリ
        /// </summary>
        public static string DatabaseDirectory {
            get { return databaseDirectory; }
        }

        /// <summary>
        /// キャッシュ済みの全フォントを列挙する列挙子
        /// </summary>
        public static IEnumerable<KeyValuePair<string, SFML.Graphics.Font>> Fonts {
            get { return fonts; }
        }

        /// <summary>
        /// キャッシュ済みの全テクスチャーを列挙する列挙子
        /// </summary>
        public static IEnumerable<KeyValuePair<string, Texture>> Textures {
            get { return textures; }
        }

        /// <summary>
        /// キャッシュ済みの全ラインを列挙する列挙子
        /// </summary>
        public static IEnumerable<KeyValuePair<string, Line[]>> Lines {
            get { return lines; }
        }

        /// <summary>
        /// キャッシュ済みのすべてのサウンド クリップを列挙する列挙子
        /// </summary>
        public static IEnumerable<KeyValuePair<string, SoundTrack>> SoundClips {
            get { return audios; }
        }

        /// <summary>
        /// キャッシュ済みのすべてのデータベースを列挙する列挙子
        /// </summary>
        public static IEnumerable<KeyValuePair<string, DbContext>> Databases {
            get { return databases; }
        }
        #endregion


        #region Method
        /// <summary>
        /// フォント ディレクトリーの変更
        /// </summary>
        /// <remarks>
        /// デフォルトは""で実行ファイルと同じディレクトリから検索します。
        /// 指定するディレクトリー名の最後は必ず"/"で終わってください。
        /// </remarks>
        /// <param name="name">ディレクトリ名</param>
        public static void SetFontDirectory (string name) {
            fontDirectory = name;
        }

        /// <summary>
        /// テクスチャー ディレクトリーの変更
        /// </summary>
        /// <remarks>
        /// デフォルトは""で実行ファイルと同じディレクトリから検索します。
        /// 指定するディレクトリー名の最後は必ず"/"で終わってください。
        /// </remarks>
        /// <param name="name">ディレクトリ名</param>
        public static void SetTextureDirectory (string name) {
            textureDirectory = name;
        }


        /// <summary>
        /// ライン ディレクトリーの変更
        /// </summary>
        /// <remarks>
        /// デフォルトは""で実行ファイルと同じディレクトリから検索します。
        /// 指定するディレクトリー名の最後は必ず"/"で終わってください。
        /// <note>
        /// ラインは他のリソースと違って非マネージド リソースを持っていないので、
        /// Resouce の管轄外にしても良いが・・・検討中
        /// </note>
        /// </remarks>
        /// <param name="name">ディレクトリ名</param>
        public static void SetLineDirectory (string name) {
            lineDirectory = name;
        }

        /// <summary>
        /// サウンド ディレクトリーの変更
        /// </summary>
        /// <param name="name">ディレクトリ名</param>
        public static void SetAudioDirectory (string name) {
            audioDirectory = name;
        }

        /// <summary>
        /// データベース ディレクトリーの変更
        /// </summary>
        /// <param name="name">ディレクトリ名</param>
        public static void SetDatabaseDirectory (string name) {
            databaseDirectory = name;
        }


        /// <summary>
        /// フォントの取得
        /// </summary>
        /// <remarks>
        /// 必要ならディレクトリ <see cref="FontDirectory"/> にある
        /// フォント ファイル<paramref name="name"/> をロードします。
        /// </remarks>
        /// <param name="name">フォントのファイル名</param>
        /// <returns>フォント</returns>
        public static SFML.Graphics.Font GetFont (string name) {
            if (!fonts.ContainsKey (name)) {
                fonts.Add (name, new SFML.Graphics.Font (fontDirectory + name));
            }

            return fonts[name];
        }

        /// <summary>
        /// デフォルト フォントの取得
        /// </summary>
        /// <remarks>
        /// 日本語デフォルト フォント（"ipamp.ttf", IPAフォント(明朝)）を取得します。
        /// デフォルト フォントはユーザーが用意しなくても常に使用可能です。
        /// </remarks>
        /// <returns>フォント</returns>
        public static SFML.Graphics.Font GetDefaultFont () {
            var name = "Konatu.ttf";
            if (!fonts.ContainsKey (name)) {
                var ttf = new MemoryStream (Properties.Resources.ipamp);
                fonts.Add (name, new SFML.Graphics.Font (ttf));
            }

            return fonts[name];
        }

        /// <summary>
        /// デフォルト テクスチャーの取得
        /// </summary>
        /// <remarks>
        /// デフォルト テクスチャー（"WhiteTexutre.png", 128x128ピクセル, 白色）を取得します。
        /// デフォルト テクスチャーはユーザーが用意しなくても常に使用可能です。
        /// </remarks>
        /// <returns></returns>
        public static Texture GetDefaultTexture () {
            var name = "WhiteTexture.png";
            if (!textures.ContainsKey (name)) {
                textures.Add (name, new Texture (name, Properties.Resources.WhiteTexture));

            }
            return textures[name];

        }

        /// <summary>
        /// テクスチャーの取得
        /// </summary>
        /// <remarks>
        /// 初回のみファイルからテクスチャーをロードし、リソース管理に組み入れます。
        /// 2回目以降は同一のオブジェクトが返ります。
        /// </remarks>
        /// <param name="name">テクスチャー ファイル名</param>
        /// <returns>テクスチャー</returns>
        public static Texture GetTexture (string name) {
            if (!textures.ContainsKey (name)) {
                textures.Add (name, new Texture (textureDirectory + name));
            }

            return textures[name];
        }

        /// <summary>
        /// テクスチャーの取得
        /// </summary>
        /// <remarks>
        /// 初回のみ <see cref="Bitmap"/> からテクスチャーをロードし、リソース管理に組み入れます。
        /// 2回目以降は同一のオブジェクトが返ります。
        /// </remarks>
        /// <param name="bitmap">ビットマップ オブジェクト</param>
        /// <param name="name">テクスチャー名</param>
        /// <returns>テクスチャー</returns>
        public static Texture GetTexture (Bitmap bitmap, string name) {
            if (!textures.ContainsKey (name)) {
                textures.Add (name, new Texture (name, bitmap));
            }

            return textures[name];
        }

        /// <summary>
        /// テクスチャーの取得
        /// </summary>
        /// <remarks>
        /// 初回のみストリーム <paramref name="stream"/> からテクスチャーをロードし、リソース管理に組み入れます。
        /// 2回目以降は同一のオブジェクトが返ります。
        /// </remarks>
        /// <param name="stream">ストリーム</param>
        /// <param name="name">テクスチャー名</param>
        /// <returns>テクスチャー</returns>
        public static Texture GetTexture (MemoryStream stream, string name) {
            if (!textures.ContainsKey (name)) {
                textures.Add (name, new Texture (name, stream));
            }

            return textures[name];
        }

        /// <summary>
        /// ラインの取得
        /// </summary>
        /// <param name="name">ライン ファイル名</param>
        /// <returns>ラインの配列</returns>
        public static Line[] GetLine (string name) {
            if (!lines.ContainsKey (name)) {
                lines.Add (name, Line.Load (name));
            }
            return lines[name];
        }

        /// <summary>
        /// ミュージック トラックの取得
        /// </summary>
        /// <param name="name">サウンド ファイル名</param>
        /// <returns>サウンド クリップ</returns>
        public static MusicTrack GetMusicTrack (string name) {
            if (!audios.ContainsKey (name)) {
                audios.Add (name, new MusicTrack (name));
            }
            return audios[name] as MusicTrack;
        }

        /// <summary>
        /// サウンド エフェクト トラックの取得
        /// </summary>
        /// <param name="name">サウンドファイル名</param>
        /// <returns></returns>
        public static SoundEffectTrack GetSoundTrack (string name) {
            if (!audios.ContainsKey (name)) {
                audios.Add (name, new SoundEffectTrack (name));
            }
            return audios[name] as SoundEffectTrack;
        }

        /// <summary>
        /// データベースの取得
        /// </summary>
        /// <remarks>
        /// 型パラメーター <typeref name="T"/> は、Entity Frameworkによって自動生成される DbContext の派生型です。
        /// データベースへの接続文字列は App.Config に記載されているものをそのまま使用します。
        /// このメソッドによるデータベースの取得は名前ベースではなく型ベースです。
        /// このメソッドによって開かれたデータベースの識別名はTの型名が使用されます。
        /// 従って他の同様のメソッドと比べて若干動作が異なります。
        /// </remarks>
        /// <typeparam name="T">データベース（DbContexの派生型）</typeparam>
        /// <returns></returns>
        public static T GetDatabase<T> () where T : DbContext, new () {
            foreach (var pair in databases) {
                if (pair.Value is T) {
                    return pair.Value as T;
                }
            }

            var query = (from db in databases.Values
                         where db is T
                         select db as T).FirstOrDefault ();
            if (query != null) {
                return query;
            }


            var name = typeof (T).Name;
            var newdb = new T ();
            databases.Add (name, newdb);

            return newdb;
        }

        /// <summary>
        /// データベースの取得
        /// </summary>
        /// <remarks>
        /// 名前ベースでデータベースを取得します。
        /// このメソッドは指定のデーターベースが見つからない場合 <c>null</c> を返します。
        /// （型がわからないのでインスタンス化できない）
        /// </remarks>
        /// <param name="name">データベース識別名</param>
        /// <returns></returns>
        public static DbContext GetDatabase (string name) {
            if (!databases.ContainsKey (name)) {
                return null;
            }
            return databases[name];
        }

        /// <inheritdoc/>
        public static void Dispose () {
            foreach (var font in fonts) {
                font.Value.Dispose ();
            }
            foreach (var texture in textures) {
                texture.Value.Dispose ();
            }
            foreach (var sound in audios) {
                sound.Value.Dispose ();
            }
            fonts.Clear ();
            textures.Clear ();
            audios.Clear ();
        }
        #endregion
    }
}
