using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SFML.Graphics;
using SFML.Window;
using SFML.Audio;
using System.Drawing;

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
    public partial class Resource : IDisposable {

        #region Field
        static string fontDirectory;
        static string textureDirectory;
        static string lineDirectory;
        static string soundDirectory;
        static Dictionary<string, SFML.Graphics.Font> fonts;
        static Dictionary<string, Texture> textures;
        static Dictionary<string, Line[]> lines;
        static Dictionary<string, SoundClip> sounds;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        static Resource () {
            fontDirectory = "./";
            textureDirectory = "./";
            lineDirectory = "./";
            fonts = new Dictionary<string, SFML.Graphics.Font> ();
            textures = new Dictionary<string, Texture> ();
            lines = new Dictionary<string, Line[]> ();
            sounds = new Dictionary<string, SoundClip> ();
        }

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
            get { return soundDirectory; }
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
        public static IEnumerable<KeyValuePair<string, SoundClip>> SoundClips {
            get { return sounds; }
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
        public static void SoundClipDirectory (string name) {
            soundDirectory = name;
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
        /// 日本語デフォルト フォント（"Konatu.ttf", 子夏フォント）を取得します。
        /// デフォルト フォントはユーザーが用意しなくても常に使用可能です。
        /// </remarks>
        /// <returns>フォント</returns>
        public static SFML.Graphics.Font GetDefaultFont () {
            var name = "Konatu.ttf";
            if (!fonts.ContainsKey (name)) {
                var ttf = new MemoryStream (Properties.Resources.Konatu);
                fonts.Add (name, new SFML.Graphics.Font (ttf));
            }

            return fonts[name];
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
                textures.Add (name, new Texture (bitmap, name));
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
                textures.Add (name, new Texture (stream, name));
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
        /// サウンド クリップの取得
        /// </summary>
        /// <param name="name">サウンド ファイル名</param>
        /// <param name="streaming">ストリーミング再生</param>
        /// <returns>サウンド クリップ</returns>
        public static SoundClip GetSoundClip (string name, bool streaming) {
            if (!sounds.ContainsKey (name)) {
                sounds.Add (name, new SoundClip(name, streaming));
            }
            return sounds[name];
        }



        /// <summary>
        /// 全リソースの解放
        /// </summary>
        public void ReleaseAll () {
            foreach (var font in fonts) {
                font.Value.Dispose ();
            }
            foreach (var texture in textures) {
                texture.Value.Dispose ();
            }
            foreach (var sound in sounds) {
                sound.Value.Dispose ();
            }
            fonts.Clear ();
            textures.Clear ();
            sounds.Clear ();
        }

        /// <inheritdoc/>
        public void Dispose () {
            ReleaseAll();
        }
        #endregion
    }
}
