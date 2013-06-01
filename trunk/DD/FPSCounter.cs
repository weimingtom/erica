using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;
using SFML.Graphics;
using System.IO;
using System.Diagnostics;

namespace DD {
    /// <summary>
    /// FPS測定コンポーネント
    /// </summary>
    /// <remarks>
    /// ゲームのFPSを計測し画面に表示します。
    /// FPSの計測には.Netのストップウォッチを直接使用します。
    /// </remarks>
    public partial class FPSCounter : Component {
        #region Field
        long prev;
        int count;
        int fps;
        Font font;
        Stopwatch watch;
        #endregion


        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public FPSCounter () {
            this.prev = 0;
            this.count = 0;
            this.fps = 0;
            this.font = new Font (new MemoryStream (Properties.Resources.arial));
            this.watch = new Stopwatch ();
            this.watch.Start ();
        }
        #endregion

        #region Property
        /// <summary>
        /// FPS
        /// </summary>
        public int Fps {
            get { return fps; }
        }
        #endregion

        #region Method
        /// <inheritdoc/>
        public override void OnUpdate (long msec) {
            msec = watch.ElapsedMilliseconds;
            if (msec - prev > 1000) {
                this.fps = count;
                this.prev = msec;
                this.count = 0;
            }
            this.count += 1;
        }

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            var win = window as RenderWindow;
            var txt = new Text (fps.ToString () + " fps", font);
            txt.CharacterSize = 16;
            txt.Position = new Vector2f (Node.WindowX, Node.WindowY);
            win.Draw (txt);
        }
        #endregion

    }
}
