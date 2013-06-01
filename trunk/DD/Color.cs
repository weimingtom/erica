using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DD {
    /// <summary>
    /// カラー構造体
    /// </summary>
    public struct Color {

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        public Color (byte r, byte g, byte b)
            : this () {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = 1;
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="r">赤</param>
        /// <param name="g">緑</param>
        /// <param name="b">青</param>
        /// <param name="a">不透明度</param>
        public Color (byte r, byte g, byte b, byte a)
            : this () {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        /// <summary>
        /// 赤
        /// </summary>
        public byte R {
            get;
            set;
        }

        /// <summary>
        /// 緑
        /// </summary>
        public byte G {
            get;
            set;
        }

        /// <summary>
        /// 青
        /// </summary>
        public byte B {
            get;
            set;
        }

        /// <summary>
        /// 不透明度
        /// </summary>
        public byte A {
            get;
            set;
        }

        /// <summary>
        /// 白
        /// </summary>
        public static Color White {
            get { return new Color (255, 255, 255, 255); }
        }

        /// <summary>
        /// 黒
        /// </summary>
        public static Color Black {
            get { return new Color (0,0,0,1); }
        }
    }
}
