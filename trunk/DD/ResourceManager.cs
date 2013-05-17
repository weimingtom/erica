using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SFML.Graphics;
using SFML.Window;

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
    public partial class ResourceManager : IDisposable {

        #region Field
        static ResourceManager rm;
        string fontDirectory;
        string textureDirectory;
        Dictionary<string, Font> fonts;
        Dictionary<string, Texture> textures;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        internal ResourceManager () {
            this.fontDirectory = "./";
            this.textureDirectory = "./";
            this.fonts = new Dictionary<string, Font> ();
            this.textures = new Dictionary<string, Texture> ();
        }

        /// <summary>
        /// シングルトン インスタンスの取得
        /// </summary>
        /// <returns><see cref="ResourceManager"/> インスタンス</returns>
        public static ResourceManager GetInstance () {
            if (rm == null) {
                rm = new ResourceManager ();
            }
            return rm;
        }

        #endregion

        #region Property
        /// <summary>
        /// フォント ディレクトリ
        /// </summary>
        public string FontDirectory {
            get { return fontDirectory; }
        }

        /// <summary>
        /// テクスチャー ディレクトリ
        /// </summary>
        public string TextureDirectory {
            get { return textureDirectory; }
        }

        /// <summary>
        /// キャッシュ済みの全フォントを列挙する列挙子
        /// </summary>
        public IEnumerable<KeyValuePair<string, Font>> Fonts {
            get { return fonts; }
        }

        /// <summary>
        /// キャッシュ済みの全テクスチャーを列挙する列挙子
        /// </summary>
        public IEnumerable<KeyValuePair<string, Texture>> Textures {
            get { return textures; }
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
        public void SetFontDirectory (string name) {
            this.fontDirectory = name;
        }

        /// <summary>
        /// テクスチャー ディレクトリーの変更
        /// </summary>
        /// <remarks>
        /// デフォルトは""で実行ファイルと同じディレクトリから検索します。
        /// 指定するディレクトリー名の最後は必ず"/"で終わってください。
        /// </remarks>
        /// <param name="name">ディレクトリ名</param>
        public void SetTextureDirectory (string name) {
            this.textureDirectory = name;
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
        public Font GetFont (string name) {
            if (!fonts.ContainsKey (name)) {
                this.fonts.Add (name, new Font (fontDirectory + name));
            }

            return fonts[name];
        }

        /// <summary>
        /// デフォルト フォントの取得
        /// </summary>
        /// <remarks>
        /// デフォルト フォント（"arial.ttf"）を取得します。
        /// デフォルト フォントはユーザーが用意しなくても常に使用可能です。
        /// </remarks>
        /// <returns>フォント</returns>
        public Font GetDefaultFont () {
            var name = "arial.ttf";
            if (!fonts.ContainsKey (name)) {
                var ttf = new MemoryStream (Properties.Resources.arial);
                this.fonts.Add (name, new Font (ttf));
            }

            return fonts[name];
        }

        /// <summary>
        /// テクスチャーの取得
        /// </summary>
        /// <remarks>
        /// 必要ならディレクトリ <see cref="FontDirectory"/> にある
        /// フォント ファイル<paramref name="name"/> をロードします。
        /// </remarks>
        /// <param name="name">テクスチャー ファイル名</param>
        /// <returns>テクスチャー</returns>
        public Texture GetTexture (string name) {
            if (!textures.ContainsKey (name)) {
                this.textures.Add (name, new Texture (textureDirectory + name));
            }

            return textures[name];
        }



        /// <summary>
        /// フォント リソースの解放
        /// </summary>
        private void ReleaseFont () {
            foreach (var font in fonts) {
                font.Value.Dispose ();
            }
            fonts.Clear ();
        }

        /// <summary>
        /// テクスチャー リソースの解放
        /// </summary>
        private void ReleaseTexture () {
            foreach (var texture in textures) {
                texture.Value.Dispose ();
            }
            textures.Clear ();

        }


        /// <summary>
        /// 全リソースの解放
        /// </summary>
        public void ReleaseAll () {
            ReleaseFont ();
            ReleaseTexture ();
        }

        /// <inheritdoc/>
        public void Dispose () {
            ReleaseAll();
        }
        #endregion
    }
}
