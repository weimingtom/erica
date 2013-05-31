using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;

namespace DD {
    /// <summary>
    /// ライン表示 コンポーネント
    /// </summary>
    /// <remarks>
    /// ラインは日本語で言う台詞（セリフ）の事で、「発言者の名前」「発言テキスト（2～3行）」「音声ファイル」からなります。
    /// このコンポーネントは複数のラインを順番にクリックで再生するのに適しています。
    /// </remarks>
    /// <seealso cref="Line"/>
    public class LineReader : Component {
        #region Field
        List<Line> lines;
        int index;
        int width;
        int height;
        int charSize;
        bool lineChanged;
        #endregion

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="width">表示領域の横幅（ピクセル数）</param>
        /// <param name="height">表示領域の縦幅（ピクセル数）</param>
        public LineReader (int width, int height) {
            this.lines = new List<Line>();
            this.index = 0;
            this.width = width;
            this.height = height;
            this.charSize = 24;
            this.lineChanged = false;
        }

        /// <summary>
        /// 表示領域の横幅（ピクセル数）
        /// </summary>
        public int Width {
            get { return width; }
        }

        /// <summary>
        /// 表示領域の縦幅（ピクセル数）
        /// </summary>
        public int Height {
            get { return height; }
        }
        
        /// <summary>
        /// 1文字のサイズ
        /// </summary>
        public int CharacterSize {
            get { return charSize; }
            set {
                var size = value;
                if (size <= 0 || size > 128) {
                    throw new ArgumentException ("Character size is invalid");
                }
                this.charSize = value;
            }
        }

        /// <summary>
        /// ライン総数
        /// </summary>
        public int LineCount {
            get { return lines.Count; }
        }

        /// <summary>
        /// 現在再生中のライン位置
        /// </summary>
        public int CurrentPosition {
            get { return index; }
        }

        /// <summary>
        /// 現在の再生中のライン
        /// </summary>
        public Line CurrentLine {
            get { return lines.ElementAtOrDefault(index); }
        }

        /// <summary>
        /// ラインの追加
        /// </summary>
        /// <param name="line">ライン オブジェクト</param>
        public void AddLine (Line line) {
            if (line == null) {
                throw new ArgumentNullException ("Line is null");
            }
            this.lines.Add (line);
        }

        /// <summary>
        /// ラインのロード
        /// </summary>
        /// <remarks>
        /// ファイルからライン形式で書かれたラインをロードします。
        /// </remarks>
        /// <param name="name">ファイル名</param>
        public void LoadLine (string name) {
            this.lines = ResourceManager.GetInstance ().GetLine (name).ToList();
            this.index = 0;
            this.lineChanged = true;
        }

        /// <summary>
        /// 次のラインに進む
        /// </summary>
        public void Next () {
            if (index < LineCount - 1) {
                this.lineChanged = true;
                this.index = index + 1;
            }
        }

        /// <summary>
        /// 前のラインに戻る
        /// </summary>
        public void Prev () {
            if (index > 0) {
                this.lineChanged = true;
                this.index = index - 1;
            }
        }

        /// <summary>
        /// 先頭のラインに戻す
        /// </summary>
        public void Rewind () {
            if (index != 0) {
                this.lineChanged = true;
                this.index = 0;
            }
        }

        /// <summary>
        /// 指定位置のラインに飛ぶ
        /// </summary>
        /// <param name="i">インデックス</param>
        public void Jump (int i) {
            if (i < 0 || i > LineCount - 1) {
                throw new IndexOutOfRangeException ("Index is out of range");
            }
            if (index != i) {
                this.lineChanged = true;
                this.index = i;
            }
        }

        /// <inheritdoc/>
        public override void OnAttached () {
            Node.SetBoundingBox (0, 0, width, height);
        }

        /// <inheritdoc/>
        public override void OnDispatch () {
            if (lineChanged) {
                var args = CurrentLine.Event;
                if (args != null) {
                    foreach (var cmp in Node.Components) {
                        cmp.OnLineEvent (CurrentLine, YAMLParser.Parse (args));
                    }
                }
                this.lineChanged = false;
            }
        }

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            var win = window as RenderWindow;
            var font = ResourceManager.GetInstance ().GetDefaultFont ();
            var txt = new Text (lines[index].Words, font);
            txt.Position = new Vector2f (Node.WindowX, Node.WindowY);
            txt.CharacterSize = (uint)charSize;
            win.Draw (txt);
        }


    }
}
