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
    /// </remarks>
    public struct Rectangle {

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="x">矩形領域の左上のX座標（ピクセル）</param>
        /// <param name="y">矩形領域の左上のY座標（ピクセル）</param>
        /// <param name="width">矩形領域の幅（ピクセル）</param>
        /// <param name="height">矩形領域の高さ（ピクセル）</param>
        public Rectangle (float x, float y, float width, float height) : this() {
            if (width < 0 || height < 0) {
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
        public float X {
            get;
            private set;
        }

        /// <summary>
        /// 矩形領域の左上のY座標（ピクセル）
        /// </summary>
        public float Y {
            get;
            private set;
        }

        /// <summary>
        /// 矩形領域の幅（ピクセル）
        /// </summary>
        public float Width {
            get;
            private set;
        }

        /// <summary>
        /// 矩形領域の高さ（ピクセル）
        /// </summary>
        public float Height {
            get;
            private set;
        }

        /// <summary>
        /// 矩形領域の右下のX座標（ピクセル）
        /// </summary>
        public float X2 {
            get { return X + Width; }
        }

        /// <summary>
        /// 矩形領域の右下のY座標（ピクセル）
        /// </summary>
        public float Y2 {
            get { return Y + Height;}
        }


        /// <summary>
        /// 矩形領域と点の包含関係の検査
        /// </summary>
        /// <param name="x">点のX座標</param>
        /// <param name="y">点のY座標</param>
        /// <returns></returns>
        public bool Contain (float x, float y) {
            return (x >= X && x < X2) && (y >= Y && y < Y2);
        }
    }
}
