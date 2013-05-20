using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;
using SFML.Graphics;
using System.IO;

namespace DD {
    /// <summary>
    /// FPS測定コンポーネント
    /// </summary>
    /// <remarks>
    /// ゲームのFPSを計測し画面に表示します
    /// </remarks>
    public partial class FPSCounter : Component {
        #region Field
        long prev;
        int count;
        int fps;
        Font font;
        #endregion


        #region Constructor
        /// <summary>
        /// コンストラクター
        /// </summary>
        public FPSCounter () {
            this.prev = 0;
            this.count = 0;
            this.fps = 0;
            this.font = null;
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
            if (msec - prev > 1000) {
                this.fps = count;
                this.prev = msec;
                this.count = 0;
            }
            this.count += 1;
        }

        /// <inheritdoc/>
        public override void OnDraw (object window) {
            if (font == null) {
                font = new Font (new MemoryStream (Properties.Resources.arial));
            }
            var win = window as RenderWindow;
            var txt = new Text (fps.ToString () + " fps", font);
            txt.CharacterSize = 16;
            txt.Position = new Vector2f (Node.WindowX, Node.WindowY);
            win.Draw (txt);
        }
        #endregion

    }
}
