using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// バウンディング ボックス構造体
    /// </summary>
    /// <remarks>
    /// 2Dのバウンディング ボックスをあらわす構造体です。
    /// ボックスの左と上側の境界線上は有効で、右と下側の境界線上は無効です。
    /// <note>
    /// これ整数型で大丈夫か・・・
    /// </note>
    /// </remarks>
    public struct BoundingBox {

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="x1">ボックスの左上のX座標（ピクセル）</param>
        /// <param name="y1">ボックスの左上のY座標（ピクセル）</param>
        /// <param name="x2">ボックスの右下のX座標（ピクセル）</param>
        /// <param name="y2">ボックスの右下のY座標（ピクセル）</param>
        public BoundingBox (int x1, int y1, int x2, int y2) : this() {
            if (x1 > x2 || y1 > y2) {
                throw new ArgumentException ("Box coordinate is invalid");
            }
            this.X = x1;
            this.Y = y1;
            this.X2 = x2;
            this.Y2 = y2;
        }

        /// <summary>
        /// ボックスの左上のX座標（ピクセル）
        /// </summary>
        public int X {
            get;
            set;
        }

        /// <summary>
        /// ボックスの左上のY座標（ピクセル）
        /// </summary>
        public int Y {
            get;
            private set;
        }

        /// <summary>
        /// ボックスの右下のX座標（ピクセル）
        /// </summary>
        public int X2 {
            get;
            private set;
        }

        /// <summary>
        /// ボックスの右下のY座標（ピクセル）
        /// </summary>
        public int Y2 {
            get;
            private set;
        }

        /// <summary>
        /// ボックスの幅（ピクセル）
        /// </summary>
        public int Width {
            get { return X2 - X; }
        }

        /// <summary>
        /// ボックスの高さ（ピクセル）
        /// </summary>
        public int Height {
            get { return Y2 - Y; }
        }

        /// <summary>
        /// ボックスと点の包含関係の検査
        /// </summary>
        /// <param name="x">点のX座標</param>
        /// <param name="y">点のY座標</param>
        /// <returns></returns>
        public bool Contain (int x, int y) {
            return (x >= X && x < X2) && (y >= Y && y < Y2);
        }
    }
}
