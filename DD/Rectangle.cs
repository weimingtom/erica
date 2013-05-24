using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// 矩形領域
    /// </summary>
    /// <remarks>
    /// 2Dの矩形領域をあらわす構造体です。
    /// 矩形領域の左と上側の境界線上は有効で、右と下側の境界線上は無効です。
    /// <note>
    /// これ整数型で大丈夫か・・・
    /// </note>
    /// </remarks>
    public struct Rectangle {

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="x">矩形領域の左上のX座標（ピクセル）</param>
        /// <param name="y">矩形領域の左上のY座標（ピクセル）</param>
        /// <param name="width">矩形領域の幅（ピクセル）</param>
        /// <param name="height">矩形領域の高さ（ピクセル）</param>
        public Rectangle (int x, int y, int width, int height) : this() {
            if (width <= 0 || height <= 0) {
                throw new ArgumentException ("Rectangle is invalid");
            }
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// 矩形領域の左上のX座標（ピクセル）
        /// </summary>
        public int X {
            get;
            set;
        }

        /// <summary>
        /// 矩形領域の左上のY座標（ピクセル）
        /// </summary>
        public int Y {
            get;
            private set;
        }

        /// <summary>
        /// 矩形領域の幅（ピクセル）
        /// </summary>
        public int Width {
            get;
            private set;
        }

        /// <summary>
        /// 矩形領域の高さ（ピクセル）
        /// </summary>
        public int Height {
            get;
            private set;
        }

        /// <summary>
        /// 矩形領域の右下のX座標（ピクセル）
        /// </summary>
        public int X2 {
            get { return X + Width; }
        }

        /// <summary>
        /// 矩形領域の右下のY座標（ピクセル）
        /// </summary>
        public int Y2 {
            get { return Y + Height;}
        }


        /// <summary>
        /// 矩形領域と点の包含関係の検査
        /// </summary>
        /// <param name="x">点のX座標</param>
        /// <param name="y">点のY座標</param>
        /// <returns></returns>
        public bool Contain (int x, int y) {
            return (x >= X && x < X2) && (y >= Y && y < Y2);
        }
    }
}
