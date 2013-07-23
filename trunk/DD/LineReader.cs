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
        /// ラインの自動送りの時に、文字表示のタイミング制御に使用されるパラメーターです。
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
            public int TimeAfterOneCharacter { get; private set; }

            /// <summary>
            /// 一行あたりのウェイト値（msec）
            /// </summary>
            public int TimeAfterOneSentense { get; private set; }
        }
        #endregion

        #region Field
        List<Line> lines;
        int index;
        int width;
        int height;
        int charSize;
        Color color;
        FeedMode feedMode;
        FeedParameters feedParams;
        TimeCounter tick;
        TimeCounter tack;
        TimeCounter tackWait;
        #endregion

        /// <summary>
        /// チック イベント
        /// </summary>
        /// <remarks>
        /// 1文字表示される度にこのチック イベントが呼び出されます。
        /// </remarks>
        public event EventHandler Ticked;

        /// <summary>
        /// タック イベント
        /// </summary>
        /// <remarks>
        /// 1ライン表示される度にこのタック イベントが呼び出されます。
        /// </remarks>
        public event EventHandler Tacked;

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
            this.feedMode = FeedMode.Manual;
            this.feedParams = new FeedParameters (0, 0);
            this.tick = null;
            this.tack = null;
            this.tackWait = null;
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
            get { return color; }
            set { SetColor (value.R, value.G, value.B, value.A); }
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
                return !tack.IsRunning;
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
            set { SetFeedMode (value); }
        }

        /// <summary>
        /// フィード パラメーター
        /// </summary>
        /// <remarks>
        /// ラインを表示するときのパラメーターです。
        /// </remarks>
        public FeedParameters FeedParameter {
            get { return feedParams; }
            set { SetFeedParameter (value); }
        }
        #endregion


        /// <summary>
        /// フィード モードの変更
        /// </summary>
        /// <remarks>
        /// フィード（自動送り）機能を自動送りに変更します。
        /// </remarks>
        /// <note>
        /// フィード パラメーターを変更すると内部の時計がリセットされ、
        /// 自動送り中の文字列が最初から表示し直します。仕様です。
        /// </note>
        /// <param name="mode">フィード モード</param>
        public void SetFeedMode (FeedMode mode) {
            this.feedMode = mode;
        }

        /// <summary>
        /// フィード パラメーターの変更
        /// </summary>
        /// <remarks>
        /// フィードで使用するパラメーターも一緒に指定します。
        /// </remarks>
        /// <param name="param">パラメーター</param>
        public void SetFeedParameter (FeedParameters param) {
            this.feedParams = param;
     
            RecreateTimeCounter ();
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
        public void LoadFromFile (string name) {
            this.lines = Resource.GetLine (name).ToList ();
            this.index = 0;
            LineChange (0);
        }

        /// <summary>
        /// 次のラインに進む
        /// </summary>
        public void Next () {
            if (index < LineCount - 1) {
                LineChange (index + 1);
            }
        }

        /// <summary>
        /// 前のラインに戻る
        /// </summary>
        public void Prev () {
            if (index > 0) {
                LineChange (index - 1);
            }
        }

        /// <summary>
        /// 先頭のラインに戻す
        /// </summary>
        public void Rewind () {
            if (index != 0) {
                LineChange (0);
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
                LineChange (i);
            }
        }

        /// <summary>
        /// ラインの変更
        /// </summary>
        /// <param name="next"></param>
        private void LineChange (int next) {
            this.index = next;
            
            if (lines[index].Event != null) {
                if (Node != null) {
                    foreach (var cmp in Node.Components) {
                        cmp.OnLineEvent (CurrentLine, YAMLParser.Parse (lines[index].Event));
                    }
                }
            }

            RecreateTimeCounter ();
         }

        private void RecreateTimeCounter () {
            if (LineCount == 0) {
                return;
            }
            if (tick != null) {
                this.tick.Close ();
            }
            if (tack != null) {
                this.tack.Close ();
            }
            if (tackWait != null) {
                this.tackWait.Close ();
            }

            var wordCount = lines[index].Words.Length;
            var charWait = feedParams.TimeAfterOneCharacter;
            var lineWait = feedParams.TimeAfterOneSentense;

            this.tick = new TimeCounter (wordCount, charWait);                         // 1文字終わり
            this.tack = new TimeCounter (1, wordCount * charWait + 1);                 // 1行終わり
            this.tackWait = new TimeCounter (1, wordCount * charWait + lineWait + 1);  // 1行終わり + 自動送り用のウェイト
                                         
            
            tick.Elapsed += () => {
                if (Ticked != null) {
                    Ticked.Invoke (lines[index], null);
                }
            };
            tack.Elapsed += () => {
                if (Tacked != null) {
                    Tacked.Invoke (lines[index], null);
                }
            };

            tick.Start ();
            tack.Start ();
            tackWait.Start ();
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
        }

        #endregion

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            var win = window as RenderWindow;
            var words = lines[index].Words.Substring (0, tick.Count);
            var font = Resource.GetDefaultFont ();
            var txt = new Text (words, font);
            txt.Position = new Vector2f (Node.Point.X, Node.Point.Y);
            txt.CharacterSize = (uint)charSize;
            txt.Color = color.ToSFML ();
            win.Draw (txt);
        }

        /// <inheritdoc/>
        public override void OnUpdate (long msec) {
            if (feedMode == FeedMode.Automatic && !tackWait.IsRunning) {
                Next ();
            }
        }
        #endregion

    }
}
