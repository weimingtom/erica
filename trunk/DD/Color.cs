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
            this.A = 255;
            this.ComponentCount = 3;
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
            this.ComponentCount = 4;
        }

        /// <summary>
        /// 赤
        /// </summary>
        public byte R {
            get;
            internal set;
        }

        /// <summary>
        /// 緑
        /// </summary>
        public byte G {
            get;
            internal set;
        }

        /// <summary>
        /// 青
        /// </summary>
        public byte B {
            get;
            internal set;
        }

        /// <summary>
        /// 不透明度
        /// </summary>
        public byte A {
            get;
            internal set;
        }

        /// <summary>
        /// コンポーネント数
        /// </summary>
        public int ComponentCount {
            get;
            private set;
        }

        /// <summary>
        /// コンポーネントにアクセスするインデクサー
        /// </summary>
        /// <param name="index">コンポーネント番号</param>
        /// <returns>コンポーネント</returns>
        public byte this[int index] {
            get {
                switch (index) {
                    case 0: return R;
                    case 1: return G;
                    case 2: return B;
                    case 3: return A;
                    default: throw new IndexOutOfRangeException ("Index is out of ragne");
                }
            }
            set {
                switch (index) {
                    case 0: this.R = value; break;
                    case 1: this.G = value; break;
                    case 2: this.B = value; break;
                    case 3: this.A = value; break;
                    default: throw new IndexOutOfRangeException ("Index is out of ragne");
                }
            }
        }


        /// <summary>
        /// 白
        /// </summary>
        public static Color White {
            get { return new Color (255, 255, 255, 255); }
        }

        /// <summary>
        /// 灰色
        /// </summary>
        public static Color Gray {
            get { return new Color (128, 128, 128, 255); }
        }

        /// <summary>
        /// 黒
        /// </summary>
        public static Color Black {
            get { return new Color (0,0,0,255); }
        }


        /// <summary>
        /// 赤
        /// </summary>
        public static Color Red {
            get { return new Color (255, 0, 0, 255); }
        }

        /// <summary>
        /// 黄色
        /// </summary>
        public static Color Yellow {
            get { return new Color (255, 255, 0, 255); }
        }

        /// <summary>
        /// 緑
        /// </summary>
        public static Color Green {
            get { return new Color (0, 255, 0, 255); }
        }

        /// <summary>
        /// 青
        /// </summary>
        public static Color Blue {
            get { return new Color (0, 0, 255, 255); }
        }

        /// <summary>
        /// 水色
        /// </summary>
        public static Color Cyan {
            get { return new Color (0, 255, 255, 255); }
        }

        /// <summary>
        /// 紫
        /// </summary>
        public static Color Purple {
            get { return new Color (255, 0, 255, 255); }
        }


        /// <inheritdoc/>
        public override bool Equals (object obj) {
            if (obj == null || this.GetType () != obj.GetType ()) {
                return false;
            }
            return this.Equals ((Color)obj);
        }

        /// <inheritdoc/>
        public bool Equals (Color other) {
            return R == other.R && G == other.G && B == other.B && A == other.A;
        }

        /// <inheritdoc/>
        public override int GetHashCode () {
            return R.GetHashCode () ^ G.GetHashCode () ^ B.GetHashCode () ^ A.GetHashCode ();
        }

        /// <inheritdoc/>
        public static bool operator == (Color a, Color b) {
            return (a.R == b.R) && (a.G == b.G) && (a.B == b.B) && (a.A == b.A);
        }

        /// <inheritdoc/>
        public static bool operator != (Color a, Color b) {
            return !(a == b);
        }

    }
}
