using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprache;
using Parse2 = Sprache.Parse;
using System.IO;
using System.Text.RegularExpressions;

namespace DD {
    /// <summary>
    /// ライン形式のパーサー クラス
    /// </summary>
    /// <remarks>
    /// ライン形式の文字列をパースしてラインの配列を作成します。
    /// 通常このクラスをユーザーが使用する必要はありません。
    /// </remarks>
    public static class LineParser {

        static readonly Parser<char> NewLine = Parse2.Char ('\n');

        static readonly Parser<string> Delimiter = from a in NewLine
                                                   from b in NewLine.AtLeastOnce ()
                                                   select "";


        static readonly Parser<string> Actor = from op in Parse2.Char ('-').Token ()
                                               from actor in (Parse2.AnyChar.Until (NewLine)).Text ()
                                               select actor;


        static readonly Parser<string> Voice = from op in Parse2.Char ('+').Token ()
                                               from voice in (Parse2.AnyChar.Until (NewLine)).Text ()
                                               select voice;

        static readonly Parser<string> Event = from op in Parse2.Char ('*').Token ()
                                               from events in (Parse2.AnyChar.Until (NewLine)).Text ()
                                               select events;


        static readonly Parser<string> Content = from content in (Parse2.AnyChar.Until (Delimiter)).Text ()
                                                 select content;


        static readonly Parser<Line> Line = from author in Actor
                                            from voice in Voice.Optional ()
                                            from events in Event.Optional ()
                                            from content in Content
                                            select new Line (author, content, voice.GetOrDefault (), events.GetOrDefault ());

        static readonly Parser<string> Comment = from op in Parse2.Char ('#').Token ()
                                                 from comment in (Parse2.AnyChar.Until (NewLine)).Text ()
                                                 select comment;

        static readonly Parser<IEnumerable<Line>> Lines = from comment in Comment.Many ()
                                                          from line in Line.Many ().End ()
                                                          select line;

        /// <summary>
        /// ライン形式のパース
        /// </summary>
        /// <param name="parsingString">ライン形式の文字列</param>
        /// <returns>ラインの配列</returns>
        static public Line[] Parse (string parsingString) {

            // Windowsの改行コードは"\r\n"で、C#のは"\n".
            // ここで変換しておかないと後々めんどくさい。
            parsingString = parsingString.Replace ("\r\n", "\n");

            return Lines.Parse (parsingString).ToArray ();
        }
    }




}
