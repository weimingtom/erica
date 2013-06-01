using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.Window;
using System.Diagnostics;

namespace DD {
    /// <summary>
    /// ライン表示 コンポーネント
    /// </summary>
    /// <remarks>
    /// ラインは日本語で言う台詞（セリフ）の事で、「発言者の名前」「発言テキスト（2～3行）」「音声ファイル」からなります。
    /// このコンポーネントは複数のラインを順番にクリックで再生するのに適しています。
    /// ライン リーダーはノードのバウンディング ボックスを自身のサイズで書き換えます。
    /// </remarks>
    /// <seealso cref="Line"/>
    public class LineReader : Component {

        #region InnderClass
        /// <summary>
        /// フィード パラメーター
        /// </summary>
        /// <remarks>
        /// ラインの表示で使用されるパラメーターです。
        /// <list type="bullet">
        ///   <item><see cref="FeedParameters.TimeAfterOneCharacter"/> = 一文字あたりのウェイト値（msec）</item>
        ///   <item><see cref="FeedParameters.TimeAfterOneSentense"/> = 一行あたりのウェイト値（msec）</item>
        /// </list>
        /// </remarks>
        public struct FeedParameters {
            /// <summary>
            /// コンストラクター
            /// </summary>
            /// <param name="oneChar">一文字あたりのウェイト値（msec）</param>
            /// <param name="oneSent"> 一行あたりのウェイト値（msec）</param>
            public FeedParameters (int oneChar, int oneSent)
                : this () {
                this.TimeAfterOneCharacter = oneChar;
                this.TimeAfterOneSentense = oneSent;
            }
            /// <summary>
            /// 一文字あたりのウェイト値（msec）
            /// </summary>
            public int TimeAfterOneCharacter {get; private set;}

            /// <summary>
            /// 一行あたりのウェイト値（msec）
            /// </summary>
            public int TimeAfterOneSentense{get;private set;}
        }
        #endregion

        #region Field
        List<Line> lines;
        int index;
        int width;
        int height;
        int charSize;
        Color color;
        Stopwatch watch;
        FeedMode feedMode;
        FeedParameters feedParams;
        bool lineChanged;
        #endregion

        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <remarks>
        /// ノードのバウンディング ボックスは書き換えません。
        /// </remarks>
        /// <param name="width">表示領域の横幅（ピクセル数）</param>
        /// <param name="height">表示領域の縦幅（ピクセル数）</param>
        public LineReader (int width, int height) {
            this.lines = new List<Line> ();
            this.index = 0;
            this.width = width;
            this.height = height;
            this.charSize = 24;
            this.color = new Color (255, 255, 255, 255);
            this.lineChanged = false;
            this.watch = new Stopwatch ();
            this.feedMode = FeedMode.Normal;
            this.feedParams = new FeedParameters (0, 0);
        }
        #endregion

        #region Property
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
            set { SetCharacterSize (value); }
        }

        /// <summary>
        /// 文字色
        /// </summary>
        public Color Color {
            get{return color;}
            set{SetColor(value.R, value.G, value.B, value.A); }
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
            get { return lines.ElementAtOrDefault (index); }
        }

        /// <summary>
        /// 1ラインの表示完了判定
        /// </summary>
        /// <remarks>
        /// Automaticフィードの時にすべての文字の表示が完了している事を示します。
        /// 
        /// </remarks>
        public bool IsOver {
            get {
                if (feedMode == FeedMode.Normal) {
                    return true;
                }
                else {
                    var time = watch.ElapsedMilliseconds;
                    var duration = feedParams.TimeAfterOneCharacter * lines[index].Words.Length + feedParams.TimeAfterOneSentense;
                    return (time >= duration) ? true : false;
                }
            }
        }

        /// <summary>
        /// フィード モード
        /// </summary>
        /// <remarks>
        /// ライン送りを手動か自動から選択します。
        /// </remarks>
        public FeedMode FeedMode {
            get { return feedMode; }
            set { SetFeedMode (value, feedParams); }
        }

        /// <summary>
        /// フィード パラメーター
        /// </summary>
        /// <remarks>
        /// ラインを表示するときのパラメーターです。
        /// </remarks>
        public FeedParameters FeedParameter {
            get { return feedParams; }
            set { SetFeedMode (feedMode, value); }
        }
        #endregion


        /// <summary>
        /// フィードモードの変更
        /// </summary>
        /// <remarks>
        /// フィード（自動送り）機能を自動送りに変更します。
        /// フィードで使用するパラメーターも一緒に指定します。
        /// </remarks>
        /// <param name="mode">フィード モード</param>
        /// <param name="param">パラメーター</param>
        public void SetFeedMode (FeedMode mode, FeedParameters param) {
            this.feedMode = mode;
            this.feedParams = param;
        }

        #region Method

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
            this.lines = Resource.GetLine (name).ToList ();
            this.index = 0;
            this.lineChanged = true;
            watch.Restart ();
        }

        /// <summary>
        /// 次のラインに進む
        /// </summary>
        public void Next () {
            if (index < LineCount - 1) {
                this.lineChanged = true;
                this.index = index + 1;
                watch.Restart ();
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

        /// <summary>
        /// 文字サイズの変更
        /// </summary>
        /// <param name="size">サイズ</param>
        public void SetCharacterSize (int size) {
            if (size <= 0 || size > 128) {
                throw new ArgumentException ("Size is invalid");
            }
            this.charSize = size;
        }

        /// <summary>
        /// 文字色の変更
        /// </summary>
        /// <param name="r">赤</param>
        /// <param name="g">青</param>
        /// <param name="b">緑</param>
        /// <param name="a">不透明度</param>
        public void SetColor (byte r, byte g, byte b, byte a) {
            this.color = new Color (r, g, b, a);
        }

        #region Override
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
        #endregion

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            var win = window as RenderWindow;
            var font = Resource.GetDefaultFont ();
            var drawWords = lines[index].Words;

            var time = (int)watch.ElapsedMilliseconds;
            var waitTime = feedParams.TimeAfterOneCharacter;
            if (waitTime > 0) {
                var n = Math.Min(time/waitTime, drawWords.Length);
                drawWords = drawWords.Substring(0, n);
            }

            var txt = new Text (drawWords, font);
            txt.Position = new Vector2f (Node.WindowX, Node.WindowY);
            txt.CharacterSize = (uint)charSize;
            win.Draw (txt);
        }

        /// <inheritdoc/>
        public override void OnUpdate (long msec) {
            if (feedMode == FeedMode.Automatic && IsOver) {
                Next ();
            }
        }
        #endregion

    }
}
